namespace FEBuilderGBA
{
    partial class ImageChapterTitleForm
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
            this.label23 = new System.Windows.Forms.Label();
            this.P4 = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.P8 = new System.Windows.Forms.NumericUpDown();
            this.J_8 = new System.Windows.Forms.Label();
            this.J_4 = new System.Windows.Forms.Label();
            this.J_0 = new System.Windows.Forms.Label();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.X_SAVE_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ImportButton3 = new System.Windows.Forms.Button();
            this.X_TITLE_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.ImportButton2 = new System.Windows.Forms.Button();
            this.ExportButton3 = new System.Windows.Forms.Button();
            this.ImportButton1 = new System.Windows.Forms.Button();
            this.X_CHAPTER_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.ExportButton2 = new System.Windows.Forms.Button();
            this.ExportButton1 = new System.Windows.Forms.Button();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.EXPLAIN = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_SAVE_PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_TITLE_PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_CHAPTER_PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.AddressPanel.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-2, -1);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(84, 30);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P4
            // 
            this.P4.Hexadecimal = true;
            this.P4.Location = new System.Drawing.Point(142, 68);
            this.P4.Margin = new System.Windows.Forms.Padding(2);
            this.P4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P4.Name = "P4";
            this.P4.Size = new System.Drawing.Size(130, 25);
            this.P4.TabIndex = 87;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(20, 18);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1177, 30);
            this.panel1.TabIndex = 66;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(451, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(264, -1);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(354, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(125, 2);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // P8
            // 
            this.P8.Hexadecimal = true;
            this.P8.Location = new System.Drawing.Point(142, 130);
            this.P8.Margin = new System.Windows.Forms.Padding(2);
            this.P8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P8.Name = "P8";
            this.P8.Size = new System.Drawing.Size(130, 25);
            this.P8.TabIndex = 86;
            // 
            // J_8
            // 
            this.J_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8.Location = new System.Drawing.Point(5, 130);
            this.J_8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_8.Name = "J_8";
            this.J_8.Size = new System.Drawing.Size(130, 30);
            this.J_8.TabIndex = 70;
            this.J_8.Text = "章タイトル";
            this.J_8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(5, 68);
            this.J_4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(130, 30);
            this.J_4.TabIndex = 69;
            this.J_4.Text = "章画像";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(5, 11);
            this.J_0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(130, 30);
            this.J_0.TabIndex = 68;
            this.J_0.Text = "セーブ画像";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(312, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(225, -2);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_SAVE_PIC
            // 
            this.X_SAVE_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_SAVE_PIC.Location = new System.Drawing.Point(282, 3);
            this.X_SAVE_PIC.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.X_SAVE_PIC.Name = "X_SAVE_PIC";
            this.X_SAVE_PIC.Size = new System.Drawing.Size(351, 54);
            this.X_SAVE_PIC.TabIndex = 67;
            this.X_SAVE_PIC.TabStop = false;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(140, 11);
            this.P0.Margin = new System.Windows.Forms.Padding(2);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 88;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.EXPLAIN);
            this.panel2.Controls.Add(this.ImportButton3);
            this.panel2.Controls.Add(this.X_TITLE_PIC);
            this.panel2.Controls.Add(this.ImportButton2);
            this.panel2.Controls.Add(this.ExportButton3);
            this.panel2.Controls.Add(this.ImportButton1);
            this.panel2.Controls.Add(this.X_CHAPTER_PIC);
            this.panel2.Controls.Add(this.P0);
            this.panel2.Controls.Add(this.ExportButton2);
            this.panel2.Controls.Add(this.P4);
            this.panel2.Controls.Add(this.P8);
            this.panel2.Controls.Add(this.ExportButton1);
            this.panel2.Controls.Add(this.J_8);
            this.panel2.Controls.Add(this.J_4);
            this.panel2.Controls.Add(this.J_0);
            this.panel2.Controls.Add(this.X_SAVE_PIC);
            this.panel2.Location = new System.Drawing.Point(285, 77);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(914, 437);
            this.panel2.TabIndex = 67;
            // 
            // ImportButton3
            // 
            this.ImportButton3.Location = new System.Drawing.Point(581, 260);
            this.ImportButton3.Margin = new System.Windows.Forms.Padding(2);
            this.ImportButton3.Name = "ImportButton3";
            this.ImportButton3.Size = new System.Drawing.Size(285, 30);
            this.ImportButton3.TabIndex = 64;
            this.ImportButton3.Text = "章タイトル読込";
            this.ImportButton3.UseVisualStyleBackColor = true;
            this.ImportButton3.Click += new System.EventHandler(this.ImportButton3_Click);
            // 
            // X_TITLE_PIC
            // 
            this.X_TITLE_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_TITLE_PIC.Location = new System.Drawing.Point(281, 132);
            this.X_TITLE_PIC.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.X_TITLE_PIC.Name = "X_TITLE_PIC";
            this.X_TITLE_PIC.Size = new System.Drawing.Size(351, 60);
            this.X_TITLE_PIC.TabIndex = 90;
            this.X_TITLE_PIC.TabStop = false;
            // 
            // ImportButton2
            // 
            this.ImportButton2.Location = new System.Drawing.Point(281, 260);
            this.ImportButton2.Margin = new System.Windows.Forms.Padding(2);
            this.ImportButton2.Name = "ImportButton2";
            this.ImportButton2.Size = new System.Drawing.Size(288, 30);
            this.ImportButton2.TabIndex = 62;
            this.ImportButton2.Text = "章画像読込";
            this.ImportButton2.UseVisualStyleBackColor = true;
            this.ImportButton2.Click += new System.EventHandler(this.ImportButton2_Click);
            // 
            // ExportButton3
            // 
            this.ExportButton3.Location = new System.Drawing.Point(581, 212);
            this.ExportButton3.Margin = new System.Windows.Forms.Padding(2);
            this.ExportButton3.Name = "ExportButton3";
            this.ExportButton3.Size = new System.Drawing.Size(285, 30);
            this.ExportButton3.TabIndex = 63;
            this.ExportButton3.Text = "章タイトル取出し";
            this.ExportButton3.UseVisualStyleBackColor = true;
            this.ExportButton3.Click += new System.EventHandler(this.ExportButton3_Click);
            // 
            // ImportButton1
            // 
            this.ImportButton1.Location = new System.Drawing.Point(2, 260);
            this.ImportButton1.Margin = new System.Windows.Forms.Padding(2);
            this.ImportButton1.Name = "ImportButton1";
            this.ImportButton1.Size = new System.Drawing.Size(268, 30);
            this.ImportButton1.TabIndex = 60;
            this.ImportButton1.Text = "セーブ画像読込";
            this.ImportButton1.UseVisualStyleBackColor = true;
            this.ImportButton1.Click += new System.EventHandler(this.ImportButton1_Click);
            // 
            // X_CHAPTER_PIC
            // 
            this.X_CHAPTER_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_CHAPTER_PIC.Location = new System.Drawing.Point(282, 65);
            this.X_CHAPTER_PIC.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.X_CHAPTER_PIC.Name = "X_CHAPTER_PIC";
            this.X_CHAPTER_PIC.Size = new System.Drawing.Size(351, 59);
            this.X_CHAPTER_PIC.TabIndex = 89;
            this.X_CHAPTER_PIC.TabStop = false;
            // 
            // ExportButton2
            // 
            this.ExportButton2.Location = new System.Drawing.Point(281, 212);
            this.ExportButton2.Margin = new System.Windows.Forms.Padding(2);
            this.ExportButton2.Name = "ExportButton2";
            this.ExportButton2.Size = new System.Drawing.Size(288, 30);
            this.ExportButton2.TabIndex = 61;
            this.ExportButton2.Text = "章画像取出し";
            this.ExportButton2.UseVisualStyleBackColor = true;
            this.ExportButton2.Click += new System.EventHandler(this.ExportButton2_Click);
            // 
            // ExportButton1
            // 
            this.ExportButton1.Location = new System.Drawing.Point(2, 212);
            this.ExportButton1.Margin = new System.Windows.Forms.Padding(2);
            this.ExportButton1.Name = "ExportButton1";
            this.ExportButton1.Size = new System.Drawing.Size(268, 30);
            this.ExportButton1.TabIndex = 59;
            this.ExportButton1.Text = "セーブ画像取出し";
            this.ExportButton1.UseVisualStyleBackColor = true;
            this.ExportButton1.Click += new System.EventHandler(this.ExportButton1_Click);
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(746, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(168, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(89, 4);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 8;
            // 
            // AddressPanel
            // 
            this.AddressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressPanel.Controls.Add(this.BlockSize);
            this.AddressPanel.Controls.Add(this.label3);
            this.AddressPanel.Controls.Add(this.SelectAddress);
            this.AddressPanel.Controls.Add(this.label22);
            this.AddressPanel.Controls.Add(this.WriteButton);
            this.AddressPanel.Controls.Add(this.Address);
            this.AddressPanel.Controls.Add(this.label23);
            this.AddressPanel.Location = new System.Drawing.Point(285, 48);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(913, 30);
            this.AddressPanel.TabIndex = 65;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(532, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(150, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(409, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(20, 47);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(258, 468);
            this.panel6.TabIndex = 112;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(-1, 434);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(252, 30);
            this.AddressListExpandsButton.TabIndex = 114;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(259, 31);
            this.LabelFilter.TabIndex = 106;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 30);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(259, 398);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // EXPLAIN
            // 
            this.EXPLAIN.Location = new System.Drawing.Point(3, 324);
            this.EXPLAIN.Multiline = true;
            this.EXPLAIN.Name = "EXPLAIN";
            this.EXPLAIN.ReadOnly = true;
            this.EXPLAIN.Size = new System.Drawing.Size(861, 108);
            this.EXPLAIN.TabIndex = 91;
            // 
            // ImageChapterTitleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1208, 526);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.AddressPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "ImageChapterTitleForm";
            this.Text = "章タイトル";
            this.Load += new System.EventHandler(this.ImageChapterTitleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.P4)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_SAVE_PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_TITLE_PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_CHAPTER_PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown P4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.NumericUpDown P8;
        private System.Windows.Forms.Label J_8;
        private System.Windows.Forms.Label J_4;
        private System.Windows.Forms.Label J_0;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Button ExportButton1;
        private System.Windows.Forms.Panel AddressPanel;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button ImportButton3;
        private System.Windows.Forms.Button ExportButton3;
        private System.Windows.Forms.Button ImportButton2;
        private System.Windows.Forms.Button ExportButton2;
        private System.Windows.Forms.Button ImportButton1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private InterpolatedPictureBox X_SAVE_PIC;
        private InterpolatedPictureBox X_TITLE_PIC;
        private InterpolatedPictureBox X_CHAPTER_PIC;
        private System.Windows.Forms.TextBox EXPLAIN;
    }
}