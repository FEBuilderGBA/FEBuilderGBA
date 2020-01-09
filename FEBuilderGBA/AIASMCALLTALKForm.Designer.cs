namespace FEBuilderGBA
{
    partial class AIASMCALLTALKForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.L_1_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.L_1_UNIT = new FEBuilderGBA.TextBoxEx();
            this.L_0_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.L_0_UNIT = new FEBuilderGBA.TextBoxEx();
            this.B3 = new System.Windows.Forms.NumericUpDown();
            this.J_3 = new System.Windows.Forms.Label();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.J_1_UNIT = new System.Windows.Forms.Label();
            this.J_2 = new System.Windows.Forms.Label();
            this.J_0_UNIT = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.AllWriteButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_1_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.L_1_UNITICON);
            this.panel1.Controls.Add(this.L_1_UNIT);
            this.panel1.Controls.Add(this.L_0_UNITICON);
            this.panel1.Controls.Add(this.L_0_UNIT);
            this.panel1.Controls.Add(this.B3);
            this.panel1.Controls.Add(this.J_3);
            this.panel1.Controls.Add(this.B1);
            this.panel1.Controls.Add(this.B2);
            this.panel1.Controls.Add(this.B0);
            this.panel1.Controls.Add(this.J_1_UNIT);
            this.panel1.Controls.Add(this.J_2);
            this.panel1.Controls.Add(this.J_0_UNIT);
            this.panel1.Location = new System.Drawing.Point(14, 42);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 295);
            this.panel1.TabIndex = 155;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 153);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(566, 125);
            this.textBox1.TabIndex = 111;
            this.textBox1.Text = "FE7のファリナのように、敵AIから会話イベントを実行する設定を作ります。\r\n章に会話イベントが設定されていれば、敵AIは会話イベントを呼び出します。";
            // 
            // L_1_UNITICON
            // 
            this.L_1_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_1_UNITICON.Location = new System.Drawing.Point(374, 37);
            this.L_1_UNITICON.Margin = new System.Windows.Forms.Padding(2);
            this.L_1_UNITICON.Name = "L_1_UNITICON";
            this.L_1_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_1_UNITICON.TabIndex = 108;
            this.L_1_UNITICON.TabStop = false;
            // 
            // L_1_UNIT
            // 
            this.L_1_UNIT.ErrorMessage = "";
            this.L_1_UNIT.Location = new System.Drawing.Point(223, 42);
            this.L_1_UNIT.Margin = new System.Windows.Forms.Padding(2);
            this.L_1_UNIT.Name = "L_1_UNIT";
            this.L_1_UNIT.Placeholder = "";
            this.L_1_UNIT.ReadOnly = true;
            this.L_1_UNIT.Size = new System.Drawing.Size(147, 25);
            this.L_1_UNIT.TabIndex = 107;
            // 
            // L_0_UNITICON
            // 
            this.L_0_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_UNITICON.Location = new System.Drawing.Point(374, 8);
            this.L_0_UNITICON.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_UNITICON.Name = "L_0_UNITICON";
            this.L_0_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_UNITICON.TabIndex = 104;
            this.L_0_UNITICON.TabStop = false;
            // 
            // L_0_UNIT
            // 
            this.L_0_UNIT.ErrorMessage = "";
            this.L_0_UNIT.Location = new System.Drawing.Point(223, 13);
            this.L_0_UNIT.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_UNIT.Name = "L_0_UNIT";
            this.L_0_UNIT.Placeholder = "";
            this.L_0_UNIT.ReadOnly = true;
            this.L_0_UNIT.Size = new System.Drawing.Size(147, 25);
            this.L_0_UNIT.TabIndex = 103;
            // 
            // B3
            // 
            this.B3.Hexadecimal = true;
            this.B3.Location = new System.Drawing.Point(122, 100);
            this.B3.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(82, 25);
            this.B3.TabIndex = 4;
            // 
            // J_3
            // 
            this.J_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_3.Location = new System.Drawing.Point(-1, 95);
            this.J_3.Name = "J_3";
            this.J_3.Size = new System.Drawing.Size(117, 30);
            this.J_3.TabIndex = 45;
            this.J_3.Text = "00";
            this.J_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B1
            // 
            this.B1.Hexadecimal = true;
            this.B1.Location = new System.Drawing.Point(122, 42);
            this.B1.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(82, 25);
            this.B1.TabIndex = 1;
            // 
            // B2
            // 
            this.B2.Hexadecimal = true;
            this.B2.Location = new System.Drawing.Point(122, 70);
            this.B2.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(82, 25);
            this.B2.TabIndex = 2;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(122, 12);
            this.B0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(82, 25);
            this.B0.TabIndex = 0;
            // 
            // J_1_UNIT
            // 
            this.J_1_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_1_UNIT.Location = new System.Drawing.Point(-1, 37);
            this.J_1_UNIT.Name = "J_1_UNIT";
            this.J_1_UNIT.Size = new System.Drawing.Size(117, 30);
            this.J_1_UNIT.TabIndex = 26;
            this.J_1_UNIT.Text = "To";
            this.J_1_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(-1, 66);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(117, 30);
            this.J_2.TabIndex = 25;
            this.J_2.Text = "00";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_0_UNIT
            // 
            this.J_0_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_UNIT.Location = new System.Drawing.Point(-1, 8);
            this.J_0_UNIT.Name = "J_0_UNIT";
            this.J_0_UNIT.Size = new System.Drawing.Size(117, 30);
            this.J_0_UNIT.TabIndex = 24;
            this.J_0_UNIT.Text = "From";
            this.J_0_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panel3.Size = new System.Drawing.Size(574, 30);
            this.panel3.TabIndex = 154;
            // 
            // AllWriteButton
            // 
            this.AllWriteButton.Location = new System.Drawing.Point(405, -2);
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
            this.label8.Location = new System.Drawing.Point(-1, -2);
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
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(314, 4);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 58;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(532, 2);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(150, 25);
            this.SelectAddress.TabIndex = 57;
            // 
            // AIASMCALLTALKForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(610, 350);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "AIASMCALLTALKForm";
            this.Text = "敵AIから会話イベントを実行する";
            this.Load += new System.EventHandler(this.AIASMCALLTALKForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_1_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FEBuilderGBA.TextBoxEx BlockSize;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown B3;
        private System.Windows.Forms.Label J_3;
        private System.Windows.Forms.NumericUpDown B1;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.Label J_1_UNIT;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.Label J_0_UNIT;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button AllWriteButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private InterpolatedPictureBox L_1_UNITICON;
        private TextBoxEx L_1_UNIT;
        private InterpolatedPictureBox L_0_UNITICON;
        private TextBoxEx L_0_UNIT;
        private System.Windows.Forms.TextBox textBox1;

    }
}