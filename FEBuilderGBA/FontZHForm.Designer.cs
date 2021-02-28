namespace FEBuilderGBA
{
    partial class FontZHForm
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
            this.FontType = new System.Windows.Forms.ComboBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchChar = new FEBuilderGBA.TextBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.ZoomComboBox = new System.Windows.Forms.ComboBox();
            this.FontWidth = new System.Windows.Forms.NumericUpDown();
            this.FontSample = new FEBuilderGBA.TextBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.FontPictureBox = new FEBuilderGBA.InterpolatedPictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.UseFontNameButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.UseFontNameTextEdit = new FEBuilderGBA.TextBoxEx();
            this.AutoGenbutton = new System.Windows.Forms.Button();
            this.customColorGroupBox2 = new FEBuilderGBA.CustomColorGroupBox();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.customColorGroupBox3 = new FEBuilderGBA.CustomColorGroupBox();
            this.ExportALLButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontPictureBox)).BeginInit();
            this.customColorGroupBox1.SuspendLayout();
            this.customColorGroupBox2.SuspendLayout();
            this.customColorGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // FontType
            // 
            this.FontType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FontType.FormattingEnabled = true;
            this.FontType.Items.AddRange(new object[] {
            "アイテムフォント",
            "セリフフォント"});
            this.FontType.Location = new System.Drawing.Point(172, 10);
            this.FontType.Margin = new System.Windows.Forms.Padding(2);
            this.FontType.Name = "FontType";
            this.FontType.Size = new System.Drawing.Size(380, 26);
            this.FontType.TabIndex = 0;
            this.FontType.SelectedIndexChanged += new System.EventHandler(this.FontType_SelectedIndexChanged);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(452, 43);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(2);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(100, 30);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "検索";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(11, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 30);
            this.label2.TabIndex = 33;
            this.label2.Text = "フォントの種類";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.SearchChar);
            this.panel1.Controls.Add(this.FontType);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(568, 80);
            this.panel1.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(11, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 30);
            this.label1.TabIndex = 34;
            this.label1.Text = "検索文字";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SearchChar
            // 
            this.SearchChar.ErrorMessage = "";
            this.SearchChar.Location = new System.Drawing.Point(242, 46);
            this.SearchChar.Margin = new System.Windows.Forms.Padding(2);
            this.SearchChar.Name = "SearchChar";
            this.SearchChar.Placeholder = "";
            this.SearchChar.Size = new System.Drawing.Size(100, 25);
            this.SearchChar.TabIndex = 1;
            this.SearchChar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchChar_KeyDown);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.Address);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.ZoomComboBox);
            this.panel2.Controls.Add(this.FontWidth);
            this.panel2.Controls.Add(this.FontSample);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.FontPictureBox);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.WriteButton);
            this.panel2.Location = new System.Drawing.Point(12, 99);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(568, 284);
            this.panel2.TabIndex = 35;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(173, 248);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Size = new System.Drawing.Size(119, 25);
            this.Address.TabIndex = 43;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(11, 248);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 30);
            this.label6.TabIndex = 42;
            this.label6.Text = "アドレス";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ZoomComboBox
            // 
            this.ZoomComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ZoomComboBox.FormattingEnabled = true;
            this.ZoomComboBox.Items.AddRange(new object[] {
            "拡大しない",
            "2倍拡大",
            "3倍拡大",
            "4倍拡大"});
            this.ZoomComboBox.Location = new System.Drawing.Point(266, 175);
            this.ZoomComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.ZoomComboBox.Name = "ZoomComboBox";
            this.ZoomComboBox.Size = new System.Drawing.Size(286, 26);
            this.ZoomComboBox.TabIndex = 39;
            this.ZoomComboBox.SelectedIndexChanged += new System.EventHandler(this.ZoomComboBox_SelectedIndexChanged);
            // 
            // FontWidth
            // 
            this.FontWidth.Location = new System.Drawing.Point(172, 211);
            this.FontWidth.Margin = new System.Windows.Forms.Padding(2);
            this.FontWidth.Name = "FontWidth";
            this.FontWidth.Size = new System.Drawing.Size(96, 25);
            this.FontWidth.TabIndex = 38;
            this.FontWidth.ValueChanged += new System.EventHandler(this.FontWidth_ValueChanged);
            // 
            // FontSample
            // 
            this.FontSample.ErrorMessage = "";
            this.FontSample.Location = new System.Drawing.Point(242, 11);
            this.FontSample.Margin = new System.Windows.Forms.Padding(2);
            this.FontSample.Name = "FontSample";
            this.FontSample.Placeholder = "";
            this.FontSample.Size = new System.Drawing.Size(310, 25);
            this.FontSample.TabIndex = 37;
            this.FontSample.TextChanged += new System.EventHandler(this.FontSample_TextChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(11, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 30);
            this.label4.TabIndex = 36;
            this.label4.Text = "サンプル";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FontPictureBox
            // 
            this.FontPictureBox.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.FontPictureBox.Location = new System.Drawing.Point(40, 45);
            this.FontPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.FontPictureBox.Name = "FontPictureBox";
            this.FontPictureBox.Size = new System.Drawing.Size(512, 126);
            this.FontPictureBox.TabIndex = 35;
            this.FontPictureBox.TabStop = false;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(11, 211);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 30);
            this.label3.TabIndex = 34;
            this.label3.Text = "フォント幅";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(314, 244);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(238, 30);
            this.WriteButton.TabIndex = 2;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.UseFontNameButton);
            this.customColorGroupBox1.Controls.Add(this.label5);
            this.customColorGroupBox1.Controls.Add(this.UseFontNameTextEdit);
            this.customColorGroupBox1.Controls.Add(this.AutoGenbutton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 391);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(568, 133);
            this.customColorGroupBox1.TabIndex = 40;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "自動作成";
            this.customColorGroupBox1.Visible = false;
            // 
            // UseFontNameButton
            // 
            this.UseFontNameButton.Location = new System.Drawing.Point(457, 58);
            this.UseFontNameButton.Margin = new System.Windows.Forms.Padding(2);
            this.UseFontNameButton.Name = "UseFontNameButton";
            this.UseFontNameButton.Size = new System.Drawing.Size(100, 30);
            this.UseFontNameButton.TabIndex = 95;
            this.UseFontNameButton.Text = "変更";
            this.UseFontNameButton.UseVisualStyleBackColor = true;
            this.UseFontNameButton.Click += new System.EventHandler(this.UseFontNameButton_Click);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(12, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 30);
            this.label5.TabIndex = 93;
            this.label5.Text = "利用フォント";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UseFontNameTextEdit
            // 
            this.UseFontNameTextEdit.ErrorMessage = "";
            this.UseFontNameTextEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UseFontNameTextEdit.Location = new System.Drawing.Point(12, 62);
            this.UseFontNameTextEdit.Margin = new System.Windows.Forms.Padding(2);
            this.UseFontNameTextEdit.Name = "UseFontNameTextEdit";
            this.UseFontNameTextEdit.Placeholder = "";
            this.UseFontNameTextEdit.ReadOnly = true;
            this.UseFontNameTextEdit.Size = new System.Drawing.Size(441, 26);
            this.UseFontNameTextEdit.TabIndex = 94;
            this.UseFontNameTextEdit.DoubleClick += new System.EventHandler(this.UseFontNameButton_Click);
            // 
            // AutoGenbutton
            // 
            this.AutoGenbutton.Location = new System.Drawing.Point(180, 98);
            this.AutoGenbutton.Margin = new System.Windows.Forms.Padding(2);
            this.AutoGenbutton.Name = "AutoGenbutton";
            this.AutoGenbutton.Size = new System.Drawing.Size(377, 30);
            this.AutoGenbutton.TabIndex = 40;
            this.AutoGenbutton.Text = "自動生成";
            this.AutoGenbutton.UseVisualStyleBackColor = true;
            this.AutoGenbutton.Click += new System.EventHandler(this.AutoGenbutton_Click);
            // 
            // customColorGroupBox2
            // 
            this.customColorGroupBox2.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox2.Controls.Add(this.ImportButton);
            this.customColorGroupBox2.Controls.Add(this.ExportButton);
            this.customColorGroupBox2.Location = new System.Drawing.Point(12, 537);
            this.customColorGroupBox2.Name = "customColorGroupBox2";
            this.customColorGroupBox2.Size = new System.Drawing.Size(568, 64);
            this.customColorGroupBox2.TabIndex = 39;
            this.customColorGroupBox2.TabStop = false;
            this.customColorGroupBox2.Text = "インポート/エクスポート";
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(13, 26);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(274, 30);
            this.ImportButton.TabIndex = 92;
            this.ImportButton.Text = "画像読込";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Visible = false;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(295, 26);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(260, 30);
            this.ExportButton.TabIndex = 91;
            this.ExportButton.Text = "画像取出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // customColorGroupBox3
            // 
            this.customColorGroupBox3.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox3.Controls.Add(this.ExportALLButton);
            this.customColorGroupBox3.Location = new System.Drawing.Point(15, 612);
            this.customColorGroupBox3.Name = "customColorGroupBox3";
            this.customColorGroupBox3.Size = new System.Drawing.Size(568, 63);
            this.customColorGroupBox3.TabIndex = 41;
            this.customColorGroupBox3.TabStop = false;
            this.customColorGroupBox3.Text = "一括インポート/エクスポート";
            // 
            // ExportALLButton
            // 
            this.ExportALLButton.Location = new System.Drawing.Point(292, 26);
            this.ExportALLButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportALLButton.Name = "ExportALLButton";
            this.ExportALLButton.Size = new System.Drawing.Size(258, 30);
            this.ExportALLButton.TabIndex = 93;
            this.ExportALLButton.Text = "一括エクスポート";
            this.ExportALLButton.UseVisualStyleBackColor = true;
            this.ExportALLButton.Click += new System.EventHandler(this.ExportALLButton_Click);
            // 
            // FontZHForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(598, 678);
            this.Controls.Add(this.customColorGroupBox3);
            this.Controls.Add(this.customColorGroupBox1);
            this.Controls.Add(this.customColorGroupBox2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FontZHForm";
            this.Text = "フォント";
            this.Load += new System.EventHandler(this.FontForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontPictureBox)).EndInit();
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            this.customColorGroupBox2.ResumeLayout(false);
            this.customColorGroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox FontType;
        private FEBuilderGBA.TextBoxEx SearchChar;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button WriteButton;
        private FEBuilderGBA.TextBoxEx FontSample;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown FontWidth;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportButton;
        private InterpolatedPictureBox FontPictureBox;
        private System.Windows.Forms.ComboBox ZoomComboBox;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label6;
        private CustomColorGroupBox customColorGroupBox2;
        private CustomColorGroupBox customColorGroupBox1;
        private System.Windows.Forms.Button UseFontNameButton;
        private System.Windows.Forms.Label label5;
        private TextBoxEx UseFontNameTextEdit;
        private System.Windows.Forms.Button AutoGenbutton;
        private CustomColorGroupBox customColorGroupBox3;
        private System.Windows.Forms.Button ExportALLButton;
    }
}