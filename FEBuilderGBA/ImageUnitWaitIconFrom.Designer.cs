﻿namespace FEBuilderGBA
{
    partial class ImageUnitWaitIconFrom
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
            this.label23 = new System.Windows.Forms.Label();
            this.DragTargetPanel = new System.Windows.Forms.Panel();
            this.L_2_COMBO = new System.Windows.Forms.ComboBox();
            this.J_2 = new System.Windows.Forms.Label();
            this.JumpToSystemPalette = new System.Windows.Forms.Label();
            this.Comment = new FEBuilderGBA.TextBoxEx();
            this.X_PALETTE = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.X_ONE_STEP = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.X_ONE_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.X_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.P4 = new System.Windows.Forms.NumericUpDown();
            this.J_4 = new System.Windows.Forms.Label();
            this.W2 = new System.Windows.Forms.NumericUpDown();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.J_0 = new System.Windows.Forms.Label();
            this.X_JUMP_MOVEICON = new System.Windows.Forms.Label();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton_255 = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.DragTargetPanel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.DragTargetPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_ONE_STEP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_ONE_PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            this.panel6.SuspendLayout();
            this.DragTargetPanel2.SuspendLayout();
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
            this.panel1.Location = new System.Drawing.Point(8, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1253, 30);
            this.panel1.TabIndex = 56;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(577, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(111, 31);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -2);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 32);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(346, -1);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 32);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(441, 1);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(79, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(208, 2);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
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
            this.AddressPanel.Controls.Add(this.label23);
            this.AddressPanel.Location = new System.Drawing.Point(355, 34);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(907, 30);
            this.AddressPanel.TabIndex = 55;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(308, 0);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(83, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(231, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(546, -1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(138, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(402, 0);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(139, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(739, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(166, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(94, 2);
            this.Address.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 8;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, -1);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(86, 32);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DragTargetPanel
            // 
            this.DragTargetPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DragTargetPanel.Controls.Add(this.L_2_COMBO);
            this.DragTargetPanel.Controls.Add(this.J_2);
            this.DragTargetPanel.Controls.Add(this.JumpToSystemPalette);
            this.DragTargetPanel.Controls.Add(this.Comment);
            this.DragTargetPanel.Controls.Add(this.X_PALETTE);
            this.DragTargetPanel.Controls.Add(this.label8);
            this.DragTargetPanel.Controls.Add(this.label7);
            this.DragTargetPanel.Controls.Add(this.X_ONE_STEP);
            this.DragTargetPanel.Controls.Add(this.label6);
            this.DragTargetPanel.Controls.Add(this.X_ONE_PIC);
            this.DragTargetPanel.Controls.Add(this.X_PIC);
            this.DragTargetPanel.Controls.Add(this.P4);
            this.DragTargetPanel.Controls.Add(this.J_4);
            this.DragTargetPanel.Controls.Add(this.W2);
            this.DragTargetPanel.Controls.Add(this.W0);
            this.DragTargetPanel.Controls.Add(this.J_0);
            this.DragTargetPanel.Location = new System.Drawing.Point(355, 64);
            this.DragTargetPanel.Margin = new System.Windows.Forms.Padding(5);
            this.DragTargetPanel.Name = "DragTargetPanel";
            this.DragTargetPanel.Size = new System.Drawing.Size(907, 392);
            this.DragTargetPanel.TabIndex = 57;
            // 
            // L_2_COMBO
            // 
            this.L_2_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_2_COMBO.FormattingEnabled = true;
            this.L_2_COMBO.Items.AddRange(new object[] {
            "0=16x16",
            "1=16x32",
            "2=32x32"});
            this.L_2_COMBO.Location = new System.Drawing.Point(234, 41);
            this.L_2_COMBO.Name = "L_2_COMBO";
            this.L_2_COMBO.Size = new System.Drawing.Size(171, 26);
            this.L_2_COMBO.TabIndex = 203;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(-1, 38);
            this.J_2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(153, 31);
            this.J_2.TabIndex = 202;
            this.J_2.Text = "サイズ";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // JumpToSystemPalette
            // 
            this.JumpToSystemPalette.AccessibleDescription = "@EXPLAIN_UNITICON_SYSTEM_PALETTE";
            this.JumpToSystemPalette.AutoSize = true;
            this.JumpToSystemPalette.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JumpToSystemPalette.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.JumpToSystemPalette.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.JumpToSystemPalette.Location = new System.Drawing.Point(697, 370);
            this.JumpToSystemPalette.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JumpToSystemPalette.Name = "JumpToSystemPalette";
            this.JumpToSystemPalette.Size = new System.Drawing.Size(111, 18);
            this.JumpToSystemPalette.TabIndex = 201;
            this.JumpToSystemPalette.Text = "パレットの変更";
            this.JumpToSystemPalette.Click += new System.EventHandler(this.JumpToSystemPalette_Click);
            // 
            // Comment
            // 
            this.Comment.ErrorMessage = "";
            this.Comment.Location = new System.Drawing.Point(159, 152);
            this.Comment.Name = "Comment";
            this.Comment.Placeholder = "";
            this.Comment.Size = new System.Drawing.Size(308, 25);
            this.Comment.TabIndex = 200;
            // 
            // X_PALETTE
            // 
            this.X_PALETTE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.X_PALETTE.FormattingEnabled = true;
            this.X_PALETTE.Items.AddRange(new object[] {
            "0=自軍",
            "1=友軍",
            "2=敵軍",
            "3=グレー",
            "4=4軍"});
            this.X_PALETTE.Location = new System.Drawing.Point(769, 187);
            this.X_PALETTE.Margin = new System.Windows.Forms.Padding(2);
            this.X_PALETTE.Name = "X_PALETTE";
            this.X_PALETTE.Size = new System.Drawing.Size(134, 26);
            this.X_PALETTE.TabIndex = 72;
            this.X_PALETTE.SelectedIndexChanged += new System.EventHandler(this.X_ONE_STEP_ValueChanged);
            // 
            // label8
            // 
            this.label8.AccessibleDescription = "@COMMENT";
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-1, 149);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 31);
            this.label8.TabIndex = 199;
            this.label8.Text = "コメント";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(687, 187);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 31);
            this.label7.TabIndex = 71;
            this.label7.Text = "パレット";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_ONE_STEP
            // 
            this.X_ONE_STEP.Hexadecimal = true;
            this.X_ONE_STEP.Location = new System.Drawing.Point(763, 148);
            this.X_ONE_STEP.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.X_ONE_STEP.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.X_ONE_STEP.Name = "X_ONE_STEP";
            this.X_ONE_STEP.Size = new System.Drawing.Size(60, 25);
            this.X_ONE_STEP.TabIndex = 70;
            this.X_ONE_STEP.ValueChanged += new System.EventHandler(this.X_ONE_STEP_ValueChanged);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(445, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 75);
            this.label6.TabIndex = 69;
            this.label6.Text = "表示例";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_ONE_PIC
            // 
            this.X_ONE_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_ONE_PIC.Location = new System.Drawing.Point(687, 5);
            this.X_ONE_PIC.Margin = new System.Windows.Forms.Padding(5);
            this.X_ONE_PIC.Name = "X_ONE_PIC";
            this.X_ONE_PIC.Size = new System.Drawing.Size(136, 136);
            this.X_ONE_PIC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.X_ONE_PIC.TabIndex = 68;
            this.X_ONE_PIC.TabStop = false;
            // 
            // X_PIC
            // 
            this.X_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_PIC.Location = new System.Drawing.Point(548, 2);
            this.X_PIC.Margin = new System.Windows.Forms.Padding(5);
            this.X_PIC.Name = "X_PIC";
            this.X_PIC.Size = new System.Drawing.Size(137, 389);
            this.X_PIC.TabIndex = 67;
            this.X_PIC.TabStop = false;
            // 
            // P4
            // 
            this.P4.Hexadecimal = true;
            this.P4.Location = new System.Drawing.Point(159, 81);
            this.P4.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.P4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P4.Name = "P4";
            this.P4.Size = new System.Drawing.Size(130, 25);
            this.P4.TabIndex = 63;
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(-1, 78);
            this.J_4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(153, 31);
            this.J_4.TabIndex = 62;
            this.J_4.Text = "画像アドレス";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // W2
            // 
            this.W2.Hexadecimal = true;
            this.W2.Location = new System.Drawing.Point(159, 42);
            this.W2.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.W2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W2.Name = "W2";
            this.W2.Size = new System.Drawing.Size(60, 25);
            this.W2.TabIndex = 60;
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(159, 6);
            this.W0.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.W0.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(60, 25);
            this.W0.TabIndex = 58;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(-1, 2);
            this.J_0.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(153, 31);
            this.J_0.TabIndex = 24;
            this.J_0.Text = "アニメパターン";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_JUMP_MOVEICON
            // 
            this.X_JUMP_MOVEICON.AutoSize = true;
            this.X_JUMP_MOVEICON.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_JUMP_MOVEICON.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.X_JUMP_MOVEICON.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.X_JUMP_MOVEICON.Location = new System.Drawing.Point(697, 25);
            this.X_JUMP_MOVEICON.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X_JUMP_MOVEICON.Name = "X_JUMP_MOVEICON";
            this.X_JUMP_MOVEICON.Size = new System.Drawing.Size(152, 18);
            this.X_JUMP_MOVEICON.TabIndex = 73;
            this.X_JUMP_MOVEICON.Text = "移動アイコンへJump";
            this.X_JUMP_MOVEICON.Click += new System.EventHandler(this.X_JUMP_MOVEICON_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(4, 14);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(160, 30);
            this.ImportButton.TabIndex = 61;
            this.ImportButton.Text = "画像読込";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(195, 14);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(160, 30);
            this.ExportButton.TabIndex = 60;
            this.ExportButton.Text = "画像取出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton_255);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(8, 34);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(341, 480);
            this.panel6.TabIndex = 74;
            // 
            // AddressListExpandsButton_255
            // 
            this.AddressListExpandsButton_255.Location = new System.Drawing.Point(2, 449);
            this.AddressListExpandsButton_255.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton_255.Name = "AddressListExpandsButton_255";
            this.AddressListExpandsButton_255.Size = new System.Drawing.Size(335, 30);
            this.AddressListExpandsButton_255.TabIndex = 114;
            this.AddressListExpandsButton_255.Text = "リストの拡張";
            this.AddressListExpandsButton_255.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(341, 30);
            this.LabelFilter.TabIndex = 55;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 29);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(341, 418);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // DragTargetPanel2
            // 
            this.DragTargetPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DragTargetPanel2.Controls.Add(this.X_JUMP_MOVEICON);
            this.DragTargetPanel2.Controls.Add(this.ImportButton);
            this.DragTargetPanel2.Controls.Add(this.ExportButton);
            this.DragTargetPanel2.Location = new System.Drawing.Point(355, 458);
            this.DragTargetPanel2.Margin = new System.Windows.Forms.Padding(5);
            this.DragTargetPanel2.Name = "DragTargetPanel2";
            this.DragTargetPanel2.Size = new System.Drawing.Size(906, 56);
            this.DragTargetPanel2.TabIndex = 58;
            // 
            // ImageUnitWaitIconFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1266, 518);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.DragTargetPanel2);
            this.Controls.Add(this.DragTargetPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressPanel);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ImageUnitWaitIconFrom";
            this.Text = "待機アイコン";
            this.Load += new System.EventHandler(this.ImageUnitWaitIconFrom_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.DragTargetPanel.ResumeLayout(false);
            this.DragTargetPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_ONE_STEP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_ONE_PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
            this.panel6.ResumeLayout(false);
            this.DragTargetPanel2.ResumeLayout(false);
            this.DragTargetPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
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
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel DragTargetPanel;
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.Label J_4;
        private System.Windows.Forms.NumericUpDown W2;
        private System.Windows.Forms.NumericUpDown W0;
        private System.Windows.Forms.NumericUpDown P4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown X_ONE_STEP;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button AddressListExpandsButton_255;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox X_PALETTE;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Label X_JUMP_MOVEICON;
        private InterpolatedPictureBox X_PIC;
        private InterpolatedPictureBox X_ONE_PIC;
        private System.Windows.Forms.Panel DragTargetPanel2;
        private TextBoxEx Comment;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label JumpToSystemPalette;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.ComboBox L_2_COMBO;
    }
}