namespace FEBuilderGBA
{
    partial class SupportTalkFE6Form
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
            this.label13 = new System.Windows.Forms.Label();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.WriteButton = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.J_1_UNIT = new System.Windows.Forms.Label();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.L_1_UNIT = new FEBuilderGBA.TextBoxEx();
            this.J_0_UNIT = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.L_1_UNITICON_BIG = new FEBuilderGBA.InterpolatedPictureBox();
            this.B15 = new System.Windows.Forms.NumericUpDown();
            this.L_0_UNITICON_BIG = new FEBuilderGBA.InterpolatedPictureBox();
            this.B14 = new System.Windows.Forms.NumericUpDown();
            this.J_14 = new System.Windows.Forms.Label();
            this.L_12_TEXT_CONVERSATION = new FEBuilderGBA.TextBoxEx();
            this.W12 = new System.Windows.Forms.NumericUpDown();
            this.J_12_TEXT = new System.Windows.Forms.Label();
            this.L_8_TEXT_CONVERSATION = new FEBuilderGBA.TextBoxEx();
            this.W8 = new System.Windows.Forms.NumericUpDown();
            this.J_8_TEXT = new System.Windows.Forms.Label();
            this.L_4_TEXT_CONVERSATION = new FEBuilderGBA.TextBoxEx();
            this.W4 = new System.Windows.Forms.NumericUpDown();
            this.J_4_TEXT = new System.Windows.Forms.Label();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.L_0_UNIT = new FEBuilderGBA.TextBoxEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.AddressList = new ListBoxEx();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_1_UNITICON_BIG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON_BIG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.panel3.SuspendLayout();
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
            this.panel1.Location = new System.Drawing.Point(8, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1248, 30);
            this.panel1.TabIndex = 35;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(596, 0);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -1);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(413, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 

            this.ReadCount.Location = new System.Drawing.Point(501, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(176, 2);
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
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(218, -2);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 30);
            this.label13.TabIndex = 52;
            this.label13.Text = "Size:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(304, 0);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(516, -4);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(665, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(392, -2);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddressPanel
            // 
            this.AddressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressPanel.Controls.Add(this.BlockSize);
            this.AddressPanel.Controls.Add(this.label13);
            this.AddressPanel.Controls.Add(this.SelectAddress);
            this.AddressPanel.Controls.Add(this.label22);
            this.AddressPanel.Controls.Add(this.WriteButton);
            this.AddressPanel.Controls.Add(this.Address);
            this.AddressPanel.Controls.Add(this.label3);
            this.AddressPanel.Location = new System.Drawing.Point(422, 38);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(832, 30);
            this.AddressPanel.TabIndex = 32;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(88, 4);
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
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(-1, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 30);
            this.label3.TabIndex = 1;
            this.label3.Text = "アドレス";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_1_UNIT
            // 
            this.J_1_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_1_UNIT.Location = new System.Drawing.Point(14, 48);
            this.J_1_UNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_1_UNIT.Name = "J_1_UNIT";
            this.J_1_UNIT.Size = new System.Drawing.Size(124, 32);
            this.J_1_UNIT.TabIndex = 18;
            this.J_1_UNIT.Text = "支援相手2";
            this.J_1_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B1
            // 
            this.B1.Hexadecimal = true;
            this.B1.Location = new System.Drawing.Point(146, 46);
            this.B1.Margin = new System.Windows.Forms.Padding(2);
            this.B1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(60, 25);
            this.B1.TabIndex = 19;
            // 
            // L_1_UNIT
            // 
            this.L_1_UNIT.ErrorMessage = "";
            this.L_1_UNIT.Location = new System.Drawing.Point(218, 44);
            this.L_1_UNIT.Margin = new System.Windows.Forms.Padding(2);
            this.L_1_UNIT.Name = "L_1_UNIT";
            this.L_1_UNIT.Placeholder = "";
            this.L_1_UNIT.ReadOnly = true;
            this.L_1_UNIT.Size = new System.Drawing.Size(180, 25);
            this.L_1_UNIT.TabIndex = 20;
            // 
            // J_0_UNIT
            // 
            this.J_0_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_UNIT.Location = new System.Drawing.Point(14, 12);
            this.J_0_UNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_UNIT.Name = "J_0_UNIT";
            this.J_0_UNIT.Size = new System.Drawing.Size(124, 32);
            this.J_0_UNIT.TabIndex = 15;
            this.J_0_UNIT.Text = "支援相手1";
            this.J_0_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.L_1_UNITICON_BIG);
            this.panel2.Controls.Add(this.B15);
            this.panel2.Controls.Add(this.L_0_UNITICON_BIG);
            this.panel2.Controls.Add(this.B14);
            this.panel2.Controls.Add(this.J_14);
            this.panel2.Controls.Add(this.L_12_TEXT_CONVERSATION);
            this.panel2.Controls.Add(this.W12);
            this.panel2.Controls.Add(this.J_12_TEXT);
            this.panel2.Controls.Add(this.L_8_TEXT_CONVERSATION);
            this.panel2.Controls.Add(this.W8);
            this.panel2.Controls.Add(this.J_8_TEXT);
            this.panel2.Controls.Add(this.L_4_TEXT_CONVERSATION);
            this.panel2.Controls.Add(this.W4);
            this.panel2.Controls.Add(this.J_4_TEXT);
            this.panel2.Controls.Add(this.J_1_UNIT);
            this.panel2.Controls.Add(this.B1);
            this.panel2.Controls.Add(this.L_1_UNIT);
            this.panel2.Controls.Add(this.J_0_UNIT);
            this.panel2.Controls.Add(this.B0);
            this.panel2.Controls.Add(this.L_0_UNIT);
            this.panel2.Location = new System.Drawing.Point(422, 71);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(834, 448);
            this.panel2.TabIndex = 34;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(516, 49);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 28);
            this.label4.TabIndex = 187;
            this.label4.Text = "&&";
            // 
            // L_1_UNITICON_BIG
            // 
            this.L_1_UNITICON_BIG.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.L_1_UNITICON_BIG.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_1_UNITICON_BIG.Location = new System.Drawing.Point(575, 1);
            this.L_1_UNITICON_BIG.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_1_UNITICON_BIG.Name = "L_1_UNITICON_BIG";
            this.L_1_UNITICON_BIG.Size = new System.Drawing.Size(84, 84);
            this.L_1_UNITICON_BIG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_1_UNITICON_BIG.TabIndex = 186;
            this.L_1_UNITICON_BIG.TabStop = false;
            // 
            // B15
            // 
            this.B15.Hexadecimal = true;
            this.B15.Location = new System.Drawing.Point(230, 416);
            this.B15.Margin = new System.Windows.Forms.Padding(2);
            this.B15.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B15.Name = "B15";
            this.B15.Size = new System.Drawing.Size(60, 25);
            this.B15.TabIndex = 40;
            // 
            // L_0_UNITICON_BIG
            // 
            this.L_0_UNITICON_BIG.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.L_0_UNITICON_BIG.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_UNITICON_BIG.Location = new System.Drawing.Point(409, 1);
            this.L_0_UNITICON_BIG.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_UNITICON_BIG.Name = "L_0_UNITICON_BIG";
            this.L_0_UNITICON_BIG.Size = new System.Drawing.Size(84, 84);
            this.L_0_UNITICON_BIG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_0_UNITICON_BIG.TabIndex = 185;
            this.L_0_UNITICON_BIG.TabStop = false;
            // 
            // B14
            // 
            this.B14.Hexadecimal = true;
            this.B14.Location = new System.Drawing.Point(164, 416);
            this.B14.Margin = new System.Windows.Forms.Padding(2);
            this.B14.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B14.Name = "B14";
            this.B14.Size = new System.Drawing.Size(60, 25);
            this.B14.TabIndex = 39;
            // 
            // J_14
            // 
            this.J_14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_14.Location = new System.Drawing.Point(14, 410);
            this.J_14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_14.Name = "J_14";
            this.J_14.Size = new System.Drawing.Size(124, 32);
            this.J_14.TabIndex = 34;
            this.J_14.Text = "00";
            this.J_14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_12_TEXT_CONVERSATION
            // 
            this.L_12_TEXT_CONVERSATION.ErrorMessage = "";
            this.L_12_TEXT_CONVERSATION.Location = new System.Drawing.Point(146, 311);
            this.L_12_TEXT_CONVERSATION.Margin = new System.Windows.Forms.Padding(2);
            this.L_12_TEXT_CONVERSATION.Multiline = true;
            this.L_12_TEXT_CONVERSATION.Name = "L_12_TEXT_CONVERSATION";
            this.L_12_TEXT_CONVERSATION.Placeholder = "";
            this.L_12_TEXT_CONVERSATION.ReadOnly = true;
            this.L_12_TEXT_CONVERSATION.Size = new System.Drawing.Size(692, 84);
            this.L_12_TEXT_CONVERSATION.TabIndex = 33;
            // 
            // W12
            // 
            this.W12.Hexadecimal = true;
            this.W12.Location = new System.Drawing.Point(14, 346);
            this.W12.Margin = new System.Windows.Forms.Padding(2);
            this.W12.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W12.Name = "W12";
            this.W12.Size = new System.Drawing.Size(124, 25);
            this.W12.TabIndex = 32;
            // 
            // J_12_TEXT
            // 
            this.J_12_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_12_TEXT.Location = new System.Drawing.Point(14, 311);
            this.J_12_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_12_TEXT.Name = "J_12_TEXT";
            this.J_12_TEXT.Size = new System.Drawing.Size(124, 32);
            this.J_12_TEXT.TabIndex = 31;
            this.J_12_TEXT.Text = "A会話";
            this.J_12_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_8_TEXT_CONVERSATION
            // 
            this.L_8_TEXT_CONVERSATION.ErrorMessage = "";
            this.L_8_TEXT_CONVERSATION.Location = new System.Drawing.Point(146, 195);
            this.L_8_TEXT_CONVERSATION.Margin = new System.Windows.Forms.Padding(2);
            this.L_8_TEXT_CONVERSATION.Multiline = true;
            this.L_8_TEXT_CONVERSATION.Name = "L_8_TEXT_CONVERSATION";
            this.L_8_TEXT_CONVERSATION.Placeholder = "";
            this.L_8_TEXT_CONVERSATION.ReadOnly = true;
            this.L_8_TEXT_CONVERSATION.Size = new System.Drawing.Size(692, 84);
            this.L_8_TEXT_CONVERSATION.TabIndex = 30;
            // 
            // W8
            // 
            this.W8.Hexadecimal = true;
            this.W8.Location = new System.Drawing.Point(14, 230);
            this.W8.Margin = new System.Windows.Forms.Padding(2);
            this.W8.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W8.Name = "W8";
            this.W8.Size = new System.Drawing.Size(124, 25);
            this.W8.TabIndex = 29;
            // 
            // J_8_TEXT
            // 
            this.J_8_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8_TEXT.Location = new System.Drawing.Point(14, 195);
            this.J_8_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_8_TEXT.Name = "J_8_TEXT";
            this.J_8_TEXT.Size = new System.Drawing.Size(124, 32);
            this.J_8_TEXT.TabIndex = 28;
            this.J_8_TEXT.Text = "B会話";
            this.J_8_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_4_TEXT_CONVERSATION
            // 
            this.L_4_TEXT_CONVERSATION.ErrorMessage = "";
            this.L_4_TEXT_CONVERSATION.Location = new System.Drawing.Point(146, 90);
            this.L_4_TEXT_CONVERSATION.Margin = new System.Windows.Forms.Padding(2);
            this.L_4_TEXT_CONVERSATION.Multiline = true;
            this.L_4_TEXT_CONVERSATION.Name = "L_4_TEXT_CONVERSATION";
            this.L_4_TEXT_CONVERSATION.Placeholder = "";
            this.L_4_TEXT_CONVERSATION.ReadOnly = true;
            this.L_4_TEXT_CONVERSATION.Size = new System.Drawing.Size(692, 84);
            this.L_4_TEXT_CONVERSATION.TabIndex = 27;
            // 
            // W4
            // 
            this.W4.Hexadecimal = true;
            this.W4.Location = new System.Drawing.Point(14, 124);
            this.W4.Margin = new System.Windows.Forms.Padding(2);
            this.W4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W4.Name = "W4";
            this.W4.Size = new System.Drawing.Size(122, 25);
            this.W4.TabIndex = 26;
            // 
            // J_4_TEXT
            // 
            this.J_4_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_TEXT.Location = new System.Drawing.Point(14, 89);
            this.J_4_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_TEXT.Name = "J_4_TEXT";
            this.J_4_TEXT.Size = new System.Drawing.Size(124, 32);
            this.J_4_TEXT.TabIndex = 25;
            this.J_4_TEXT.Text = "C会話";
            this.J_4_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(146, 10);
            this.B0.Margin = new System.Windows.Forms.Padding(2);
            this.B0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(60, 25);
            this.B0.TabIndex = 16;
            // 
            // L_0_UNIT
            // 
            this.L_0_UNIT.ErrorMessage = "";
            this.L_0_UNIT.Location = new System.Drawing.Point(216, 8);
            this.L_0_UNIT.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_UNIT.Name = "L_0_UNIT";
            this.L_0_UNIT.Placeholder = "";
            this.L_0_UNIT.ReadOnly = true;
            this.L_0_UNIT.Size = new System.Drawing.Size(180, 25);
            this.L_0_UNIT.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.AddressListExpandsButton);
            this.panel3.Controls.Add(this.AddressList);
            this.panel3.Controls.Add(this.LabelFilter);
            this.panel3.Location = new System.Drawing.Point(8, 38);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(410, 481);
            this.panel3.TabIndex = 84;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(2, 449);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(256, 30);
            this.AddressListExpandsButton.TabIndex = 114;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(0, 29);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(409, 418);
            this.AddressList.TabIndex = 108;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(0, 0);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(409, 30);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SupportTalkFE6Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1263, 533);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressPanel);
            this.Controls.Add(this.panel2);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "SupportTalkFE6Form";
            this.Text = "支援会話";
            this.Load += new System.EventHandler(this.SupportTalkFE6Form_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_1_UNITICON_BIG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON_BIG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Label label13;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label J_1_UNIT;
        private System.Windows.Forms.NumericUpDown B1;
        private FEBuilderGBA.TextBoxEx L_1_UNIT;
        private System.Windows.Forms.Label J_0_UNIT;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown B0;
        private FEBuilderGBA.TextBoxEx L_0_UNIT;
        private System.Windows.Forms.NumericUpDown W4;
        private System.Windows.Forms.Label J_4_TEXT;
        private FEBuilderGBA.TextBoxEx L_12_TEXT_CONVERSATION;
        private System.Windows.Forms.NumericUpDown W12;
        private System.Windows.Forms.Label J_12_TEXT;
        private FEBuilderGBA.TextBoxEx L_8_TEXT_CONVERSATION;
        private System.Windows.Forms.Label J_8_TEXT;
        private FEBuilderGBA.TextBoxEx L_4_TEXT_CONVERSATION;
        private System.Windows.Forms.Panel panel3;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label LabelFilter;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.NumericUpDown B15;
        private System.Windows.Forms.NumericUpDown B14;
        private System.Windows.Forms.Label J_14;
        private System.Windows.Forms.NumericUpDown W8;
        private System.Windows.Forms.Label label4;
        private InterpolatedPictureBox L_1_UNITICON_BIG;
        private InterpolatedPictureBox L_0_UNITICON_BIG;
    }
}