namespace FEBuilderGBA
{
    partial class AOERANGEForm
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.MapController = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.B3 = new System.Windows.Forms.NumericUpDown();
            this.J_3 = new System.Windows.Forms.Label();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.J_2 = new System.Windows.Forms.Label();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.J_1 = new System.Windows.Forms.Label();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.J_0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.MapController);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.B3);
            this.panel4.Controls.Add(this.J_3);
            this.panel4.Controls.Add(this.B2);
            this.panel4.Controls.Add(this.J_2);
            this.panel4.Controls.Add(this.B1);
            this.panel4.Controls.Add(this.J_1);
            this.panel4.Controls.Add(this.B0);
            this.panel4.Controls.Add(this.J_0);
            this.panel4.Location = new System.Drawing.Point(8, 44);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1118, 677);
            this.panel4.TabIndex = 96;
            // 
            // label1
            // 
            this.label1.AccessibleDescription = "";
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(554, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(559, 97);
            this.label1.TabIndex = 50;
            this.label1.Text = "AoE攻撃の範囲を指定します。\r\n中心点は、攻撃が炸裂する中心点です。\r\n攻撃するマスを1に、それ以外を0に指定してください。";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MapController
            // 
            this.MapController.Location = new System.Drawing.Point(3, 111);
            this.MapController.Name = "MapController";
            this.MapController.Size = new System.Drawing.Size(1110, 561);
            this.MapController.TabIndex = 49;
            // 
            // label4
            // 
            this.label4.AccessibleDescription = "攻撃が炸裂する中心点を指定します。";
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(-1, 36);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(531, 33);
            this.label4.TabIndex = 48;
            this.label4.Text = "中心点";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B3
            // 
            this.B3.Location = new System.Drawing.Point(453, 76);
            this.B3.Margin = new System.Windows.Forms.Padding(2);
            this.B3.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(78, 25);
            this.B3.TabIndex = 46;
            this.B3.ValueChanged += new System.EventHandler(this.B2_ValueChanged);
            // 
            // J_3
            // 
            this.J_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_3.Location = new System.Drawing.Point(272, 68);
            this.J_3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_3.Name = "J_3";
            this.J_3.Size = new System.Drawing.Size(173, 33);
            this.J_3.TabIndex = 47;
            this.J_3.Text = "中心点Y";
            this.J_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B2
            // 
            this.B2.Location = new System.Drawing.Point(177, 76);
            this.B2.Margin = new System.Windows.Forms.Padding(2);
            this.B2.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(78, 25);
            this.B2.TabIndex = 44;
            this.B2.ValueChanged += new System.EventHandler(this.B2_ValueChanged);
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(-1, 68);
            this.J_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(174, 33);
            this.J_2.TabIndex = 45;
            this.J_2.Text = "中心点X";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B1
            // 
            this.B1.Location = new System.Drawing.Point(452, 7);
            this.B1.Margin = new System.Windows.Forms.Padding(2);
            this.B1.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(78, 25);
            this.B1.TabIndex = 42;
            this.B1.ValueChanged += new System.EventHandler(this.B0_ValueChanged);
            // 
            // J_1
            // 
            this.J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_1.Location = new System.Drawing.Point(271, -1);
            this.J_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_1.Name = "J_1";
            this.J_1.Size = new System.Drawing.Size(173, 33);
            this.J_1.TabIndex = 43;
            this.J_1.Text = "高さ";
            this.J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B0
            // 
            this.B0.Location = new System.Drawing.Point(176, 7);
            this.B0.Margin = new System.Windows.Forms.Padding(2);
            this.B0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(78, 25);
            this.B0.TabIndex = 0;
            this.B0.ValueChanged += new System.EventHandler(this.B0_ValueChanged);
            // 
            // J_0
            // 
            this.J_0.AccessibleDescription = "";
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(-2, -1);
            this.J_0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(174, 33);
            this.J_0.TabIndex = 24;
            this.J_0.Text = "幅";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.WriteButton);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(8, 11);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1118, 34);
            this.panel3.TabIndex = 95;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(704, -2);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(160, 30);
            this.ReloadListButton.TabIndex = 3;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            this.ReloadListButton.Click += new System.EventHandler(this.ReloadListButton_Click);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-2, -2);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 36);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(945, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(168, 30);
            this.WriteButton.TabIndex = 55;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(141, 4);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(113, 25);
            this.ReadStartAddress.TabIndex = 0;
            // 
            // AOERANGEForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1136, 733);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Name = "AOERANGEForm";
            this.Text = "AoE 攻撃レンジマスク";
            this.Load += new System.EventHandler(this.AOERANGEForm_Load);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown B1;
        private System.Windows.Forms.Label J_1;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown B3;
        private System.Windows.Forms.Label J_3;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel MapController;
        private System.Windows.Forms.Label label1;
    }
}