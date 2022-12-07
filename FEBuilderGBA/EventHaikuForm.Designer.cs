namespace FEBuilderGBA
{
    partial class EventHaikuForm
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
            this.B3 = new System.Windows.Forms.NumericUpDown();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.L_1_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.L_1_UNIT_ANY = new FEBuilderGBA.TextBoxEx();
            this.J_1 = new System.Windows.Forms.Label();
            this.L_8_NEWALLOC_ALTEVENT_W6 = new System.Windows.Forms.Button();
            this.L_8_EVENT = new FEBuilderGBA.TextBoxEx();
            this.P8 = new System.Windows.Forms.NumericUpDown();
            this.J_8_EVENT = new System.Windows.Forms.Label();
            this.L_2_COMBO = new System.Windows.Forms.ComboBox();
            this.EXPLAIN = new FEBuilderGBA.TextBoxEx();
            this.L_4_FLAG = new FEBuilderGBA.TextBoxEx();
            this.L_0_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.J_2 = new System.Windows.Forms.Label();
            this.L_3_MAP_ANYFF = new FEBuilderGBA.TextBoxEx();
            this.L_0_UNIT = new FEBuilderGBA.TextBoxEx();
            this.L_6_TEXT_DEATHQUOTE = new FEBuilderGBA.TextBoxEx();
            this.W4 = new System.Windows.Forms.NumericUpDown();
            this.W6 = new System.Windows.Forms.NumericUpDown();
            this.J_6_TEXT = new System.Windows.Forms.Label();
            this.J_4_FLAG = new System.Windows.Forms.Label();
            this.J_3_MAP = new System.Windows.Forms.Label();
            this.J_0_UNIT = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_1_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W6)).BeginInit();
            this.AddressPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // B3
            // 
            this.B3.Hexadecimal = true;
            this.B3.Location = new System.Drawing.Point(181, 117);
            this.B3.Margin = new System.Windows.Forms.Padding(2);
            this.B3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(68, 25);
            this.B3.TabIndex = 5;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(95, 2);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 8;
            // 
            // label55
            // 
            this.label55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label55.Location = new System.Drawing.Point(0, 0);
            this.label55.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(84, 30);
            this.label55.TabIndex = 1;
            this.label55.Text = "アドレス";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(181, 10);
            this.B0.Margin = new System.Windows.Forms.Padding(2);
            this.B0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(68, 25);
            this.B0.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.L_1_UNITICON);
            this.panel2.Controls.Add(this.B1);
            this.panel2.Controls.Add(this.L_1_UNIT_ANY);
            this.panel2.Controls.Add(this.J_1);
            this.panel2.Controls.Add(this.L_8_NEWALLOC_ALTEVENT_W6);
            this.panel2.Controls.Add(this.L_8_EVENT);
            this.panel2.Controls.Add(this.P8);
            this.panel2.Controls.Add(this.J_8_EVENT);
            this.panel2.Controls.Add(this.L_2_COMBO);
            this.panel2.Controls.Add(this.EXPLAIN);
            this.panel2.Controls.Add(this.L_4_FLAG);
            this.panel2.Controls.Add(this.L_0_UNITICON);
            this.panel2.Controls.Add(this.B2);
            this.panel2.Controls.Add(this.J_2);
            this.panel2.Controls.Add(this.B3);
            this.panel2.Controls.Add(this.B0);
            this.panel2.Controls.Add(this.L_3_MAP_ANYFF);
            this.panel2.Controls.Add(this.L_0_UNIT);
            this.panel2.Controls.Add(this.L_6_TEXT_DEATHQUOTE);
            this.panel2.Controls.Add(this.W4);
            this.panel2.Controls.Add(this.W6);
            this.panel2.Controls.Add(this.J_6_TEXT);
            this.panel2.Controls.Add(this.J_4_FLAG);
            this.panel2.Controls.Add(this.J_3_MAP);
            this.panel2.Controls.Add(this.J_0_UNIT);
            this.panel2.Location = new System.Drawing.Point(368, 74);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(859, 598);
            this.panel2.TabIndex = 74;
            // 
            // L_1_UNITICON
            // 
            this.L_1_UNITICON.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.L_1_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_1_UNITICON.Location = new System.Drawing.Point(600, 42);
            this.L_1_UNITICON.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_1_UNITICON.Name = "L_1_UNITICON";
            this.L_1_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_1_UNITICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_1_UNITICON.TabIndex = 252;
            this.L_1_UNITICON.TabStop = false;
            this.L_1_UNITICON.Visible = false;
            // 
            // B1
            // 
            this.B1.Hexadecimal = true;
            this.B1.Location = new System.Drawing.Point(181, 44);
            this.B1.Margin = new System.Windows.Forms.Padding(2);
            this.B1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(68, 25);
            this.B1.TabIndex = 1;
            // 
            // L_1_UNIT_ANY
            // 
            this.L_1_UNIT_ANY.ErrorMessage = "";
            this.L_1_UNIT_ANY.Location = new System.Drawing.Point(260, 44);
            this.L_1_UNIT_ANY.Margin = new System.Windows.Forms.Padding(2);
            this.L_1_UNIT_ANY.Name = "L_1_UNIT_ANY";
            this.L_1_UNIT_ANY.Placeholder = "";
            this.L_1_UNIT_ANY.ReadOnly = true;
            this.L_1_UNIT_ANY.Size = new System.Drawing.Size(337, 25);
            this.L_1_UNIT_ANY.TabIndex = 10;
            this.L_1_UNIT_ANY.Visible = false;
            // 
            // J_1
            // 
            this.J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_1.Location = new System.Drawing.Point(12, 44);
            this.J_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_1.Name = "J_1";
            this.J_1.Size = new System.Drawing.Size(165, 32);
            this.J_1.TabIndex = 249;
            this.J_1.Text = "??";
            this.J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_8_NEWALLOC_ALTEVENT_W6
            // 
            this.L_8_NEWALLOC_ALTEVENT_W6.Location = new System.Drawing.Point(342, 323);
            this.L_8_NEWALLOC_ALTEVENT_W6.Name = "L_8_NEWALLOC_ALTEVENT_W6";
            this.L_8_NEWALLOC_ALTEVENT_W6.Size = new System.Drawing.Size(300, 28);
            this.L_8_NEWALLOC_ALTEVENT_W6.TabIndex = 9;
            this.L_8_NEWALLOC_ALTEVENT_W6.Text = "新規イベント";
            this.L_8_NEWALLOC_ALTEVENT_W6.UseVisualStyleBackColor = true;
            // 
            // L_8_EVENT
            // 
            this.L_8_EVENT.ErrorMessage = "";
            this.L_8_EVENT.Location = new System.Drawing.Point(342, 326);
            this.L_8_EVENT.Margin = new System.Windows.Forms.Padding(2);
            this.L_8_EVENT.Name = "L_8_EVENT";
            this.L_8_EVENT.Placeholder = "";
            this.L_8_EVENT.ReadOnly = true;
            this.L_8_EVENT.Size = new System.Drawing.Size(301, 25);
            this.L_8_EVENT.TabIndex = 247;
            this.L_8_EVENT.Visible = false;
            // 
            // P8
            // 
            this.P8.Hexadecimal = true;
            this.P8.Location = new System.Drawing.Point(196, 327);
            this.P8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P8.Name = "P8";
            this.P8.Size = new System.Drawing.Size(141, 25);
            this.P8.TabIndex = 8;
            // 
            // J_8_EVENT
            // 
            this.J_8_EVENT.AccessibleDescription = "@ALTEVENT";
            this.J_8_EVENT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8_EVENT.Location = new System.Drawing.Point(12, 322);
            this.J_8_EVENT.Name = "J_8_EVENT";
            this.J_8_EVENT.Size = new System.Drawing.Size(177, 32);
            this.J_8_EVENT.TabIndex = 245;
            this.J_8_EVENT.Text = "イベント";
            this.J_8_EVENT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_2_COMBO
            // 
            this.L_2_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_2_COMBO.FormattingEnabled = true;
            this.L_2_COMBO.Items.AddRange(new object[] {
            "01=序盤",
            "02=エイリーク編",
            "03=エフラム編",
            "FF=無条件"});
            this.L_2_COMBO.Location = new System.Drawing.Point(260, 80);
            this.L_2_COMBO.Margin = new System.Windows.Forms.Padding(2);
            this.L_2_COMBO.Name = "L_2_COMBO";
            this.L_2_COMBO.Size = new System.Drawing.Size(293, 26);
            this.L_2_COMBO.TabIndex = 4;
            // 
            // EXPLAIN
            // 
            this.EXPLAIN.ErrorMessage = "";
            this.EXPLAIN.Location = new System.Drawing.Point(15, 385);
            this.EXPLAIN.Multiline = true;
            this.EXPLAIN.Name = "EXPLAIN";
            this.EXPLAIN.Placeholder = "";
            this.EXPLAIN.ReadOnly = true;
            this.EXPLAIN.Size = new System.Drawing.Size(810, 212);
            this.EXPLAIN.TabIndex = 239;
            this.EXPLAIN.Text = "ユニットが、指定した章で死亡した時に、設定をします。\r\nユニットが死亡すると、「テキスト」を表示し、「達成フラグ」が有効になります。\r\n「達成フラグ」を、「常時条" +
    "件」で拾うことで、ボス撃破のイベントや、ユニット生存に失敗イベントなどを実装できます。\r\nボス撃破には、通常はフラグ0x02を利用します。";
            // 
            // L_4_FLAG
            // 
            this.L_4_FLAG.ErrorMessage = "";
            this.L_4_FLAG.Location = new System.Drawing.Point(294, 152);
            this.L_4_FLAG.Margin = new System.Windows.Forms.Padding(2);
            this.L_4_FLAG.Name = "L_4_FLAG";
            this.L_4_FLAG.Placeholder = "";
            this.L_4_FLAG.ReadOnly = true;
            this.L_4_FLAG.Size = new System.Drawing.Size(338, 25);
            this.L_4_FLAG.TabIndex = 191;
            // 
            // L_0_UNITICON
            // 
            this.L_0_UNITICON.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.L_0_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_UNITICON.Location = new System.Drawing.Point(600, 8);
            this.L_0_UNITICON.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_UNITICON.Name = "L_0_UNITICON";
            this.L_0_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_UNITICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_0_UNITICON.TabIndex = 189;
            this.L_0_UNITICON.TabStop = false;
            // 
            // B2
            // 
            this.B2.Hexadecimal = true;
            this.B2.Location = new System.Drawing.Point(181, 79);
            this.B2.Margin = new System.Windows.Forms.Padding(2);
            this.B2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(68, 25);
            this.B2.TabIndex = 3;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(12, 79);
            this.J_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(165, 32);
            this.J_2.TabIndex = 176;
            this.J_2.Text = "編";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_3_MAP_ANYFF
            // 
            this.L_3_MAP_ANYFF.ErrorMessage = "";
            this.L_3_MAP_ANYFF.Location = new System.Drawing.Point(260, 117);
            this.L_3_MAP_ANYFF.Margin = new System.Windows.Forms.Padding(2);
            this.L_3_MAP_ANYFF.Name = "L_3_MAP_ANYFF";
            this.L_3_MAP_ANYFF.Placeholder = "";
            this.L_3_MAP_ANYFF.ReadOnly = true;
            this.L_3_MAP_ANYFF.Size = new System.Drawing.Size(372, 25);
            this.L_3_MAP_ANYFF.TabIndex = 173;
            // 
            // L_0_UNIT
            // 
            this.L_0_UNIT.ErrorMessage = "";
            this.L_0_UNIT.Location = new System.Drawing.Point(260, 10);
            this.L_0_UNIT.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_UNIT.Name = "L_0_UNIT";
            this.L_0_UNIT.Placeholder = "";
            this.L_0_UNIT.ReadOnly = true;
            this.L_0_UNIT.Size = new System.Drawing.Size(337, 25);
            this.L_0_UNIT.TabIndex = 172;
            // 
            // L_6_TEXT_DEATHQUOTE
            // 
            this.L_6_TEXT_DEATHQUOTE.ErrorMessage = "";
            this.L_6_TEXT_DEATHQUOTE.Location = new System.Drawing.Point(12, 223);
            this.L_6_TEXT_DEATHQUOTE.Margin = new System.Windows.Forms.Padding(2);
            this.L_6_TEXT_DEATHQUOTE.Multiline = true;
            this.L_6_TEXT_DEATHQUOTE.Name = "L_6_TEXT_DEATHQUOTE";
            this.L_6_TEXT_DEATHQUOTE.Placeholder = "";
            this.L_6_TEXT_DEATHQUOTE.ReadOnly = true;
            this.L_6_TEXT_DEATHQUOTE.Size = new System.Drawing.Size(829, 84);
            this.L_6_TEXT_DEATHQUOTE.TabIndex = 171;
            // 
            // W4
            // 
            this.W4.Hexadecimal = true;
            this.W4.Location = new System.Drawing.Point(181, 153);
            this.W4.Margin = new System.Windows.Forms.Padding(2);
            this.W4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W4.Name = "W4";
            this.W4.Size = new System.Drawing.Size(88, 25);
            this.W4.TabIndex = 6;
            // 
            // W6
            // 
            this.W6.Hexadecimal = true;
            this.W6.Location = new System.Drawing.Point(181, 190);
            this.W6.Margin = new System.Windows.Forms.Padding(2);
            this.W6.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W6.Name = "W6";
            this.W6.Size = new System.Drawing.Size(88, 25);
            this.W6.TabIndex = 7;
            // 
            // J_6_TEXT
            // 
            this.J_6_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_6_TEXT.Location = new System.Drawing.Point(12, 187);
            this.J_6_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_6_TEXT.Name = "J_6_TEXT";
            this.J_6_TEXT.Size = new System.Drawing.Size(165, 32);
            this.J_6_TEXT.TabIndex = 6;
            this.J_6_TEXT.Text = "テキスト";
            this.J_6_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_4_FLAG
            // 
            this.J_4_FLAG.AccessibleDescription = "@AchievementFlag";
            this.J_4_FLAG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_FLAG.Location = new System.Drawing.Point(12, 151);
            this.J_4_FLAG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_FLAG.Name = "J_4_FLAG";
            this.J_4_FLAG.Size = new System.Drawing.Size(165, 32);
            this.J_4_FLAG.TabIndex = 5;
            this.J_4_FLAG.Text = "達成フラグ";
            this.J_4_FLAG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_3_MAP
            // 
            this.J_3_MAP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_3_MAP.Location = new System.Drawing.Point(12, 115);
            this.J_3_MAP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_3_MAP.Name = "J_3_MAP";
            this.J_3_MAP.Size = new System.Drawing.Size(165, 32);
            this.J_3_MAP.TabIndex = 3;
            this.J_3_MAP.Text = "章ID";
            this.J_3_MAP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_0_UNIT
            // 
            this.J_0_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_UNIT.Location = new System.Drawing.Point(12, 10);
            this.J_0_UNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_UNIT.Name = "J_0_UNIT";
            this.J_0_UNIT.Size = new System.Drawing.Size(165, 32);
            this.J_0_UNIT.TabIndex = 2;
            this.J_0_UNIT.Text = "ユニット";
            this.J_0_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(697, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(163, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(569, 0);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 70;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(422, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(546, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(150, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // AddressPanel
            // 
            this.AddressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressPanel.Controls.Add(this.BlockSize);
            this.AddressPanel.Controls.Add(this.label3);
            this.AddressPanel.Controls.Add(this.SelectAddress);
            this.AddressPanel.Controls.Add(this.label22);
            this.AddressPanel.Controls.Add(this.WriteButton);
            this.AddressPanel.Controls.Add(this.Address);
            this.AddressPanel.Controls.Add(this.label55);
            this.AddressPanel.Location = new System.Drawing.Point(368, 44);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(860, 30);
            this.AddressPanel.TabIndex = 73;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(331, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(249, -1);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(351, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(16, 14);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1211, 30);
            this.panel1.TabIndex = 72;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(446, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(212, 2);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(16, 44);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(345, 631);
            this.panel6.TabIndex = 145;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(1, 597);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(340, 30);
            this.AddressListExpandsButton.TabIndex = 112;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, 2);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(348, 26);
            this.LabelFilter.TabIndex = 106;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 27);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(345, 562);
            this.AddressList.TabIndex = 0;
            // 
            // EventHaikuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1236, 686);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.AddressPanel);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "EventHaikuForm";
            this.Text = "死亡セリフ";
            this.Load += new System.EventHandler(this.EventHaikuForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_1_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W6)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown B3;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.Panel panel2;
        private FEBuilderGBA.TextBoxEx L_3_MAP_ANYFF;
        private FEBuilderGBA.TextBoxEx L_0_UNIT;
        private FEBuilderGBA.TextBoxEx L_6_TEXT_DEATHQUOTE;
        private System.Windows.Forms.NumericUpDown W4;
        private System.Windows.Forms.NumericUpDown W6;
        private System.Windows.Forms.Label J_6_TEXT;
        private System.Windows.Forms.Label J_4_FLAG;
        private System.Windows.Forms.Label J_3_MAP;
        private System.Windows.Forms.Label J_0_UNIT;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label22;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Panel AddressPanel;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.Label J_2;
        private InterpolatedPictureBox L_0_UNITICON;
        private FEBuilderGBA.TextBoxEx L_4_FLAG;
        private FEBuilderGBA.TextBoxEx EXPLAIN;
        private System.Windows.Forms.ComboBox L_2_COMBO;
        private TextBoxEx L_8_EVENT;
        private System.Windows.Forms.NumericUpDown P8;
        private System.Windows.Forms.Label J_8_EVENT;
        private System.Windows.Forms.Button L_8_NEWALLOC_ALTEVENT_W6;
        private InterpolatedPictureBox L_1_UNITICON;
        private System.Windows.Forms.NumericUpDown B1;
        private TextBoxEx L_1_UNIT_ANY;
        private System.Windows.Forms.Label J_1;
    }
}