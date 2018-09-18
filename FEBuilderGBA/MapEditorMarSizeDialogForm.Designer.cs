namespace FEBuilderGBA
{
    partial class MapEditorMarSizeDialogForm
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
            this.OKButton = new System.Windows.Forms.Button();
            this.ERROR_WIDTH = new System.Windows.Forms.Label();
            this.W = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.ERROR_WIDTH);
            this.panel1.Controls.Add(this.W);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 186);
            this.panel1.TabIndex = 0;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(230, 140);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(99, 43);
            this.OKButton.TabIndex = 13;
            this.OKButton.Text = "適応";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // ERROR_WIDTH
            // 
            this.ERROR_WIDTH.AutoSize = true;
            this.ERROR_WIDTH.Location = new System.Drawing.Point(14, 58);
            this.ERROR_WIDTH.Name = "ERROR_WIDTH";
            this.ERROR_WIDTH.Size = new System.Drawing.Size(275, 36);
            this.ERROR_WIDTH.TabIndex = 12;
            this.ERROR_WIDTH.Text = "データサイズが不一致。\r\n(データ数/2) % 幅 == 0 ではありません";
            // 
            // W
            // 
            this.W.Location = new System.Drawing.Point(129, 4);
            this.W.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.W.Name = "W";
            this.W.Size = new System.Drawing.Size(73, 25);
            this.W.TabIndex = 11;
            this.W.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.W.ValueChanged += new System.EventHandler(this.W_ValueChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "幅";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MapEditorMarSizeDialogForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(356, 209);
            this.Controls.Add(this.panel1);
            this.Name = "MapEditorMarSizeDialogForm";
            this.Text = "MAR読込";
            this.Load += new System.EventHandler(this.MapEditorMarSizeDialogForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown W;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label ERROR_WIDTH;
    }
}