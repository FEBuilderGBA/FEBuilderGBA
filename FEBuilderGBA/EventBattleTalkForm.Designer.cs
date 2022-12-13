namespace FEBuilderGBA
{
    partial class EventBattleTalkForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.L_12_NEWALLOC_ALTEVENT_W8 = new System.Windows.Forms.Button();
            this.L_12_EVENT = new FEBuilderGBA.TextBoxEx();
            this.J_12_EVENT = new System.Windows.Forms.Label();
            this.P12 = new System.Windows.Forms.NumericUpDown();
            this.EXPLAIN = new FEBuilderGBA.TextBoxEx();
            this.J_5 = new System.Windows.Forms.Label();
            this.L_6_FLAG = new FEBuilderGBA.TextBoxEx();
            this.L_2_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.L_0_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.L_4_MAP_ANYFF = new FEBuilderGBA.TextBoxEx();
            this.W2 = new System.Windows.Forms.NumericUpDown();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.L_2_UNIT_ANY = new FEBuilderGBA.TextBoxEx();
            this.L_0_UNIT_ANY = new FEBuilderGBA.TextBoxEx();
            this.L_8_TEXT_DEATHQUOTE = new FEBuilderGBA.TextBoxEx();
            this.B11 = new System.Windows.Forms.NumericUpDown();
            this.B10 = new System.Windows.Forms.NumericUpDown();
            this.J_10 = new System.Windows.Forms.Label();
            this.W6 = new System.Windows.Forms.NumericUpDown();
            this.B5 = new System.Windows.Forms.NumericUpDown();
            this.B4 = new System.Windows.Forms.NumericUpDown();
            this.W8 = new System.Windows.Forms.NumericUpDown();
            this.J_8_TEXT = new System.Windows.Forms.Label();
            this.J_6_FLAG = new System.Windows.Forms.Label();
            this.J_4_MAP = new System.Windows.Forms.Label();
            this.J_2_UNIT = new System.Windows.Forms.Label();
            this.J_0_UNIT = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_2_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W8)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1413, 30);
            this.panel1.TabIndex = 68;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(526, -2);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 70;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(311, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(402, 1);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(124, 1);
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
            this.AddressPanel.Location = new System.Drawing.Point(538, 38);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(884, 30);
            this.AddressPanel.TabIndex = 69;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(328, 1);
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
            this.label3.Location = new System.Drawing.Point(242, -1);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(541, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(150, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(416, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(711, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(168, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(90, 2);
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
            this.label55.Location = new System.Drawing.Point(-1, 0);
            this.label55.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(84, 30);
            this.label55.TabIndex = 1;
            this.label55.Text = "アドレス";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.L_12_NEWALLOC_ALTEVENT_W8);
            this.panel2.Controls.Add(this.L_12_EVENT);
            this.panel2.Controls.Add(this.J_12_EVENT);
            this.panel2.Controls.Add(this.P12);
            this.panel2.Controls.Add(this.EXPLAIN);
            this.panel2.Controls.Add(this.J_5);
            this.panel2.Controls.Add(this.L_6_FLAG);
            this.panel2.Controls.Add(this.L_2_UNITICON);
            this.panel2.Controls.Add(this.L_0_UNITICON);
            this.panel2.Controls.Add(this.L_4_MAP_ANYFF);
            this.panel2.Controls.Add(this.W2);
            this.panel2.Controls.Add(this.W0);
            this.panel2.Controls.Add(this.L_2_UNIT_ANY);
            this.panel2.Controls.Add(this.L_0_UNIT_ANY);
            this.panel2.Controls.Add(this.L_8_TEXT_DEATHQUOTE);
            this.panel2.Controls.Add(this.B11);
            this.panel2.Controls.Add(this.B10);
            this.panel2.Controls.Add(this.J_10);
            this.panel2.Controls.Add(this.W6);
            this.panel2.Controls.Add(this.B5);
            this.panel2.Controls.Add(this.B4);
            this.panel2.Controls.Add(this.W8);
            this.panel2.Controls.Add(this.J_8_TEXT);
            this.panel2.Controls.Add(this.J_6_FLAG);
            this.panel2.Controls.Add(this.J_4_MAP);
            this.panel2.Controls.Add(this.J_2_UNIT);
            this.panel2.Controls.Add(this.J_0_UNIT);
            this.panel2.Location = new System.Drawing.Point(538, 68);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(885, 697);
            this.panel2.TabIndex = 70;
            // 
            // L_12_NEWALLOC_ALTEVENT_W8
            // 
            this.L_12_NEWALLOC_ALTEVENT_W8.Location = new System.Drawing.Point(313, 372);
            this.L_12_NEWALLOC_ALTEVENT_W8.Name = "L_12_NEWALLOC_ALTEVENT_W8";
            this.L_12_NEWALLOC_ALTEVENT_W8.Size = new System.Drawing.Size(300, 28);
            this.L_12_NEWALLOC_ALTEVENT_W8.TabIndex = 8;
            this.L_12_NEWALLOC_ALTEVENT_W8.Text = "新規イベント";
            this.L_12_NEWALLOC_ALTEVENT_W8.UseVisualStyleBackColor = true;
            // 
            // L_12_EVENT
            // 
            this.L_12_EVENT.ErrorMessage = "";
            this.L_12_EVENT.Location = new System.Drawing.Point(313, 374);
            this.L_12_EVENT.Margin = new System.Windows.Forms.Padding(2);
            this.L_12_EVENT.Name = "L_12_EVENT";
            this.L_12_EVENT.Placeholder = "";
            this.L_12_EVENT.ReadOnly = true;
            this.L_12_EVENT.Size = new System.Drawing.Size(301, 25);
            this.L_12_EVENT.TabIndex = 9;
            this.L_12_EVENT.Visible = false;
            // 
            // J_12_EVENT
            // 
            this.J_12_EVENT.AccessibleDescription = "@ALTEVENT";
            this.J_12_EVENT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_12_EVENT.Location = new System.Drawing.Point(2, 370);
            this.J_12_EVENT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_12_EVENT.Name = "J_12_EVENT";
            this.J_12_EVENT.Size = new System.Drawing.Size(150, 32);
            this.J_12_EVENT.TabIndex = 245;
            this.J_12_EVENT.Text = "イベント";
            this.J_12_EVENT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P12
            // 
            this.P12.Hexadecimal = true;
            this.P12.Location = new System.Drawing.Point(160, 375);
            this.P12.Margin = new System.Windows.Forms.Padding(2);
            this.P12.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.P12.Name = "P12";
            this.P12.Size = new System.Drawing.Size(149, 25);
            this.P12.TabIndex = 7;
            // 
            // EXPLAIN
            // 
            this.EXPLAIN.ErrorMessage = "";
            this.EXPLAIN.Location = new System.Drawing.Point(3, 453);
            this.EXPLAIN.Multiline = true;
            this.EXPLAIN.Name = "EXPLAIN";
            this.EXPLAIN.Placeholder = "";
            this.EXPLAIN.ReadOnly = true;
            this.EXPLAIN.Size = new System.Drawing.Size(813, 239);
            this.EXPLAIN.TabIndex = 242;
            this.EXPLAIN.Text = "指定したユニット同士が、指定した章で戦闘を行った時の設定をします。\r\n戦闘に入ると、「テキスト」を表示し、「達成フラグ」が有効になります。\r\nボスとの戦闘の場合、" +
    "フラグは、0x01 「ボス会話フラグ」 が利用されることが多いです。";
            // 
            // J_5
            // 
            this.J_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_5.Location = new System.Drawing.Point(2, 116);
            this.J_5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_5.Name = "J_5";
            this.J_5.Size = new System.Drawing.Size(150, 32);
            this.J_5.TabIndex = 193;
            this.J_5.Text = "??";
            this.J_5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_6_FLAG
            // 
            this.L_6_FLAG.ErrorMessage = "";
            this.L_6_FLAG.Location = new System.Drawing.Point(253, 156);
            this.L_6_FLAG.Margin = new System.Windows.Forms.Padding(2);
            this.L_6_FLAG.Name = "L_6_FLAG";
            this.L_6_FLAG.Placeholder = "";
            this.L_6_FLAG.ReadOnly = true;
            this.L_6_FLAG.Size = new System.Drawing.Size(328, 25);
            this.L_6_FLAG.TabIndex = 192;
            // 
            // L_2_UNITICON
            // 
            this.L_2_UNITICON.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.L_2_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_2_UNITICON.Location = new System.Drawing.Point(549, 37);
            this.L_2_UNITICON.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_2_UNITICON.Name = "L_2_UNITICON";
            this.L_2_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_2_UNITICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_2_UNITICON.TabIndex = 191;
            this.L_2_UNITICON.TabStop = false;
            // 
            // L_0_UNITICON
            // 
            this.L_0_UNITICON.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.L_0_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_UNITICON.Location = new System.Drawing.Point(549, 3);
            this.L_0_UNITICON.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_UNITICON.Name = "L_0_UNITICON";
            this.L_0_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_UNITICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_0_UNITICON.TabIndex = 190;
            this.L_0_UNITICON.TabStop = false;
            // 
            // L_4_MAP_ANYFF
            // 
            this.L_4_MAP_ANYFF.ErrorMessage = "";
            this.L_4_MAP_ANYFF.Location = new System.Drawing.Point(242, 85);
            this.L_4_MAP_ANYFF.Margin = new System.Windows.Forms.Padding(2);
            this.L_4_MAP_ANYFF.Name = "L_4_MAP_ANYFF";
            this.L_4_MAP_ANYFF.Placeholder = "";
            this.L_4_MAP_ANYFF.ReadOnly = true;
            this.L_4_MAP_ANYFF.Size = new System.Drawing.Size(339, 25);
            this.L_4_MAP_ANYFF.TabIndex = 176;
            // 
            // W2
            // 
            this.W2.Hexadecimal = true;
            this.W2.Location = new System.Drawing.Point(160, 46);
            this.W2.Margin = new System.Windows.Forms.Padding(2);
            this.W2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W2.Name = "W2";
            this.W2.Size = new System.Drawing.Size(68, 25);
            this.W2.TabIndex = 1;
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(160, 10);
            this.W0.Margin = new System.Windows.Forms.Padding(2);
            this.W0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(68, 25);
            this.W0.TabIndex = 0;
            // 
            // L_2_UNIT_ANY
            // 
            this.L_2_UNIT_ANY.ErrorMessage = "";
            this.L_2_UNIT_ANY.Location = new System.Drawing.Point(242, 44);
            this.L_2_UNIT_ANY.Margin = new System.Windows.Forms.Padding(2);
            this.L_2_UNIT_ANY.Name = "L_2_UNIT_ANY";
            this.L_2_UNIT_ANY.Placeholder = "";
            this.L_2_UNIT_ANY.ReadOnly = true;
            this.L_2_UNIT_ANY.Size = new System.Drawing.Size(296, 25);
            this.L_2_UNIT_ANY.TabIndex = 173;
            // 
            // L_0_UNIT_ANY
            // 
            this.L_0_UNIT_ANY.ErrorMessage = "";
            this.L_0_UNIT_ANY.Location = new System.Drawing.Point(242, 10);
            this.L_0_UNIT_ANY.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_UNIT_ANY.Name = "L_0_UNIT_ANY";
            this.L_0_UNIT_ANY.Placeholder = "";
            this.L_0_UNIT_ANY.ReadOnly = true;
            this.L_0_UNIT_ANY.Size = new System.Drawing.Size(296, 25);
            this.L_0_UNIT_ANY.TabIndex = 172;
            // 
            // L_8_TEXT_DEATHQUOTE
            // 
            this.L_8_TEXT_DEATHQUOTE.ErrorMessage = "";
            this.L_8_TEXT_DEATHQUOTE.Location = new System.Drawing.Point(70, 225);
            this.L_8_TEXT_DEATHQUOTE.Margin = new System.Windows.Forms.Padding(2);
            this.L_8_TEXT_DEATHQUOTE.Multiline = true;
            this.L_8_TEXT_DEATHQUOTE.Name = "L_8_TEXT_DEATHQUOTE";
            this.L_8_TEXT_DEATHQUOTE.Placeholder = "";
            this.L_8_TEXT_DEATHQUOTE.ReadOnly = true;
            this.L_8_TEXT_DEATHQUOTE.Size = new System.Drawing.Size(746, 84);
            this.L_8_TEXT_DEATHQUOTE.TabIndex = 171;
            // 
            // B11
            // 
            this.B11.Hexadecimal = true;
            this.B11.Location = new System.Drawing.Point(242, 338);
            this.B11.Margin = new System.Windows.Forms.Padding(2);
            this.B11.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B11.Name = "B11";
            this.B11.Size = new System.Drawing.Size(68, 25);
            this.B11.TabIndex = 164;
            // 
            // B10
            // 
            this.B10.Hexadecimal = true;
            this.B10.Location = new System.Drawing.Point(160, 338);
            this.B10.Margin = new System.Windows.Forms.Padding(2);
            this.B10.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B10.Name = "B10";
            this.B10.Size = new System.Drawing.Size(68, 25);
            this.B10.TabIndex = 6;
            // 
            // J_10
            // 
            this.J_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_10.Location = new System.Drawing.Point(2, 335);
            this.J_10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_10.Name = "J_10";
            this.J_10.Size = new System.Drawing.Size(150, 32);
            this.J_10.TabIndex = 162;
            this.J_10.Text = "??";
            this.J_10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // W6
            // 
            this.W6.Hexadecimal = true;
            this.W6.Location = new System.Drawing.Point(160, 155);
            this.W6.Margin = new System.Windows.Forms.Padding(2);
            this.W6.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W6.Name = "W6";
            this.W6.Size = new System.Drawing.Size(88, 25);
            this.W6.TabIndex = 4;
            // 
            // B5
            // 
            this.B5.Hexadecimal = true;
            this.B5.Location = new System.Drawing.Point(160, 118);
            this.B5.Margin = new System.Windows.Forms.Padding(2);
            this.B5.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B5.Name = "B5";
            this.B5.Size = new System.Drawing.Size(68, 25);
            this.B5.TabIndex = 3;
            // 
            // B4
            // 
            this.B4.Hexadecimal = true;
            this.B4.Location = new System.Drawing.Point(160, 85);
            this.B4.Margin = new System.Windows.Forms.Padding(2);
            this.B4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B4.Name = "B4";
            this.B4.Size = new System.Drawing.Size(68, 25);
            this.B4.TabIndex = 2;
            // 
            // W8
            // 
            this.W8.Hexadecimal = true;
            this.W8.Location = new System.Drawing.Point(160, 194);
            this.W8.Margin = new System.Windows.Forms.Padding(2);
            this.W8.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W8.Name = "W8";
            this.W8.Size = new System.Drawing.Size(88, 25);
            this.W8.TabIndex = 5;
            // 
            // J_8_TEXT
            // 
            this.J_8_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8_TEXT.Location = new System.Drawing.Point(2, 189);
            this.J_8_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_8_TEXT.Name = "J_8_TEXT";
            this.J_8_TEXT.Size = new System.Drawing.Size(150, 32);
            this.J_8_TEXT.TabIndex = 6;
            this.J_8_TEXT.Text = "テキスト";
            this.J_8_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_6_FLAG
            // 
            this.J_6_FLAG.AccessibleDescription = "@AchievementFlag";
            this.J_6_FLAG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_6_FLAG.Location = new System.Drawing.Point(2, 152);
            this.J_6_FLAG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_6_FLAG.Name = "J_6_FLAG";
            this.J_6_FLAG.Size = new System.Drawing.Size(150, 32);
            this.J_6_FLAG.TabIndex = 5;
            this.J_6_FLAG.Text = "達成フラグ";
            this.J_6_FLAG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_4_MAP
            // 
            this.J_4_MAP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_MAP.Location = new System.Drawing.Point(2, 82);
            this.J_4_MAP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_MAP.Name = "J_4_MAP";
            this.J_4_MAP.Size = new System.Drawing.Size(150, 32);
            this.J_4_MAP.TabIndex = 4;
            this.J_4_MAP.Text = "マップ";
            this.J_4_MAP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_2_UNIT
            // 
            this.J_2_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2_UNIT.Location = new System.Drawing.Point(2, 44);
            this.J_2_UNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2_UNIT.Name = "J_2_UNIT";
            this.J_2_UNIT.Size = new System.Drawing.Size(150, 32);
            this.J_2_UNIT.TabIndex = 3;
            this.J_2_UNIT.Text = "反撃側ユニット";
            this.J_2_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_0_UNIT
            // 
            this.J_0_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_UNIT.Location = new System.Drawing.Point(2, 10);
            this.J_0_UNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_UNIT.Name = "J_0_UNIT";
            this.J_0_UNIT.Size = new System.Drawing.Size(150, 32);
            this.J_0_UNIT.TabIndex = 2;
            this.J_0_UNIT.Text = "攻撃側ユニット";
            this.J_0_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(10, 40);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(524, 726);
            this.panel6.TabIndex = 150;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(4, 695);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(519, 30);
            this.AddressListExpandsButton.TabIndex = 144;
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
            this.LabelFilter.Size = new System.Drawing.Size(523, 26);
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
            this.AddressList.Size = new System.Drawing.Size(523, 652);
            this.AddressList.TabIndex = 0;
            // 
            // EventBattleTalkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1429, 777);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "EventBattleTalkForm";
            this.Text = "交戦会話";
            this.Load += new System.EventHandler(this.EventBattleTalkForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_2_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W8)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel AddressPanel;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label J_2_UNIT;
        private System.Windows.Forms.Label J_0_UNIT;
        private System.Windows.Forms.Label J_8_TEXT;
        private System.Windows.Forms.Label J_6_FLAG;
        private System.Windows.Forms.Label J_4_MAP;
        private System.Windows.Forms.NumericUpDown W8;
        private System.Windows.Forms.NumericUpDown B11;
        private System.Windows.Forms.NumericUpDown B10;
        private System.Windows.Forms.Label J_10;
        private System.Windows.Forms.NumericUpDown W6;
        private System.Windows.Forms.NumericUpDown B5;
        private System.Windows.Forms.NumericUpDown B4;
        private FEBuilderGBA.TextBoxEx L_8_TEXT_DEATHQUOTE;
        private System.Windows.Forms.NumericUpDown W2;
        private System.Windows.Forms.NumericUpDown W0;
        private FEBuilderGBA.TextBoxEx L_2_UNIT_ANY;
        private FEBuilderGBA.TextBoxEx L_0_UNIT_ANY;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private FEBuilderGBA.TextBoxEx L_4_MAP_ANYFF;
        private InterpolatedPictureBox L_2_UNITICON;
        private InterpolatedPictureBox L_0_UNITICON;
        private FEBuilderGBA.TextBoxEx L_6_FLAG;
        private System.Windows.Forms.Label J_5;
        private FEBuilderGBA.TextBoxEx EXPLAIN;
        private TextBoxEx L_12_EVENT;
        private System.Windows.Forms.Label J_12_EVENT;
        private System.Windows.Forms.NumericUpDown P12;
        private System.Windows.Forms.Button L_12_NEWALLOC_ALTEVENT_W8;
    }
}