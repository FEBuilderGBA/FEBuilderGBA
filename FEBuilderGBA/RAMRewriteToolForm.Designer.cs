namespace FEBuilderGBA
{
    partial class RAMRewriteToolForm
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
            this.CopyPointer = new System.Windows.Forms.Button();
            this.CopyNoDollGBARadBreakPoint = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ReWriteValue = new System.Windows.Forms.NumericUpDown();
            this.ReWriteButton = new System.Windows.Forms.Button();
            this.ValueTextBox = new FEBuilderGBA.TextBoxEx();
            this.CopyLittleEndian = new System.Windows.Forms.Button();
            this.CopyClipboard = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ReWriteValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "値";
            // 
            // CopyPointer
            // 
            this.CopyPointer.Location = new System.Drawing.Point(19, 56);
            this.CopyPointer.Name = "CopyPointer";
            this.CopyPointer.Size = new System.Drawing.Size(404, 44);
            this.CopyPointer.TabIndex = 2;
            this.CopyPointer.Text = "ポインタとしてクリップボードへコピー";
            this.CopyPointer.UseVisualStyleBackColor = true;
            this.CopyPointer.Click += new System.EventHandler(this.CopyPointer_Click);
            // 
            // CopyNoDollGBARadBreakPoint
            // 
            this.CopyNoDollGBARadBreakPoint.Location = new System.Drawing.Point(19, 227);
            this.CopyNoDollGBARadBreakPoint.Margin = new System.Windows.Forms.Padding(4);
            this.CopyNoDollGBARadBreakPoint.Name = "CopyNoDollGBARadBreakPoint";
            this.CopyNoDollGBARadBreakPoint.Size = new System.Drawing.Size(404, 44);
            this.CopyNoDollGBARadBreakPoint.TabIndex = 8;
            this.CopyNoDollGBARadBreakPoint.Text = "no$gbaの読込ブレークポイントとしてコピー";
            this.CopyNoDollGBARadBreakPoint.UseVisualStyleBackColor = true;
            this.CopyNoDollGBARadBreakPoint.Click += new System.EventHandler(this.CopyNoDollGBARadBreakPoint_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 18);
            this.label2.TabIndex = 9;
            this.label2.Text = "データの直書き換え";
            // 
            // ReWriteValue
            // 
            this.ReWriteValue.Hexadecimal = true;
            this.ReWriteValue.Location = new System.Drawing.Point(21, 338);
            this.ReWriteValue.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReWriteValue.Name = "ReWriteValue";
            this.ReWriteValue.Size = new System.Drawing.Size(186, 25);
            this.ReWriteValue.TabIndex = 10;
            this.ReWriteValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReWriteValue_KeyDown);
            // 
            // ReWriteButton
            // 
            this.ReWriteButton.Location = new System.Drawing.Point(21, 370);
            this.ReWriteButton.Margin = new System.Windows.Forms.Padding(4);
            this.ReWriteButton.Name = "ReWriteButton";
            this.ReWriteButton.Size = new System.Drawing.Size(404, 44);
            this.ReWriteButton.TabIndex = 11;
            this.ReWriteButton.Text = "値の書き換え";
            this.ReWriteButton.UseVisualStyleBackColor = true;
            this.ReWriteButton.Click += new System.EventHandler(this.ReWriteButton_Click);
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.ErrorMessage = "";
            this.ValueTextBox.Location = new System.Drawing.Point(74, 17);
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.Placeholder = "";
            this.ValueTextBox.ReadOnly = true;
            this.ValueTextBox.Size = new System.Drawing.Size(100, 25);
            this.ValueTextBox.TabIndex = 1;
            // 
            // CopyLittleEndian
            // 
            this.CopyLittleEndian.Location = new System.Drawing.Point(19, 167);
            this.CopyLittleEndian.Margin = new System.Windows.Forms.Padding(4);
            this.CopyLittleEndian.Name = "CopyLittleEndian";
            this.CopyLittleEndian.Size = new System.Drawing.Size(404, 44);
            this.CopyLittleEndian.TabIndex = 6;
            this.CopyLittleEndian.Text = "リトルエンディアンポインタとしてクリップボードへコピー";
            this.CopyLittleEndian.UseVisualStyleBackColor = true;
            this.CopyLittleEndian.Click += new System.EventHandler(this.CopyLittleEndian_Click);
            // 
            // CopyClipboard
            // 
            this.CopyClipboard.Location = new System.Drawing.Point(19, 115);
            this.CopyClipboard.Margin = new System.Windows.Forms.Padding(4);
            this.CopyClipboard.Name = "CopyClipboard";
            this.CopyClipboard.Size = new System.Drawing.Size(404, 44);
            this.CopyClipboard.TabIndex = 5;
            this.CopyClipboard.Text = "クリップボードへコピー";
            this.CopyClipboard.UseVisualStyleBackColor = true;
            this.CopyClipboard.Click += new System.EventHandler(this.CopyClipboard_Click);
            // 
            // RAMRewriteToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(447, 448);
            this.Controls.Add(this.ReWriteButton);
            this.Controls.Add(this.ReWriteValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CopyNoDollGBARadBreakPoint);
            this.Controls.Add(this.CopyLittleEndian);
            this.Controls.Add(this.CopyClipboard);
            this.Controls.Add(this.CopyPointer);
            this.Controls.Add(this.ValueTextBox);
            this.Controls.Add(this.label1);
            this.Name = "RAMRewriteToolForm";
            this.Text = "RAM コピー方法";
            this.Load += new System.EventHandler(this.RAMRewriteToolForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReWriteValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private TextBoxEx ValueTextBox;
        private System.Windows.Forms.Button CopyPointer;
        private System.Windows.Forms.Button CopyNoDollGBARadBreakPoint;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReWriteValue;
        private System.Windows.Forms.Button ReWriteButton;
        private System.Windows.Forms.Button CopyLittleEndian;
        private System.Windows.Forms.Button CopyClipboard;
    }
}