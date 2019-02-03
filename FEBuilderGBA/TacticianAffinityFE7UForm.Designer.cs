namespace FEBuilderGBA
{
    partial class TacticianAffinityFE7UForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.birth_month = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.Affinity = new System.Windows.Forms.ComboBox();
            this.save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.birth_month)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "先頭アドレス";
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ReadStartAddress.Location = new System.Drawing.Point(110, 4);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(58, 21);
            this.ReadStartAddress.TabIndex = 1;
            this.ReadStartAddress.Value = new decimal(new int[] {
            1850048,
            0,
            0,
            0});
            this.ReadStartAddress.ValueChanged += new System.EventHandler(this.ReadStartAddress_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "誕生月";
            // 
            // birth_month
            // 
            this.birth_month.Location = new System.Drawing.Point(110, 29);
            this.birth_month.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.birth_month.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.birth_month.Name = "birth_month";
            this.birth_month.Size = new System.Drawing.Size(58, 21);
            this.birth_month.TabIndex = 3;
            this.birth_month.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.birth_month.ValueChanged += new System.EventHandler(this.birth_month_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "属性";
            // 
            // Affinity
            // 
            this.Affinity.FormattingEnabled = true;
            this.Affinity.Items.AddRange(new object[] {
            "無",
            "炎",
            "雷",
            "風",
            "氷",
            "闇",
            "光",
            "理"});
            this.Affinity.Location = new System.Drawing.Point(110, 53);
            this.Affinity.Name = "Affinity";
            this.Affinity.Size = new System.Drawing.Size(58, 20);
            this.Affinity.TabIndex = 7;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(52, 79);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 8;
            this.save.Text = "書き込み";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.button1_Click);
            // 
            // TacticianAffinityFE7UForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 104);
            this.Controls.Add(this.save);
            this.Controls.Add(this.Affinity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.birth_month);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ReadStartAddress);
            this.Controls.Add(this.label1);
            this.Name = "TacticianAffinityFE7UForm";
            this.Text = "軍師属性決定";
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.birth_month)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown birth_month;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Affinity;
        private System.Windows.Forms.Button save;
    }
}