namespace FEBuilderGBA
{
    partial class SongInstrumentDirectSoundForm
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
            this.L_4_HZ1024 = new FEBuilderGBA.TextBoxEx();
            this.Infomation = new System.Windows.Forms.Label();
            this.L_0_COMBO = new System.Windows.Forms.ComboBox();
            this.D12 = new System.Windows.Forms.NumericUpDown();
            this.J_12 = new System.Windows.Forms.Label();
            this.D4 = new System.Windows.Forms.NumericUpDown();
            this.D8 = new System.Windows.Forms.NumericUpDown();
            this.D0 = new System.Windows.Forms.NumericUpDown();
            this.J_2 = new System.Windows.Forms.Label();
            this.J_8 = new System.Windows.Forms.Label();
            this.J_0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.AllWriteButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.X_COMPRESSED_SIZE = new System.Windows.Forms.NumericUpDown();
            this.X_L_COMPRESSED_SIZE = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.D12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D0)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_COMPRESSED_SIZE)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.X_COMPRESSED_SIZE);
            this.panel1.Controls.Add(this.X_L_COMPRESSED_SIZE);
            this.panel1.Controls.Add(this.L_4_HZ1024);
            this.panel1.Controls.Add(this.Infomation);
            this.panel1.Controls.Add(this.L_0_COMBO);
            this.panel1.Controls.Add(this.D12);
            this.panel1.Controls.Add(this.J_12);
            this.panel1.Controls.Add(this.D4);
            this.panel1.Controls.Add(this.D8);
            this.panel1.Controls.Add(this.D0);
            this.panel1.Controls.Add(this.J_2);
            this.panel1.Controls.Add(this.J_8);
            this.panel1.Controls.Add(this.J_0);
            this.panel1.Location = new System.Drawing.Point(14, 42);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(837, 234);
            this.panel1.TabIndex = 157;
            // 
            // L_4_HZ1024
            // 
            this.L_4_HZ1024.ErrorMessage = "";
            this.L_4_HZ1024.Location = new System.Drawing.Point(434, 37);
            this.L_4_HZ1024.Name = "L_4_HZ1024";
            this.L_4_HZ1024.Placeholder = "";
            this.L_4_HZ1024.ReadOnly = true;
            this.L_4_HZ1024.Size = new System.Drawing.Size(211, 25);
            this.L_4_HZ1024.TabIndex = 112;
            // 
            // Infomation
            // 
            this.Infomation.AutoSize = true;
            this.Infomation.Location = new System.Drawing.Point(25, 160);
            this.Infomation.Name = "Infomation";
            this.Infomation.Size = new System.Drawing.Size(547, 18);
            this.Infomation.TabIndex = 111;
            this.Infomation.Text = "これより下のアドレスには、waveデータが、LengthByteだけ格納されています。";
            // 
            // L_0_COMBO
            // 
            this.L_0_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_0_COMBO.FormattingEnabled = true;
            this.L_0_COMBO.Items.AddRange(new object[] {
            "00000000=DirectSoundFixedFreq?",
            "40000000=DirectSound"});
            this.L_0_COMBO.Location = new System.Drawing.Point(434, 8);
            this.L_0_COMBO.Name = "L_0_COMBO";
            this.L_0_COMBO.Size = new System.Drawing.Size(401, 26);
            this.L_0_COMBO.TabIndex = 110;
            this.L_0_COMBO.Visible = false;
            // 
            // D12
            // 
            this.D12.Location = new System.Drawing.Point(223, 96);
            this.D12.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D12.Name = "D12";
            this.D12.ReadOnly = true;
            this.D12.Size = new System.Drawing.Size(205, 25);
            this.D12.TabIndex = 3;
            // 
            // J_12
            // 
            this.J_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_12.Location = new System.Drawing.Point(-1, 95);
            this.J_12.Name = "J_12";
            this.J_12.Size = new System.Drawing.Size(218, 30);
            this.J_12.TabIndex = 45;
            this.J_12.Text = "LengthByte";
            this.J_12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // D4
            // 
            this.D4.Hexadecimal = true;
            this.D4.Location = new System.Drawing.Point(223, 38);
            this.D4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D4.Name = "D4";
            this.D4.ReadOnly = true;
            this.D4.Size = new System.Drawing.Size(205, 25);
            this.D4.TabIndex = 2;
            // 
            // D8
            // 
            this.D8.Location = new System.Drawing.Point(223, 66);
            this.D8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D8.Name = "D8";
            this.D8.Size = new System.Drawing.Size(205, 25);
            this.D8.TabIndex = 1;
            // 
            // D0
            // 
            this.D0.Hexadecimal = true;
            this.D0.Location = new System.Drawing.Point(223, 8);
            this.D0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D0.Name = "D0";
            this.D0.Size = new System.Drawing.Size(205, 25);
            this.D0.TabIndex = 0;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(-1, 37);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(218, 30);
            this.J_2.TabIndex = 26;
            this.J_2.Text = "frequency Hz*1024";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_8
            // 
            this.J_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8.Location = new System.Drawing.Point(-1, 66);
            this.J_8.Name = "J_8";
            this.J_8.Size = new System.Drawing.Size(218, 30);
            this.J_8.TabIndex = 25;
            this.J_8.Text = "LoopStartByte";
            this.J_8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(-1, 8);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(218, 30);
            this.J_0.TabIndex = 24;
            this.J_0.Text = "Header";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.AllWriteButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(14, 13);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(837, 30);
            this.panel3.TabIndex = 156;
            // 
            // AllWriteButton
            // 
            this.AllWriteButton.Location = new System.Drawing.Point(668, -1);
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
            this.label8.Size = new System.Drawing.Size(218, 30);
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
            // X_COMPRESSED_SIZE
            // 
            this.X_COMPRESSED_SIZE.Location = new System.Drawing.Point(223, 125);
            this.X_COMPRESSED_SIZE.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.X_COMPRESSED_SIZE.Name = "X_COMPRESSED_SIZE";
            this.X_COMPRESSED_SIZE.ReadOnly = true;
            this.X_COMPRESSED_SIZE.Size = new System.Drawing.Size(205, 25);
            this.X_COMPRESSED_SIZE.TabIndex = 113;
            // 
            // X_L_COMPRESSED_SIZE
            // 
            this.X_L_COMPRESSED_SIZE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_L_COMPRESSED_SIZE.Location = new System.Drawing.Point(-1, 124);
            this.X_L_COMPRESSED_SIZE.Name = "X_L_COMPRESSED_SIZE";
            this.X_L_COMPRESSED_SIZE.Size = new System.Drawing.Size(218, 30);
            this.X_L_COMPRESSED_SIZE.TabIndex = 114;
            this.X_L_COMPRESSED_SIZE.Text = "LengthByte";
            this.X_L_COMPRESSED_SIZE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SongInstrumentDirectSoundForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(863, 289);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "SongInstrumentDirectSoundForm";
            this.Text = "DirectSound";
            this.Load += new System.EventHandler(this.SongInstrumentDirectSoundForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.D12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D0)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_COMPRESSED_SIZE)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown D12;
        private System.Windows.Forms.Label J_12;
        private System.Windows.Forms.NumericUpDown D4;
        private System.Windows.Forms.NumericUpDown D8;
        private System.Windows.Forms.NumericUpDown D0;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.Label J_8;
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button AllWriteButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.ComboBox L_0_COMBO;
        private System.Windows.Forms.Label Infomation;
        private FEBuilderGBA.TextBoxEx L_4_HZ1024;
        private System.Windows.Forms.NumericUpDown X_COMPRESSED_SIZE;
        private System.Windows.Forms.Label X_L_COMPRESSED_SIZE;
    }
}