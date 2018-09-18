namespace FEBuilderGBA
{
    partial class EventForceSortieFE7Form
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
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.N_L_0_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.N_L_0_UNIT = new FEBuilderGBA.TextBoxEx();
            this.N_B3 = new System.Windows.Forms.NumericUpDown();
            this.N_J_3 = new System.Windows.Forms.Label();
            this.N_B2 = new System.Windows.Forms.NumericUpDown();
            this.N_J_2 = new System.Windows.Forms.Label();
            this.N_B1 = new System.Windows.Forms.NumericUpDown();
            this.N_B0 = new System.Windows.Forms.NumericUpDown();
            this.N_J_1 = new System.Windows.Forms.Label();
            this.N_J_0_UNIT = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.N_BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label6 = new System.Windows.Forms.Label();
            this.N_SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.N_WriteButton = new System.Windows.Forms.Button();
            this.N_Address = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new ListBoxEx();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.N_ReloadListButton = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.N_ReadCount = new System.Windows.Forms.NumericUpDown();
            this.WriteButton = new System.Windows.Forms.Button();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.panel22 = new System.Windows.Forms.Panel();
            this.N_AddressList = new ListBoxEx();
            this.N_LabelFilter = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_L_0_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B0)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Address)).BeginInit();
            this.panel14.SuspendLayout();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            this.panel22.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(7, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1254, 30);
            this.panel1.TabIndex = 67;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(449, -2);
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
            this.label1.Location = new System.Drawing.Point(-1, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(125, 2);
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
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.N_L_0_UNITICON);
            this.panel3.Controls.Add(this.N_L_0_UNIT);
            this.panel3.Controls.Add(this.N_B3);
            this.panel3.Controls.Add(this.N_J_3);
            this.panel3.Controls.Add(this.N_B2);
            this.panel3.Controls.Add(this.N_J_2);
            this.panel3.Controls.Add(this.N_B1);
            this.panel3.Controls.Add(this.N_B0);
            this.panel3.Controls.Add(this.N_J_1);
            this.panel3.Controls.Add(this.N_J_0_UNIT);
            this.panel3.Location = new System.Drawing.Point(438, 160);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(821, 241);
            this.panel3.TabIndex = 179;
            // 
            // N_L_0_UNITICON
            // 
            this.N_L_0_UNITICON.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.N_L_0_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.N_L_0_UNITICON.Location = new System.Drawing.Point(412, 11);
            this.N_L_0_UNITICON.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.N_L_0_UNITICON.Name = "N_L_0_UNITICON";
            this.N_L_0_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.N_L_0_UNITICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.N_L_0_UNITICON.TabIndex = 192;
            this.N_L_0_UNITICON.TabStop = false;
            // 
            // N_L_0_UNIT
            // 
            this.N_L_0_UNIT.Location = new System.Drawing.Point(251, 13);
            this.N_L_0_UNIT.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_L_0_UNIT.Name = "N_L_0_UNIT";
            this.N_L_0_UNIT.ReadOnly = true;
            this.N_L_0_UNIT.Size = new System.Drawing.Size(161, 25);
            this.N_L_0_UNIT.TabIndex = 191;
            // 
            // N_B3
            // 
            this.N_B3.Hexadecimal = true;
            this.N_B3.Location = new System.Drawing.Point(157, 122);
            this.N_B3.Margin = new System.Windows.Forms.Padding(2);
            this.N_B3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N_B3.Name = "N_B3";
            this.N_B3.Size = new System.Drawing.Size(88, 25);
            this.N_B3.TabIndex = 190;
            // 
            // N_J_3
            // 
            this.N_J_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_3.Location = new System.Drawing.Point(2, 120);
            this.N_J_3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_3.Name = "N_J_3";
            this.N_J_3.Size = new System.Drawing.Size(135, 26);
            this.N_J_3.TabIndex = 189;
            this.N_J_3.Text = "??";
            this.N_J_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_B2
            // 
            this.N_B2.Hexadecimal = true;
            this.N_B2.Location = new System.Drawing.Point(157, 84);
            this.N_B2.Margin = new System.Windows.Forms.Padding(2);
            this.N_B2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N_B2.Name = "N_B2";
            this.N_B2.Size = new System.Drawing.Size(88, 25);
            this.N_B2.TabIndex = 188;
            // 
            // N_J_2
            // 
            this.N_J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_2.Location = new System.Drawing.Point(2, 83);
            this.N_J_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_2.Name = "N_J_2";
            this.N_J_2.Size = new System.Drawing.Size(135, 26);
            this.N_J_2.TabIndex = 187;
            this.N_J_2.Text = "??";
            this.N_J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_B1
            // 
            this.N_B1.Hexadecimal = true;
            this.N_B1.Location = new System.Drawing.Point(156, 49);
            this.N_B1.Margin = new System.Windows.Forms.Padding(2);
            this.N_B1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N_B1.Name = "N_B1";
            this.N_B1.Size = new System.Drawing.Size(88, 25);
            this.N_B1.TabIndex = 186;
            // 
            // N_B0
            // 
            this.N_B0.Hexadecimal = true;
            this.N_B0.Location = new System.Drawing.Point(156, 13);
            this.N_B0.Margin = new System.Windows.Forms.Padding(2);
            this.N_B0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N_B0.Name = "N_B0";
            this.N_B0.Size = new System.Drawing.Size(88, 25);
            this.N_B0.TabIndex = 185;
            // 
            // N_J_1
            // 
            this.N_J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_1.Location = new System.Drawing.Point(1, 49);
            this.N_J_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_1.Name = "N_J_1";
            this.N_J_1.Size = new System.Drawing.Size(135, 26);
            this.N_J_1.TabIndex = 184;
            this.N_J_1.Text = "??";
            this.N_J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_J_0_UNIT
            // 
            this.N_J_0_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_0_UNIT.Location = new System.Drawing.Point(1, 11);
            this.N_J_0_UNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_0_UNIT.Name = "N_J_0_UNIT";
            this.N_J_0_UNIT.Size = new System.Drawing.Size(135, 26);
            this.N_J_0_UNIT.TabIndex = 182;
            this.N_J_0_UNIT.Text = "ユニット";
            this.N_J_0_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.N_BlockSize);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.N_SelectAddress);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.N_WriteButton);
            this.panel5.Controls.Add(this.N_Address);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Location = new System.Drawing.Point(439, 73);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(821, 87);
            this.panel5.TabIndex = 177;
            // 
            // N_BlockSize
            // 
            this.N_BlockSize.Location = new System.Drawing.Point(91, 52);
            this.N_BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_BlockSize.Name = "N_BlockSize";
            this.N_BlockSize.ReadOnly = true;
            this.N_BlockSize.Size = new System.Drawing.Size(54, 25);
            this.N_BlockSize.TabIndex = 55;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(2, 49);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 30);
            this.label6.TabIndex = 56;
            this.label6.Text = "Size:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_SelectAddress
            // 
            this.N_SelectAddress.Location = new System.Drawing.Point(281, 53);
            this.N_SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_SelectAddress.Name = "N_SelectAddress";
            this.N_SelectAddress.ReadOnly = true;
            this.N_SelectAddress.Size = new System.Drawing.Size(129, 25);
            this.N_SelectAddress.TabIndex = 54;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(152, 52);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 53;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_WriteButton
            // 
            this.N_WriteButton.Location = new System.Drawing.Point(242, 2);
            this.N_WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_WriteButton.Name = "N_WriteButton";
            this.N_WriteButton.Size = new System.Drawing.Size(167, 30);
            this.N_WriteButton.TabIndex = 9;
            this.N_WriteButton.Text = "書き込み";
            this.N_WriteButton.UseVisualStyleBackColor = true;
            // 
            // N_Address
            // 
            this.N_Address.Hexadecimal = true;
            this.N_Address.Location = new System.Drawing.Point(97, 6);
            this.N_Address.Margin = new System.Windows.Forms.Padding(2);
            this.N_Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.N_Address.Name = "N_Address";
            this.N_Address.Size = new System.Drawing.Size(130, 25);
            this.N_Address.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(2, 8);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 30);
            this.label10.TabIndex = 1;
            this.label10.Text = "アドレス";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel14
            // 
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.LabelFilter);
            this.panel14.Controls.Add(this.AddressList);
            this.panel14.Location = new System.Drawing.Point(7, 42);
            this.panel14.Margin = new System.Windows.Forms.Padding(2);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(258, 358);
            this.panel14.TabIndex = 188;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-2, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(258, 31);
            this.LabelFilter.TabIndex = 106;
            this.LabelFilter.Text = "マップ名";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 28);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(258, 328);
            this.AddressList.TabIndex = 1;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // AddressPanel
            // 
            this.AddressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressPanel.Controls.Add(this.N_ReloadListButton);
            this.AddressPanel.Controls.Add(this.label15);
            this.AddressPanel.Controls.Add(this.N_ReadCount);
            this.AddressPanel.Controls.Add(this.WriteButton);
            this.AddressPanel.Controls.Add(this.P0);
            this.AddressPanel.Controls.Add(this.label46);
            this.AddressPanel.Location = new System.Drawing.Point(267, 42);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(994, 30);
            this.AddressPanel.TabIndex = 189;
            // 
            // N_ReloadListButton
            // 
            this.N_ReloadListButton.Location = new System.Drawing.Point(481, -1);
            this.N_ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_ReloadListButton.Name = "N_ReloadListButton";
            this.N_ReloadListButton.Size = new System.Drawing.Size(238, 32);
            this.N_ReloadListButton.TabIndex = 29;
            this.N_ReloadListButton.Text = "強制出撃ポイント再取得";
            this.N_ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Location = new System.Drawing.Point(307, 0);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(84, 32);
            this.label15.TabIndex = 24;
            this.label15.Text = "読込数";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_ReadCount
            // 

            this.N_ReadCount.Location = new System.Drawing.Point(397, 5);
            this.N_ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.N_ReadCount.Name = "N_ReadCount";
            this.N_ReadCount.Size = new System.Drawing.Size(78, 25);
            this.N_ReadCount.TabIndex = 28;
            this.N_ReadCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(744, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(247, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "強制出撃ポインタ書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(170, 4);
            this.P0.Margin = new System.Windows.Forms.Padding(2);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 8;
            // 
            // label46
            // 
            this.label46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label46.Location = new System.Drawing.Point(-2, -1);
            this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(170, 32);
            this.label46.TabIndex = 1;
            this.label46.Text = "強制出撃ポインタ";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel22
            // 
            this.panel22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel22.Controls.Add(this.N_AddressList);
            this.panel22.Controls.Add(this.N_LabelFilter);
            this.panel22.Location = new System.Drawing.Point(266, 73);
            this.panel22.Margin = new System.Windows.Forms.Padding(2);
            this.panel22.Name = "panel22";
            this.panel22.Size = new System.Drawing.Size(170, 327);
            this.panel22.TabIndex = 192;
            // 
            // N_AddressList
            // 
            this.N_AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.N_AddressList.FormattingEnabled = true;
            this.N_AddressList.ItemHeight = 18;
            this.N_AddressList.Location = new System.Drawing.Point(-1, 24);
            this.N_AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.N_AddressList.Name = "N_AddressList";
            this.N_AddressList.Size = new System.Drawing.Size(170, 292);
            this.N_AddressList.TabIndex = 109;
            // 
            // N_LabelFilter
            // 
            this.N_LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.N_LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_LabelFilter.Location = new System.Drawing.Point(-2, -1);
            this.N_LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_LabelFilter.Name = "N_LabelFilter";
            this.N_LabelFilter.Size = new System.Drawing.Size(171, 26);
            this.N_LabelFilter.TabIndex = 110;
            this.N_LabelFilter.Text = "名前";
            this.N_LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EventForceSortieFE7Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1280, 412);
            this.Controls.Add(this.panel22);
            this.Controls.Add(this.AddressPanel);
            this.Controls.Add(this.panel14);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "EventForceSortieFE7Form";
            this.Text = "強制出撃";
            this.Load += new System.EventHandler(this.MapExitPointForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_L_0_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B0)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Address)).EndInit();
            this.panel14.ResumeLayout(false);
            this.AddressPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            this.panel22.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button N_WriteButton;
        private System.Windows.Forms.NumericUpDown N_Address;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label N_J_0_UNIT;
        private System.Windows.Forms.Label N_J_1;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown N_ReadCount;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Panel panel22;
        private ListBoxEx N_AddressList;
        private System.Windows.Forms.Label N_LabelFilter;
        private FEBuilderGBA.TextBoxEx N_BlockSize;
        private System.Windows.Forms.Label label6;
        private FEBuilderGBA.TextBoxEx N_SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button N_ReloadListButton;
        private System.Windows.Forms.NumericUpDown N_B0;
        private System.Windows.Forms.NumericUpDown N_B3;
        private System.Windows.Forms.Label N_J_3;
        private System.Windows.Forms.NumericUpDown N_B2;
        private System.Windows.Forms.Label N_J_2;
        private System.Windows.Forms.NumericUpDown N_B1;
        private FEBuilderGBA.TextBoxEx N_L_0_UNIT;
        private InterpolatedPictureBox N_L_0_UNITICON;
    }
}