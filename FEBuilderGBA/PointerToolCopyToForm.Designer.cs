namespace FEBuilderGBA
{
    partial class PointerToolCopyToForm
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
            this.CopyClipboard = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CopyPointer = new System.Windows.Forms.Button();
            this.CopyLittleEndian = new System.Windows.Forms.Button();
            this.HexButton = new System.Windows.Forms.Button();
            this.CopyNoDollGBARadBreakPoint = new System.Windows.Forms.Button();
            this.ValueTextBox = new FEBuilderGBA.TextBoxEx();
            this.SuspendLayout();
            // 
            // CopyClipboard
            // 
            this.CopyClipboard.Location = new System.Drawing.Point(13, 127);
            this.CopyClipboard.Margin = new System.Windows.Forms.Padding(4);
            this.CopyClipboard.Name = "CopyClipboard";
            this.CopyClipboard.Size = new System.Drawing.Size(404, 44);
            this.CopyClipboard.TabIndex = 1;
            this.CopyClipboard.Text = "クリップボードへコピー";
            this.CopyClipboard.UseVisualStyleBackColor = true;
            this.CopyClipboard.Click += new System.EventHandler(this.CopyClipboard_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "値:";
            // 
            // CopyPointer
            // 
            this.CopyPointer.Location = new System.Drawing.Point(17, 75);
            this.CopyPointer.Margin = new System.Windows.Forms.Padding(4);
            this.CopyPointer.Name = "CopyPointer";
            this.CopyPointer.Size = new System.Drawing.Size(404, 44);
            this.CopyPointer.TabIndex = 0;
            this.CopyPointer.Text = "ポインタとしてクリップボードへコピー";
            this.CopyPointer.UseVisualStyleBackColor = true;
            this.CopyPointer.Click += new System.EventHandler(this.CopyPointer_Click);
            // 
            // CopyLittleEndian
            // 
            this.CopyLittleEndian.Location = new System.Drawing.Point(13, 179);
            this.CopyLittleEndian.Margin = new System.Windows.Forms.Padding(4);
            this.CopyLittleEndian.Name = "CopyLittleEndian";
            this.CopyLittleEndian.Size = new System.Drawing.Size(404, 44);
            this.CopyLittleEndian.TabIndex = 2;
            this.CopyLittleEndian.Text = "リトルエンディアンポインタとしてクリップボードへコピー";
            this.CopyLittleEndian.UseVisualStyleBackColor = true;
            this.CopyLittleEndian.Click += new System.EventHandler(this.CopyLittleEndian_Click);
            // 
            // HexButton
            // 
            this.HexButton.Location = new System.Drawing.Point(16, 231);
            this.HexButton.Margin = new System.Windows.Forms.Padding(4);
            this.HexButton.Name = "HexButton";
            this.HexButton.Size = new System.Drawing.Size(404, 44);
            this.HexButton.TabIndex = 3;
            this.HexButton.Text = "バイナリエディタ";
            this.HexButton.UseVisualStyleBackColor = true;
            this.HexButton.Click += new System.EventHandler(this.HexButton_Click);
            // 
            // CopyNoDollGBARadBreakPoint
            // 
            this.CopyNoDollGBARadBreakPoint.Location = new System.Drawing.Point(17, 301);
            this.CopyNoDollGBARadBreakPoint.Margin = new System.Windows.Forms.Padding(4);
            this.CopyNoDollGBARadBreakPoint.Name = "CopyNoDollGBARadBreakPoint";
            this.CopyNoDollGBARadBreakPoint.Size = new System.Drawing.Size(404, 44);
            this.CopyNoDollGBARadBreakPoint.TabIndex = 4;
            this.CopyNoDollGBARadBreakPoint.Text = "no$gbaの読込ブレークポイントとしてコピー";
            this.CopyNoDollGBARadBreakPoint.UseVisualStyleBackColor = true;
            this.CopyNoDollGBARadBreakPoint.Click += new System.EventHandler(this.CopyNoDollGBARadBreakPoint_Click);
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.ErrorMessage = "";
            this.ValueTextBox.Location = new System.Drawing.Point(76, 14);
            this.ValueTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.Placeholder = "";
            this.ValueTextBox.Size = new System.Drawing.Size(119, 25);
            this.ValueTextBox.TabIndex = 4;
            // 
            // PointerToolCopyToForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(433, 366);
            this.Controls.Add(this.CopyNoDollGBARadBreakPoint);
            this.Controls.Add(this.HexButton);
            this.Controls.Add(this.CopyLittleEndian);
            this.Controls.Add(this.CopyPointer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ValueTextBox);
            this.Controls.Add(this.CopyClipboard);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PointerToolCopyToForm";
            this.Text = "コピー方法";
            this.Load += new System.EventHandler(this.PointerToolCopyToForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CopyClipboard;
        private FEBuilderGBA.TextBoxEx ValueTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CopyPointer;
        private System.Windows.Forms.Button CopyLittleEndian;
        private System.Windows.Forms.Button HexButton;
        private System.Windows.Forms.Button CopyNoDollGBARadBreakPoint;
    }
}