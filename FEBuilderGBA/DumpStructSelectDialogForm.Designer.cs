namespace FEBuilderGBA
{
    partial class DumpStructSelectDialogForm
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
            this.BinaryButton = new System.Windows.Forms.Button();
            this.EAALLButton = new System.Windows.Forms.Button();
            this.TSVALLButton = new System.Windows.Forms.Button();
            this.CSVButton = new System.Windows.Forms.Button();
            this.STRUCTButton = new System.Windows.Forms.Button();
            this.NMMButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CopyNoDollGBARadBreakPoint = new System.Windows.Forms.Button();
            this.CopyLittleEndian = new System.Windows.Forms.Button();
            this.CopyPointer = new System.Windows.Forms.Button();
            this.CopyClipboard = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ValueTextBox = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ImportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BinaryButton
            // 
            this.BinaryButton.Location = new System.Drawing.Point(12, 70);
            this.BinaryButton.Name = "BinaryButton";
            this.BinaryButton.Size = new System.Drawing.Size(655, 51);
            this.BinaryButton.TabIndex = 0;
            this.BinaryButton.Text = "選択しているアドレスをバイナリエディタで表示";
            this.BinaryButton.UseVisualStyleBackColor = true;
            this.BinaryButton.Click += new System.EventHandler(this.BinaryButton_Click);
            // 
            // EAALLButton
            // 
            this.EAALLButton.Location = new System.Drawing.Point(12, 380);
            this.EAALLButton.Name = "EAALLButton";
            this.EAALLButton.Size = new System.Drawing.Size(655, 51);
            this.EAALLButton.TabIndex = 5;
            this.EAALLButton.Text = "表示しているリストにあるすべてのデータのEA形式を取得";
            this.EAALLButton.UseVisualStyleBackColor = true;
            this.EAALLButton.Click += new System.EventHandler(this.EAALLButton_Click);
            // 
            // TSVALLButton
            // 
            this.TSVALLButton.Location = new System.Drawing.Point(12, 489);
            this.TSVALLButton.Name = "TSVALLButton";
            this.TSVALLButton.Size = new System.Drawing.Size(655, 51);
            this.TSVALLButton.TabIndex = 7;
            this.TSVALLButton.Text = "表示しているリストにあるすべてのデータのTSV形式を取得";
            this.TSVALLButton.UseVisualStyleBackColor = true;
            this.TSVALLButton.Click += new System.EventHandler(this.TSVButton_Click);
            // 
            // CSVButton
            // 
            this.CSVButton.Location = new System.Drawing.Point(12, 435);
            this.CSVButton.Name = "CSVButton";
            this.CSVButton.Size = new System.Drawing.Size(655, 51);
            this.CSVButton.TabIndex = 6;
            this.CSVButton.Text = "表示しているリストにあるすべてのデータのCSV形式を取得";
            this.CSVButton.UseVisualStyleBackColor = true;
            this.CSVButton.Click += new System.EventHandler(this.CSVButton_Click);
            // 
            // STRUCTButton
            // 
            this.STRUCTButton.Location = new System.Drawing.Point(12, 574);
            this.STRUCTButton.Name = "STRUCTButton";
            this.STRUCTButton.Size = new System.Drawing.Size(655, 51);
            this.STRUCTButton.TabIndex = 8;
            this.STRUCTButton.Text = "C言語の構造体の表示";
            this.STRUCTButton.UseVisualStyleBackColor = true;
            this.STRUCTButton.Click += new System.EventHandler(this.STRUCTButton_Click);
            // 
            // NMMButton
            // 
            this.NMMButton.Location = new System.Drawing.Point(12, 629);
            this.NMMButton.Name = "NMMButton";
            this.NMMButton.Size = new System.Drawing.Size(655, 51);
            this.NMMButton.TabIndex = 10;
            this.NMMButton.Text = "NightmareModule nmmファイルの作成";
            this.NMMButton.UseVisualStyleBackColor = true;
            this.NMMButton.Click += new System.EventHandler(this.NMMButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 355);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 75;
            this.label1.Text = "データダンプ";
            // 
            // CopyNoDollGBARadBreakPoint
            // 
            this.CopyNoDollGBARadBreakPoint.Location = new System.Drawing.Point(13, 301);
            this.CopyNoDollGBARadBreakPoint.Margin = new System.Windows.Forms.Padding(4);
            this.CopyNoDollGBARadBreakPoint.Name = "CopyNoDollGBARadBreakPoint";
            this.CopyNoDollGBARadBreakPoint.Size = new System.Drawing.Size(651, 44);
            this.CopyNoDollGBARadBreakPoint.TabIndex = 4;
            this.CopyNoDollGBARadBreakPoint.Text = "no$gbaの読込ブレークポイントとしてコピー";
            this.CopyNoDollGBARadBreakPoint.UseVisualStyleBackColor = true;
            this.CopyNoDollGBARadBreakPoint.Click += new System.EventHandler(this.CopyNoDollGBARadBreakPoint_Click);
            // 
            // CopyLittleEndian
            // 
            this.CopyLittleEndian.Location = new System.Drawing.Point(13, 252);
            this.CopyLittleEndian.Margin = new System.Windows.Forms.Padding(4);
            this.CopyLittleEndian.Name = "CopyLittleEndian";
            this.CopyLittleEndian.Size = new System.Drawing.Size(651, 44);
            this.CopyLittleEndian.TabIndex = 3;
            this.CopyLittleEndian.Text = "リトルエンディアンポインタとしてクリップボードへコピー";
            this.CopyLittleEndian.UseVisualStyleBackColor = true;
            this.CopyLittleEndian.Click += new System.EventHandler(this.CopyLittleEndian_Click);
            // 
            // CopyPointer
            // 
            this.CopyPointer.Location = new System.Drawing.Point(13, 153);
            this.CopyPointer.Margin = new System.Windows.Forms.Padding(4);
            this.CopyPointer.Name = "CopyPointer";
            this.CopyPointer.Size = new System.Drawing.Size(651, 44);
            this.CopyPointer.TabIndex = 1;
            this.CopyPointer.Text = "ポインタとしてクリップボードへコピー";
            this.CopyPointer.UseVisualStyleBackColor = true;
            this.CopyPointer.Click += new System.EventHandler(this.CopyPointer_Click);
            // 
            // CopyClipboard
            // 
            this.CopyClipboard.Location = new System.Drawing.Point(13, 203);
            this.CopyClipboard.Margin = new System.Windows.Forms.Padding(4);
            this.CopyClipboard.Name = "CopyClipboard";
            this.CopyClipboard.Size = new System.Drawing.Size(651, 44);
            this.CopyClipboard.TabIndex = 2;
            this.CopyClipboard.Text = "クリップボードへコピー";
            this.CopyClipboard.UseVisualStyleBackColor = true;
            this.CopyClipboard.Click += new System.EventHandler(this.CopyClipboard_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 80;
            this.label2.Text = "クリップボード";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 18);
            this.label3.TabIndex = 81;
            this.label3.Text = "バイナリエディタ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 553);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 18);
            this.label4.TabIndex = 82;
            this.label4.Text = "構造体";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 18);
            this.label5.TabIndex = 83;
            this.label5.Text = "値:";
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.AutoSize = true;
            this.ValueTextBox.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ValueTextBox.Location = new System.Drawing.Point(83, 6);
            this.ValueTextBox.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.Size = new System.Drawing.Size(72, 28);
            this.ValueTextBox.TabIndex = 85;
            this.ValueTextBox.Text = "1234";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 690);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 18);
            this.label6.TabIndex = 87;
            this.label6.Text = "インポート";
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(12, 713);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(655, 51);
            this.ImportButton.TabIndex = 86;
            this.ImportButton.Text = "ダンプしていたデータのインポート";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // DumpStructSelectDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(679, 792);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.ValueTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CopyNoDollGBARadBreakPoint);
            this.Controls.Add(this.CopyLittleEndian);
            this.Controls.Add(this.CopyPointer);
            this.Controls.Add(this.CopyClipboard);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NMMButton);
            this.Controls.Add(this.STRUCTButton);
            this.Controls.Add(this.TSVALLButton);
            this.Controls.Add(this.CSVButton);
            this.Controls.Add(this.EAALLButton);
            this.Controls.Add(this.BinaryButton);
            this.Name = "DumpStructSelectDialogForm";
            this.Text = "データを表示";
            this.Load += new System.EventHandler(this.DumpStructSelectDialogForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BinaryButton;
        private System.Windows.Forms.Button EAALLButton;
        private System.Windows.Forms.Button TSVALLButton;
        private System.Windows.Forms.Button CSVButton;
        private System.Windows.Forms.Button STRUCTButton;
        private System.Windows.Forms.Button NMMButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CopyNoDollGBARadBreakPoint;
        private System.Windows.Forms.Button CopyLittleEndian;
        private System.Windows.Forms.Button CopyPointer;
        private System.Windows.Forms.Button CopyClipboard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ValueTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ImportButton;
    }
}