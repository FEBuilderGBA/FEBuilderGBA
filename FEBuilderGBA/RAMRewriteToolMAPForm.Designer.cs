namespace FEBuilderGBA
{
    partial class RAMRewriteToolMAPForm
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
            this.CopyLittleEndian = new System.Windows.Forms.Button();
            this.CopyClipboard = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ReWriteValueX = new System.Windows.Forms.NumericUpDown();
            this.ReWriteButton = new System.Windows.Forms.Button();
            this.ValueTextBox = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ReWriteValueY = new System.Windows.Forms.NumericUpDown();
            this.MapPictureBox = new FEBuilderGBA.MapPictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ReWriteValueX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReWriteValueY)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "値";
            // 
            // CopyPointer
            // 
            this.CopyPointer.Location = new System.Drawing.Point(6, 45);
            this.CopyPointer.Name = "CopyPointer";
            this.CopyPointer.Size = new System.Drawing.Size(384, 44);
            this.CopyPointer.TabIndex = 2;
            this.CopyPointer.Text = "ポインタとしてクリップボードへコピー";
            this.CopyPointer.UseVisualStyleBackColor = true;
            this.CopyPointer.Click += new System.EventHandler(this.CopyPointer_Click);
            // 
            // CopyNoDollGBARadBreakPoint
            // 
            this.CopyNoDollGBARadBreakPoint.Location = new System.Drawing.Point(6, 216);
            this.CopyNoDollGBARadBreakPoint.Margin = new System.Windows.Forms.Padding(4);
            this.CopyNoDollGBARadBreakPoint.Name = "CopyNoDollGBARadBreakPoint";
            this.CopyNoDollGBARadBreakPoint.Size = new System.Drawing.Size(384, 44);
            this.CopyNoDollGBARadBreakPoint.TabIndex = 8;
            this.CopyNoDollGBARadBreakPoint.Text = "no$gbaの読込ブレークポイントとしてコピー";
            this.CopyNoDollGBARadBreakPoint.UseVisualStyleBackColor = true;
            this.CopyNoDollGBARadBreakPoint.Click += new System.EventHandler(this.CopyNoDollGBARadBreakPoint_Click);
            // 
            // CopyLittleEndian
            // 
            this.CopyLittleEndian.Location = new System.Drawing.Point(6, 156);
            this.CopyLittleEndian.Margin = new System.Windows.Forms.Padding(4);
            this.CopyLittleEndian.Name = "CopyLittleEndian";
            this.CopyLittleEndian.Size = new System.Drawing.Size(384, 44);
            this.CopyLittleEndian.TabIndex = 6;
            this.CopyLittleEndian.Text = "リトルエンディアンポインタとしてクリップボードへコピー";
            this.CopyLittleEndian.UseVisualStyleBackColor = true;
            this.CopyLittleEndian.Click += new System.EventHandler(this.CopyLittleEndian_Click);
            // 
            // CopyClipboard
            // 
            this.CopyClipboard.Location = new System.Drawing.Point(6, 104);
            this.CopyClipboard.Margin = new System.Windows.Forms.Padding(4);
            this.CopyClipboard.Name = "CopyClipboard";
            this.CopyClipboard.Size = new System.Drawing.Size(384, 44);
            this.CopyClipboard.TabIndex = 5;
            this.CopyClipboard.Text = "クリップボードへコピー";
            this.CopyClipboard.UseVisualStyleBackColor = true;
            this.CopyClipboard.Click += new System.EventHandler(this.CopyClipboard_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 297);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 18);
            this.label2.TabIndex = 9;
            this.label2.Text = "データの直書き換え";
            // 
            // ReWriteValueX
            // 
            this.ReWriteValueX.Hexadecimal = true;
            this.ReWriteValueX.Location = new System.Drawing.Point(47, 327);
            this.ReWriteValueX.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReWriteValueX.Name = "ReWriteValueX";
            this.ReWriteValueX.Size = new System.Drawing.Size(95, 25);
            this.ReWriteValueX.TabIndex = 10;
            this.ReWriteValueX.ValueChanged += new System.EventHandler(this.ReWriteValueX_ValueChanged);
            // 
            // ReWriteButton
            // 
            this.ReWriteButton.Location = new System.Drawing.Point(8, 359);
            this.ReWriteButton.Margin = new System.Windows.Forms.Padding(4);
            this.ReWriteButton.Name = "ReWriteButton";
            this.ReWriteButton.Size = new System.Drawing.Size(384, 44);
            this.ReWriteButton.TabIndex = 11;
            this.ReWriteButton.Text = "値の書き換え";
            this.ReWriteButton.UseVisualStyleBackColor = true;
            this.ReWriteButton.Click += new System.EventHandler(this.ReWriteButton_Click);
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.ErrorMessage = "";
            this.ValueTextBox.Location = new System.Drawing.Point(61, 6);
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.Placeholder = "";
            this.ValueTextBox.ReadOnly = true;
            this.ValueTextBox.Size = new System.Drawing.Size(100, 25);
            this.ValueTextBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 329);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 18);
            this.label3.TabIndex = 12;
            this.label3.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 327);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "Y";
            // 
            // ReWriteValueY
            // 
            this.ReWriteValueY.Hexadecimal = true;
            this.ReWriteValueY.Location = new System.Drawing.Point(218, 325);
            this.ReWriteValueY.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReWriteValueY.Name = "ReWriteValueY";
            this.ReWriteValueY.Size = new System.Drawing.Size(95, 25);
            this.ReWriteValueY.TabIndex = 14;
            this.ReWriteValueY.ValueChanged += new System.EventHandler(this.ReWriteValueX_ValueChanged);
            // 
            // MapPictureBox
            // 
            this.MapPictureBox.AutoScroll = true;
            this.MapPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapPictureBox.Location = new System.Drawing.Point(0, 0);
            this.MapPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.MapPictureBox.Name = "MapPictureBox";
            this.MapPictureBox.Size = new System.Drawing.Size(1044, 928);
            this.MapPictureBox.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CopyLittleEndian);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ReWriteValueY);
            this.panel1.Controls.Add(this.ValueTextBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.CopyPointer);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.CopyClipboard);
            this.panel1.Controls.Add(this.ReWriteButton);
            this.panel1.Controls.Add(this.CopyNoDollGBARadBreakPoint);
            this.panel1.Controls.Add(this.ReWriteValueX);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 928);
            this.panel1.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.MapPictureBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(396, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1044, 928);
            this.panel2.TabIndex = 17;
            // 
            // RAMRewriteToolMAPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1440, 928);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "RAMRewriteToolMAPForm";
            this.Text = "RAM コピー方法";
            this.Load += new System.EventHandler(this.RAMRewriteToolForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReWriteValueX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReWriteValueY)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private TextBoxEx ValueTextBox;
        private System.Windows.Forms.Button CopyPointer;
        private System.Windows.Forms.Button CopyNoDollGBARadBreakPoint;
        private System.Windows.Forms.Button CopyLittleEndian;
        private System.Windows.Forms.Button CopyClipboard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReWriteValueX;
        private System.Windows.Forms.Button ReWriteButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ReWriteValueY;
        private MapPictureBox MapPictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}