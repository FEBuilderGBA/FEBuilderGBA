namespace FEBuilderGBA
{
    partial class SongTrackImportMidiForm
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
            this.OKButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.Mid2agbV = new System.Windows.Forms.NumericUpDown();
            this.Explain_mid2agb = new System.Windows.Forms.TextBox();
            this.Mid2AGBOKButton = new System.Windows.Forms.Button();
            this.customColorGroupBox2 = new FEBuilderGBA.CustomColorGroupBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.MuoonTabPage1 = new System.Windows.Forms.TabPage();
            this.UseMidi2agb_modsc_Nud = new System.Windows.Forms.NumericUpDown();
            this.UseMidi2agb_modsc_checkBox = new System.Windows.Forms.CheckBox();
            this.UseMidfix4agb_checkBox = new System.Windows.Forms.CheckBox();
            this.WARNING_midfix4agb_label = new System.Windows.Forms.Label();
            this.MuoonTabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.IgnoreMODCheckBox = new System.Windows.Forms.CheckBox();
            this.IgnoreLFOSCheckBox = new System.Windows.Forms.CheckBox();
            this.IgnoreBENDCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.customColorGroupBox3 = new FEBuilderGBA.CustomColorGroupBox();
            this.IgnoreBACKCheckBox = new System.Windows.Forms.CheckBox();
            this.IgnoreHEADCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InstrumentSelectComboBox = new System.Windows.Forms.ComboBox();
            this.J_4_INSTRUMENT_ADDR = new System.Windows.Forms.Label();
            this.Instrument = new System.Windows.Forms.NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Mid2agbV)).BeginInit();
            this.customColorGroupBox2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.MuoonTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UseMidi2agb_modsc_Nud)).BeginInit();
            this.MuoonTabPage2.SuspendLayout();
            this.customColorGroupBox3.SuspendLayout();
            this.customColorGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Instrument)).BeginInit();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(413, 187);
            this.OKButton.Margin = new System.Windows.Forms.Padding(4);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(264, 28);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "選択する";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 489);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(688, 259);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.customColorGroupBox3);
            this.tabPage1.Controls.Add(this.OKButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(680, 227);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "FEBuilderGBAでインポート";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.Mid2agbV);
            this.tabPage2.Controls.Add(this.Explain_mid2agb);
            this.tabPage2.Controls.Add(this.Mid2AGBOKButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(680, 227);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "mid2agbでインポート";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(15, 147);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 26);
            this.label4.TabIndex = 178;
            this.label4.Text = "マスターボリューム";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Mid2agbV
            // 
            this.Mid2agbV.Location = new System.Drawing.Point(263, 147);
            this.Mid2agbV.Margin = new System.Windows.Forms.Padding(2);
            this.Mid2agbV.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.Mid2agbV.Name = "Mid2agbV";
            this.Mid2agbV.Size = new System.Drawing.Size(134, 25);
            this.Mid2agbV.TabIndex = 179;
            // 
            // Explain_mid2agb
            // 
            this.Explain_mid2agb.Location = new System.Drawing.Point(15, 16);
            this.Explain_mid2agb.Multiline = true;
            this.Explain_mid2agb.Name = "Explain_mid2agb";
            this.Explain_mid2agb.ReadOnly = true;
            this.Explain_mid2agb.Size = new System.Drawing.Size(628, 108);
            this.Explain_mid2agb.TabIndex = 177;
            this.Explain_mid2agb.Text = "mid2agbで音楽をインポートします。\r\nループを作りたい場合は、DTMソフトでmidiにループラベルを入れてください。\r\n[ と ] のラベルを入れると、その" +
    "間がループとして変換されます。";
            // 
            // Mid2AGBOKButton
            // 
            this.Mid2AGBOKButton.Location = new System.Drawing.Point(392, 186);
            this.Mid2AGBOKButton.Margin = new System.Windows.Forms.Padding(4);
            this.Mid2AGBOKButton.Name = "Mid2AGBOKButton";
            this.Mid2AGBOKButton.Size = new System.Drawing.Size(264, 28);
            this.Mid2AGBOKButton.TabIndex = 176;
            this.Mid2AGBOKButton.Text = "選択する";
            this.Mid2AGBOKButton.UseVisualStyleBackColor = true;
            this.Mid2AGBOKButton.Click += new System.EventHandler(this.Mid2AGBOKButton_Click);
            // 
            // customColorGroupBox2
            // 
            this.customColorGroupBox2.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox2.Controls.Add(this.tabControl2);
            this.customColorGroupBox2.Controls.Add(this.label2);
            this.customColorGroupBox2.Location = new System.Drawing.Point(13, 228);
            this.customColorGroupBox2.Name = "customColorGroupBox2";
            this.customColorGroupBox2.Size = new System.Drawing.Size(687, 242);
            this.customColorGroupBox2.TabIndex = 176;
            this.customColorGroupBox2.TabStop = false;
            this.customColorGroupBox2.Text = "みょーん補正";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.MuoonTabPage1);
            this.tabControl2.Controls.Add(this.MuoonTabPage2);
            this.tabControl2.Location = new System.Drawing.Point(11, 65);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(650, 177);
            this.tabControl2.TabIndex = 8;
            // 
            // MuoonTabPage1
            // 
            this.MuoonTabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.MuoonTabPage1.Controls.Add(this.UseMidi2agb_modsc_Nud);
            this.MuoonTabPage1.Controls.Add(this.UseMidi2agb_modsc_checkBox);
            this.MuoonTabPage1.Controls.Add(this.UseMidfix4agb_checkBox);
            this.MuoonTabPage1.Controls.Add(this.WARNING_midfix4agb_label);
            this.MuoonTabPage1.Location = new System.Drawing.Point(4, 28);
            this.MuoonTabPage1.Name = "MuoonTabPage1";
            this.MuoonTabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.MuoonTabPage1.Size = new System.Drawing.Size(642, 145);
            this.MuoonTabPage1.TabIndex = 0;
            this.MuoonTabPage1.Text = "オプション";
            // 
            // UseMidi2agb_modsc_Nud
            // 
            this.UseMidi2agb_modsc_Nud.Location = new System.Drawing.Point(342, 39);
            this.UseMidi2agb_modsc_Nud.Margin = new System.Windows.Forms.Padding(2);
            this.UseMidi2agb_modsc_Nud.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.UseMidi2agb_modsc_Nud.Name = "UseMidi2agb_modsc_Nud";
            this.UseMidi2agb_modsc_Nud.Size = new System.Drawing.Size(134, 25);
            this.UseMidi2agb_modsc_Nud.TabIndex = 180;
            // 
            // UseMidi2agb_modsc_checkBox
            // 
            this.UseMidi2agb_modsc_checkBox.AutoSize = true;
            this.UseMidi2agb_modsc_checkBox.Location = new System.Drawing.Point(6, 42);
            this.UseMidi2agb_modsc_checkBox.Name = "UseMidi2agb_modsc_checkBox";
            this.UseMidi2agb_modsc_checkBox.Size = new System.Drawing.Size(255, 22);
            this.UseMidi2agb_modsc_checkBox.TabIndex = 8;
            this.UseMidi2agb_modsc_checkBox.Text = "midi2agbのmodscを有効にする";
            this.UseMidi2agb_modsc_checkBox.UseVisualStyleBackColor = true;
            this.UseMidi2agb_modsc_checkBox.CheckedChanged += new System.EventHandler(this.UseMidi2agb_modsc_checkBox_CheckedChanged);
            // 
            // UseMidfix4agb_checkBox
            // 
            this.UseMidfix4agb_checkBox.AutoSize = true;
            this.UseMidfix4agb_checkBox.Location = new System.Drawing.Point(6, 10);
            this.UseMidfix4agb_checkBox.Name = "UseMidfix4agb_checkBox";
            this.UseMidfix4agb_checkBox.Size = new System.Drawing.Size(191, 22);
            this.UseMidfix4agb_checkBox.TabIndex = 0;
            this.UseMidfix4agb_checkBox.Text = "midfix4agbを利用する";
            this.UseMidfix4agb_checkBox.UseVisualStyleBackColor = true;
            // 
            // WARNING_midfix4agb_label
            // 
            this.WARNING_midfix4agb_label.AutoSize = true;
            this.WARNING_midfix4agb_label.Location = new System.Drawing.Point(30, 12);
            this.WARNING_midfix4agb_label.Name = "WARNING_midfix4agb_label";
            this.WARNING_midfix4agb_label.Size = new System.Drawing.Size(310, 18);
            this.WARNING_midfix4agb_label.TabIndex = 7;
            this.WARNING_midfix4agb_label.Text = "midfix4agbのPATHが設定されていません。";
            // 
            // MuoonTabPage2
            // 
            this.MuoonTabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.MuoonTabPage2.Controls.Add(this.label5);
            this.MuoonTabPage2.Controls.Add(this.IgnoreMODCheckBox);
            this.MuoonTabPage2.Controls.Add(this.IgnoreLFOSCheckBox);
            this.MuoonTabPage2.Controls.Add(this.IgnoreBENDCheckBox);
            this.MuoonTabPage2.Location = new System.Drawing.Point(4, 28);
            this.MuoonTabPage2.Name = "MuoonTabPage2";
            this.MuoonTabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.MuoonTabPage2.Size = new System.Drawing.Size(642, 145);
            this.MuoonTabPage2.TabIndex = 1;
            this.MuoonTabPage2.Text = "古いオプション";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 12);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(426, 18);
            this.label5.TabIndex = 6;
            this.label5.Text = "それでも改善しない場合は、次のオプションも利用できます。";
            // 
            // IgnoreMODCheckBox
            // 
            this.IgnoreMODCheckBox.AutoSize = true;
            this.IgnoreMODCheckBox.Location = new System.Drawing.Point(7, 42);
            this.IgnoreMODCheckBox.Name = "IgnoreMODCheckBox";
            this.IgnoreMODCheckBox.Size = new System.Drawing.Size(240, 22);
            this.IgnoreMODCheckBox.TabIndex = 1;
            this.IgnoreMODCheckBox.Text = "MOD,MODT命令を無視する";
            this.IgnoreMODCheckBox.UseVisualStyleBackColor = true;
            // 
            // IgnoreLFOSCheckBox
            // 
            this.IgnoreLFOSCheckBox.AutoSize = true;
            this.IgnoreLFOSCheckBox.Location = new System.Drawing.Point(7, 108);
            this.IgnoreLFOSCheckBox.Name = "IgnoreLFOSCheckBox";
            this.IgnoreLFOSCheckBox.Size = new System.Drawing.Size(252, 22);
            this.IgnoreLFOSCheckBox.TabIndex = 3;
            this.IgnoreLFOSCheckBox.Text = "LFOS,LFODL命令を無視する";
            this.IgnoreLFOSCheckBox.UseVisualStyleBackColor = true;
            // 
            // IgnoreBENDCheckBox
            // 
            this.IgnoreBENDCheckBox.AutoSize = true;
            this.IgnoreBENDCheckBox.Location = new System.Drawing.Point(7, 75);
            this.IgnoreBENDCheckBox.Name = "IgnoreBENDCheckBox";
            this.IgnoreBENDCheckBox.Size = new System.Drawing.Size(254, 22);
            this.IgnoreBENDCheckBox.TabIndex = 2;
            this.IgnoreBENDCheckBox.Text = "BEND,BENDR命令を無視する";
            this.IgnoreBENDCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 33);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(388, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "音が「みょーん」となる場合は、有効にしてみてください。";
            // 
            // customColorGroupBox3
            // 
            this.customColorGroupBox3.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox3.Controls.Add(this.IgnoreBACKCheckBox);
            this.customColorGroupBox3.Controls.Add(this.IgnoreHEADCheckBox);
            this.customColorGroupBox3.Controls.Add(this.label3);
            this.customColorGroupBox3.Location = new System.Drawing.Point(6, 11);
            this.customColorGroupBox3.Name = "customColorGroupBox3";
            this.customColorGroupBox3.Size = new System.Drawing.Size(672, 155);
            this.customColorGroupBox3.TabIndex = 177;
            this.customColorGroupBox3.TabStop = false;
            this.customColorGroupBox3.Text = "トリミング";
            // 
            // IgnoreBACKCheckBox
            // 
            this.IgnoreBACKCheckBox.AutoSize = true;
            this.IgnoreBACKCheckBox.Location = new System.Drawing.Point(10, 104);
            this.IgnoreBACKCheckBox.Name = "IgnoreBACKCheckBox";
            this.IgnoreBACKCheckBox.Size = new System.Drawing.Size(287, 22);
            this.IgnoreBACKCheckBox.TabIndex = 2;
            this.IgnoreBACKCheckBox.Text = "楽譜の後方の無音区間を無視する";
            this.IgnoreBACKCheckBox.UseVisualStyleBackColor = true;
            // 
            // IgnoreHEADCheckBox
            // 
            this.IgnoreHEADCheckBox.AutoSize = true;
            this.IgnoreHEADCheckBox.Location = new System.Drawing.Point(10, 71);
            this.IgnoreHEADCheckBox.Name = "IgnoreHEADCheckBox";
            this.IgnoreHEADCheckBox.Size = new System.Drawing.Size(287, 22);
            this.IgnoreHEADCheckBox.TabIndex = 1;
            this.IgnoreHEADCheckBox.Text = "楽譜の前方の無音区間を無視する";
            this.IgnoreHEADCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 34);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "無音区間を除去します。";
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.label1);
            this.customColorGroupBox1.Controls.Add(this.InstrumentSelectComboBox);
            this.customColorGroupBox1.Controls.Add(this.J_4_INSTRUMENT_ADDR);
            this.customColorGroupBox1.Controls.Add(this.Instrument);
            this.customColorGroupBox1.Location = new System.Drawing.Point(13, 13);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(688, 199);
            this.customColorGroupBox1.TabIndex = 1;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "楽器";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(445, 108);
            this.label1.TabIndex = 0;
            this.label1.Text = "曲を演奏するときに利用する楽器を選択してください。\r\nディフォルトは現在の曲の楽器アドレスになっています。\r\n\r\nmidi楽器とFEの楽器は、並び順が違うので、注" +
    "意してください。\r\n自分で楽器の順番を調整するか、\r\nNIMAP(Native Instrument Map)パッチを利用してください。";
            // 
            // InstrumentSelectComboBox
            // 
            this.InstrumentSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InstrumentSelectComboBox.FormattingEnabled = true;
            this.InstrumentSelectComboBox.Location = new System.Drawing.Point(356, 159);
            this.InstrumentSelectComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.InstrumentSelectComboBox.Name = "InstrumentSelectComboBox";
            this.InstrumentSelectComboBox.Size = new System.Drawing.Size(325, 26);
            this.InstrumentSelectComboBox.TabIndex = 1;
            this.InstrumentSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.InstrumentSelectComboBox_SelectedIndexChanged);
            // 
            // J_4_INSTRUMENT_ADDR
            // 
            this.J_4_INSTRUMENT_ADDR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_INSTRUMENT_ADDR.Location = new System.Drawing.Point(10, 161);
            this.J_4_INSTRUMENT_ADDR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_INSTRUMENT_ADDR.Name = "J_4_INSTRUMENT_ADDR";
            this.J_4_INSTRUMENT_ADDR.Size = new System.Drawing.Size(202, 26);
            this.J_4_INSTRUMENT_ADDR.TabIndex = 172;
            this.J_4_INSTRUMENT_ADDR.Text = "楽器セット";
            this.J_4_INSTRUMENT_ADDR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Instrument
            // 
            this.Instrument.Hexadecimal = true;
            this.Instrument.Location = new System.Drawing.Point(216, 161);
            this.Instrument.Margin = new System.Windows.Forms.Padding(2);
            this.Instrument.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Instrument.Name = "Instrument";
            this.Instrument.Size = new System.Drawing.Size(134, 25);
            this.Instrument.TabIndex = 0;
            // 
            // SongTrackImportMidiForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(712, 756);
            this.Controls.Add(this.customColorGroupBox2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.customColorGroupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SongTrackImportMidiForm";
            this.Text = "midiインポート設定";
            this.Load += new System.EventHandler(this.SongTrackImportMidiForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Mid2agbV)).EndInit();
            this.customColorGroupBox2.ResumeLayout(false);
            this.customColorGroupBox2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.MuoonTabPage1.ResumeLayout(false);
            this.MuoonTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UseMidi2agb_modsc_Nud)).EndInit();
            this.MuoonTabPage2.ResumeLayout(false);
            this.MuoonTabPage2.PerformLayout();
            this.customColorGroupBox3.ResumeLayout(false);
            this.customColorGroupBox3.PerformLayout();
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Instrument)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ComboBox InstrumentSelectComboBox;
        private System.Windows.Forms.NumericUpDown Instrument;
        private System.Windows.Forms.Label J_4_INSTRUMENT_ADDR;
        private CustomColorGroupBox customColorGroupBox1;
        private CustomColorGroupBox customColorGroupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox IgnoreBENDCheckBox;
        private System.Windows.Forms.CheckBox IgnoreMODCheckBox;
        private CustomColorGroupBox customColorGroupBox3;
        private System.Windows.Forms.CheckBox IgnoreBACKCheckBox;
        private System.Windows.Forms.CheckBox IgnoreHEADCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox IgnoreLFOSCheckBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button Mid2AGBOKButton;
        private System.Windows.Forms.TextBox Explain_mid2agb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown Mid2agbV;
        private System.Windows.Forms.CheckBox UseMidfix4agb_checkBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label WARNING_midfix4agb_label;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage MuoonTabPage1;
        private System.Windows.Forms.TabPage MuoonTabPage2;
        private System.Windows.Forms.CheckBox UseMidi2agb_modsc_checkBox;
        private System.Windows.Forms.NumericUpDown UseMidi2agb_modsc_Nud;
    }
}