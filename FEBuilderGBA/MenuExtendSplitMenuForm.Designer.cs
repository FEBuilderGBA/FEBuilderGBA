namespace FEBuilderGBA
{
    partial class MenuExtendSplitMenuForm
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.AllWriteButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.STR8_TEXT = new FEBuilderGBA.TextBoxEx();
            this.STR8 = new System.Windows.Forms.NumericUpDown();
            this.LSTR8 = new System.Windows.Forms.Label();
            this.STR7_TEXT = new FEBuilderGBA.TextBoxEx();
            this.STR6_TEXT = new FEBuilderGBA.TextBoxEx();
            this.STR7 = new System.Windows.Forms.NumericUpDown();
            this.STR6 = new System.Windows.Forms.NumericUpDown();
            this.LSTR7 = new System.Windows.Forms.Label();
            this.LSTR6 = new System.Windows.Forms.Label();
            this.D4 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.STR5_TEXT = new FEBuilderGBA.TextBoxEx();
            this.STR5 = new System.Windows.Forms.NumericUpDown();
            this.LSTR5 = new System.Windows.Forms.Label();
            this.STR4_TEXT = new FEBuilderGBA.TextBoxEx();
            this.STR3_TEXT = new FEBuilderGBA.TextBoxEx();
            this.STR2_TEXT = new FEBuilderGBA.TextBoxEx();
            this.STR1_TEXT = new FEBuilderGBA.TextBoxEx();
            this.STR4 = new System.Windows.Forms.NumericUpDown();
            this.STR3 = new System.Windows.Forms.NumericUpDown();
            this.STR2 = new System.Windows.Forms.NumericUpDown();
            this.STR1 = new System.Windows.Forms.NumericUpDown();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.LSTR4 = new System.Windows.Forms.Label();
            this.LSTR3 = new System.Windows.Forms.Label();
            this.LSTR2 = new System.Windows.Forms.Label();
            this.LSTR1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.L_32_ASM = new FEBuilderGBA.TextBoxEx();
            this.L_28_ASM = new FEBuilderGBA.TextBoxEx();
            this.L_24_ASM = new FEBuilderGBA.TextBoxEx();
            this.L_20_ASM = new FEBuilderGBA.TextBoxEx();
            this.L_16_ASM = new FEBuilderGBA.TextBoxEx();
            this.L_12_ASM = new FEBuilderGBA.TextBoxEx();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.STR8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.AllWriteButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(9, 6);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(860, 30);
            this.panel3.TabIndex = 152;
            // 
            // AllWriteButton
            // 
            this.AllWriteButton.Location = new System.Drawing.Point(688, -2);
            this.AllWriteButton.Name = "AllWriteButton";
            this.AllWriteButton.Size = new System.Drawing.Size(167, 30);
            this.AllWriteButton.TabIndex = 1;
            this.AllWriteButton.Text = "書き込み";
            this.AllWriteButton.UseVisualStyleBackColor = true;
            this.AllWriteButton.Click += new System.EventHandler(this.AllWriteButton_Click);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-1, -1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(206, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(223, 3);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.STR8_TEXT);
            this.panel1.Controls.Add(this.STR8);
            this.panel1.Controls.Add(this.LSTR8);
            this.panel1.Controls.Add(this.STR7_TEXT);
            this.panel1.Controls.Add(this.STR6_TEXT);
            this.panel1.Controls.Add(this.STR7);
            this.panel1.Controls.Add(this.STR6);
            this.panel1.Controls.Add(this.LSTR7);
            this.panel1.Controls.Add(this.LSTR6);
            this.panel1.Controls.Add(this.D4);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.STR5_TEXT);
            this.panel1.Controls.Add(this.STR5);
            this.panel1.Controls.Add(this.LSTR5);
            this.panel1.Controls.Add(this.STR4_TEXT);
            this.panel1.Controls.Add(this.STR3_TEXT);
            this.panel1.Controls.Add(this.STR2_TEXT);
            this.panel1.Controls.Add(this.STR1_TEXT);
            this.panel1.Controls.Add(this.STR4);
            this.panel1.Controls.Add(this.STR3);
            this.panel1.Controls.Add(this.STR2);
            this.panel1.Controls.Add(this.STR1);
            this.panel1.Controls.Add(this.B2);
            this.panel1.Controls.Add(this.B1);
            this.panel1.Controls.Add(this.B0);
            this.panel1.Controls.Add(this.LSTR4);
            this.panel1.Controls.Add(this.LSTR3);
            this.panel1.Controls.Add(this.LSTR2);
            this.panel1.Controls.Add(this.LSTR1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(10, 44);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(859, 383);
            this.panel1.TabIndex = 153;
            // 
            // STR8_TEXT
            // 
            this.STR8_TEXT.ErrorMessage = "";
            this.STR8_TEXT.Location = new System.Drawing.Point(328, 348);
            this.STR8_TEXT.Name = "STR8_TEXT";
            this.STR8_TEXT.Placeholder = "";
            this.STR8_TEXT.Size = new System.Drawing.Size(514, 25);
            this.STR8_TEXT.TabIndex = 54;
            // 
            // STR8
            // 
            this.STR8.Hexadecimal = true;
            this.STR8.Location = new System.Drawing.Point(222, 346);
            this.STR8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.STR8.Name = "STR8";
            this.STR8.Size = new System.Drawing.Size(91, 25);
            this.STR8.TabIndex = 11;
            // 
            // LSTR8
            // 
            this.LSTR8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LSTR8.Location = new System.Drawing.Point(1, 341);
            this.LSTR8.Name = "LSTR8";
            this.LSTR8.Size = new System.Drawing.Size(203, 30);
            this.LSTR8.TabIndex = 53;
            this.LSTR8.Text = "文字列7";
            this.LSTR8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // STR7_TEXT
            // 
            this.STR7_TEXT.ErrorMessage = "";
            this.STR7_TEXT.Location = new System.Drawing.Point(328, 319);
            this.STR7_TEXT.Name = "STR7_TEXT";
            this.STR7_TEXT.Placeholder = "";
            this.STR7_TEXT.Size = new System.Drawing.Size(514, 25);
            this.STR7_TEXT.TabIndex = 52;
            // 
            // STR6_TEXT
            // 
            this.STR6_TEXT.ErrorMessage = "";
            this.STR6_TEXT.Location = new System.Drawing.Point(328, 288);
            this.STR6_TEXT.Name = "STR6_TEXT";
            this.STR6_TEXT.Placeholder = "";
            this.STR6_TEXT.Size = new System.Drawing.Size(514, 25);
            this.STR6_TEXT.TabIndex = 51;
            // 
            // STR7
            // 
            this.STR7.Hexadecimal = true;
            this.STR7.Location = new System.Drawing.Point(222, 317);
            this.STR7.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.STR7.Name = "STR7";
            this.STR7.Size = new System.Drawing.Size(91, 25);
            this.STR7.TabIndex = 10;
            // 
            // STR6
            // 
            this.STR6.Hexadecimal = true;
            this.STR6.Location = new System.Drawing.Point(222, 286);
            this.STR6.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.STR6.Name = "STR6";
            this.STR6.Size = new System.Drawing.Size(91, 25);
            this.STR6.TabIndex = 9;
            // 
            // LSTR7
            // 
            this.LSTR7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LSTR7.Location = new System.Drawing.Point(1, 312);
            this.LSTR7.Name = "LSTR7";
            this.LSTR7.Size = new System.Drawing.Size(203, 30);
            this.LSTR7.TabIndex = 50;
            this.LSTR7.Text = "文字列6";
            this.LSTR7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LSTR6
            // 
            this.LSTR6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LSTR6.Location = new System.Drawing.Point(1, 283);
            this.LSTR6.Name = "LSTR6";
            this.LSTR6.Size = new System.Drawing.Size(203, 30);
            this.LSTR6.TabIndex = 49;
            this.LSTR6.Text = "文字列5";
            this.LSTR6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // D4
            // 
            this.D4.Location = new System.Drawing.Point(222, 98);
            this.D4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D4.Name = "D4";
            this.D4.Size = new System.Drawing.Size(91, 25);
            this.D4.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(1, 95);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(203, 30);
            this.label10.TabIndex = 45;
            this.label10.Text = "Style";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // STR5_TEXT
            // 
            this.STR5_TEXT.ErrorMessage = "";
            this.STR5_TEXT.Location = new System.Drawing.Point(328, 260);
            this.STR5_TEXT.Name = "STR5_TEXT";
            this.STR5_TEXT.Placeholder = "";
            this.STR5_TEXT.Size = new System.Drawing.Size(514, 25);
            this.STR5_TEXT.TabIndex = 44;
            // 
            // STR5
            // 
            this.STR5.Hexadecimal = true;
            this.STR5.Location = new System.Drawing.Point(222, 259);
            this.STR5.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.STR5.Name = "STR5";
            this.STR5.Size = new System.Drawing.Size(91, 25);
            this.STR5.TabIndex = 8;
            // 
            // LSTR5
            // 
            this.LSTR5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LSTR5.Location = new System.Drawing.Point(1, 254);
            this.LSTR5.Name = "LSTR5";
            this.LSTR5.Size = new System.Drawing.Size(203, 30);
            this.LSTR5.TabIndex = 42;
            this.LSTR5.Text = "文字列4";
            this.LSTR5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // STR4_TEXT
            // 
            this.STR4_TEXT.ErrorMessage = "";
            this.STR4_TEXT.Location = new System.Drawing.Point(328, 230);
            this.STR4_TEXT.Name = "STR4_TEXT";
            this.STR4_TEXT.Placeholder = "";
            this.STR4_TEXT.Size = new System.Drawing.Size(514, 25);
            this.STR4_TEXT.TabIndex = 41;
            // 
            // STR3_TEXT
            // 
            this.STR3_TEXT.ErrorMessage = "";
            this.STR3_TEXT.Location = new System.Drawing.Point(328, 201);
            this.STR3_TEXT.Name = "STR3_TEXT";
            this.STR3_TEXT.Placeholder = "";
            this.STR3_TEXT.Size = new System.Drawing.Size(514, 25);
            this.STR3_TEXT.TabIndex = 40;
            // 
            // STR2_TEXT
            // 
            this.STR2_TEXT.ErrorMessage = "";
            this.STR2_TEXT.Location = new System.Drawing.Point(328, 170);
            this.STR2_TEXT.Name = "STR2_TEXT";
            this.STR2_TEXT.Placeholder = "";
            this.STR2_TEXT.Size = new System.Drawing.Size(514, 25);
            this.STR2_TEXT.TabIndex = 39;
            // 
            // STR1_TEXT
            // 
            this.STR1_TEXT.ErrorMessage = "";
            this.STR1_TEXT.Location = new System.Drawing.Point(328, 139);
            this.STR1_TEXT.Name = "STR1_TEXT";
            this.STR1_TEXT.Placeholder = "";
            this.STR1_TEXT.Size = new System.Drawing.Size(514, 25);
            this.STR1_TEXT.TabIndex = 38;
            // 
            // STR4
            // 
            this.STR4.Hexadecimal = true;
            this.STR4.Location = new System.Drawing.Point(222, 230);
            this.STR4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.STR4.Name = "STR4";
            this.STR4.Size = new System.Drawing.Size(91, 25);
            this.STR4.TabIndex = 7;
            // 
            // STR3
            // 
            this.STR3.Hexadecimal = true;
            this.STR3.Location = new System.Drawing.Point(222, 199);
            this.STR3.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.STR3.Name = "STR3";
            this.STR3.Size = new System.Drawing.Size(91, 25);
            this.STR3.TabIndex = 6;
            // 
            // STR2
            // 
            this.STR2.Hexadecimal = true;
            this.STR2.Location = new System.Drawing.Point(222, 169);
            this.STR2.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.STR2.Name = "STR2";
            this.STR2.Size = new System.Drawing.Size(91, 25);
            this.STR2.TabIndex = 5;
            // 
            // STR1
            // 
            this.STR1.Hexadecimal = true;
            this.STR1.Location = new System.Drawing.Point(222, 140);
            this.STR1.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.STR1.Name = "STR1";
            this.STR1.Size = new System.Drawing.Size(91, 25);
            this.STR1.TabIndex = 4;
            // 
            // B2
            // 
            this.B2.Location = new System.Drawing.Point(222, 69);
            this.B2.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(91, 25);
            this.B2.TabIndex = 2;
            // 
            // B1
            // 
            this.B1.Location = new System.Drawing.Point(222, 39);
            this.B1.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(91, 25);
            this.B1.TabIndex = 1;
            // 
            // B0
            // 
            this.B0.Location = new System.Drawing.Point(222, 10);
            this.B0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(91, 25);
            this.B0.TabIndex = 0;
            // 
            // LSTR4
            // 
            this.LSTR4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LSTR4.Location = new System.Drawing.Point(1, 225);
            this.LSTR4.Name = "LSTR4";
            this.LSTR4.Size = new System.Drawing.Size(203, 30);
            this.LSTR4.TabIndex = 30;
            this.LSTR4.Text = "文字列3";
            this.LSTR4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LSTR3
            // 
            this.LSTR3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LSTR3.Location = new System.Drawing.Point(1, 196);
            this.LSTR3.Name = "LSTR3";
            this.LSTR3.Size = new System.Drawing.Size(203, 30);
            this.LSTR3.TabIndex = 29;
            this.LSTR3.Text = "文字列2";
            this.LSTR3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LSTR2
            // 
            this.LSTR2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LSTR2.Location = new System.Drawing.Point(1, 167);
            this.LSTR2.Name = "LSTR2";
            this.LSTR2.Size = new System.Drawing.Size(203, 30);
            this.LSTR2.TabIndex = 28;
            this.LSTR2.Text = "文字列1";
            this.LSTR2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LSTR1
            // 
            this.LSTR1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LSTR1.Location = new System.Drawing.Point(1, 138);
            this.LSTR1.Name = "LSTR1";
            this.LSTR1.Size = new System.Drawing.Size(203, 30);
            this.LSTR1.TabIndex = 27;
            this.LSTR1.Text = "文字列0";
            this.LSTR1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(1, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 30);
            this.label3.TabIndex = 26;
            this.label3.Text = "Width";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(1, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 30);
            this.label2.TabIndex = 25;
            this.label2.Text = "Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(1, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 30);
            this.label1.TabIndex = 24;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_32_ASM
            // 
            this.L_32_ASM.ErrorMessage = "";
            this.L_32_ASM.Location = new System.Drawing.Point(552, 319);
            this.L_32_ASM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_32_ASM.Name = "L_32_ASM";
            this.L_32_ASM.Placeholder = "";
            this.L_32_ASM.ReadOnly = true;
            this.L_32_ASM.Size = new System.Drawing.Size(298, 25);
            this.L_32_ASM.TabIndex = 127;
            this.L_32_ASM.Visible = false;
            // 
            // L_28_ASM
            // 
            this.L_28_ASM.ErrorMessage = "";
            this.L_28_ASM.Location = new System.Drawing.Point(552, 292);
            this.L_28_ASM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_28_ASM.Name = "L_28_ASM";
            this.L_28_ASM.Placeholder = "";
            this.L_28_ASM.ReadOnly = true;
            this.L_28_ASM.Size = new System.Drawing.Size(298, 25);
            this.L_28_ASM.TabIndex = 126;
            this.L_28_ASM.Visible = false;
            // 
            // L_24_ASM
            // 
            this.L_24_ASM.ErrorMessage = "";
            this.L_24_ASM.Location = new System.Drawing.Point(552, 262);
            this.L_24_ASM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_24_ASM.Name = "L_24_ASM";
            this.L_24_ASM.Placeholder = "";
            this.L_24_ASM.ReadOnly = true;
            this.L_24_ASM.Size = new System.Drawing.Size(298, 25);
            this.L_24_ASM.TabIndex = 125;
            this.L_24_ASM.Visible = false;
            // 
            // L_20_ASM
            // 
            this.L_20_ASM.ErrorMessage = "";
            this.L_20_ASM.Location = new System.Drawing.Point(552, 235);
            this.L_20_ASM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_20_ASM.Name = "L_20_ASM";
            this.L_20_ASM.Placeholder = "";
            this.L_20_ASM.ReadOnly = true;
            this.L_20_ASM.Size = new System.Drawing.Size(298, 25);
            this.L_20_ASM.TabIndex = 124;
            this.L_20_ASM.Visible = false;
            // 
            // L_16_ASM
            // 
            this.L_16_ASM.ErrorMessage = "";
            this.L_16_ASM.Location = new System.Drawing.Point(552, 203);
            this.L_16_ASM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_16_ASM.Name = "L_16_ASM";
            this.L_16_ASM.Placeholder = "";
            this.L_16_ASM.ReadOnly = true;
            this.L_16_ASM.Size = new System.Drawing.Size(298, 25);
            this.L_16_ASM.TabIndex = 123;
            this.L_16_ASM.Visible = false;
            // 
            // L_12_ASM
            // 
            this.L_12_ASM.ErrorMessage = "";
            this.L_12_ASM.Location = new System.Drawing.Point(552, 173);
            this.L_12_ASM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_12_ASM.Name = "L_12_ASM";
            this.L_12_ASM.Placeholder = "";
            this.L_12_ASM.ReadOnly = true;
            this.L_12_ASM.Size = new System.Drawing.Size(298, 25);
            this.L_12_ASM.TabIndex = 122;
            this.L_12_ASM.Visible = false;
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
            // MenuExtendSplitMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(883, 434);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "MenuExtendSplitMenuForm";
            this.Text = "分岐メニュー定義";
            this.Load += new System.EventHandler(this.MenuExtendSplitMenuForm_Load);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.STR8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FEBuilderGBA.TextBoxEx BlockSize;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private FEBuilderGBA.TextBoxEx L_32_ASM;
        private FEBuilderGBA.TextBoxEx L_28_ASM;
        private FEBuilderGBA.TextBoxEx L_24_ASM;
        private FEBuilderGBA.TextBoxEx L_20_ASM;
        private FEBuilderGBA.TextBoxEx L_16_ASM;
        private FEBuilderGBA.TextBoxEx L_12_ASM;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LSTR4;
        private System.Windows.Forms.Label LSTR3;
        private System.Windows.Forms.Label LSTR2;
        private System.Windows.Forms.Label LSTR1;
        private TextBoxEx STR4_TEXT;
        private TextBoxEx STR3_TEXT;
        private TextBoxEx STR2_TEXT;
        private TextBoxEx STR1_TEXT;
        private System.Windows.Forms.NumericUpDown STR4;
        private System.Windows.Forms.NumericUpDown STR3;
        private System.Windows.Forms.NumericUpDown STR2;
        private System.Windows.Forms.NumericUpDown STR1;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.NumericUpDown B1;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.Button AllWriteButton;
        private TextBoxEx STR5_TEXT;
        private System.Windows.Forms.NumericUpDown STR5;
        private System.Windows.Forms.Label LSTR5;
        private System.Windows.Forms.NumericUpDown D4;
        private System.Windows.Forms.Label label10;
        private TextBoxEx STR8_TEXT;
        private System.Windows.Forms.NumericUpDown STR8;
        private System.Windows.Forms.Label LSTR8;
        private TextBoxEx STR7_TEXT;
        private TextBoxEx STR6_TEXT;
        private System.Windows.Forms.NumericUpDown STR7;
        private System.Windows.Forms.NumericUpDown STR6;
        private System.Windows.Forms.Label LSTR7;
        private System.Windows.Forms.Label LSTR6;
    }
}