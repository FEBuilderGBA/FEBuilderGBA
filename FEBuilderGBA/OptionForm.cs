using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class OptionForm : Form
    {
        public OptionForm()
        {
            InitializeComponent();
            MakeExplainFunctions();

            func_lang.Items.Clear();
            func_lang.Items.Add("auto=自動判別");
            func_lang.Items.Add("ja=日本語");     ///No Translate
            func_lang.Items.Add("en=English");    ///No Translate
            func_lang.Items.Add("zh=中文");       ///No Translate

            InputFormRef.markupJumpLabel(X_EXPLAIN_NECESSARY_PROGRAM);
            this.Icon = Properties.Resources.icon_settings;

            U.SetIcon(ColorSaveASbutton, Properties.Resources.icon_arrow);
            U.SetIcon(ColorLoadButton, Properties.Resources.icon_upload);
        }

        public void AutoClose(int autocolor = 0)
        {
            OptionForm_Load(null, null);
            if (autocolor >= 1) 
            {
                U.SelectedIndexSafety(ColorSetComboBox, autocolor);
            }

            Save();
            this.Close();
        }

        private void OptionForm_Load(object sender, EventArgs e)
        {
            emulator.Text = Program.Config.at("emulator");
            emulator2.Text = Program.Config.at("emulator2");
            sappy.Text = Program.Config.at("sappy");
            program1.Text = Program.Config.at("program1");
            program2.Text = Program.Config.at("program2");
            program3.Text = Program.Config.at("program3");
            devkitpro_eabi.Text = Program.Config.at("devkitpro_eabi");
            goldroad_asm.Text = Program.Config.at("goldroad_asm");
            event_assembler.Text = Program.Config.at("event_assembler");
            mid2agb.Text = Program.Config.at("mid2agb");
            mid2agb_default.Checked = midi_importer() == midi_importer_enum.FEBuilderGBA ? false : true;
            gba_mus_riper.Text = Program.Config.at("gba_mus_riper");
            sox.Text = Program.Config.at("sox");
            midfix4agb.Text = Program.Config.at("midfix4agb");
            FECLIB.Text = Program.Config.at("FECLIB");
            CFLAGS.Text = GetCFLAGS();
            retdec.Text = GetRetDec();
            python3.Text = GetPython3();
            srccode_texteditor.Text = GetSrccodeTexteditor();
            srccode_directory.Text = GetSrccodeDirectory();

            Color_Control_BackColor_button.BackColor = Color_Control_BackColor();
            Color_Control_ForeColor_button.BackColor = Color_Control_ForeColor();
            Color_Input_BackColor_button.BackColor = Color_Input_BackColor();
            Color_Input_ForeColor_button.BackColor = Color_Input_ForeColor();
            Color_InputDecimal_BackColor_button.BackColor = Color_InputDecimal_BackColor();
            Color_InputDecimal_ForeColor_button.BackColor = Color_InputDecimal_ForeColor();
            Color_NotifyWrite_BackColor_button.BackColor = Color_NotifyWrite_BackColor();
            Color_NotifyWrite_ForeColor_button.BackColor = Color_NotifyWrite_ForeColor();
            Color_Error_BackColor_button.BackColor = Color_Error_BackColor();
            Color_Error_ForeColor_button.BackColor = Color_Error_ForeColor();
            Color_List_SelectedColor_button.BackColor = Color_List_SelectedColor();
            Color_List_HoverColor_button.BackColor = Color_List_HoverColor();
            Color_Keyword_BackColor_button.BackColor = Color_Keyword_BackColor();
            Color_Keyword_ForeColor_button.BackColor = Color_Keyword_ForeColor();
            Color_Comment_ForeColor_button.BackColor = Color_Comment_ForeColor();
            Color_ControlComment_ForeColor_button.BackColor = Color_ControlComment_ForeColor();
            Color_List_RelatedLine_BackColor_button.BackColor = Color_List_RelatedLine_BackColor();

            foreach(String a in ShortCutValue1.Items)
            {
                ShortCutValue2.Items.Add(a);
                ShortCutValue3.Items.Add(a);
                ShortCutValue4.Items.Add(a);
                ShortCutValue5.Items.Add(a);
                ShortCutValue6.Items.Add(a);
                ShortCutValue7.Items.Add(a);
                ShortCutValue8.Items.Add(a);
                ShortCutValue9.Items.Add(a);
                ShortCutValue10.Items.Add(a);
                ShortCutValue11.Items.Add(a);
                ShortCutValue12.Items.Add(a);
                ShortCutValue13.Items.Add(a);
                ShortCutValue14.Items.Add(a);
                ShortCutValue15.Items.Add(a);
            }

            ShortCutKey1.Text = Program.Config.at("ShortCutKey1");
            ShortCutValue1.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue1"));  //F5 エミュレータ
            ShortCutKey2.Text = Program.Config.at("ShortCutKey2");
            ShortCutValue2.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue2"));  //Ctrl+F5 デバッガー
            ShortCutKey3.Text = Program.Config.at("ShortCutKey3");
            ShortCutValue3.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue3")); //Ctrl+K 書き込み
            ShortCutKey4.Text = Program.Config.at("ShortCutKey4");
            ShortCutValue4.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue4")); //Pause メインへ
            ShortCutKey5.Text = Program.Config.at("ShortCutKey5");
            ShortCutValue5.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue5")); //F11 バイナリエディタ
            ShortCutKey6.Text = Program.Config.at("ShortCutKey6");
            ShortCutValue6.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue6")); //リストから次を検索
            ShortCutKey7.Text = Program.Config.at("ShortCutKey7");
            ShortCutValue7.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue7"));
            ShortCutKey8.Text = Program.Config.at("ShortCutKey8");
            ShortCutValue8.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue8"));
            ShortCutKey9.Text = Program.Config.at("ShortCutKey9");
            ShortCutValue9.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue9"));
            ShortCutKey10.Text = Program.Config.at("ShortCutKey10");
            ShortCutValue10.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue10"));
            ShortCutKey11.Text = Program.Config.at("ShortCutKey11");
            ShortCutValue11.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue11")); //Ctrl+W 閉じる
            ShortCutKey12.Text = Program.Config.at("ShortCutKey12");
            ShortCutValue12.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue12"));
            ShortCutKey13.Text = Program.Config.at("ShortCutKey13");
            ShortCutValue13.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue13"));
            ShortCutKey14.Text = Program.Config.at("ShortCutKey14");
            ShortCutValue14.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue14"));
            ShortCutKey15.Text = Program.Config.at("ShortCutKey15");
            ShortCutValue15.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue15"));
          
            U.SelectedIndexSafety(func_rom_extends,(int)rom_extends());
            U.SelectedIndexSafety(func_rom_extends_option, rom_extends_option());
            U.SelectedIndexSafety(func_alloc_program_area_option, alloc_program_area_option());
            U.SelectedIndexSafety(func_auto_backup, (int)auto_backup());
            U.SelectedIndexSafety(func_first_form,(int)first_form());
            U.SelectedIndexSafety(func_lang,(int)FindComboboxText(Program.Config.at("func_lang", "auto"), func_lang));
            U.SelectedIndexSafety(func_textextencodingtencoding,(int)textencoding());
            U.SelectedIndexSafety(func_auto_update,(int)FindComboboxText( auto_update().ToString(), func_auto_update));
            U.SelectedIndexSafety(func_text_escape,(int)text_escape());
            U.SelectedIndexSafety(func_write_notify_time,(int)FindComboboxText(write_notify_time().ToString(), func_write_notify_time));
            U.SelectedIndexSafety(func_write_out_of_range,(int)write_out_of_range());
            U.SelectedIndexSafety(func_write_low_address, (int)write_low_address());
            U.SelectedIndexSafety(func_write_00, (int)write_00());
            U.SelectedIndexSafety(func_select_in_explorer_when_export, (int)select_in_explorer_when_export());
            U.SelectedIndexSafety(func_lookup_feditor,(int)lookup_feditor());
            U.SelectedIndexSafety(func_lint_text_skip_bug,(int)lint_text_skip_bug());
            U.SelectedIndexSafety(func_show_class_extends,(int)show_class_extends());
            U.SelectedIndexSafety(func_show_chapter_extends, (int)show_chapter_extends());
            U.SelectedIndexSafety(func_show_worldmap_node_extends, (int)show_worldmap_node_extends());
            U.SelectedIndexSafety(func_show_worldmap_path_extends, (int)show_worldmap_path_extends());
            U.SelectedIndexSafety(func_show_fe76_item_icon_extends, (int)show_fe76_item_icon_extends());
            U.SelectedIndexSafety(func_show_song_table_extends, (int)show_song_table_extends());
            U.SelectedIndexSafety(func_show_gameoption_extends, (int)show_gameoption_extends());
            U.SelectedIndexSafety(func_texteditor_auto_convert_space, (int)texteditor_auto_convert_space());
            U.SelectedIndexSafety(func_auto_connect_emulator, (int)auto_connect_emulator());
            U.SelectedIndexSafety(func_proxy_server_when_connecting, (int)proxy_server_when_connecting());
            U.SelectedIndexSafety(func_notify_upper_time, (int)notify_upper_time());
            U.SelectedIndexSafety(func_create_nodoll_gba_sym, (int)create_nodoll_gba_sym());
            U.SelectedIndexSafety(func_overraide_simple_error_check, (int)overraide_simple_error_check());
            U.SelectedIndexSafety(func_alert_unk_event_code, (int)alert_unk_event_code());
            U.SelectedIndexSafety(func_skillsystems_sanctuary, (int)skillsystems_sanctuary());
            U.SelectedIndexSafety(func_portrait_lz77, (int)portrait_lz77());
            U.SelectedIndexSafety(func_felint_haxs, U.BoolToInt(felint_haxs()));
            U.SelectedIndexSafety(func_felint_support_validation, U.BoolToInt(felint_support_validation()));
            U.SelectedIndexSafety(func_felint_check_melee_item_motionid, U.BoolToInt(felint_check_melee_item_motionid()));
            U.SelectedIndexSafety(func_felint_check_seriftext_width, U.BoolToInt(felint_check_text_width()));
            U.SelectedIndexSafety(func_felint_check_exist_tilechange, U.BoolToInt(felint_check_exist_tilechange()));
            U.SelectedIndexSafety(func_felint_check_song_tie_eot, U.BoolToInt(felint_check_song_tie_eot()));
            U.SelectedIndexSafety(RunTestMessage, (int)U.atoi(Program.Config.at("RunTestMessage")));

            ChangeColorWriteButtonWhenChangingSetting();
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
            ERROR_IsOldEA.Text += R._("最新版は{0}です。", CurrentEAVersion);
            CheckEAVersion();
        }

        private void emulator_TextChanged(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
        }
        void ChangeColorWriteButtonWhenChangingSetting()
        {
            List<Control> list = InputFormRef.GetAllControls(this);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == "WriteButton"
                    || list[i].Name == "KeyFinder")
                {
                    continue;
                }
                if (list[i] is Button)
                {
                    ((Button)list[i]).BackColorChanged += this.emulator_TextChanged;
                }
                if (list[i] is TextBox)
                {
                    ((TextBox)list[i]).TextChanged += this.emulator_TextChanged;
                }
                if (list[i] is ComboBox)
                {
                    ((ComboBox)list[i]).SelectedIndexChanged += this.emulator_TextChanged;
                }
            }

        }

        int FindComboboxText(string lang,ComboBox list)
        {
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (list.Items[i].ToString().IndexOf(lang) == 0)
                {
                    return i;
                }
            }
            return 0;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                Save();
            }
        }
        private void Save()
        {
            ClearCache();
            Program.Config["emulator"] = emulator.Text;
            Program.Config["emulator2"] = emulator2.Text;
            Program.Config["sappy"] = sappy.Text;
            Program.Config["program1"] = program1.Text;
            Program.Config["program2"] = program2.Text;
            Program.Config["program3"] = program3.Text;
            Program.Config["devkitpro_eabi"] = devkitpro_eabi.Text;
            Program.Config["goldroad_asm"] = goldroad_asm.Text;
            Program.Config["event_assembler"] = event_assembler.Text;
            Program.Config["mid2agb"] = mid2agb.Text;
            Program.Config["CFLAGS"] = CFLAGS.Text;
            Program.Config["retdec"] = retdec.Text;
            Program.Config["python3"] = python3.Text;
            Program.Config["gba_mus_riper"] = gba_mus_riper.Text;
            Program.Config["sox"] = sox.Text;
            Program.Config["midfix4agb"] = midfix4agb.Text;
            Program.Config["FECLIB"] = FECLIB.Text;
            Program.Config["srccode_texteditor"] = srccode_texteditor.Text;
            Program.Config["srccode_directory"] = srccode_directory.Text;

            Program.Config["Color_Control_BackColor"] = Color_Control_BackColor_button.BackColor.Name;
            Program.Config["Color_Control_ForeColor"] = Color_Control_ForeColor_button.BackColor.Name;
            Program.Config["Color_Input_BackColor"] = Color_Input_BackColor_button.BackColor.Name;
            Program.Config["Color_Input_ForeColor"] = Color_Input_ForeColor_button.BackColor.Name;
            Program.Config["Color_InputDecimal_BackColor"] = Color_InputDecimal_BackColor_button.BackColor.Name;
            Program.Config["Color_InputDecimal_ForeColor"] = Color_InputDecimal_ForeColor_button.BackColor.Name;
            Program.Config["Color_NotifyWrite_BackColor"] = Color_NotifyWrite_BackColor_button.BackColor.Name;
            Program.Config["Color_NotifyWrite_ForeColor"] = Color_NotifyWrite_ForeColor_button.BackColor.Name;
            Program.Config["Color_List_SelectedColor"] = Color_List_SelectedColor_button.BackColor.Name;
            Program.Config["Color_List_HoverColor"] = Color_List_HoverColor_button.BackColor.Name;
            Program.Config["Color_Keyword_BackColor"] = Color_Keyword_BackColor_button.BackColor.Name;
            Program.Config["Color_Keyword_ForeColor"] = Color_Keyword_ForeColor_button.BackColor.Name;
            Program.Config["Color_Comment_ForeColor"] = Color_Comment_ForeColor_button.BackColor.Name;
            Program.Config["Color_ControlComment_ForeColor"] = Color_ControlComment_ForeColor_button.BackColor.Name;
            Program.Config["Color_List_RelatedLine_BackColor"] = Color_List_RelatedLine_BackColor_button.BackColor.Name;

            Program.Config["ShortCutKey1"] = ShortCutKey1.Text;
            Program.Config["ShortCutValue1"] = ShortCutValue1.SelectedIndex.ToString();
            Program.Config["ShortCutKey2"] = ShortCutKey2.Text;
            Program.Config["ShortCutValue2"] = ShortCutValue2.SelectedIndex.ToString();
            Program.Config["ShortCutKey3"] = ShortCutKey3.Text;
            Program.Config["ShortCutValue3"] = ShortCutValue3.SelectedIndex.ToString();
            Program.Config["ShortCutKey4"] = ShortCutKey4.Text;
            Program.Config["ShortCutValue4"] = ShortCutValue4.SelectedIndex.ToString();
            Program.Config["ShortCutKey5"] = ShortCutKey5.Text;
            Program.Config["ShortCutValue5"] = ShortCutValue5.SelectedIndex.ToString();
            Program.Config["ShortCutKey6"] = ShortCutKey6.Text;
            Program.Config["ShortCutValue6"] = ShortCutValue6.SelectedIndex.ToString();
            Program.Config["ShortCutKey7"] = ShortCutKey7.Text;
            Program.Config["ShortCutValue7"] = ShortCutValue7.SelectedIndex.ToString();
            Program.Config["ShortCutKey8"] = ShortCutKey8.Text;
            Program.Config["ShortCutValue8"] = ShortCutValue8.SelectedIndex.ToString();
            Program.Config["ShortCutKey9"] = ShortCutKey9.Text;
            Program.Config["ShortCutValue9"] = ShortCutValue9.SelectedIndex.ToString();
            Program.Config["ShortCutKey10"] = ShortCutKey10.Text;
            Program.Config["ShortCutValue10"] = ShortCutValue10.SelectedIndex.ToString();
            Program.Config["ShortCutKey11"] = ShortCutKey11.Text;
            Program.Config["ShortCutValue11"] = ShortCutValue11.SelectedIndex.ToString();
            Program.Config["ShortCutKey12"] = ShortCutKey12.Text;
            Program.Config["ShortCutValue12"] = ShortCutValue12.SelectedIndex.ToString();
            Program.Config["ShortCutKey13"] = ShortCutKey13.Text;
            Program.Config["ShortCutValue13"] = ShortCutValue13.SelectedIndex.ToString();
            Program.Config["ShortCutKey14"] = ShortCutKey14.Text;
            Program.Config["ShortCutValue14"] = ShortCutValue14.SelectedIndex.ToString();
            Program.Config["ShortCutKey15"] = ShortCutKey15.Text;
            Program.Config["ShortCutValue15"] = ShortCutValue15.SelectedIndex.ToString();

            Program.Config["func_rom_extends"] = func_rom_extends.SelectedIndex.ToString();
            Program.Config["func_rom_extends_option"] = func_rom_extends_option.SelectedIndex.ToString();
            Program.Config["func_alloc_program_area_option"] = func_alloc_program_area_option.SelectedIndex.ToString();
            Program.Config["func_auto_backup"] = func_auto_backup.SelectedIndex.ToString();
            Program.Config["func_first_form"] = func_first_form.SelectedIndex.ToString();
            Program.Config["func_lang"] = U.SelectValueComboboxText(func_lang.Text);
            Program.Config["func_textencoding"] = func_textextencodingtencoding.SelectedIndex.ToString();
            Program.Config["func_auto_update"] = U.atoi(func_auto_update.Text).ToString();
            Program.Config["func_text_escape"] = func_text_escape.SelectedIndex.ToString();
            Program.Config["func_write_notify_time"] = U.SelectValueComboboxText(func_write_notify_time.Text);
            Program.Config["func_write_out_of_range"] = U.SelectValueComboboxText(func_write_out_of_range.Text);
            Program.Config["func_write_low_address"] = U.SelectValueComboboxText(func_write_low_address.Text);
            Program.Config["func_write_00"] = U.SelectValueComboboxText(func_write_00.Text);
            Program.Config["func_select_in_explorer_when_export"] = U.SelectValueComboboxText(func_select_in_explorer_when_export.Text);
            Program.Config["func_lookup_feditor"] = U.SelectValueComboboxText(func_lookup_feditor.Text);
            Program.Config["func_lint_text_skip_bug"] = U.SelectValueComboboxText(func_lint_text_skip_bug.Text);
            Program.Config["func_midi_importer"] = mid2agb_default.Checked ? "1" : "0";
            Program.Config["func_show_class_extends"] = U.SelectValueComboboxText(func_show_class_extends.Text);
            Program.Config["func_show_chapter_extends"] = U.SelectValueComboboxText(func_show_chapter_extends.Text);
            Program.Config["func_show_worldmap_node_extends"] = U.SelectValueComboboxText(func_show_worldmap_node_extends.Text);
            Program.Config["func_show_worldmap_path_extends"] = U.SelectValueComboboxText(func_show_worldmap_path_extends.Text);
            Program.Config["func_show_fe76_item_icon_extends"] = U.SelectValueComboboxText(func_show_fe76_item_icon_extends.Text);
            Program.Config["func_show_song_table_extends"] = U.SelectValueComboboxText(func_show_song_table_extends.Text);
            Program.Config["func_show_gameoption_extends"] = U.SelectValueComboboxText(func_show_gameoption_extends.Text);
            Program.Config["func_texteditor_auto_convert_space"] = U.SelectValueComboboxText(func_texteditor_auto_convert_space.Text);
            Program.Config["func_auto_connect_emulator"] = U.SelectValueComboboxText(func_auto_connect_emulator.Text);
            Program.Config["func_proxy_server_when_connecting"] = U.SelectValueComboboxText(func_proxy_server_when_connecting.Text);
            Program.Config["func_notify_upper_time"] = U.SelectValueComboboxText(func_notify_upper_time.Text);
            Program.Config["func_create_nodoll_gba_sym"] = U.SelectValueComboboxText(func_create_nodoll_gba_sym.Text);
            Program.Config["func_overraide_simple_error_check"] = U.SelectValueComboboxText(func_overraide_simple_error_check.Text);
            Program.Config["func_alert_unk_event_code"] = U.SelectValueComboboxText(func_alert_unk_event_code.Text);
            Program.Config["func_skillsystems_sanctuary"] = U.SelectValueComboboxText(func_skillsystems_sanctuary.Text);
            Program.Config["func_portrait_lz77"] = U.SelectValueComboboxText(func_portrait_lz77.Text);
            Program.Config["func_felint_haxs"] = U.SelectValueComboboxText(func_felint_haxs.Text);
            Program.Config["func_felint_support_validation"] = U.SelectValueComboboxText(func_felint_support_validation.Text);
            Program.Config["func_felint_check_melee_item_motionid"] = U.SelectValueComboboxText(func_felint_check_melee_item_motionid.Text);
            Program.Config["func_felint_check_seriftext_width"] = U.SelectValueComboboxText(func_felint_check_seriftext_width.Text);
            Program.Config["func_felint_check_exist_tilechange"] = U.SelectValueComboboxText(func_felint_check_exist_tilechange.Text);
            Program.Config["func_felint_check_song_tie_eot"] = U.SelectValueComboboxText(func_felint_check_song_tie_eot.Text);
            Program.Config["RunTestMessage"] = U.SelectValueComboboxText(RunTestMessage.Text);

            //configの保存
            Program.Config.Save();

            Program.Config.UpdateShortcutKeys();

            Program.ReLoadSetting();

            //すべてのフォームを再描画
            InputFormRef.InvalidateALLForms();

            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
            this.Close();
        }

        public static void ClearCache()
        {
            g_Cache_write_text_escape_enum = text_escape_enum.NoCache;
            g_Cache_lint_text_skip_bug_enum = lint_text_skip_bug_enum.NoCache;
            g_Cache_lang = null;
            g_Cache_textencoding = textencoding_enum.NoChace;
            g_skillsystems_sanctuary = skillsystems_sanctuary_enum.NoCache;

            g_Color_Control_BackColor = Color.Empty;
            g_Color_Control_ForeColor = Color.Empty;
            g_Color_Input_BackColor = Color.Empty;
            g_Color_Input_ForeColor = Color.Empty;
            g_Color_InputDecimal_BackColor = Color.Empty;
            g_Color_InputDecimal_ForeColor = Color.Empty;
            g_Color_NotifyWrite_BackColor = Color.Empty;
            g_Color_NotifyWrite_ForeColor = Color.Empty;
            g_Color_Error_BackColor = Color.Empty;
            g_Color_Error_ForeColor = Color.Empty;
            g_Color_List_SelectedColor = Color.Empty;
            g_Color_List_HoverColor = Color.Empty;
            g_Color_Keyword_BackColor = Color.Empty;
            g_Color_Keyword_ForeColor = Color.Empty;
            g_Color_Comment_ForeColor = Color.Empty;
            g_Color_ControlComment_ForeColor = Color.Empty;
            g_Color_List_RelatedLine_BackColor = Color.Empty;
        }

        public static string EXESearch(string first_filter)
        {
            string title = R._("ツールを選択してください");
            string filter = first_filter + "EXE|*.exe|All files|*";

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            open.Multiselect = false;
            open.CheckFileExists = true;
            open.CheckPathExists = true;
            open.ShowDialog();
            if (!U.CanReadFileRetry(open))
            {
                return "";
            }
            return open.FileNames[0];
        }

        static Color g_Color_Control_BackColor = Color.Empty;
        public static Color Color_Control_BackColor()
        {
            if (g_Color_Control_BackColor.IsEmpty)
            {
                g_Color_Control_BackColor = U.ColorFromName(Program.Config.at("Color_Control_BackColor", "Control"));
            }
            return g_Color_Control_BackColor;
        }

        static Color g_Color_Control_ForeColor = Color.Empty;
        public static Color Color_Control_ForeColor()
        {
            if (g_Color_Control_ForeColor.IsEmpty)
            {
                g_Color_Control_ForeColor = U.ColorFromName(Program.Config.at("Color_Control_ForeColor", "ControlText"));
            }
            return g_Color_Control_ForeColor;
        }

        static Color g_Color_Input_BackColor = Color.Empty;
        public static Color Color_Input_BackColor()
        {
            if (g_Color_Input_BackColor.IsEmpty)
            {
                g_Color_Input_BackColor = U.ColorFromName(Program.Config.at("Color_Input_BackColor", "Window"));
            }
            return g_Color_Input_BackColor;
        }

        static Color g_Color_Input_ForeColor = Color.Empty;
        public static Color Color_Input_ForeColor()
        {
            if (g_Color_Input_ForeColor.IsEmpty)
            {
                g_Color_Input_ForeColor = U.ColorFromName(Program.Config.at("Color_Input_ForeColor", "WindowText"));
            }
            return g_Color_Input_ForeColor;
        }

        static Color g_Color_InputDecimal_BackColor = Color.Empty;
        public static Color Color_InputDecimal_BackColor()
        {
            if (g_Color_InputDecimal_BackColor.IsEmpty)
            {
                g_Color_InputDecimal_BackColor = U.ColorFromName(Program.Config.at("Color_InputDecimal_BackColor", "Window"));
            }
            return g_Color_InputDecimal_BackColor;
        }

        static Color g_Color_InputDecimal_ForeColor = Color.Empty;
        public static Color Color_InputDecimal_ForeColor()
        {
            if (g_Color_InputDecimal_ForeColor.IsEmpty)
            {
                return U.ColorFromName(Program.Config.at("Color_InputDecimal_ForeColor", "OrangeRed"));
            }
            return g_Color_InputDecimal_ForeColor;
        }

        static Color g_Color_NotifyWrite_BackColor = Color.Empty;
        public static Color Color_NotifyWrite_BackColor()
        {
            if (g_Color_NotifyWrite_BackColor.IsEmpty)
            {
                g_Color_NotifyWrite_BackColor = U.ColorFromName(Program.Config.at("Color_NotifyWrite_BackColor", "Yellow"));
            }
            return g_Color_NotifyWrite_BackColor;
        }

        static Color g_Color_NotifyWrite_ForeColor = Color.Empty;
        public static Color Color_NotifyWrite_ForeColor()
        {
            if (g_Color_NotifyWrite_ForeColor.IsEmpty)
            {
                g_Color_NotifyWrite_ForeColor = U.ColorFromName(Program.Config.at("Color_NotifyWrite_ForeColor", "Black"));
            }
            return g_Color_NotifyWrite_ForeColor;
        }

        static Color g_Color_Error_BackColor = Color.Empty;
        public static Color Color_Error_BackColor()
        {
            if (g_Color_Error_BackColor.IsEmpty)
            {
                g_Color_Error_BackColor = U.ColorFromName(Program.Config.at("Color_Error_BackColor", "Control"));
            }
            return g_Color_Error_BackColor;
        }
        static Color g_Color_Error_ForeColor = Color.Empty;
        public static Color Color_Error_ForeColor()
        {
            if (g_Color_Error_ForeColor.IsEmpty)
            {
                g_Color_Error_ForeColor = U.ColorFromName(Program.Config.at("Color_Error_ForeColor", "Red"));
            }
            return g_Color_Error_ForeColor;
        }
        static Color g_Color_List_SelectedColor = Color.Empty;
        public static Color Color_List_SelectedColor()
        {
            if (g_Color_List_SelectedColor.IsEmpty)
            {
                return U.ColorFromName(Program.Config.at("Color_List_SelectedColor", "Highlight"));
            }
            return g_Color_List_SelectedColor;
        }
        static Color g_Color_List_HoverColor = Color.Empty;
        public static Color Color_List_HoverColor()
        {
            if (g_Color_List_HoverColor.IsEmpty)
            {
                g_Color_List_HoverColor = U.ColorFromName(Program.Config.at("Color_List_HoverColor", "WhiteSmoke"));
            }
            return g_Color_List_HoverColor;
        }
        static Color g_Color_Keyword_BackColor = Color.Empty;
        public static Color Color_Keyword_BackColor()
        {
            if (g_Color_Keyword_BackColor.IsEmpty)
            {
                g_Color_Keyword_BackColor = U.ColorFromName(Program.Config.at("Color_Keyword_BackColor", "Window"));
            }
            return g_Color_Keyword_BackColor;
        }
        static Color g_Color_Keyword_ForeColor = Color.Empty;
        public static Color Color_Keyword_ForeColor()
        {
            if (g_Color_Keyword_ForeColor.IsEmpty)
            {
                g_Color_Keyword_ForeColor = U.ColorFromName(Program.Config.at("Color_Keyword_ForeColor", "Blue"));
            }
            return g_Color_Keyword_ForeColor;
        }

        static Color g_Color_Comment_ForeColor = Color.Empty;
        public static Color Color_Comment_ForeColor()
        {
            if (g_Color_Comment_ForeColor.IsEmpty)
            {
                g_Color_Comment_ForeColor = U.ColorFromName(Program.Config.at("Color_Comment_ForeColor", "DarkGreen"));
            }
            return g_Color_Comment_ForeColor;
        }

        static Color g_Color_ControlComment_ForeColor = Color.Empty;
        public static Color Color_ControlComment_ForeColor()
        {
            if (g_Color_ControlComment_ForeColor.IsEmpty)
            {
                g_Color_ControlComment_ForeColor = U.ColorFromName(Program.Config.at("Color_ControlComment_ForeColor", "Green"));
            }
            return g_Color_ControlComment_ForeColor;
        }

        static Color g_Color_List_RelatedLine_BackColor = Color.Empty;
        public static Color Color_List_RelatedLine_BackColor()
        {
            if (g_Color_List_RelatedLine_BackColor.IsEmpty)
            {
                //下位互換性のため色判定を通します
                string color = Program.Config.at("Color_List_RelatedLine_BackColor", "");
                if (color == "")
                {
                    if (Color_Control_ForeColor() == U.ColorFromName("White"))
                    {//黒背景
                        color = "ff295454";
                    }
                    else
                    {//白背景
                        color = "ffffe1ff";
                    }
                }
                g_Color_List_RelatedLine_BackColor = U.ColorFromName(color);
            }
            return g_Color_List_RelatedLine_BackColor;
        }
        

        Color SelectColorDialog(Color def)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = def;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //選択された色の取得
                return cd.Color;
            }
            //元の色
            return def;
        }

        private void emulator_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("");
            if (r != "")
            {
                emulator.Text = r;
            }
        }

        private void emulator2_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("");
            if (r != "")
            {
                emulator2.Text = r;
            }
        }

        private void sappy_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("SAPPY,VGMusicStudio|sappy.exe;VG Music Studio.exe|");
            if (r != "")
            {
                sappy.Text = r;
            }
        }

        private void program1_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("");
            if (r != "")
            {
                program1.Text = r;
            }
        }

        private void program2_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("");
            if (r != "")
            {
                program2.Text = r;
            }
        }

        private void program3_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("");
            if (r != "")
            {
                program3.Text = r;
            }
        }

        private void Color_Control_BackColor_button_Click(object sender, EventArgs e)
        {
            Color_Control_BackColor_button.BackColor = SelectColorDialog(Color_Control_BackColor_button.BackColor);
        }

        private void Color_Control_ForeColor_button_Click(object sender, EventArgs e)
        {
            Color_Control_ForeColor_button.BackColor = SelectColorDialog(Color_Control_ForeColor_button.BackColor);
        }

        private void Color_Input_BackColor_button_Click(object sender, EventArgs e)
        {
            Color_Input_BackColor_button.BackColor = SelectColorDialog(Color_Input_BackColor_button.BackColor);
        }

        private void Color_Input_ForeColor_button_Click(object sender, EventArgs e)
        {
            Color_Input_ForeColor_button.BackColor = SelectColorDialog(Color_Input_ForeColor_button.BackColor);
        }

        private void Color_InputDecimal_BackColor_button_Click(object sender, EventArgs e)
        {
            Color_InputDecimal_BackColor_button.BackColor = SelectColorDialog(Color_InputDecimal_BackColor_button.BackColor);
        }

        private void Color_InputDecimal_ForeColor_button_Click(object sender, EventArgs e)
        {
            Color_InputDecimal_ForeColor_button.BackColor = SelectColorDialog(Color_InputDecimal_ForeColor_button.BackColor);
        }

        private void Color_NotifyWrite_BackColor_button_Click(object sender, EventArgs e)
        {
            Color_NotifyWrite_BackColor_button.BackColor = SelectColorDialog(Color_NotifyWrite_BackColor_button.BackColor);
        }

        private void Color_NotifyWrite_ForeColor_button_Click(object sender, EventArgs e)
        {
            Color_NotifyWrite_ForeColor_button.BackColor = SelectColorDialog(Color_NotifyWrite_ForeColor_button.BackColor);
        }



        private void ShortCutKey1_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey1.Text, out keys))
            {
                ShortCutKey1.Text = "";
            }
        }
        private void ShortCutKey2_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey2.Text, out keys))
            {
                ShortCutKey2.Text = "";
            }
        }
        private void ShortCutKey3_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey3.Text, out keys))
            {
                ShortCutKey3.Text = "";
            }
        }
        private void ShortCutKey4_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey4.Text, out keys))
            {
                ShortCutKey4.Text = "";
            }
        }
        private void ShortCutKey5_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey5.Text, out keys))
            {
                ShortCutKey5.Text = "";
            }
        }
        private void ShortCutKey6_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey6.Text, out keys))
            {
                ShortCutKey6.Text = "";
            }
        }
        private void ShortCutKey7_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey7.Text, out keys))
            {
                ShortCutKey7.Text = "";
            }
        }
        private void ShortCutKey8_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey8.Text, out keys))
            {
                ShortCutKey8.Text = "";
            }
        }
        private void ShortCutKey9_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey9.Text, out keys))
            {
                ShortCutKey9.Text = "";
            }
        }
        private void ShortCutKey10_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey10.Text, out keys))
            {
                ShortCutKey10.Text = "";
            }
        }

        private void ShortCutKey11_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey11.Text, out keys))
            {
                ShortCutKey11.Text = "";
            }
        }
        private void ShortCutKey12_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey12.Text, out keys))
            {
                ShortCutKey12.Text = "";
            }
        }
        private void ShortCutKey13_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey13.Text, out keys))
            {
                ShortCutKey13.Text = "";
            }
        }
        private void ShortCutKey14_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey14.Text, out keys))
            {
                ShortCutKey14.Text = "";
            }
        }
        private void ShortCutKey15_Leave(object sender, EventArgs e)
        {
            Keys keys;
            if (!U.CheckKeys(ShortCutKey15.Text, out keys))
            {
                ShortCutKey15.Text = "";
            }
        }

        private void Color_Error_BackColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_NotifyWrite_BackColor_button.BackColor = Color.FromName("Control");
        }

        private void Color_Error_ForeColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_NotifyWrite_ForeColor_button.BackColor = Color.FromName("Red");
        }

        private void Color_Error_BackColor_button_Click(object sender, EventArgs e)
        {
            Color_Error_BackColor_button.BackColor = SelectColorDialog(Color_Error_BackColor_button.BackColor);
        }

        private void Color_Error_ForeColor_button_Click(object sender, EventArgs e)
        {
            Color_Error_ForeColor_button.BackColor = SelectColorDialog(Color_Error_ForeColor_button.BackColor);
        }

        private void devkitpro_eabi_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("arm-none-eabi-as.exe|arm-none-eabi-as.exe;as.exe|");
            if (r != "")
            {
                devkitpro_eabi.Text = r;
            }
        }

        private void goldroad_asm_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("goldroad.exe|goldroad.exe|");
            if (r != "")
            {
                goldroad_asm.Text = r;
            }
        }


        public enum rom_extends_enum
        {
             NO   = 0
            ,YES  = 1
        };
        public static rom_extends_enum rom_extends()
        {
            return (rom_extends_enum)U.atoi(Program.Config.at("func_rom_extends", "1"));
        }

        public static int rom_extends_option()
        {
            return (int)U.atoi(Program.Config.at("func_rom_extends_option", "1"));
        }

        public static int alloc_program_area_option()
        {
            return (int)U.atoi(Program.Config.at("func_alloc_program_area_option", "0"));
        }

        public static int auto_update()
        {
            return (int)U.atoi(Program.Config.at("func_auto_update", "3"));
        }


        public enum auto_backup_enum
        {
             NO = 0
           , YES = 1
           , YES_7Z = 2
        };
        public static auto_backup_enum auto_backup()
        {
             return (auto_backup_enum)U.atoi(Program.Config.at("func_auto_backup", "2"));
        }

        public enum first_form_enum
        {
             EASY = 0
           , DETAIL = 1
        };
        public static first_form_enum first_form()
        {
             return (first_form_enum)U.atoi(Program.Config.at("func_first_form", "0"));
        }

        public enum text_escape_enum
        {
             ProjectFEGBA = 0
           , FEditorAdv = 1
           , NoCache = 0xFF
        };
        static text_escape_enum g_Cache_write_text_escape_enum = text_escape_enum.NoCache;
        public static text_escape_enum text_escape()
        {
            if (g_Cache_write_text_escape_enum == text_escape_enum.NoCache)
            {
                g_Cache_write_text_escape_enum = text_escape_low();
            }
            return g_Cache_write_text_escape_enum;
        }
        static text_escape_enum text_escape_low()
        {
            if ( Program.Config.ContainsKey("func_text_escape") )
            {
                return (text_escape_enum)U.atoi(Program.Config.at("func_text_escape", "1"));
            }
            string lang = OptionForm.lang();
            if (lang == "ja" || lang == "zh")
            {//日本語と中国語版は、ProjectFEGBAをディフォルトにする.
                return text_escape_enum.ProjectFEGBA;
            }
            else
            {//英語版は、FEEditorAdv形式をディフォルトにする.
                return text_escape_enum.FEditorAdv;
            }
        }

        public static int write_notify_time()
        {
            return (int)U.atoi(Program.Config.at("func_write_notify_time", "2"));
        }


        public enum write_out_of_range_enum
        {
            NoWarning = 0
          , Warning = 1
          , Deny = 2
        };
        public static write_out_of_range_enum write_out_of_range()
        {
            return (write_out_of_range_enum)U.atoi(Program.Config.at("func_write_out_of_range", "1")); 
        }

        public enum write_low_address_enum
        {
            NoWarning = 0
          , Warning = 1
          , Deny = 2
        };
        public static write_low_address_enum write_low_address()
        {
            return (write_low_address_enum)U.atoi(Program.Config.at("func_write_low_address", "2")); 
        }

        public enum write_00_enum
        {
            NoWarning = 0
          , Warning = 1
          , Deny = 2
        };
        public static write_00_enum write_00()
        {
            return (write_00_enum)U.atoi(Program.Config.at("func_write_00", "2")); 
        }
        

        public enum select_in_explorer_when_export_enum
        {
            None = 0
          , Open = 1
        };
        public static select_in_explorer_when_export_enum select_in_explorer_when_export()
        {
            return (select_in_explorer_when_export_enum)U.atoi(Program.Config.at("func_select_in_explorer_when_export", "1"));
        }

        public enum lookup_feditor_enum
        {
            None = 0
          , Lookup = 1
        };
        public static lookup_feditor_enum lookup_feditor()
        {
            return (lookup_feditor_enum)U.atoi(Program.Config.at("func_lookup_feditor", "0")); 
        }

        public enum lint_text_skip_bug_enum
        {
            None = 0
          , MoreThan4Lines = 1
          , DetectButExceptForVanilla = 2
          , Detect = 3
          , NoCache = 0xFF
        };
        static lint_text_skip_bug_enum g_Cache_lint_text_skip_bug_enum = lint_text_skip_bug_enum.NoCache;
        public static lint_text_skip_bug_enum lint_text_skip_bug()
        {
            if (g_Cache_lint_text_skip_bug_enum == lint_text_skip_bug_enum.NoCache)
            {
                g_Cache_lint_text_skip_bug_enum = (lint_text_skip_bug_enum)U.atoi(Program.Config.at("func_lint_text_skip_bug", "2"));
            }
            return g_Cache_lint_text_skip_bug_enum;
        }

        public enum show_extends_enum
        {
            None = 0
          , Show = 1
        };
        public static show_extends_enum show_class_extends()
        {
            return (show_extends_enum)U.atoi(Program.Config.at("func_show_class_extends", "0")); 
        }

        public static show_extends_enum show_chapter_extends()
        {
            return (show_extends_enum)U.atoi(Program.Config.at("func_show_chapter_extends", "0")); 
        }
        public static show_extends_enum show_worldmap_node_extends()
        {
            return (show_extends_enum)U.atoi(Program.Config.at("func_show_worldmap_node_extends", "0"));
        }
        public static show_extends_enum show_worldmap_path_extends()
        {
            return (show_extends_enum)U.atoi(Program.Config.at("func_show_worldmap_path_extends", "0"));
        }
        public static show_extends_enum show_fe76_item_icon_extends()
        {
            return (show_extends_enum)U.atoi(Program.Config.at("func_show_fe76_item_icon_extends", "0"));
        }
        public static show_extends_enum show_song_table_extends()
        {
            return (show_extends_enum)U.atoi(Program.Config.at("func_show_song_table_extends", "0"));
        }
        public static show_extends_enum show_gameoption_extends()
        {
            return (show_extends_enum)U.atoi(Program.Config.at("func_show_gameoption_extends", "0"));
        }
        

        public enum texteditor_auto_convert_space_enum
        {
            None = 0
          , AutoConvertByLang = 1
        };
        public static texteditor_auto_convert_space_enum texteditor_auto_convert_space()
        {
            return (texteditor_auto_convert_space_enum)U.atoi(Program.Config.at("func_texteditor_auto_convert_space", "1"));
        }

        public enum auto_connect_emulator_enum
        {
            None = 0
          , AutoConnect = 1
          , AutoConnectAndAutoClose = 2
        };
        public static auto_connect_emulator_enum auto_connect_emulator()
        {
            return (auto_connect_emulator_enum)U.atoi(Program.Config.at("func_auto_connect_emulator", "2"));
        }
        
        public enum midi_importer_enum
        {
            FEBuilderGBA = 0
          , MID2AGB = 1
        };
        public static midi_importer_enum midi_importer()
        {
            return (midi_importer_enum)U.atoi(Program.Config.at("func_midi_importer", "0"));
        }


        public enum proxy_server_when_connecting_enum
        {
            None = 0
          , AutoProxy = 1 //自動でプロキシサーバを見つける
        };
        public static proxy_server_when_connecting_enum proxy_server_when_connecting()
        {
            return (proxy_server_when_connecting_enum)U.atoi(Program.Config.at("func_proxy_server_when_connecting", "1"));
        }

        public static int notify_upper_time()
        {
            return (int)U.atoi(Program.Config.at("func_notify_upper_time", "3"));
        }



        public enum create_nodoll_gba_sym_enum
        {
            None = 0
          , GenSYM = 1 //自動でSYMを作る.
        };
        public static create_nodoll_gba_sym_enum create_nodoll_gba_sym()
        {
            return (create_nodoll_gba_sym_enum)U.atoi(Program.Config.at("func_create_nodoll_gba_sym", "1"));
        }

        public enum overraide_simple_error_check_enum
        {
            None = 0
          , SimpleAlert = 1
          , AllCheck = 9
        };
        public static overraide_simple_error_check_enum overraide_simple_error_check()
        {
            return (overraide_simple_error_check_enum)U.atoi(Program.Config.at("func_overraide_simple_error_check", "1"));
        }

        public enum skillsystems_sanctuary_enum
        {
            Always = 0
          , IfSkillSystemsInstalled = 1
          , None = 2
          , NoCache = 0xff
        };
        static skillsystems_sanctuary_enum g_skillsystems_sanctuary = skillsystems_sanctuary_enum.NoCache;
        public static skillsystems_sanctuary_enum skillsystems_sanctuary()
        {
            if (g_skillsystems_sanctuary == skillsystems_sanctuary_enum.NoCache)
            {
                g_skillsystems_sanctuary = (skillsystems_sanctuary_enum)U.atoi(Program.Config.at("func_skillsystems_sanctuary", "0"));
            }
            return g_skillsystems_sanctuary;
        }

        public enum func_portrait_lz77_enum
        {
            Vanilla = 0
          , AlwaysL77Compress = 1
        };
        public static func_portrait_lz77_enum portrait_lz77()
        {
            return (func_portrait_lz77_enum)U.atoi(Program.Config.at("func_portrait_lz77", "0"));
        }

        public static bool felint_haxs()
        {
            return U.stringbool(Program.Config.at("func_felint_haxs", "1"));
        }
        public static bool felint_support_validation()
        {//支援の妥当性を検証するかどうか
            uint a = U.atoi(Program.Config.at("func_felint_support_validation", "0"));
            return (a == 1);
        }
        public static bool felint_support_system_off()
        {//支援システムそのものを利用しないかどうか
            uint a = U.atoi(Program.Config.at("func_felint_support_validation", "0"));
            return (a == 2);
        }
        public static bool felint_check_melee_item_motionid()
        {
            return U.stringbool(Program.Config.at("func_felint_check_melee_item_motionid", "0"));
        }
        public static bool felint_check_text_width()
        {
            return U.stringbool(Program.Config.at("func_felint_check_seriftext_width", "1"));
        }
        public static bool felint_check_exist_tilechange()
        {
            return U.stringbool(Program.Config.at("func_felint_check_exist_tilechange", "1"));
        }
        public static bool felint_check_song_tie_eot()
        {
            return U.stringbool(Program.Config.at("func_felint_check_song_tie_eot", "1"));
        }

        public static int alert_unk_event_code()
        {
            return (int)U.atoi(Program.Config.at("func_alert_unk_event_code", "1"));
        }
        


        static string g_Cache_lang = null;
        public static string lang()
        {
            if (g_Cache_lang == null)
            {
                g_Cache_lang = lang_low();
            }
            return g_Cache_lang;
        }
        static string lang_low()
        {
            string l =  Program.Config.at("func_lang", "auto");
            if (l == "ja" || l == "en" || l == "zh")
            {//ディフォルト対応の言語
                return l;
            }
            if (l == "auto")
            {
                string system_lang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

                string path = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate",system_lang + ".txt");
                if (File.Exists(path))
                {//翻訳ファイルがあるので利用可能.
                    return system_lang;
                }
                else if (system_lang == "ja")
                {//日本語
                    return "ja";
                }
            }
            //不明な場合は、とりあえず、英語とする. 日本語にしたいが、英語の方が困る人が少ないだろう.
            return "en";
        }
        public enum textencoding_enum
        {
            Auto = 0
          , LAT1 = 1
          , Shift_JIS = 2
          , UTF8 = 3
          , ZH_TBL = 4
          , EN_TBL = 5
          , AR_TBL = 6
          , KR_TBL = 7
          , NoChace = 99
        };
        static textencoding_enum g_Cache_textencoding;
        public static textencoding_enum textencoding()
        {
            if (g_Cache_textencoding == textencoding_enum.NoChace)
            {
                g_Cache_textencoding = (textencoding_enum)U.atoi(Program.Config.at("func_textencoding", "0"));
            }
            return g_Cache_textencoding;
        }

        private void Color_List_SelectedColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_List_SelectedColor_button.BackColor = Color.FromArgb(51, 164, 255);
        }

        private void Color_List_HoverColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_List_HoverColor_button.BackColor = Color.FromName("WhiteSmoke");
        }

        private void Color_List_SelectedColor_button_Click(object sender, EventArgs e)
        {
            Color_List_SelectedColor_button.BackColor = SelectColorDialog(Color_List_SelectedColor_button.BackColor);
        }

        private void Color_List_HoverColor_button_Click(object sender, EventArgs e)
        {
            Color_List_HoverColor_button.BackColor = SelectColorDialog(Color_List_HoverColor_button.BackColor);
        }

        private void ColorSetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int colorset = ColorSetComboBox.SelectedIndex;
            if (colorset == 1)
            {//white
                Color_Control_BackColor_button.BackColor = U.ColorFromName("Control");
                Color_Control_ForeColor_button.BackColor = U.ColorFromName("ControlText");
                Color_Input_BackColor_button.BackColor = U.ColorFromName("Window");
                Color_Input_ForeColor_button.BackColor = U.ColorFromName("WindowText");
                Color_InputDecimal_BackColor_button.BackColor = U.ColorFromName("Window");
                Color_InputDecimal_ForeColor_button.BackColor = U.ColorFromName("OrangeRed");
                Color_NotifyWrite_BackColor_button.BackColor = U.ColorFromName("Yellow");
                Color_NotifyWrite_ForeColor_button.BackColor = U.ColorFromName("Black");
                Color_Error_BackColor_button.BackColor = U.ColorFromName("Control");
                Color_Error_ForeColor_button.BackColor = U.ColorFromName("Red");
                Color_List_SelectedColor_button.BackColor = Color.FromArgb(51, 164, 255);
                Color_List_HoverColor_button.BackColor = U.ColorFromName("WhiteSmoke");
                Color_Keyword_BackColor_button.BackColor = U.ColorFromName("Window");
                Color_Keyword_ForeColor_button.BackColor = U.ColorFromName("Blue");
                Color_Comment_ForeColor_button.BackColor = U.ColorFromName("DarkGreen");
                Color_ControlComment_ForeColor_button.BackColor = U.ColorFromName("Green");
                Color_List_RelatedLine_BackColor_button.BackColor = U.ColorFromName("ffffe1ff");

                return ;
            }
            if (colorset == 2)
            {//black
                Color_Control_BackColor_button.BackColor = U.ColorFromName("Black");
                Color_Control_ForeColor_button.BackColor = U.ColorFromName("White");
                Color_Input_BackColor_button.BackColor = U.ColorFromName("Black");
                Color_Input_ForeColor_button.BackColor = U.ColorFromName("White");
                Color_InputDecimal_BackColor_button.BackColor = U.ColorFromName("Black");
                Color_InputDecimal_ForeColor_button.BackColor = U.ColorFromName("Pink");
                Color_NotifyWrite_BackColor_button.BackColor = U.ColorFromName("Yellow");
                Color_NotifyWrite_ForeColor_button.BackColor = U.ColorFromName("Black");
                Color_Error_BackColor_button.BackColor = U.ColorFromName("Black");
                Color_Error_ForeColor_button.BackColor = U.ColorFromName("Red");
                Color_List_SelectedColor_button.BackColor = U.ColorFromName("Highlight");
                Color_List_HoverColor_button.BackColor = U.ColorFromName("ff0080c0");
                Color_Keyword_BackColor_button.BackColor = U.ColorFromName("Black");
                Color_Keyword_ForeColor_button.BackColor = U.ColorFromName("LightBlue");
                Color_Comment_ForeColor_button.BackColor = U.ColorFromName("DeepPink");
                Color_ControlComment_ForeColor_button.BackColor = U.ColorFromName("GreenYellow");
                Color_List_RelatedLine_BackColor_button.BackColor = U.ColorFromName("ff295454");

                return;
            }
            if (colorset == 3)
            {//black2
                Color_Control_BackColor_button.BackColor = U.ColorFromName("ff2c2f33");
                Color_Control_ForeColor_button.BackColor = U.ColorFromName("White");
                Color_Input_BackColor_button.BackColor = U.ColorFromName("ff282828");
                Color_Input_ForeColor_button.BackColor = U.ColorFromName("White");
                Color_InputDecimal_BackColor_button.BackColor = U.ColorFromName("Black");
                Color_InputDecimal_ForeColor_button.BackColor = U.ColorFromName("Pink");
                Color_NotifyWrite_BackColor_button.BackColor = U.ColorFromName("Yellow");
                Color_NotifyWrite_ForeColor_button.BackColor = U.ColorFromName("Black");
                Color_Error_BackColor_button.BackColor = U.ColorFromName("Black");
                Color_Error_ForeColor_button.BackColor = U.ColorFromName("Red");
                Color_List_SelectedColor_button.BackColor = U.ColorFromName("Highlight");
                Color_List_HoverColor_button.BackColor = U.ColorFromName("ff3c3c3c");
                Color_Keyword_BackColor_button.BackColor = U.ColorFromName("ff0000a0");
                Color_Keyword_ForeColor_button.BackColor = U.ColorFromName("ff8080ff");
                Color_Comment_ForeColor_button.BackColor = U.ColorFromName("ff0080c0");
                Color_ControlComment_ForeColor_button.BackColor = U.ColorFromName("ff0080c0");
                Color_List_RelatedLine_BackColor_button.BackColor = U.ColorFromName("ff295454");

                return;
            }
        }
        

        private void KeyFinder_KeyDown(object sender, KeyEventArgs e)
        {
            string keyname = "";
            if (e.Control)
            {
                keyname = "Ctrl+";
            }
            if (e.Alt)
            {
                keyname = "Alt+";
            }
            if (e.Shift)
            {
                keyname = "Shift+";
            }
            keyname += e.KeyCode.ToString();
            KeyFinder.Text = keyname;
        }

        private void event_assembler_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("Core.exe|Core.exe|");
            if (r != "")
            {
                event_assembler.Text = r;
            }
            CheckEAVersion();
        }
        void CheckEAVersion()
        {
            ERROR_IsOldEA.Visible = IsOldEA(event_assembler.Text);
        }

        public struct TBLTableSt
        {
            public string name;
            public uint ver;
            public uint enc;
            public uint pointer;
            public byte[] data;
        };
        public static void AutoUpdateTBLOption()
        {
            //地形:平地 の名前の付け方でtblを判定します.
            //平地は普通　変更しないだろうから
            //NAME	VER	enc	POINTER VALUE
            TBLTableSt[] table = new TBLTableSt[]{
                new TBLTableSt{name = "FE6U" ,ver = 6,enc = 5,pointer = 0x60D010,data = new byte[]{0x82, 0xBD, 0x82 ,0xE6}},
                new TBLTableSt{name = "FE6CN",ver = 6,enc = 4,pointer = 0x60D010,data = new byte[]{0x8C ,0xE2 ,0x85 ,0xE1}},
                new TBLTableSt{name = "FE7CN",ver = 7,enc = 4,pointer = 0xC542F4,data = new byte[]{0x8D ,0xFC ,0x86 ,0x87}},
                new TBLTableSt{name = "FE7CN",ver = 7,enc = 4,pointer = 0xC542F4,data = new byte[]{0x8D ,0xFB ,0x86 ,0x86}},
                new TBLTableSt{name = "FE8CN",ver = 8,enc = 4,pointer = 0x8617CC,data = new byte[]{0x8D ,0xBD ,0x85 ,0xF9}},
                new TBLTableSt{name = "FE8KR",ver = 8,enc = 7,pointer = 0x8617CC,data = new byte[]{0x9D ,0xD2 ,0x9B ,0x6B}},
            }; 

            uint version = (uint)Program.ROM.RomInfo.version;
            uint tbl_index = 0;

            foreach (TBLTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス 地形:平地
                uint start = Program.ROM.p32(t.pointer);
                if (!U.isSafetyOffset(start))
                {
                    continue;
                }

                byte[] data = Program.ROM.getBinaryData(start, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }

                tbl_index = t.enc;
                break;
            }
            uint now_tbl = U.atoi(Program.Config.at("func_textencoding"));
            if (now_tbl != tbl_index)
            {
                Program.Config["func_textencoding"] = tbl_index.ToString();
            }
        }

        private void emulator_DoubleClick(object sender, EventArgs e)
        {
            this.emulator_button.PerformClick();
        }

        private void emulator2_DoubleClick(object sender, EventArgs e)
        {
            this.emulator2_button.PerformClick();
        }

        private void sappy_DoubleClick(object sender, EventArgs e)
        {
            this.sappy_button.PerformClick();
        }

        private void program1_DoubleClick(object sender, EventArgs e)
        {
            this.program1_button.PerformClick();
        }

        private void program2_DoubleClick(object sender, EventArgs e)
        {
            this.program2_button.PerformClick();
        }

        private void program3_DoubleClick(object sender, EventArgs e)
        {
            this.program3_button.PerformClick();
        }

        private void devkitpro_eabi_DoubleClick(object sender, EventArgs e)
        {
            this.devkitpro_eabi_button.PerformClick();
        }

        private void goldroad_asm_DoubleClick(object sender, EventArgs e)
        {
            this.goldroad_asm_button.PerformClick();
        }

        private void event_assembler_DoubleClick(object sender, EventArgs e)
        {
            this.event_assembler_button.PerformClick();
        }

        private void Color_Keyword_BackColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_Keyword_BackColor_button.BackColor = Color.FromName("Window");
        }

        private void Color_Keyword_ForeColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_Keyword_ForeColor_button.BackColor = Color.FromName("Blue");
        }

        private void Color_Keyword_BackColor_button_Click(object sender, EventArgs e)
        {
            Color_Keyword_BackColor_button.BackColor = SelectColorDialog(Color_Keyword_BackColor_button.BackColor);
        }

        private void Color_Keyword_ForeColor_button_Click(object sender, EventArgs e)
        {
            Color_Keyword_ForeColor_button.BackColor = SelectColorDialog(Color_Keyword_ForeColor_button.BackColor);
        }

        private void mid2agb_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("mid2agb,midi2agb|mid2agb.exe;midi2agb.exe|");
            if (r != "")
            {
                mid2agb.Text = r;
                mid2agb_default.Checked = true;
            }
        }

        private void mid2agb_DoubleClick(object sender, EventArgs e)
        {
            mid2agb_button.PerformClick();
        }

        private void mid2agb_TextChanged(object sender, EventArgs e)
        {
            if (this.mid2agb.Text.Length > 0)
            {
                mid2agb_default.Show();
            }
            else
            {
                mid2agb_default.Hide();
                mid2agb_default.Checked = false;
            }
        }

        private void Color_Comment_ForeColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_Comment_ForeColor_button.BackColor = Color.FromName("DarkGreen");
        }

        private void Color_Comment_ForeColor_button_Click(object sender, EventArgs e)
        {
            Color_Comment_ForeColor_button.BackColor = SelectColorDialog(Color_Comment_ForeColor_button.BackColor);
        }
        private void X_EXPLAIN_NECESSARY_PROGRAM_Click(object sender, EventArgs e)
        {
            this.Close();
            MainFormUtil.RunToolInitWizard();
        }

        //利用しているエミュレータの種類を知りたいので、名前を取得する
        public static string GetEmulatorName()
        {
            //エミュレータの名前
            string emu = Program.Config.at("emulator");
            if (emu == "" || !File.Exists(emu))
            {
                return "";
            }

            return Path.GetFileNameWithoutExtension(emu);
        }
        //利用しているエミュレータの種類を知りたいので、名前を取得する
        public static string GetEmulatorVersion()
        {
            //エミュレータの名前
            string emu = Program.Config.at("emulator");
            if (emu == "" || !File.Exists(emu))
            {
                return "";
            }

            string emulator = Path.GetFileNameWithoutExtension(emu);
            if (emulator == "mGBA")
            {//mGBAだとバージョン番号をちゃんととれます
                try
                {
                    System.Diagnostics.FileVersionInfo vi =
                        System.Diagnostics.FileVersionInfo.GetVersionInfo(emu);

                    //ファイル名 + バージョン番号
                    return vi.FileVersion;
                }
                catch (Exception)
                {
                }
            }

            {//それ以外は取れないので、日付を見ましょう
                return File.GetLastWriteTime(emu).ToString("yyyyMMdd");
            }
        }

        public static string GetCFLAGS()
        {
            return Program.Config.at("CFLAGS", "-c -mthumb -O2");
        }

        public static string GetRetDec()
        {
            return Program.Config.at("retdec", "");
        }
        public static string GetPython3()
        {
            return Program.Config.at("python3", "");
        }
        public static string GetGBAMusRiper()
        {
            return Program.Config.at("gba_mus_riper", "");
        }
        public static string GetSox()
        {
            return Program.Config.at("sox", "");
        }
        public static string GetFECLIB()
        {
            return Program.Config.at("FECLIB", "");
        }
        public static string GetSrccodeTexteditor()
        {
            return Program.Config.at("srccode_texteditor", "");
        }
        public static string GetSrccodeDirectory()
        {
            return Program.Config.at("srccode_directory", Program.BaseDirectory);
        }

        public static bool IsKanaToNumberMode()
        {
            return U.stringbool(Program.Config.at("IsKanaToNumberMode", "false"));
        }

        void MakeExplainFunctions()
        {
            explain_func_rom_extends.AccessibleDescription = R._("可変長データを更新した時に、元のデータより長くなってしまったときに、\r\n自動的にリポイントするかを決定します。");
            explain_func_rom_extends_option.AccessibleDescription = R._("ROMを拡張するときに、どのアドレスから拡張を開始するかを決定します。");
            explain_func_alloc_program_area_option.AccessibleDescription = R._("プログラムを設置するアドレスを決定します。\r\n上位にしておくとリビルドに成功しやすいです。\r\n通常のアドレスにすると、画像とかを配置する拡張領域に割り当てます。");
            explain_func_auto_backup.AccessibleDescription = R._("ROMを上書きした時に自動的にバックアップを取るかを決定します。\r\n必ずバックアップを取ることをお勧めします。");
            explain_func_first_form.AccessibleDescription = R._("ROMを開いたとの最初の画面を決定します。\r\nディフォルトは、マップを表示する簡易メニューです。\r\n");
            explain_func_auto_update.AccessibleDescription = R._("このソフトウェアを自動的に更新する更新間隔を設定します。");
            explain_func_text_escape.AccessibleDescription = R._("テキストに利用するエスケープシーケンスをどうするかを決定します。\r\nProjectFEGBA形式(@0003)、\r\nFEditor形式[A]、\r\nどちらを利用するかを決定します");
            explain_func_write_notify_time.AccessibleDescription = R._("データを書き込んだときに、書き込み増したという通知を画面下に出しますが、\r\nあの表示の表示時間を決定します");
            explain_func_write_out_of_range.AccessibleDescription = R._("範囲外に書き込みそうになった時に、\r\n自動的に拒否するか、警告を出すか、何もしないかを決定します。");
            explain_func_write_low_address.AccessibleDescription = R._("プログラムコードが格納されている下位アドレスに書き込みそうになった時に、\r\n自動的に拒否するか、警告を出すか、何もしないかを決定します。");
            explain_func_write_00.AccessibleDescription = R._("TextID 0x00や、ClassID 0x00などの変えてはいけない 0x00へ書き込みをしそうになった時に、\r\n自動的に拒否するか、警告を出すか、何もしないかを決定します。");
            explain_func_select_in_explorer_when_export.AccessibleDescription = R._("ファイルをエクスポートした後に、\r\nそのファイルがある場所をエクスプローラで開いて選択するかどうかを決定します");
            explain_func_lookup_feditor.AccessibleDescription = R._("既存の改造ROMを開くときの設定です。\r\n通常はオフにしてください。\r\n他の改造ツールで作られた既存ROMを開くときだけ利用してください。\r\n\r\nFEditorは、アドレスの枠外(開始アドレス-4)にデータの個数を書き込みます。\r\nこの値を参考にするかどうかを決定します。");
            explain_func_lint_text_skip_bug.AccessibleDescription = R._("Lintが改行などが正しくないテキストもバグとして報告するかどうかを決定します。\r\n例えば、セリフは2行しか利用できませんが、そこに3行目を入れた場合、バグとして検出します。\r\n");
            explain_func_show_class_extends.AccessibleDescription = R._("クラス拡張ボタンを表示するかどうかを決定します。\r\nクラス拡張はさまざまな問題を引き起こしますので、ディフォルトは非表示になっています。\r\n私は、クラスを拡張しないことをお勧めします。FEには利用されていないクラスがたくさんあります。\r\n危険を伴うクラスの拡張ではなく、利用されていないクラスを再利用するべきです。");
            explain_func_texteditor_auto_convert_space.AccessibleDescription = R._("マルチバイト圏用の設定です。シングルバイト圏では利用しません。\r\n半角スペースで入力されたテキストを倍角スペースに自動変換するかどうかを設定します。\r\nマルチバイト圏では、マルチバイト用のスペースが存在するためです。\r\n");
            explain_func_auto_connect_emulator.AccessibleDescription = R._("F5で実行しているエミュレータからイベントやProcsの値を取り出して画面に表示するかかどうかを設定します。\r\nエミュレータから値を取得すると、イベントのデバッグがとても楽になります。\r\n");
            explain_func_proxy_server_when_connecting.AccessibleDescription = R._("ネット接続にプロキシサーバを利用するか選択します。\r\nプロキシサーバの自動プロキシ検出にすると、IEのプロキシサーバの設定を利用します。\r\nただし、自動検出には少し時間がかかります。\r\n");
            explain_func_lang.AccessibleDescription = R._("FEBuilderGBAの画面の表示に利用する言語を切り替えます。\r\n\r\n翻訳データは、「config/data/func_lang_ALL.*.txt」と「config/translate」です。\r\nあなたの国の言語で表示させたい場合、翻訳データを送ってください。\r\nお待ちしています。\r\n");
            explain_func_textencoding.AccessibleDescription = R._("ゲーム内で利用する言語を設定します。\r\nFE6の英語版、及び中国語版は、日本語版をtblデータを利用して翻訳したファンサブです。\r\nこれらを利用する場合に設定します。\r\n(現在は、これらのROMを開くと、自動的に適用されるので、手動で変更しなくても問題ありません。)\r\n");
            explain_func_notify_upper_time.AccessibleDescription = R._("参照リンクから飛んだ場合に、\r\n画面の上に、「このリストをダブルクリックで選択できます」というメッセージの通知を行う時間を設定します。\r\n");
            explain_func_create_nodoll_gba_sym.AccessibleDescription = R._("no$gba debuggerを起動したときに、現在のROMのSYMを自動的に作るかどうかを設定します。\r\n自動的に作る場合は、ROMが変更されていれば、SYMを作成します。\r\nSYMの作成には少し時間がかかります。");
            explain_func_overraide_simple_error_check.AccessibleDescription = R._("上書き時に、簡易メニュー画面にエラーが表示されている場合、警告を出すようにします。\r\nすべての章を確認するエラーチェック違い、システムエラーと現在の章のエラーしかチェックできませんが、高速に動作します。");
            explain_func_alert_unk_event_code.AccessibleDescription = R._("イベントを解析したときに不明な命令が連続していたら、壊れていると警告を出します。\r\n数字を下げると、壊れたイベントを検知しやすくなりますが、未知の命令を誤認する可能性もあります。\r\nFE8とFE6はほぼすべての命令を解析されているので厳し目の値にしてもいい。\r\nFE7はまだ未知の命令があるので下げ過ぎると誤認識があります。\r\n");
            X_EXPLAIN_VBA_M.AccessibleDescription = R._("エミュレータを設定すると、F5キーでテスト実行できるようになります。\r\nまた、エミュレータのRAM領域にアクセスしてデータを表示できます。\r\nVBA-M(sourceforge),mGBA,no$gba等に対応しています。\r\n\r\nただし、VBA-M(github)のRAM領域へは接続できません。");
            X_EXPLAIN_NODOLL_GBA_DEBUGGER.AccessibleDescription = R._("Ctrl+F5を押したときに起動するデバッガーを設定します。\r\nFEBuilderGBAは、no$gba debuggerへ、セーブデータの転送と、シンボルの自動生成を行うことができます。");
            X_EXPLAIN_SAPPY.AccessibleDescription = R._("音楽プレイヤを設定すると、音符ボタンをクリックした時に、BGMを再生できます。\r\nsappyまたはVGMusicStudioに対応しています。\r\nsappyをインストールするときは、マニュアルに書いてあるインストーラーを利用すると便利です。\r\nVGMusicStudioは起動オプションに対応しているバージョンを利用してください。");
            X_EXPLAIN_EA.AccessibleDescription = R._("FEBuilderGBAでは、EA形式で書かれていパッチをインストール時に利用します。\r\nEA Ver11以上をインストールしてください。");
            explain_func_show_chapter_extends.AccessibleDescription = R._("章テーブルを拡張するボタンを表示します。\r\n章を拡張するときは、PLISTの構造について理解している必要があります。");
            explain_func_show_worldmap_node_extends.AccessibleDescription = R._("FE8のワールドマップ拠点にも拡張ボタンを表示します。\r\nFE8の拠点を増やすにセーブデータの改造が必要だと言われています。\r\n詳細はまだよくわかっていません。\r\n研究のため以外には利用しないでください。");
            explain_func_show_worldmap_path_extends.AccessibleDescription = R._("FE8のワールドマップ道にも拡張ボタンを表示します。\r\nFE8の道を増やすにセーブデータの改造が必要だと言われています。\r\n詳細はまだよくわかっていません。\r\n研究のため以外には利用しないでください。");
            explain_func_show_fe76_item_icon_extends.AccessibleDescription = R._("FE7とFE6のアイテムアイコンにも拡張ボタンを表示します。\r\nアイテムアイコンを拡張するには、パッチが必要です。\r\nFE8のパッチは存在しますが、FE7とFE6用には不安定なものしか存在しません。");
            explain_func_show_song_table_extends.AccessibleDescription = R._("SongTableを拡張するボタンを表示します。\r\nSongTableをrepointするとsapplyでは認識できなくなります。\r\nSongTableには空き領域がたくさんあるので拡張する必要はほとんどありません。");
            explain_func_show_gameoption_extends.AccessibleDescription = R._("ゲームオプションを拡張するボタンを表示します。\r\nGameOptionの追加にはASMが必要になります。\r\n既存のオプションを消すだけなら「ゲームオプションの順番」から変更可能です。\r\n拡張するデメリットの方が多いので非表示になっています。");
            X_EXPLAIN_DECOMPILER.AccessibleDescription = R._("逆コンパイラを指定します。\r\nただ、あまり期待しない方がいいですよ。\r\n");
            X_EXPLAIN_CLANG.AccessibleDescription = R._("C言語を利用してパッチを作る場合に使用する項目を設定します。\r\n\r\nCFLAGSは、gccに渡すオプションを指定します。\r\nたいていの場合、ディフォルト値から変更する必要は特にありません。\r\n\r\nFE-CLibには、ヘッダーファイルのライブラリを渡すします。\r\n現在は、FE8Uバージョンのみが開発されています。\r\n");
            X_EXPLAIN_COMPILER.AccessibleDescription = R._("ASMやC言語をマシン語バイナリに変換するコンパイラー(アセンブラ)を設定します。\r\ndevkitProは、現在よく使われている処理系です。\r\n言語系は、gnu asmです。\r\ndevkitProを公式サイトからダウンロードして、インストールすると、gccも利用できます。\r\n\r\ngoldroadは、昔使われていた処理系です。\r\n言語系は、MASMに似ています。\r\n\r\n\r\nFEBuilderGBAは両者をソースコードにより自動的に切り替えて利用します。");
            X_EXPLAIN_PROGRAMN.AccessibleDescription = R._("FEBuilderGBAから動作させたいプログラムがあれば指定してください。\r\nたいていの場合は、特に設定する必要はありません。");
            X_EXPLAIN_MUSICTOOL.AccessibleDescription = R._("音楽を変換するツールを設定します。\r\ngba_mus_riperは、midiへエクスポートするためと、soundfontをエクスポートするために利用されます。\r\nsoxは、wavをインポートするときの周波数や余白の削除等に利用されます。\r\n");
            X_EXPLAIN_MID2AGB.AccessibleDescription = R._("Midiをインポートする時に、mid2agbを利用するならば設定してください。\r\nたいていの場合、mid2agbを利用して変換した方がよりよい結果を得ることができます。");
            X_EXPLAIN_SRCCODE.AccessibleDescription = R._("バニラのソースコードを簡単に確認するために利用する項目です。\r\nディフォルトではF10キーを押すとバニラのコードにアクセスできます。\r\nバニラのソースコードは数十万行になるので、テキストエディターには、大きなファイルを表示できるテキストエディタを指定してください。");
            explain_func_skillsystems_sanctuary.AccessibleDescription = R._("スキル拡張パッチをインストールする領域を予約し保護するかどうかを設定します。\r\n「常に」(ディフォルト)は、その領域には一切干渉しないようにして保護します。\r\n「インストールされていれば保護」は、スキル拡張がインストールされていれば保護します。後からスキル拡張がインストールされる可能性もあるので推奨はしません。\r\n「保護しない」は、一切保護しません。");
            explain_func_portrait_lz77.AccessibleDescription = R._("顔画像を圧縮するかどうかを設定します。GBAFEはバージョンによって無圧縮であることがあります。圧縮すると約半分の容量になります。\r\nディフォルトはバニラの設定に従います。");
            explain_func_felint_haxs.AccessibleDescription = R._("投げ斧の戦闘アニメモーションが登録されていない場合、警告を出します。");
            func_felint_support_validation.AccessibleDescription = R._("支援が片思いなどの正しくない場合、警告を出します。\r\n支援は相手から見ても同じ値である必要があります。");
            func_felint_check_melee_item_motionid.AccessibleDescription = R._("アイテムの近接モーションの0xFFFFが設定されているかを確認します。\r\n設定しなくてもおそらく問題ないのですが、行儀がよくないのでちゃんと設定することをお勧めします。");
            func_felint_check_seriftext_width.AccessibleDescription = R._("会話文の長さをチェックします。\r\n何かの理由でチェックしたくない場合はOFFにできます。\r\n会話文が長すぎるとゲームで正しく表示されません。");
            func_felint_check_exist_tilechange.AccessibleDescription = R._("街や宝箱、古木、壊れる壁の位置にタイル変化が設定されているかチェックします。\r\nタイル変化の設定を忘れると、ゲームがハングアップすることがあります。");
            func_felint_check_song_tie_eot.AccessibleDescription = R._("音楽のTIEとEOTの妥当性を確認します。\r\nマップで利用する音楽の場合、EOTを忘れると、音が残ったままになってしまいます。\r\nこの機能はそれを地頭的に検出し警告を出します。");
        }

        private void Color_ControlComment_ForeColor_button_Click(object sender, EventArgs e)
        {
            Color_ControlComment_ForeColor_button.BackColor = Color.FromName("Green");
        }


        private void retdec_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("retdec-decompiler.py|retdec-decompiler.py|");
            if (r != "")
            {
                retdec.Text = r;
            }
        }

        private void python3_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("python|python.exe;python3.exe|");
            if (r != "")
            {
                python3.Text = r;
            }
        }

        private void retdec_DoubleClick(object sender, EventArgs e)
        {
            retdec_button.PerformClick();
        }

        private void python3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            python3_button.PerformClick();
        }

        const double CurrentEAVersion = 11.1;
        public bool IsOldEA()
        {
            return IsOldEA( Program.Config.at("event_assembler") );
        }
        public bool IsOldEA(string EAFilename)
        {
            if (! File.Exists(EAFilename))
            {//ファイルがないので判別不可能
                return false;
            }
            if (MainFormUtil.IsColorzCore(EAFilename))
            {//colorzcoreなので判別不可能
                return false;
            }
            double version = U.GetOtherProgramVersion(EAFilename);
            if (version < CurrentEAVersion)
            {//古い
                return true;
            }
            //新しいバージョン
            return false;
        }

        private void gba_mus_riper_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("song_riper|song_riper.exe|");
            if (r != "")
            {
                gba_mus_riper.Text = r;
            }
        }

        private void gba_mus_riper_DoubleClick(object sender, EventArgs e)
        {
            gba_mus_riper_button.PerformClick();
        }

        private void sox_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("sox|sox.exe|");
            if (r != "")
            {
                sox.Text = r;
            }
        }

        private void sox_DoubleClick(object sender, EventArgs e)
        {
            sox_button.PerformClick();
        }

        private void FECLIB_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("gbafe.h|gbafe.h|");
            if (r != "")
            {
                FECLIB.Text = r;
            }
        }

        private void FECLIB_DoubleClick(object sender, EventArgs e)
        {
            FECLIB_button.PerformClick();
        }

        public static string GetMidfix4agb()
        {
            return Program.Config.at("midfix4agb", "");
        }
        public static string GetMid2agb()
        {
            return Program.Config.at("mid2agb", "");
        }
        public static bool IsUsingMidi2agb()
        {
            string path = Program.Config.at("mid2agb", "");
            if (path.IndexOf("midi2agb.exe") >= 0)
            {
                return true;
            }
            return false;
        }
        public static string GetWav2agb()
        {
            return Program.Config.at("wav2agb", "");
        }

        private void midfix4agb_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("midfix4agb|midfix4agb.exe|");
            if (r != "")
            {
                midfix4agb.Text = r;
            }
        }

        private void midfix4agb_DoubleClick(object sender, EventArgs e)
        {
            midfix4agb_button.PerformClick();
        }

        private void srccode_texteditor_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("");
            if (r != "")
            {
                srccode_texteditor.Text = r;
            }
        }

        private void srccode_directory_button_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //上部に表示する説明テキストを指定する
            fbd.Description = R._("ソースコードを保存するディレクトリを指定してください。");
            //ルートフォルダを指定する
            //デフォルトでDesktop
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            fbd.SelectedPath = srccode_directory.Text;
            //ユーザーが新しいフォルダを作成できるようにする
            //デフォルトでTrue
            fbd.ShowNewFolderButton = true;

            //ダイアログを表示する
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                srccode_directory.Text = fbd.SelectedPath;
            }
        }

        private void srccode_texteditor_DoubleClick(object sender, EventArgs e)
        {
            srccode_texteditor_button.PerformClick();
        }

        private void srccode_directory_DoubleClick(object sender, EventArgs e)
        {
            srccode_directory_button.PerformClick();
        }

        private void ColorSaveASbutton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = "FEBuilderColor|*.febuildergba_color.txt|All files|*";

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.CheckPathExists = true;
            save.ShowDialog();
            if (save.FileNames.Length <= 0)
            {
                return;
            }

            string filename = save.FileNames[0];
            if (!U.CanWriteFileRetry(filename))
            {
                return;
            }
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            using (StreamWriter w = new StreamWriter(filename))
            {
                w.WriteLine("Color_Control_BackColor" + "\t" + Color_Control_BackColor_button.BackColor.Name);
                w.WriteLine("Color_Control_ForeColor" + "\t" + Color_Control_ForeColor_button.BackColor.Name);
                w.WriteLine("Color_Input_BackColor" + "\t" + Color_Input_BackColor_button.BackColor.Name);
                w.WriteLine("Color_Input_ForeColor" + "\t" + Color_Input_ForeColor_button.BackColor.Name);
                w.WriteLine("Color_InputDecimal_BackColor" + "\t" + Color_InputDecimal_BackColor_button.BackColor.Name);
                w.WriteLine("Color_InputDecimal_ForeColor" + "\t" + Color_InputDecimal_ForeColor_button.BackColor.Name);
                w.WriteLine("Color_NotifyWrite_BackColor" + "\t" + Color_NotifyWrite_BackColor_button.BackColor.Name);
                w.WriteLine("Color_NotifyWrite_ForeColor" + "\t" + Color_NotifyWrite_ForeColor_button.BackColor.Name);
                w.WriteLine("Color_List_SelectedColor" + "\t" + Color_List_SelectedColor_button.BackColor.Name);
                w.WriteLine("Color_List_HoverColor" + "\t" + Color_List_HoverColor_button.BackColor.Name);
                w.WriteLine("Color_Keyword_BackColor" + "\t" + Color_Keyword_BackColor_button.BackColor.Name);
                w.WriteLine("Color_Keyword_ForeColor" + "\t" + Color_Keyword_ForeColor_button.BackColor.Name);
                w.WriteLine("Color_Comment_ForeColor" + "\t" + Color_Comment_ForeColor_button.BackColor.Name);
                w.WriteLine("Color_ControlComment_ForeColor" + "\t" + Color_ControlComment_ForeColor_button.BackColor.Name);
                w.WriteLine("Color_List_RelatedLine_BackColor" + "\t" + Color_List_RelatedLine_BackColor_button.BackColor.Name);
                
            }
            U.SelectFileByExplorer(filename);
        }

        private void ColorLoadButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");
            string filter = "FEBuilderColor|*.febuildergba_color.txt|All files|*";

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            open.Multiselect = false;
            open.CheckFileExists = true;
            open.CheckPathExists = true;
            open.ShowDialog();
            if (!U.CanReadFileRetry(open))
            {
                return;
            }
            U.SelectedIndexSafety(ColorSetComboBox, 0);

            string filename = open.FileNames[0];
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] sp = line.Split('\t');
                    if (sp.Length <= 1)
                    {
                        continue;
                    }
                    if (sp[0] == "Color_Control_BackColor") Color_Control_BackColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_Control_ForeColor") Color_Control_ForeColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_Input_BackColor") Color_Input_BackColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_Input_ForeColor") Color_Input_ForeColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_InputDecimal_BackColor") Color_InputDecimal_BackColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_InputDecimal_ForeColor") Color_InputDecimal_ForeColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_NotifyWrite_BackColor") Color_NotifyWrite_BackColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_NotifyWrite_ForeColor") Color_NotifyWrite_ForeColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_List_SelectedColor") Color_List_SelectedColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_List_HoverColor") Color_List_HoverColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_Keyword_BackColor") Color_Keyword_BackColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_Keyword_ForeColor") Color_Keyword_ForeColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_Comment_ForeColor") Color_Comment_ForeColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_ControlComment_ForeColor") Color_ControlComment_ForeColor_button.BackColor = U.ColorFromName(sp[1]);
                    else if (sp[0] == "Color_List_RelatedLine_BackColor") Color_List_RelatedLine_BackColor_button.BackColor = U.ColorFromName(sp[1]);
                }
            }
        }

        private void Color_List_RelatedList_BackColor_button_Click(object sender, EventArgs e)
        {
            Color_List_RelatedLine_BackColor_button.BackColor = SelectColorDialog(Color_List_RelatedLine_BackColor_button.BackColor);
        }

    }
}
