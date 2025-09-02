namespace FEBuilderGBA
{
    partial class ToolTranslateROMForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SIMPLE_OVERRAIDE_JPFONT = new System.Windows.Forms.CheckBox();
            this.SimpleTranslateToTranslateDataFilename = new FEBuilderGBA.TextBoxEx();
            this.SimpleTranslateToTranslateDataFilenameButton = new System.Windows.Forms.Button();
            this.LabelSimpleTranslateToTranslateDataFilename = new System.Windows.Forms.Label();
            this.SimpleFireButton = new System.Windows.Forms.Button();
            this.LabelSimpleTranslateToROMFilename = new System.Windows.Forms.Label();
            this.SimpleTranslateToROMFilename = new FEBuilderGBA.TextBoxEx();
            this.SimpleTranslateToROMFilenameSelectButton = new System.Windows.Forms.Button();
            this.LabelSimpleTranslateFromROMFilename = new System.Windows.Forms.Label();
            this.SimpleTranslateFromROMFilename = new FEBuilderGBA.TextBoxEx();
            this.SimpleTranslateFormROMFilenameSelectButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.customColorGroupBox2 = new FEBuilderGBA.CustomColorGroupBox();
            this.FontAutoGenelatePanel = new System.Windows.Forms.Panel();
            this.UseFontNameButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.UseFontNameTextEdit = new FEBuilderGBA.TextBoxEx();
            this.FontAutoGenelateCheckBox = new System.Windows.Forms.CheckBox();
            this.ImportFontButton = new System.Windows.Forms.Button();
            this.FontROMSelectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FontROMTextBox = new FEBuilderGBA.TextBoxEx();
            this.customColorGroupBox3 = new FEBuilderGBA.CustomColorGroupBox();
            this.customColorGroupBox4 = new FEBuilderGBA.CustomColorGroupBox();
            this.TranslatePanel = new System.Windows.Forms.Panel();
            this.X_MODIFIED_TEXT_ONLY = new System.Windows.Forms.CheckBox();
            this.LabelTranslateToROMFilename = new System.Windows.Forms.Label();
            this.TranslateToROMFilename = new FEBuilderGBA.TextBoxEx();
            this.TranslateToROMFilenameSelectButton = new System.Windows.Forms.Button();
            this.LabelTranslateFormROMFilename = new System.Windows.Forms.Label();
            this.TranslateFormROMFilename = new FEBuilderGBA.TextBoxEx();
            this.TranslateFormROMFilenameSelectButton = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.Translate_to = new System.Windows.Forms.ComboBox();
            this.Translate_from = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.useAutoTranslateCheckBox = new System.Windows.Forms.CheckBox();
            this.X_ONELINER_CHECK = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ExportallTextButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.X_OVERRAIDE_JPFONT = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ImportAllTextButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ExtraFontToROMFilename = new FEBuilderGBA.TextBoxEx();
            this.ExtraFontToROMFilenameSelectButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.customColorGroupBox2.SuspendLayout();
            this.FontAutoGenelatePanel.SuspendLayout();
            this.customColorGroupBox3.SuspendLayout();
            this.customColorGroupBox4.SuspendLayout();
            this.TranslatePanel.SuspendLayout();
            this.customColorGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(916, 786);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(908, 754);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "簡易";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 293);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(894, 393);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "ROMの翻訳を行います。\r\n日本語ROMと英語ROMを利用して、無変更のテキストを自動的に変換します。\r\n\r\n簡易モードでは、全自動で翻訳します。\r\n不足している" +
    "フォントのインポートや、メニューの長さなどを自動的に調整します。\r\n全ての処理が終わるのに、10分ほど時間がかかるので、少しお待ちください。";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ExtraFontToROMFilename);
            this.panel1.Controls.Add(this.ExtraFontToROMFilenameSelectButton);
            this.panel1.Controls.Add(this.SIMPLE_OVERRAIDE_JPFONT);
            this.panel1.Controls.Add(this.SimpleTranslateToTranslateDataFilename);
            this.panel1.Controls.Add(this.SimpleTranslateToTranslateDataFilenameButton);
            this.panel1.Controls.Add(this.LabelSimpleTranslateToTranslateDataFilename);
            this.panel1.Controls.Add(this.SimpleFireButton);
            this.panel1.Controls.Add(this.LabelSimpleTranslateToROMFilename);
            this.panel1.Controls.Add(this.SimpleTranslateToROMFilename);
            this.panel1.Controls.Add(this.SimpleTranslateToROMFilenameSelectButton);
            this.panel1.Controls.Add(this.LabelSimpleTranslateFromROMFilename);
            this.panel1.Controls.Add(this.SimpleTranslateFromROMFilename);
            this.panel1.Controls.Add(this.SimpleTranslateFormROMFilenameSelectButton);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(894, 268);
            this.panel1.TabIndex = 2;
            // 
            // SIMPLE_OVERRAIDE_JPFONT
            // 
            this.SIMPLE_OVERRAIDE_JPFONT.AccessibleDescription = "日本語フォントを潰して約100kを確保します。\\r\\n翻訳により日本語がすべて消える場合にのみ指定できます。";
            this.SIMPLE_OVERRAIDE_JPFONT.AutoSize = true;
            this.SIMPLE_OVERRAIDE_JPFONT.Location = new System.Drawing.Point(206, 158);
            this.SIMPLE_OVERRAIDE_JPFONT.Name = "SIMPLE_OVERRAIDE_JPFONT";
            this.SIMPLE_OVERRAIDE_JPFONT.Size = new System.Drawing.Size(203, 22);
            this.SIMPLE_OVERRAIDE_JPFONT.TabIndex = 102;
            this.SIMPLE_OVERRAIDE_JPFONT.Text = "日本語フォントの上書き";
            this.SIMPLE_OVERRAIDE_JPFONT.UseVisualStyleBackColor = true;
            // 
            // SimpleTranslateToTranslateDataFilename
            // 
            this.SimpleTranslateToTranslateDataFilename.ErrorMessage = "";
            this.SimpleTranslateToTranslateDataFilename.Location = new System.Drawing.Point(204, 123);
            this.SimpleTranslateToTranslateDataFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateToTranslateDataFilename.Name = "SimpleTranslateToTranslateDataFilename";
            this.SimpleTranslateToTranslateDataFilename.Placeholder = "";
            this.SimpleTranslateToTranslateDataFilename.Size = new System.Drawing.Size(623, 25);
            this.SimpleTranslateToTranslateDataFilename.TabIndex = 100;
            this.SimpleTranslateToTranslateDataFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SimpleTranslateToTranslateDataFilename_MouseDoubleClick);
            // 
            // SimpleTranslateToTranslateDataFilenameButton
            // 
            this.SimpleTranslateToTranslateDataFilenameButton.Location = new System.Drawing.Point(833, 118);
            this.SimpleTranslateToTranslateDataFilenameButton.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateToTranslateDataFilenameButton.Name = "SimpleTranslateToTranslateDataFilenameButton";
            this.SimpleTranslateToTranslateDataFilenameButton.Size = new System.Drawing.Size(41, 31);
            this.SimpleTranslateToTranslateDataFilenameButton.TabIndex = 101;
            this.SimpleTranslateToTranslateDataFilenameButton.Text = "..";
            this.SimpleTranslateToTranslateDataFilenameButton.UseVisualStyleBackColor = true;
            this.SimpleTranslateToTranslateDataFilenameButton.Click += new System.EventHandler(this.SimpleTranslateToTranslateDataFilenameButton_Click);
            // 
            // LabelSimpleTranslateToTranslateDataFilename
            // 
            this.LabelSimpleTranslateToTranslateDataFilename.AccessibleDescription = "@TRANSLATE_HINT_FILE";
            this.LabelSimpleTranslateToTranslateDataFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelSimpleTranslateToTranslateDataFilename.Location = new System.Drawing.Point(5, 120);
            this.LabelSimpleTranslateToTranslateDataFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelSimpleTranslateToTranslateDataFilename.Name = "LabelSimpleTranslateToTranslateDataFilename";
            this.LabelSimpleTranslateToTranslateDataFilename.Size = new System.Drawing.Size(196, 31);
            this.LabelSimpleTranslateToTranslateDataFilename.TabIndex = 99;
            this.LabelSimpleTranslateToTranslateDataFilename.Text = "翻訳データ";
            this.LabelSimpleTranslateToTranslateDataFilename.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleFireButton
            // 
            this.SimpleFireButton.Location = new System.Drawing.Point(5, 184);
            this.SimpleFireButton.Name = "SimpleFireButton";
            this.SimpleFireButton.Size = new System.Drawing.Size(877, 67);
            this.SimpleFireButton.TabIndex = 98;
            this.SimpleFireButton.Text = "翻訳開始";
            this.SimpleFireButton.UseVisualStyleBackColor = true;
            this.SimpleFireButton.Click += new System.EventHandler(this.SimpleFireButton_Click);
            // 
            // LabelSimpleTranslateToROMFilename
            // 
            this.LabelSimpleTranslateToROMFilename.AccessibleDescription = "@TRANSLATE_TO_ROM";
            this.LabelSimpleTranslateToROMFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelSimpleTranslateToROMFilename.Location = new System.Drawing.Point(5, 44);
            this.LabelSimpleTranslateToROMFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelSimpleTranslateToROMFilename.Name = "LabelSimpleTranslateToROMFilename";
            this.LabelSimpleTranslateToROMFilename.Size = new System.Drawing.Size(196, 31);
            this.LabelSimpleTranslateToROMFilename.TabIndex = 97;
            this.LabelSimpleTranslateToROMFilename.Text = "定型文ROM TO";
            this.LabelSimpleTranslateToROMFilename.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleTranslateToROMFilename
            // 
            this.SimpleTranslateToROMFilename.ErrorMessage = "";
            this.SimpleTranslateToROMFilename.Location = new System.Drawing.Point(204, 46);
            this.SimpleTranslateToROMFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateToROMFilename.Name = "SimpleTranslateToROMFilename";
            this.SimpleTranslateToROMFilename.Placeholder = "";
            this.SimpleTranslateToROMFilename.Size = new System.Drawing.Size(623, 25);
            this.SimpleTranslateToROMFilename.TabIndex = 4;
            this.SimpleTranslateToROMFilename.DoubleClick += new System.EventHandler(this.TranslateToROMFilename_DoubleClick);
            // 
            // SimpleTranslateToROMFilenameSelectButton
            // 
            this.SimpleTranslateToROMFilenameSelectButton.Location = new System.Drawing.Point(833, 41);
            this.SimpleTranslateToROMFilenameSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateToROMFilenameSelectButton.Name = "SimpleTranslateToROMFilenameSelectButton";
            this.SimpleTranslateToROMFilenameSelectButton.Size = new System.Drawing.Size(41, 31);
            this.SimpleTranslateToROMFilenameSelectButton.TabIndex = 5;
            this.SimpleTranslateToROMFilenameSelectButton.Text = "..";
            this.SimpleTranslateToROMFilenameSelectButton.UseVisualStyleBackColor = true;
            this.SimpleTranslateToROMFilenameSelectButton.Click += new System.EventHandler(this.TranslateToROMFilenameSelectButton_Click);
            // 
            // LabelSimpleTranslateFromROMFilename
            // 
            this.LabelSimpleTranslateFromROMFilename.AccessibleDescription = "@TRANSLATE_FROM_ROM";
            this.LabelSimpleTranslateFromROMFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelSimpleTranslateFromROMFilename.Location = new System.Drawing.Point(6, 7);
            this.LabelSimpleTranslateFromROMFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelSimpleTranslateFromROMFilename.Name = "LabelSimpleTranslateFromROMFilename";
            this.LabelSimpleTranslateFromROMFilename.Size = new System.Drawing.Size(196, 31);
            this.LabelSimpleTranslateFromROMFilename.TabIndex = 94;
            this.LabelSimpleTranslateFromROMFilename.Text = "定型文ROM FROM";
            this.LabelSimpleTranslateFromROMFilename.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleTranslateFromROMFilename
            // 
            this.SimpleTranslateFromROMFilename.ErrorMessage = "";
            this.SimpleTranslateFromROMFilename.Location = new System.Drawing.Point(205, 9);
            this.SimpleTranslateFromROMFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateFromROMFilename.Name = "SimpleTranslateFromROMFilename";
            this.SimpleTranslateFromROMFilename.Placeholder = "";
            this.SimpleTranslateFromROMFilename.Size = new System.Drawing.Size(623, 25);
            this.SimpleTranslateFromROMFilename.TabIndex = 2;
            this.SimpleTranslateFromROMFilename.DoubleClick += new System.EventHandler(this.TranslateFormROMFilename_DoubleClick);
            // 
            // SimpleTranslateFormROMFilenameSelectButton
            // 
            this.SimpleTranslateFormROMFilenameSelectButton.Location = new System.Drawing.Point(834, 4);
            this.SimpleTranslateFormROMFilenameSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateFormROMFilenameSelectButton.Name = "SimpleTranslateFormROMFilenameSelectButton";
            this.SimpleTranslateFormROMFilenameSelectButton.Size = new System.Drawing.Size(41, 31);
            this.SimpleTranslateFormROMFilenameSelectButton.TabIndex = 3;
            this.SimpleTranslateFormROMFilenameSelectButton.Text = "..";
            this.SimpleTranslateFormROMFilenameSelectButton.UseVisualStyleBackColor = true;
            this.SimpleTranslateFormROMFilenameSelectButton.Click += new System.EventHandler(this.TranslateFormROMFilenameSelectButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.customColorGroupBox2);
            this.tabPage2.Controls.Add(this.customColorGroupBox3);
            this.tabPage2.Controls.Add(this.customColorGroupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(908, 754);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "詳細";
            // 
            // customColorGroupBox2
            // 
            this.customColorGroupBox2.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox2.Controls.Add(this.FontAutoGenelatePanel);
            this.customColorGroupBox2.Controls.Add(this.FontAutoGenelateCheckBox);
            this.customColorGroupBox2.Controls.Add(this.ImportFontButton);
            this.customColorGroupBox2.Controls.Add(this.FontROMSelectButton);
            this.customColorGroupBox2.Controls.Add(this.label1);
            this.customColorGroupBox2.Controls.Add(this.FontROMTextBox);
            this.customColorGroupBox2.Location = new System.Drawing.Point(3, 478);
            this.customColorGroupBox2.Name = "customColorGroupBox2";
            this.customColorGroupBox2.Size = new System.Drawing.Size(892, 216);
            this.customColorGroupBox2.TabIndex = 1;
            this.customColorGroupBox2.TabStop = false;
            this.customColorGroupBox2.Text = "フォント";
            // 
            // FontAutoGenelatePanel
            // 
            this.FontAutoGenelatePanel.Controls.Add(this.UseFontNameButton);
            this.FontAutoGenelatePanel.Controls.Add(this.label7);
            this.FontAutoGenelatePanel.Controls.Add(this.UseFontNameTextEdit);
            this.FontAutoGenelatePanel.Location = new System.Drawing.Point(14, 130);
            this.FontAutoGenelatePanel.Name = "FontAutoGenelatePanel";
            this.FontAutoGenelatePanel.Size = new System.Drawing.Size(873, 36);
            this.FontAutoGenelatePanel.TabIndex = 99;
            // 
            // UseFontNameButton
            // 
            this.UseFontNameButton.Location = new System.Drawing.Point(783, 3);
            this.UseFontNameButton.Margin = new System.Windows.Forms.Padding(2);
            this.UseFontNameButton.Name = "UseFontNameButton";
            this.UseFontNameButton.Size = new System.Drawing.Size(86, 30);
            this.UseFontNameButton.TabIndex = 98;
            this.UseFontNameButton.Text = "変更";
            this.UseFontNameButton.UseVisualStyleBackColor = true;
            this.UseFontNameButton.Click += new System.EventHandler(this.UseFontNameButton_Click);
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(2, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 30);
            this.label7.TabIndex = 96;
            this.label7.Text = "利用フォント";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UseFontNameTextEdit
            // 
            this.UseFontNameTextEdit.ErrorMessage = "";
            this.UseFontNameTextEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UseFontNameTextEdit.Location = new System.Drawing.Point(134, 5);
            this.UseFontNameTextEdit.Margin = new System.Windows.Forms.Padding(2);
            this.UseFontNameTextEdit.Name = "UseFontNameTextEdit";
            this.UseFontNameTextEdit.Placeholder = "";
            this.UseFontNameTextEdit.ReadOnly = true;
            this.UseFontNameTextEdit.Size = new System.Drawing.Size(639, 26);
            this.UseFontNameTextEdit.TabIndex = 97;
            this.UseFontNameTextEdit.DoubleClick += new System.EventHandler(this.UseFontNameButton_Click);
            // 
            // FontAutoGenelateCheckBox
            // 
            this.FontAutoGenelateCheckBox.AutoSize = true;
            this.FontAutoGenelateCheckBox.Checked = true;
            this.FontAutoGenelateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FontAutoGenelateCheckBox.Location = new System.Drawing.Point(14, 102);
            this.FontAutoGenelateCheckBox.Name = "FontAutoGenelateCheckBox";
            this.FontAutoGenelateCheckBox.Size = new System.Drawing.Size(230, 22);
            this.FontAutoGenelateCheckBox.TabIndex = 3;
            this.FontAutoGenelateCheckBox.Text = "足りないフォントの自動生成";
            this.FontAutoGenelateCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FontAutoGenelateCheckBox.UseVisualStyleBackColor = true;
            this.FontAutoGenelateCheckBox.CheckedChanged += new System.EventHandler(this.FontAutoGenelateCheckBox_CheckedChanged);
            // 
            // ImportFontButton
            // 
            this.ImportFontButton.Location = new System.Drawing.Point(14, 180);
            this.ImportFontButton.Name = "ImportFontButton";
            this.ImportFontButton.Size = new System.Drawing.Size(877, 30);
            this.ImportFontButton.TabIndex = 0;
            this.ImportFontButton.Text = "フォント取込";
            this.ImportFontButton.UseVisualStyleBackColor = true;
            this.ImportFontButton.Click += new System.EventHandler(this.ImportFontButton_Click);
            // 
            // FontROMSelectButton
            // 
            this.FontROMSelectButton.Location = new System.Drawing.Point(844, 56);
            this.FontROMSelectButton.Name = "FontROMSelectButton";
            this.FontROMSelectButton.Size = new System.Drawing.Size(42, 30);
            this.FontROMSelectButton.TabIndex = 2;
            this.FontROMSelectButton.Text = "..";
            this.FontROMSelectButton.UseVisualStyleBackColor = true;
            this.FontROMSelectButton.Click += new System.EventHandler(this.FontROMSelectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(507, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "現行ROMに足りないフォントを、以下のROMにあるフォントからコピーする";
            // 
            // FontROMTextBox
            // 
            this.FontROMTextBox.ErrorMessage = "";
            this.FontROMTextBox.Location = new System.Drawing.Point(10, 61);
            this.FontROMTextBox.Name = "FontROMTextBox";
            this.FontROMTextBox.Placeholder = "";
            this.FontROMTextBox.Size = new System.Drawing.Size(828, 25);
            this.FontROMTextBox.TabIndex = 1;
            this.FontROMTextBox.DoubleClick += new System.EventHandler(this.FontROMTextBox_DoubleClick);
            // 
            // customColorGroupBox3
            // 
            this.customColorGroupBox3.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox3.Controls.Add(this.customColorGroupBox4);
            this.customColorGroupBox3.Controls.Add(this.label4);
            this.customColorGroupBox3.Controls.Add(this.ExportallTextButton);
            this.customColorGroupBox3.Location = new System.Drawing.Point(3, 6);
            this.customColorGroupBox3.Name = "customColorGroupBox3";
            this.customColorGroupBox3.Size = new System.Drawing.Size(892, 310);
            this.customColorGroupBox3.TabIndex = 3;
            this.customColorGroupBox3.TabStop = false;
            this.customColorGroupBox3.Text = "全テキストの書出し";
            // 
            // customColorGroupBox4
            // 
            this.customColorGroupBox4.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox4.Controls.Add(this.TranslatePanel);
            this.customColorGroupBox4.Controls.Add(this.useAutoTranslateCheckBox);
            this.customColorGroupBox4.Controls.Add(this.X_ONELINER_CHECK);
            this.customColorGroupBox4.Location = new System.Drawing.Point(9, 59);
            this.customColorGroupBox4.Name = "customColorGroupBox4";
            this.customColorGroupBox4.Size = new System.Drawing.Size(877, 211);
            this.customColorGroupBox4.TabIndex = 6;
            this.customColorGroupBox4.TabStop = false;
            this.customColorGroupBox4.Text = "翻訳して書き出す";
            // 
            // TranslatePanel
            // 
            this.TranslatePanel.Controls.Add(this.X_MODIFIED_TEXT_ONLY);
            this.TranslatePanel.Controls.Add(this.LabelTranslateToROMFilename);
            this.TranslatePanel.Controls.Add(this.TranslateToROMFilename);
            this.TranslatePanel.Controls.Add(this.TranslateToROMFilenameSelectButton);
            this.TranslatePanel.Controls.Add(this.LabelTranslateFormROMFilename);
            this.TranslatePanel.Controls.Add(this.TranslateFormROMFilename);
            this.TranslatePanel.Controls.Add(this.TranslateFormROMFilenameSelectButton);
            this.TranslatePanel.Controls.Add(this.label16);
            this.TranslatePanel.Controls.Add(this.Translate_to);
            this.TranslatePanel.Controls.Add(this.Translate_from);
            this.TranslatePanel.Controls.Add(this.label17);
            this.TranslatePanel.Enabled = false;
            this.TranslatePanel.Location = new System.Drawing.Point(2, 56);
            this.TranslatePanel.Name = "TranslatePanel";
            this.TranslatePanel.Size = new System.Drawing.Size(876, 152);
            this.TranslatePanel.TabIndex = 1;
            // 
            // X_MODIFIED_TEXT_ONLY
            // 
            this.X_MODIFIED_TEXT_ONLY.AutoSize = true;
            this.X_MODIFIED_TEXT_ONLY.Checked = true;
            this.X_MODIFIED_TEXT_ONLY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.X_MODIFIED_TEXT_ONLY.Location = new System.Drawing.Point(6, 121);
            this.X_MODIFIED_TEXT_ONLY.Name = "X_MODIFIED_TEXT_ONLY";
            this.X_MODIFIED_TEXT_ONLY.Size = new System.Drawing.Size(264, 22);
            this.X_MODIFIED_TEXT_ONLY.TabIndex = 98;
            this.X_MODIFIED_TEXT_ONLY.Text = "改造されたテキストのみ取得する";
            this.X_MODIFIED_TEXT_ONLY.UseVisualStyleBackColor = true;
            // 
            // LabelTranslateToROMFilename
            // 
            this.LabelTranslateToROMFilename.AccessibleDescription = "@TRANSLATE_TO_ROM";
            this.LabelTranslateToROMFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelTranslateToROMFilename.Location = new System.Drawing.Point(5, 84);
            this.LabelTranslateToROMFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelTranslateToROMFilename.Name = "LabelTranslateToROMFilename";
            this.LabelTranslateToROMFilename.Size = new System.Drawing.Size(196, 31);
            this.LabelTranslateToROMFilename.TabIndex = 97;
            this.LabelTranslateToROMFilename.Text = "定型文ROM TO";
            this.LabelTranslateToROMFilename.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TranslateToROMFilename
            // 
            this.TranslateToROMFilename.ErrorMessage = "";
            this.TranslateToROMFilename.Location = new System.Drawing.Point(204, 86);
            this.TranslateToROMFilename.Margin = new System.Windows.Forms.Padding(4);
            this.TranslateToROMFilename.Name = "TranslateToROMFilename";
            this.TranslateToROMFilename.Placeholder = "";
            this.TranslateToROMFilename.Size = new System.Drawing.Size(623, 25);
            this.TranslateToROMFilename.TabIndex = 4;
            this.TranslateToROMFilename.DoubleClick += new System.EventHandler(this.TranslateToROMFilename_DoubleClick);
            // 
            // TranslateToROMFilenameSelectButton
            // 
            this.TranslateToROMFilenameSelectButton.Location = new System.Drawing.Point(833, 80);
            this.TranslateToROMFilenameSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.TranslateToROMFilenameSelectButton.Name = "TranslateToROMFilenameSelectButton";
            this.TranslateToROMFilenameSelectButton.Size = new System.Drawing.Size(41, 31);
            this.TranslateToROMFilenameSelectButton.TabIndex = 5;
            this.TranslateToROMFilenameSelectButton.Text = "..";
            this.TranslateToROMFilenameSelectButton.UseVisualStyleBackColor = true;
            this.TranslateToROMFilenameSelectButton.Click += new System.EventHandler(this.TranslateToROMFilenameSelectButton_Click);
            // 
            // LabelTranslateFormROMFilename
            // 
            this.LabelTranslateFormROMFilename.AccessibleDescription = "@TRANSLATE_FROM_ROM";
            this.LabelTranslateFormROMFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelTranslateFormROMFilename.Location = new System.Drawing.Point(6, 47);
            this.LabelTranslateFormROMFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelTranslateFormROMFilename.Name = "LabelTranslateFormROMFilename";
            this.LabelTranslateFormROMFilename.Size = new System.Drawing.Size(196, 31);
            this.LabelTranslateFormROMFilename.TabIndex = 94;
            this.LabelTranslateFormROMFilename.Text = "定型文ROM FROM";
            this.LabelTranslateFormROMFilename.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TranslateFormROMFilename
            // 
            this.TranslateFormROMFilename.ErrorMessage = "";
            this.TranslateFormROMFilename.Location = new System.Drawing.Point(205, 49);
            this.TranslateFormROMFilename.Margin = new System.Windows.Forms.Padding(4);
            this.TranslateFormROMFilename.Name = "TranslateFormROMFilename";
            this.TranslateFormROMFilename.Placeholder = "";
            this.TranslateFormROMFilename.Size = new System.Drawing.Size(623, 25);
            this.TranslateFormROMFilename.TabIndex = 2;
            this.TranslateFormROMFilename.DoubleClick += new System.EventHandler(this.TranslateFormROMFilename_DoubleClick);
            // 
            // TranslateFormROMFilenameSelectButton
            // 
            this.TranslateFormROMFilenameSelectButton.Location = new System.Drawing.Point(834, 43);
            this.TranslateFormROMFilenameSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.TranslateFormROMFilenameSelectButton.Name = "TranslateFormROMFilenameSelectButton";
            this.TranslateFormROMFilenameSelectButton.Size = new System.Drawing.Size(41, 31);
            this.TranslateFormROMFilenameSelectButton.TabIndex = 3;
            this.TranslateFormROMFilenameSelectButton.Text = "..";
            this.TranslateFormROMFilenameSelectButton.UseVisualStyleBackColor = true;
            this.TranslateFormROMFilenameSelectButton.Click += new System.EventHandler(this.TranslateFormROMFilenameSelectButton_Click);
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(6, 9);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(57, 30);
            this.label16.TabIndex = 7;
            this.label16.Text = "from:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Translate_to
            // 
            this.Translate_to.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Translate_to.FormattingEnabled = true;
            this.Translate_to.Items.AddRange(new object[] {
            "ja=日本語",
            "en=英語",
            "zh-CN=中国語 簡体",
            "zh-TW=中国語 繁体",
            "es=スペイン語",
            "hi=ヒンディー語",
            "ar=アラビア語",
            "pt=ポルトガル語",
            "ru=ロシア語",
            "fr=フランス語",
            "eo=エスペランド語"});
            this.Translate_to.Location = new System.Drawing.Point(284, 12);
            this.Translate_to.Margin = new System.Windows.Forms.Padding(4);
            this.Translate_to.Name = "Translate_to";
            this.Translate_to.Size = new System.Drawing.Size(166, 26);
            this.Translate_to.TabIndex = 1;
            this.Translate_to.SelectedIndexChanged += new System.EventHandler(this.Translate_to_SelectedIndexChanged);
            // 
            // Translate_from
            // 
            this.Translate_from.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Translate_from.FormattingEnabled = true;
            this.Translate_from.Items.AddRange(new object[] {
            "ja=日本語",
            "en=英語",
            "zh-CN=中国語"});
            this.Translate_from.Location = new System.Drawing.Point(66, 10);
            this.Translate_from.Margin = new System.Windows.Forms.Padding(4);
            this.Translate_from.Name = "Translate_from";
            this.Translate_from.Size = new System.Drawing.Size(167, 26);
            this.Translate_from.TabIndex = 0;
            this.Translate_from.SelectedIndexChanged += new System.EventHandler(this.Translate_from_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Location = new System.Drawing.Point(241, 9);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(39, 30);
            this.label17.TabIndex = 8;
            this.label17.Text = "to:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // useAutoTranslateCheckBox
            // 
            this.useAutoTranslateCheckBox.AutoSize = true;
            this.useAutoTranslateCheckBox.Checked = true;
            this.useAutoTranslateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useAutoTranslateCheckBox.Location = new System.Drawing.Point(7, 28);
            this.useAutoTranslateCheckBox.Name = "useAutoTranslateCheckBox";
            this.useAutoTranslateCheckBox.Size = new System.Drawing.Size(618, 22);
            this.useAutoTranslateCheckBox.TabIndex = 0;
            this.useAutoTranslateCheckBox.Text = "定型文の翻訳(FROM ROMと TO ROMから、定型文を取得し、翻訳の参考にする)";
            this.useAutoTranslateCheckBox.UseVisualStyleBackColor = true;
            this.useAutoTranslateCheckBox.CheckedChanged += new System.EventHandler(this.useAutoTranslateCheckBox_CheckedChanged);
            // 
            // X_ONELINER_CHECK
            // 
            this.X_ONELINER_CHECK.AutoSize = true;
            this.X_ONELINER_CHECK.Location = new System.Drawing.Point(6, 65);
            this.X_ONELINER_CHECK.Name = "X_ONELINER_CHECK";
            this.X_ONELINER_CHECK.Size = new System.Drawing.Size(103, 22);
            this.X_ONELINER_CHECK.TabIndex = 99;
            this.X_ONELINER_CHECK.Text = "OneLiner";
            this.X_ONELINER_CHECK.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(358, 18);
            this.label4.TabIndex = 5;
            this.label4.Text = "すべてのテキストをテキストファイルに書きだします。";
            // 
            // ExportallTextButton
            // 
            this.ExportallTextButton.Location = new System.Drawing.Point(10, 275);
            this.ExportallTextButton.Name = "ExportallTextButton";
            this.ExportallTextButton.Size = new System.Drawing.Size(877, 30);
            this.ExportallTextButton.TabIndex = 0;
            this.ExportallTextButton.Text = "全テキストの書出し";
            this.ExportallTextButton.UseVisualStyleBackColor = true;
            this.ExportallTextButton.Click += new System.EventHandler(this.ExportallTextButton_Click);
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.X_OVERRAIDE_JPFONT);
            this.customColorGroupBox1.Controls.Add(this.label2);
            this.customColorGroupBox1.Controls.Add(this.ImportAllTextButton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(3, 324);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(892, 148);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "全テキストの読込";
            // 
            // X_OVERRAIDE_JPFONT
            // 
            this.X_OVERRAIDE_JPFONT.AccessibleDescription = "日本語フォントを潰して約100kを確保します。\\r\\n翻訳により日本語がすべて消える場合にのみ指定できます。";
            this.X_OVERRAIDE_JPFONT.AutoSize = true;
            this.X_OVERRAIDE_JPFONT.Location = new System.Drawing.Point(617, 77);
            this.X_OVERRAIDE_JPFONT.Name = "X_OVERRAIDE_JPFONT";
            this.X_OVERRAIDE_JPFONT.Size = new System.Drawing.Size(203, 22);
            this.X_OVERRAIDE_JPFONT.TabIndex = 99;
            this.X_OVERRAIDE_JPFONT.Text = "日本語フォントの上書き";
            this.X_OVERRAIDE_JPFONT.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(586, 54);
            this.label2.TabIndex = 4;
            this.label2.Text = "すべてのテキストを、指定されたファイルの内容で書き換えます。\r\n利用するには、anti-HuffmanとDrawMultibye/DrawSingleByteパッ" +
    "チが必要です。\r\n処理が終わったら、別途、足りないフォントを取り込んでください。";
            // 
            // ImportAllTextButton
            // 
            this.ImportAllTextButton.Location = new System.Drawing.Point(10, 104);
            this.ImportAllTextButton.Name = "ImportAllTextButton";
            this.ImportAllTextButton.Size = new System.Drawing.Size(876, 30);
            this.ImportAllTextButton.TabIndex = 0;
            this.ImportAllTextButton.Text = "全テキストの読込";
            this.ImportAllTextButton.UseVisualStyleBackColor = true;
            this.ImportAllTextButton.Click += new System.EventHandler(this.ImportAllTextButton_Click);
            // 
            // label3
            // 
            this.label3.AccessibleDescription = "@TRANSLATE_TO_ROM";
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(6, 82);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 31);
            this.label3.TabIndex = 105;
            this.label3.Text = "追加フォント ROM";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExtraFontToROMFilename
            // 
            this.ExtraFontToROMFilename.ErrorMessage = "";
            this.ExtraFontToROMFilename.Location = new System.Drawing.Point(205, 84);
            this.ExtraFontToROMFilename.Margin = new System.Windows.Forms.Padding(4);
            this.ExtraFontToROMFilename.Name = "ExtraFontToROMFilename";
            this.ExtraFontToROMFilename.Placeholder = "";
            this.ExtraFontToROMFilename.Size = new System.Drawing.Size(623, 25);
            this.ExtraFontToROMFilename.TabIndex = 103;
            this.ExtraFontToROMFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ExtraFontToROMFilenameSelectButton_Click);
            // 
            // ExtraFontToROMFilenameSelectButton
            // 
            this.ExtraFontToROMFilenameSelectButton.Location = new System.Drawing.Point(834, 79);
            this.ExtraFontToROMFilenameSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExtraFontToROMFilenameSelectButton.Name = "ExtraFontToROMFilenameSelectButton";
            this.ExtraFontToROMFilenameSelectButton.Size = new System.Drawing.Size(41, 31);
            this.ExtraFontToROMFilenameSelectButton.TabIndex = 104;
            this.ExtraFontToROMFilenameSelectButton.Text = "..";
            this.ExtraFontToROMFilenameSelectButton.UseVisualStyleBackColor = true;
            this.ExtraFontToROMFilenameSelectButton.Click += new System.EventHandler(this.ExtraFontToROMFilenameSelectButton_Click);
            // 
            // ToolTranslateROMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(912, 722);
            this.Controls.Add(this.tabControl1);
            this.Name = "ToolTranslateROMForm";
            this.Text = "ROM翻訳ツール";
            this.Load += new System.EventHandler(this.TranslateROMForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.customColorGroupBox2.ResumeLayout(false);
            this.customColorGroupBox2.PerformLayout();
            this.FontAutoGenelatePanel.ResumeLayout(false);
            this.FontAutoGenelatePanel.PerformLayout();
            this.customColorGroupBox3.ResumeLayout(false);
            this.customColorGroupBox3.PerformLayout();
            this.customColorGroupBox4.ResumeLayout(false);
            this.customColorGroupBox4.PerformLayout();
            this.TranslatePanel.ResumeLayout(false);
            this.TranslatePanel.PerformLayout();
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomColorGroupBox customColorGroupBox1;
        private CustomColorGroupBox customColorGroupBox2;
        private System.Windows.Forms.Button ExportallTextButton;
        private System.Windows.Forms.Button ImportAllTextButton;
        private System.Windows.Forms.Label label1;
        private FEBuilderGBA.TextBoxEx FontROMTextBox;
        private System.Windows.Forms.Button FontROMSelectButton;
        private System.Windows.Forms.Button ImportFontButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Translate_to;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox Translate_from;
        private CustomColorGroupBox customColorGroupBox3;
        private System.Windows.Forms.Label label4;
        private CustomColorGroupBox customColorGroupBox4;
        private System.Windows.Forms.CheckBox useAutoTranslateCheckBox;
        private System.Windows.Forms.Panel TranslatePanel;
        private System.Windows.Forms.Label LabelTranslateFormROMFilename;
        private FEBuilderGBA.TextBoxEx TranslateFormROMFilename;
        private System.Windows.Forms.Button TranslateFormROMFilenameSelectButton;
        private System.Windows.Forms.Label LabelTranslateToROMFilename;
        private FEBuilderGBA.TextBoxEx TranslateToROMFilename;
        private System.Windows.Forms.Button TranslateToROMFilenameSelectButton;
        private System.Windows.Forms.CheckBox FontAutoGenelateCheckBox;
        private System.Windows.Forms.Button UseFontNameButton;
        private TextBoxEx UseFontNameTextEdit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel FontAutoGenelatePanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LabelSimpleTranslateToROMFilename;
        private TextBoxEx SimpleTranslateToROMFilename;
        private System.Windows.Forms.Button SimpleTranslateToROMFilenameSelectButton;
        private System.Windows.Forms.Label LabelSimpleTranslateFromROMFilename;
        private TextBoxEx SimpleTranslateFromROMFilename;
        private System.Windows.Forms.Button SimpleTranslateFormROMFilenameSelectButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SimpleFireButton;
        private System.Windows.Forms.CheckBox X_MODIFIED_TEXT_ONLY;
        private TextBoxEx SimpleTranslateToTranslateDataFilename;
        private System.Windows.Forms.Button SimpleTranslateToTranslateDataFilenameButton;
        private System.Windows.Forms.Label LabelSimpleTranslateToTranslateDataFilename;
        private System.Windows.Forms.CheckBox X_ONELINER_CHECK;
        private System.Windows.Forms.CheckBox X_OVERRAIDE_JPFONT;
        private System.Windows.Forms.CheckBox SIMPLE_OVERRAIDE_JPFONT;
        private System.Windows.Forms.Label label3;
        private TextBoxEx ExtraFontToROMFilename;
        private System.Windows.Forms.Button ExtraFontToROMFilenameSelectButton;
    }
}