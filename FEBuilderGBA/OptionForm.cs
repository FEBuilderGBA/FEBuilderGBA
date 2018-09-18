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

            InputFormRef.LoadComboResource(func_lang, U.ConfigDataFilename("func_lang_"));
            InputFormRef.markupJumpLabel(X_EXPLAIN_NECESSARY_PROGRAM);
        }


        private void OptionForm_Load(object sender, EventArgs e)
        {
            emulator.Text = Program.Config.at("emulator");
            emulator2.Text = Program.Config.at("emulator2");
            binary_editor.Text = Program.Config.at("binary_editor");
            sappy.Text = Program.Config.at("sappy");
            program1.Text = Program.Config.at("program1");
            program2.Text = Program.Config.at("program2");
            program3.Text = Program.Config.at("program3");
            devkitpro_eabi.Text = Program.Config.at("devkitpro_eabi");
            goldroad_asm.Text = Program.Config.at("goldroad_asm");
            event_assembler.Text = Program.Config.at("event_assembler");
            mid2agb.Text = Program.Config.at("mid2agb");
            mid2agb_default.Checked = midi_importer() == midi_importer_enum.FEBuilderGBA ? false : true;

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

            ShortCutKey1.Text = Program.Config.at("ShortCutKey1", "F5");
            ShortCutValue1.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue1", "1"));  //F5 エミュレータ
            ShortCutKey2.Text = Program.Config.at("ShortCutKey2", "Ctrl+F5");
            ShortCutValue2.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue2", "2"));  //Ctrl+F5 デバッガー
            ShortCutKey3.Text = Program.Config.at("ShortCutKey3", "Ctrl+K");
            ShortCutValue3.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue3", "12")); //Ctrl+K 書き込み
            ShortCutKey4.Text = Program.Config.at("ShortCutKey4", "Pause");
            ShortCutValue4.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue4", "11")); //Pause メインへ
            ShortCutKey5.Text = Program.Config.at("ShortCutKey5", "F11");
            ShortCutValue5.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue5", "3")); //F11 バイナリエディタ
            ShortCutKey6.Text = Program.Config.at("ShortCutKey6", "F3");
            ShortCutValue6.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue6", "19")); //リストから次を検索
            ShortCutKey7.Text = Program.Config.at("ShortCutKey7", "");
            ShortCutValue7.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue7", ""));
            ShortCutKey8.Text = Program.Config.at("ShortCutKey8", "");
            ShortCutValue8.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue8", ""));
            ShortCutKey9.Text = Program.Config.at("ShortCutKey9", "");
            ShortCutValue9.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue9", ""));
            ShortCutKey10.Text = Program.Config.at("ShortCutKey10", "");
            ShortCutValue10.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue10", ""));
            ShortCutKey11.Text = Program.Config.at("ShortCutKey11", "Ctrl+W");
            ShortCutValue11.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue11", "13")); //Ctrl+W 閉じる
            ShortCutKey12.Text = Program.Config.at("ShortCutKey12", "");
            ShortCutValue12.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue12", ""));
            ShortCutKey13.Text = Program.Config.at("ShortCutKey13", "");
            ShortCutValue13.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue13", ""));
            ShortCutKey14.Text = Program.Config.at("ShortCutKey14", "");
            ShortCutValue14.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue14", ""));
            ShortCutKey15.Text = Program.Config.at("ShortCutKey15", "");
            ShortCutValue15.SelectedIndex = (int)U.atoi(Program.Config.at("ShortCutValue15", ""));
          
            U.SelectedIndexSafety(func_rom_extends,(int)rom_extends());
            U.SelectedIndexSafety(func_rom_extends_option, rom_extends_option());
            U.SelectedIndexSafety(func_auto_backup,(int)auto_backup());
            U.SelectedIndexSafety(func_first_form,(int)first_form());
            U.SelectedIndexSafety(func_lang,(int)FindComboboxText(Program.Config.at("func_lang", "auto"), func_lang));
            U.SelectedIndexSafety(func_textencoding,(int)textencoding());
            U.SelectedIndexSafety(func_auto_update,(int)FindComboboxText( auto_update().ToString(), func_auto_update));
            U.SelectedIndexSafety(func_text_escape,(int)text_escape());
            U.SelectedIndexSafety(func_write_notify_time,(int)FindComboboxText(write_notify_time().ToString(), func_write_notify_time));
            U.SelectedIndexSafety(func_write_out_of_range,(int)write_out_of_range());
            U.SelectedIndexSafety(func_select_in_explorer_when_export,(int)select_in_explorer_when_export());
            U.SelectedIndexSafety(func_sgm_jump,(int)sgm_jump());
            U.SelectedIndexSafety(func_lookup_feditor,(int)lookup_feditor());
            U.SelectedIndexSafety(func_lint_text_skip_bug,(int)lint_text_skip_bug());
            U.SelectedIndexSafety(func_show_class_extends,(int)show_class_extends());
            U.SelectedIndexSafety(func_texteditor_auto_convert_space,(int)texteditor_auto_convert_space());
            U.SelectedIndexSafety(func_auto_connect_emulator, (int)auto_connect_emulator());
            U.SelectedIndexSafety(func_proxy_server_when_connecting, (int)proxy_server_when_connecting());
            U.SelectedIndexSafety(func_notify_upper_time, (int)notify_upper_time());
            U.SelectedIndexSafety(func_create_nodoll_gba_sym, (int)create_nodoll_gba_sym());
            

            ChangeColorWriteButtonWhenChangingSetting();
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
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
            Program.Config["emulator"] = emulator.Text;
            Program.Config["emulator2"] = emulator2.Text;
            Program.Config["binary_editor"] = binary_editor.Text;
            Program.Config["sappy"] = sappy.Text;
            Program.Config["program1"] = program1.Text;
            Program.Config["program2"] = program2.Text;
            Program.Config["program3"] = program3.Text;
            Program.Config["devkitpro_eabi"] = devkitpro_eabi.Text;
            Program.Config["goldroad_asm"] = goldroad_asm.Text;
            Program.Config["event_assembler"] = event_assembler.Text;
            Program.Config["mid2agb"] = mid2agb.Text;

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
            Program.Config["func_auto_backup"] = func_auto_backup.SelectedIndex.ToString();
            Program.Config["func_first_form"] = func_first_form.SelectedIndex.ToString();
            Program.Config["func_lang"] = U.SelectValueComboboxText(func_lang.Text);
            Program.Config["func_textencoding"] = func_textencoding.SelectedIndex.ToString();
            Program.Config["func_auto_update"] = U.atoi(func_auto_update.Text).ToString();
            Program.Config["func_text_escape"] = func_text_escape.SelectedIndex.ToString();
            Program.Config["func_write_notify_time"] = U.SelectValueComboboxText(func_write_notify_time.Text);
            Program.Config["func_write_out_of_range"] = U.SelectValueComboboxText(func_write_out_of_range.Text);
            Program.Config["func_select_in_explorer_when_export"] = U.SelectValueComboboxText(func_select_in_explorer_when_export.Text);
            Program.Config["func_sgm_jump"] = U.SelectValueComboboxText(func_sgm_jump.Text);
            Program.Config["func_lookup_feditor"] = U.SelectValueComboboxText(func_lookup_feditor.Text);
            Program.Config["func_lint_text_skip_bug"] = U.SelectValueComboboxText(func_lint_text_skip_bug.Text);
            Program.Config["func_midi_importer"] = mid2agb_default.Checked ? "1" : "0";
            Program.Config["func_show_class_extends"] = U.SelectValueComboboxText(func_show_class_extends.Text);
            Program.Config["func_texteditor_auto_convert_space"] = U.SelectValueComboboxText(func_texteditor_auto_convert_space.Text);
            Program.Config["func_auto_connect_emulator"] = U.SelectValueComboboxText(func_auto_connect_emulator.Text);
            Program.Config["func_proxy_server_when_connecting"] = U.SelectValueComboboxText(func_proxy_server_when_connecting.Text);
            Program.Config["func_notify_upper_time"] = U.SelectValueComboboxText(func_notify_upper_time.Text);
            Program.Config["func_create_nodoll_gba_sym"] = U.SelectValueComboboxText(func_create_nodoll_gba_sym.Text);
            
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
        }

        string EXESearch(string first_filter)
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

        public static Color Color_Control_BackColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Control_BackColor", "Control"));
        }
        public static Color Color_Control_ForeColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Control_ForeColor", "ControlText"));
        }
        public static Color Color_Input_BackColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Input_BackColor", "Window"));
        }
        public static Color Color_Input_ForeColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Input_ForeColor", "WindowText"));
        }
        public static Color Color_InputDecimal_BackColor()
        {
            return U.ColorFromName(Program.Config.at("Color_InputDecimal_BackColor", "Window"));
        }
        public static Color Color_InputDecimal_ForeColor()
        {
            return U.ColorFromName(Program.Config.at("Color_InputDecimal_ForeColor", "OrangeRed"));
        }
        public static Color Color_NotifyWrite_BackColor()
        {
            return U.ColorFromName(Program.Config.at("Color_NotifyWrite_BackColor", "Yellow"));
        }
        public static Color Color_NotifyWrite_ForeColor()
        {
            return U.ColorFromName(Program.Config.at("Color_NotifyWrite_ForeColor", "Black"));
        }
        public static Color Color_Error_BackColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Error_BackColor", "Control"));
        }
        public static Color Color_Error_ForeColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Error_ForeColor", "Red"));
        }
        public static Color Color_List_SelectedColor()
        {
            return U.ColorFromName(Program.Config.at("Color_List_SelectedColor", "Highlight"));
        }
        public static Color Color_List_HoverColor()
        {
            return U.ColorFromName(Program.Config.at("Color_List_HoverColor", "WhiteSmoke"));
        }
        public static Color Color_Keyword_BackColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Keyword_BackColor", "Window"));
        }
        public static Color Color_Keyword_ForeColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Keyword_ForeColor", "Blue"));
        }

        public static Color Color_Comment_ForeColor()
        {
            return U.ColorFromName(Program.Config.at("Color_Comment_ForeColor", "DarkGreen"));
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

        private void binary_editor_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("");
            if (r != "")
            {
                binary_editor.Text = r;
            }
        }

        private void sappy_button_Click(object sender, EventArgs e)
        {
            string r = EXESearch("SAPPY.EXE|sappy.exe|");
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

        private void Color_Control_BackColor_resetbutton_Click(object sender, EventArgs e)
        {
            Color_Control_BackColor_button.BackColor = Color.FromName("Control");
        }

        private void Color_Control_ForeColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_Control_ForeColor_button.BackColor = Color.FromName("ControlText");
        }

        private void Color_Input_BackColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_Input_BackColor_button.BackColor = Color.FromName("Window");
        }

        private void Color_Input_ForeColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_Input_ForeColor_button.BackColor = Color.FromName("WindowText");
        }

        private void Color_InputDecimal_BackColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_InputDecimal_BackColor_button.BackColor = Color.FromName("Window");
        }

        private void Color_InputDecimal_ForeColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_InputDecimal_ForeColor_button.BackColor = Color.FromName("OrangeRed");
        }

        private void Color_NotifyWrite_BackColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_NotifyWrite_BackColor_button.BackColor = Color.FromName("Yellow");
        }

        private void Color_NotifyWrite_ForeColor_reset_button_Click(object sender, EventArgs e)
        {
            Color_NotifyWrite_ForeColor_button.BackColor = Color.FromName("Black");
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

        public enum select_in_explorer_when_export_enum
        {
            None = 0
          , Open = 1
        };
        public static select_in_explorer_when_export_enum select_in_explorer_when_export()
        {
            return (select_in_explorer_when_export_enum)U.atoi(Program.Config.at("func_select_in_explorer_when_export", "1"));
        }

        public enum sgm_jump_enum
        {
            None = 0
          , Jump = 1
        };
        public static sgm_jump_enum sgm_jump()
        {
            return (sgm_jump_enum)U.atoi(Program.Config.at("func_sgm_jump", "1")); 
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

        public enum show_class_extends_enum
        {
            None = 0
          , Show = 1
        };
        public static show_class_extends_enum show_class_extends()
        {
            return (show_class_extends_enum)U.atoi(Program.Config.at("func_show_class_extends", "0")); 
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
            if (l == "de")
            {//ドイツ語は機械翻訳なので消す 2018/04/中旬ごろには消去する.
                return "en";
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
                else
                {//不明な場合は、とりあえず、英語とする. 日本語にしたいが、英語の方が困る人が少ないだろう.
                    return "en";
                }
            }
            return l;
        }
        public enum textencoding_enum
        {
            Shift_JIS = 0
          , ZH_TBL = 1
          , EN_TBL = 2
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
            Color_List_SelectedColor_button.BackColor = Color.FromName("Highlight");
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
            {
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
                Color_List_SelectedColor_button.BackColor = U.ColorFromName("Highlight");
                Color_List_HoverColor_button.BackColor = U.ColorFromName("WhiteSmoke");
                Color_Keyword_BackColor_button.BackColor = U.ColorFromName("Window");
                Color_Keyword_ForeColor_button.BackColor = U.ColorFromName("Blue");
                Color_Comment_ForeColor_button.BackColor = U.ColorFromName("DarkGreen");

                return ;
            }
            if (colorset == 2)
            {
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
        }


        public static void AutoUpdateTBLOption()
        {
            string filename = U.ConfigDataFilename("tbl_cond_");
            if (! U.IsRequiredFileExist(filename))
            {
                return;
            }

            string[] lines = File.ReadAllLines(filename);
            uint version = (uint)Program.ROM.RomInfo.version();
            uint tbl_index = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (U.IsComment(lines[i]))
                {
                    continue;
                }
                string line = U.ClipComment(lines[i]);
                string[] sp = line.Split('\t');
                if (sp.Length < 5)
                {
                    continue;
                }
                if (U.atoi(sp[1]) != version)
                {
                    continue;
                }

                string[] hexStrings = sp[4].Split(' ');
                byte[] need = new byte[hexStrings.Length];
                for (int n = 0; n < hexStrings.Length; n++)
                {
                    need[n] = (byte)U.atoh(hexStrings[n]);
                }

                //チェック開始アドレス 地形:平地
                uint pointer = U.atoh(sp[3]);
                if (!U.isSafetyOffset(pointer))
                {
                    continue;
                }

                uint start = Program.ROM.p32(pointer);
                if (!U.isSafetyOffset(start))
                {
                    continue;
                }

                byte[] data = Program.ROM.getBinaryData(start, (uint)need.Length);
                if (U.memcmp(need, data) != 0)
                {
                    continue;
                }

                tbl_index = U.atoi(sp[2]);
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

        private void binary_editor_DoubleClick(object sender, EventArgs e)
        {
            this.binary_editor_button.PerformClick();
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
            string r = EXESearch("mid2agb.exe|mid2agb.exe|");
            if (r != "")
            {
                mid2agb.Text = r;
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
            MainFormUtil.OpenURL(MainFormUtil.GetNecessaryProgramURL());
        }

        void MakeExplainFunctions()
        {
            explain_func_rom_extends.AccessibleDescription = R._("可変長データを更新した時に、元のデータより長くなってしまったときに、\r\n自動的にリポイントするかを決定します。");
            explain_func_rom_extends_option.AccessibleDescription = R._("ROMを拡張するときに、どのアドレスから拡張を開始するかを決定します。");
            explain_func_auto_backup.AccessibleDescription = R._("ROMを上書きした時に自動的にバックアップを取るかを決定します。\r\n必ずバックアップを取ることをお勧めします。");
            explain_func_first_form.AccessibleDescription = R._("ROMを開いたとの最初の画面を決定します。\r\nディフォルトは、マップを表示する簡易メニューです。\r\n");
            explain_func_auto_update.AccessibleDescription = R._("このソフトウェアを自動的に更新する更新間隔を設定します。");
            explain_func_text_escape.AccessibleDescription = R._("テキストに利用するエスケープシーケンスをどうするかを決定します。\r\nProjectFEGBA形式(@0003)、\r\nFEditor形式[A]、\r\nどちらを利用するかを決定します");
            explain_func_write_notify_time.AccessibleDescription = R._("データを書き込んだときに、書き込み増したという通知を画面下に出しますが、\r\nあの表示の表示時間を決定します");
            explain_func_write_out_of_range.AccessibleDescription = R._("範囲外に書き込みそうになった時に、\r\n自動的に拒否するか、警告を出すか、何もしないかを決定します。");
            explain_func_select_in_explorer_when_export.AccessibleDescription = R._("ファイルをエクスポートした後に、\r\nそのファイルがある場所をエクスプローラで開いて選択するかどうかを決定します");
            explain_func_sgm_jump.AccessibleDescription = R._("VBA-MでQSをした後で、エミュレータを終了すると、\r\n最後に実行していたイベントのある場所へ自動的にジャンプします。");
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
            X_EXPLAIN_VBA_M.AccessibleDescription = R._("エミュレータを設定すると、F5キーでテスト実行できるようになります。\r\nまた、エミュレータのRAM領域にアクセスしてデータを表示できます。\r\nVBA-M(sourceforge),mGBA,no$gba等に対応しています。\r\n\r\nただし、VBA-M(github)のRAM領域へは接続できません。");
            X_EXPLAIN_NODOLL_GBA_DEBUGGER.AccessibleDescription = R._("Ctrl+F5を押したときに起動するデバッガーを設定します。\r\nFEBuilderGBAは、no$gba debuggerへ、セーブデータの転送と、シンボルの自動生成を行うことができます。");
            X_EXPLAIN_SAPPY.AccessibleDescription = R._("sappyを設定すると、音符ボタンをクリックした時に、BGMを再生できます。\r\nsappyをインストールするときは、マニュアルに書いてあるインストーラーを利用すると便利です。");
//            X_EXPLAIN_EA.AccessibleDescription = R._("");
        }



    }
}
