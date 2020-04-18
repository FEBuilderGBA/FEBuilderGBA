namespace FEBuilderGBA
{
    partial class SelectAllocAreaForm
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
            this.INFO = new System.Windows.Forms.TextBox();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Area = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Area)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "新しい領域";
            // 
            // INFO
            // 
            this.INFO.Location = new System.Drawing.Point(14, 63);
            this.INFO.Multiline = true;
            this.INFO.Name = "INFO";
            this.INFO.Size = new System.Drawing.Size(478, 239);
            this.INFO.TabIndex = 1;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(351, 17);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(142, 26);
            this.WriteButton.TabIndex = 2;
            this.WriteButton.Text = "割り当て";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // Area
            // 
            this.Area.Hexadecimal = true;
            this.Area.Location = new System.Drawing.Point(142, 18);
            this.Area.Maximum = new decimal(new int[] {
            268435456,
            0,
            0,
            0});
            this.Area.Name = "Area";
            this.Area.Size = new System.Drawing.Size(143, 25);
            this.Area.TabIndex = 3;
            // 
            // SelectAllocAreaForm
            // 
            this.AcceptButton = this.WriteButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 324);
            this.Controls.Add(this.Area);
            this.Controls.Add(this.WriteButton);
            this.Controls.Add(this.INFO);
            this.Controls.Add(this.label1);
            this.Name = "SelectAllocAreaForm";
            this.Text = "再割り当てアドレスの指定";
            this.Load += new System.EventHandler(this.SelectAllocAreaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Area)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox INFO;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Area;
    }
}