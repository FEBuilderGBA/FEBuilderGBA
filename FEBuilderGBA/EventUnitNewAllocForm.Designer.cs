namespace FEBuilderGBA
{
    partial class EventUnitNewAllocForm
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
            this.AllocButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.AllocCountNumupdown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AllocCountNumupdown)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AllocButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.AllocCountNumupdown);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(16, 16);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 325);
            this.panel1.TabIndex = 0;
            // 
            // AllocButton
            // 
            this.AllocButton.Location = new System.Drawing.Point(296, 181);
            this.AllocButton.Margin = new System.Windows.Forms.Padding(4);
            this.AllocButton.Name = "AllocButton";
            this.AllocButton.Size = new System.Drawing.Size(118, 41);
            this.AllocButton.TabIndex = 28;
            this.AllocButton.Text = "確保";
            this.AllocButton.UseVisualStyleBackColor = true;
            this.AllocButton.Click += new System.EventHandler(this.AllocButton_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(0, 244);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(427, 81);
            this.label3.TabIndex = 27;
            this.label3.Text = "確保した領域を使わないで、\r\nユニット配置ウィンドウを閉じてしまうと、\r\n利用されない無駄データとなってしまうので\r\n注意してください。";
            // 
            // AllocCountNumupdown
            // 
            this.AllocCountNumupdown.Location = new System.Drawing.Point(337, 114);
            this.AllocCountNumupdown.Margin = new System.Windows.Forms.Padding(4);
            this.AllocCountNumupdown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.AllocCountNumupdown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AllocCountNumupdown.Name = "AllocCountNumupdown";
            this.AllocCountNumupdown.Size = new System.Drawing.Size(74, 25);
            this.AllocCountNumupdown.TabIndex = 26;
            this.AllocCountNumupdown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(427, 63);
            this.label2.TabIndex = 25;
            this.label2.Text = "増援等で、追加で登場させたいユニットのために、\r\n新規に領域を確保します。";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(4, 107);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(282, 36);
            this.label1.TabIndex = 24;
            this.label1.Text = "確保する人数";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EventUnitNewAllocForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(456, 355);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EventUnitNewAllocForm";
            this.Text = "ユニット配置　新割り当て";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AllocCountNumupdown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown AllocCountNumupdown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AllocButton;
        private System.Windows.Forms.Label label3;
    }
}