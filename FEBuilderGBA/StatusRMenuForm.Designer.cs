namespace FEBuilderGBA
{
    partial class StatusRMenuForm
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
            this.label8 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label11 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label30 = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.J_0 = new System.Windows.Forms.Label();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.B16 = new System.Windows.Forms.NumericUpDown();
            this.B17 = new System.Windows.Forms.NumericUpDown();
            this.J_18_TEXT = new System.Windows.Forms.Label();
            this.W18 = new System.Windows.Forms.NumericUpDown();
            this.J_20_ASM = new System.Windows.Forms.Label();
            this.P20 = new System.Windows.Forms.NumericUpDown();
            this.J_24_ASM = new System.Windows.Forms.Label();
            this.P24 = new System.Windows.Forms.NumericUpDown();
            this.L_0_STATUSRMENU = new FEBuilderGBA.TextBoxEx();
            this.J_16 = new System.Windows.Forms.Label();
            this.J_17 = new System.Windows.Forms.Label();
            this.J_4 = new System.Windows.Forms.Label();
            this.P4 = new System.Windows.Forms.NumericUpDown();
            this.L_4_STATUSRMENU = new FEBuilderGBA.TextBoxEx();
            this.J_8 = new System.Windows.Forms.Label();
            this.P8 = new System.Windows.Forms.NumericUpDown();
            this.L_8_STATUSRMENU = new FEBuilderGBA.TextBoxEx();
            this.J_12 = new System.Windows.Forms.Label();
            this.P12 = new System.Windows.Forms.NumericUpDown();
            this.L_12_STATUSRMENU = new FEBuilderGBA.TextBoxEx();
            this.panel4 = new System.Windows.Forms.Panel();
            this.L_24_ASM = new FEBuilderGBA.TextBoxEx();
            this.L_20_ASM = new FEBuilderGBA.TextBoxEx();
            this.L_18_STATUSRMENU_TID = new FEBuilderGBA.TextBoxEx();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P12)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-1, -2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.BlockSize);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.SelectAddress);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.WriteButton);
            this.panel5.Controls.Add(this.Address);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(295, 45);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(917, 30);
            this.panel5.TabIndex = 147;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(326, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 63;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(240, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 30);
            this.label11.TabIndex = 64;
            this.label11.Text = "Size:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(540, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 62;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(414, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 61;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(749, -1);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 60;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(90, 4);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 55;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(-1, -2);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "アドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label30);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(12, 45);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(277, 519);
            this.panel6.TabIndex = 150;
            // 
            // label30
            // 
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Location = new System.Drawing.Point(0, -1);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(276, 26);
            this.label30.TabIndex = 106;
            this.label30.Text = "名前";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 25);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(277, 490);
            this.AddressList.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.FilterComboBox);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1200, 30);
            this.panel3.TabIndex = 148;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(823, -1);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 31;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Location = new System.Drawing.Point(282, 2);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(537, 26);
            this.FilterComboBox.TabIndex = 30;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(146, 3);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(-1, 0);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(409, 30);
            this.J_0.TabIndex = 78;
            this.J_0.Text = "上 RMenuPointer";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(414, 4);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 80;
            // 
            // B16
            // 
            this.B16.Location = new System.Drawing.Point(414, 122);
            this.B16.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B16.Name = "B16";
            this.B16.Size = new System.Drawing.Size(84, 25);
            this.B16.TabIndex = 85;
            // 
            // B17
            // 
            this.B17.Location = new System.Drawing.Point(414, 151);
            this.B17.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B17.Name = "B17";
            this.B17.Size = new System.Drawing.Size(84, 25);
            this.B17.TabIndex = 86;
            // 
            // J_18_TEXT
            // 
            this.J_18_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_18_TEXT.Location = new System.Drawing.Point(-1, 180);
            this.J_18_TEXT.Name = "J_18_TEXT";
            this.J_18_TEXT.Size = new System.Drawing.Size(409, 30);
            this.J_18_TEXT.TabIndex = 87;
            this.J_18_TEXT.Text = "TID";
            this.J_18_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // W18
            // 
            this.W18.Hexadecimal = true;
            this.W18.Location = new System.Drawing.Point(414, 182);
            this.W18.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.W18.Name = "W18";
            this.W18.Size = new System.Drawing.Size(111, 25);
            this.W18.TabIndex = 88;
            // 
            // J_20_ASM
            // 
            this.J_20_ASM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_20_ASM.Location = new System.Drawing.Point(-1, 256);
            this.J_20_ASM.Name = "J_20_ASM";
            this.J_20_ASM.Size = new System.Drawing.Size(409, 30);
            this.J_20_ASM.TabIndex = 90;
            this.J_20_ASM.Text = "Loop";
            this.J_20_ASM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P20
            // 
            this.P20.Hexadecimal = true;
            this.P20.Location = new System.Drawing.Point(414, 257);
            this.P20.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P20.Name = "P20";
            this.P20.Size = new System.Drawing.Size(130, 25);
            this.P20.TabIndex = 91;
            // 
            // J_24_ASM
            // 
            this.J_24_ASM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_24_ASM.Location = new System.Drawing.Point(-1, 285);
            this.J_24_ASM.Name = "J_24_ASM";
            this.J_24_ASM.Size = new System.Drawing.Size(409, 30);
            this.J_24_ASM.TabIndex = 92;
            this.J_24_ASM.Text = "Getter";
            this.J_24_ASM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P24
            // 
            this.P24.Hexadecimal = true;
            this.P24.Location = new System.Drawing.Point(414, 285);
            this.P24.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P24.Name = "P24";
            this.P24.Size = new System.Drawing.Size(130, 25);
            this.P24.TabIndex = 93;
            // 
            // L_0_STATUSRMENU
            // 
            this.L_0_STATUSRMENU.ErrorMessage = "";
            this.L_0_STATUSRMENU.Location = new System.Drawing.Point(552, 4);
            this.L_0_STATUSRMENU.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_0_STATUSRMENU.Name = "L_0_STATUSRMENU";
            this.L_0_STATUSRMENU.Placeholder = "";
            this.L_0_STATUSRMENU.ReadOnly = true;
            this.L_0_STATUSRMENU.Size = new System.Drawing.Size(357, 25);
            this.L_0_STATUSRMENU.TabIndex = 65;
            // 
            // J_16
            // 
            this.J_16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_16.Location = new System.Drawing.Point(-1, 120);
            this.J_16.Name = "J_16";
            this.J_16.Size = new System.Drawing.Size(409, 30);
            this.J_16.TabIndex = 114;
            this.J_16.Text = "X";
            this.J_16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_17
            // 
            this.J_17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_17.Location = new System.Drawing.Point(-1, 149);
            this.J_17.Name = "J_17";
            this.J_17.Size = new System.Drawing.Size(409, 30);
            this.J_17.TabIndex = 116;
            this.J_17.Text = "Y";
            this.J_17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(-1, 29);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(409, 30);
            this.J_4.TabIndex = 122;
            this.J_4.Text = "下 RMenuPointer";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P4
            // 
            this.P4.Hexadecimal = true;
            this.P4.Location = new System.Drawing.Point(414, 33);
            this.P4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P4.Name = "P4";
            this.P4.Size = new System.Drawing.Size(130, 25);
            this.P4.TabIndex = 123;
            // 
            // L_4_STATUSRMENU
            // 
            this.L_4_STATUSRMENU.ErrorMessage = "";
            this.L_4_STATUSRMENU.Location = new System.Drawing.Point(552, 33);
            this.L_4_STATUSRMENU.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_4_STATUSRMENU.Name = "L_4_STATUSRMENU";
            this.L_4_STATUSRMENU.Placeholder = "";
            this.L_4_STATUSRMENU.ReadOnly = true;
            this.L_4_STATUSRMENU.Size = new System.Drawing.Size(357, 25);
            this.L_4_STATUSRMENU.TabIndex = 121;
            // 
            // J_8
            // 
            this.J_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8.Location = new System.Drawing.Point(-1, 58);
            this.J_8.Name = "J_8";
            this.J_8.Size = new System.Drawing.Size(409, 30);
            this.J_8.TabIndex = 125;
            this.J_8.Text = "左 RMenuPointer";
            this.J_8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P8
            // 
            this.P8.Hexadecimal = true;
            this.P8.Location = new System.Drawing.Point(414, 62);
            this.P8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P8.Name = "P8";
            this.P8.Size = new System.Drawing.Size(130, 25);
            this.P8.TabIndex = 126;
            // 
            // L_8_STATUSRMENU
            // 
            this.L_8_STATUSRMENU.ErrorMessage = "";
            this.L_8_STATUSRMENU.Location = new System.Drawing.Point(552, 62);
            this.L_8_STATUSRMENU.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_8_STATUSRMENU.Name = "L_8_STATUSRMENU";
            this.L_8_STATUSRMENU.Placeholder = "";
            this.L_8_STATUSRMENU.ReadOnly = true;
            this.L_8_STATUSRMENU.Size = new System.Drawing.Size(357, 25);
            this.L_8_STATUSRMENU.TabIndex = 124;
            // 
            // J_12
            // 
            this.J_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_12.Location = new System.Drawing.Point(-1, 87);
            this.J_12.Name = "J_12";
            this.J_12.Size = new System.Drawing.Size(409, 30);
            this.J_12.TabIndex = 128;
            this.J_12.Text = "右 RMenuPointer";
            this.J_12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P12
            // 
            this.P12.Hexadecimal = true;
            this.P12.Location = new System.Drawing.Point(414, 91);
            this.P12.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P12.Name = "P12";
            this.P12.Size = new System.Drawing.Size(130, 25);
            this.P12.TabIndex = 129;
            // 
            // L_12_STATUSRMENU
            // 
            this.L_12_STATUSRMENU.ErrorMessage = "";
            this.L_12_STATUSRMENU.Location = new System.Drawing.Point(552, 91);
            this.L_12_STATUSRMENU.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_12_STATUSRMENU.Name = "L_12_STATUSRMENU";
            this.L_12_STATUSRMENU.Placeholder = "";
            this.L_12_STATUSRMENU.ReadOnly = true;
            this.L_12_STATUSRMENU.Size = new System.Drawing.Size(357, 25);
            this.L_12_STATUSRMENU.TabIndex = 127;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.L_24_ASM);
            this.panel4.Controls.Add(this.L_20_ASM);
            this.panel4.Controls.Add(this.L_18_STATUSRMENU_TID);
            this.panel4.Controls.Add(this.L_12_STATUSRMENU);
            this.panel4.Controls.Add(this.P12);
            this.panel4.Controls.Add(this.J_12);
            this.panel4.Controls.Add(this.L_8_STATUSRMENU);
            this.panel4.Controls.Add(this.P8);
            this.panel4.Controls.Add(this.J_8);
            this.panel4.Controls.Add(this.L_4_STATUSRMENU);
            this.panel4.Controls.Add(this.P4);
            this.panel4.Controls.Add(this.J_4);
            this.panel4.Controls.Add(this.J_17);
            this.panel4.Controls.Add(this.J_16);
            this.panel4.Controls.Add(this.L_0_STATUSRMENU);
            this.panel4.Controls.Add(this.P24);
            this.panel4.Controls.Add(this.J_24_ASM);
            this.panel4.Controls.Add(this.P20);
            this.panel4.Controls.Add(this.J_20_ASM);
            this.panel4.Controls.Add(this.W18);
            this.panel4.Controls.Add(this.J_18_TEXT);
            this.panel4.Controls.Add(this.B17);
            this.panel4.Controls.Add(this.B16);
            this.panel4.Controls.Add(this.P0);
            this.panel4.Controls.Add(this.J_0);
            this.panel4.Location = new System.Drawing.Point(295, 76);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(916, 488);
            this.panel4.TabIndex = 149;
            // 
            // L_24_ASM
            // 
            this.L_24_ASM.ErrorMessage = "";
            this.L_24_ASM.Location = new System.Drawing.Point(552, 286);
            this.L_24_ASM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_24_ASM.Name = "L_24_ASM";
            this.L_24_ASM.Placeholder = "";
            this.L_24_ASM.ReadOnly = true;
            this.L_24_ASM.Size = new System.Drawing.Size(357, 25);
            this.L_24_ASM.TabIndex = 132;
            this.L_24_ASM.Visible = false;
            // 
            // L_20_ASM
            // 
            this.L_20_ASM.ErrorMessage = "";
            this.L_20_ASM.Location = new System.Drawing.Point(552, 259);
            this.L_20_ASM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_20_ASM.Name = "L_20_ASM";
            this.L_20_ASM.Placeholder = "";
            this.L_20_ASM.ReadOnly = true;
            this.L_20_ASM.Size = new System.Drawing.Size(357, 25);
            this.L_20_ASM.TabIndex = 131;
            this.L_20_ASM.Visible = false;
            // 
            // L_18_STATUSRMENU_TID
            // 
            this.L_18_STATUSRMENU_TID.ErrorMessage = "";
            this.L_18_STATUSRMENU_TID.Location = new System.Drawing.Point(552, 182);
            this.L_18_STATUSRMENU_TID.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_18_STATUSRMENU_TID.Multiline = true;
            this.L_18_STATUSRMENU_TID.Name = "L_18_STATUSRMENU_TID";
            this.L_18_STATUSRMENU_TID.Placeholder = "";
            this.L_18_STATUSRMENU_TID.ReadOnly = true;
            this.L_18_STATUSRMENU_TID.Size = new System.Drawing.Size(357, 74);
            this.L_18_STATUSRMENU_TID.TabIndex = 130;
            // 
            // StatusRMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1222, 566);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Name = "StatusRMenuForm";
            this.Text = "ステータスRMenu";
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P12)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label30;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.NumericUpDown Address;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label11;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.NumericUpDown B16;
        private System.Windows.Forms.NumericUpDown B17;
        private System.Windows.Forms.Label J_18_TEXT;
        private System.Windows.Forms.NumericUpDown W18;
        private System.Windows.Forms.Label J_20_ASM;
        private System.Windows.Forms.NumericUpDown P20;
        private System.Windows.Forms.Label J_24_ASM;
        private System.Windows.Forms.NumericUpDown P24;
        private FEBuilderGBA.TextBoxEx L_0_STATUSRMENU;
        private System.Windows.Forms.Label J_16;
        private System.Windows.Forms.Label J_17;
        private System.Windows.Forms.Label J_4;
        private System.Windows.Forms.NumericUpDown P4;
        private FEBuilderGBA.TextBoxEx L_4_STATUSRMENU;
        private System.Windows.Forms.Label J_8;
        private System.Windows.Forms.NumericUpDown P8;
        private FEBuilderGBA.TextBoxEx L_8_STATUSRMENU;
        private System.Windows.Forms.Label J_12;
        private System.Windows.Forms.NumericUpDown P12;
        private FEBuilderGBA.TextBoxEx L_12_STATUSRMENU;
        private System.Windows.Forms.Panel panel4;
        private FEBuilderGBA.TextBoxEx L_18_STATUSRMENU_TID;
        private System.Windows.Forms.Button ReloadListButton;
        private FEBuilderGBA.TextBoxEx L_24_ASM;
        private FEBuilderGBA.TextBoxEx L_20_ASM;
    }
}