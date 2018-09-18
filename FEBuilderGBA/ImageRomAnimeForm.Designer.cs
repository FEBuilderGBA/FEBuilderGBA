namespace FEBuilderGBA
{
    partial class ImageRomAnimeForm
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
            this.panel9 = new System.Windows.Forms.Panel();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AnimeExportButton = new System.Windows.Forms.Button();
            this.AnimeImportButton = new System.Windows.Forms.Button();
            this.X_JumpGraphicsTool = new System.Windows.Forms.Button();
            this.X_INFO = new FEBuilderGBA.TextBoxEx();
            this.label26 = new System.Windows.Forms.Label();
            this.ShowFrameUpDown = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.X_ANIME_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.panel9.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShowFrameUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_ANIME_PIC)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.SuspendLayout();
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.LabelFilter);
            this.panel9.Controls.Add(this.AddressList);
            this.panel9.Location = new System.Drawing.Point(8, 9);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(267, 719);
            this.panel9.TabIndex = 194;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(267, 26);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 25);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(267, 688);
            this.AddressList.TabIndex = 108;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.AnimeExportButton);
            this.panel2.Controls.Add(this.AnimeImportButton);
            this.panel2.Controls.Add(this.X_JumpGraphicsTool);
            this.panel2.Controls.Add(this.X_INFO);
            this.panel2.Controls.Add(this.label26);
            this.panel2.Controls.Add(this.ShowFrameUpDown);
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.X_ANIME_PIC);
            this.panel2.Location = new System.Drawing.Point(277, 40);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(884, 686);
            this.panel2.TabIndex = 193;
            // 
            // AnimeExportButton
            // 
            this.AnimeExportButton.Location = new System.Drawing.Point(194, 561);
            this.AnimeExportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.AnimeExportButton.Name = "AnimeExportButton";
            this.AnimeExportButton.Size = new System.Drawing.Size(160, 61);
            this.AnimeExportButton.TabIndex = 187;
            this.AnimeExportButton.Text = "アニメの書出し";
            this.AnimeExportButton.UseVisualStyleBackColor = true;
            this.AnimeExportButton.Click += new System.EventHandler(this.AnimeExportButton_Click);
            // 
            // AnimeImportButton
            // 
            this.AnimeImportButton.Location = new System.Drawing.Point(27, 561);
            this.AnimeImportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.AnimeImportButton.Name = "AnimeImportButton";
            this.AnimeImportButton.Size = new System.Drawing.Size(159, 61);
            this.AnimeImportButton.TabIndex = 186;
            this.AnimeImportButton.Text = "アニメの読込";
            this.AnimeImportButton.UseVisualStyleBackColor = true;
            this.AnimeImportButton.Click += new System.EventHandler(this.AnimeImportButton_Click);
            // 
            // X_JumpGraphicsTool
            // 
            this.X_JumpGraphicsTool.Location = new System.Drawing.Point(27, 627);
            this.X_JumpGraphicsTool.Name = "X_JumpGraphicsTool";
            this.X_JumpGraphicsTool.Size = new System.Drawing.Size(327, 54);
            this.X_JumpGraphicsTool.TabIndex = 185;
            this.X_JumpGraphicsTool.Text = "グラフィックツール";
            this.X_JumpGraphicsTool.UseVisualStyleBackColor = true;
            this.X_JumpGraphicsTool.Click += new System.EventHandler(this.X_JumpGraphicsTool_Click);
            // 
            // X_INFO
            // 
            this.X_INFO.ErrorMessage = "";
            this.X_INFO.Location = new System.Drawing.Point(9, 3);
            this.X_INFO.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.X_INFO.Multiline = true;
            this.X_INFO.Name = "X_INFO";
            this.X_INFO.Placeholder = "";
            this.X_INFO.ReadOnly = true;
            this.X_INFO.Size = new System.Drawing.Size(867, 326);
            this.X_INFO.TabIndex = 178;
            // 
            // label26
            // 
            this.label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label26.Location = new System.Drawing.Point(194, 369);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(92, 30);
            this.label26.TabIndex = 177;
            this.label26.Text = "フレーム";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShowFrameUpDown
            // 
            this.ShowFrameUpDown.Hexadecimal = true;
            this.ShowFrameUpDown.Location = new System.Drawing.Point(290, 372);
            this.ShowFrameUpDown.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ShowFrameUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ShowFrameUpDown.Name = "ShowFrameUpDown";
            this.ShowFrameUpDown.Size = new System.Drawing.Size(64, 25);
            this.ShowFrameUpDown.TabIndex = 176;
            this.ShowFrameUpDown.ValueChanged += new System.EventHandler(this.ShowFrameUpDown_ValueChanged);
            // 
            // label24
            // 
            this.label24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label24.Location = new System.Drawing.Point(9, 340);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(344, 30);
            this.label24.TabIndex = 172;
            this.label24.Text = "表示例";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_ANIME_PIC
            // 
            this.X_ANIME_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_ANIME_PIC.Location = new System.Drawing.Point(362, 336);
            this.X_ANIME_PIC.Margin = new System.Windows.Forms.Padding(5);
            this.X_ANIME_PIC.Name = "X_ANIME_PIC";
            this.X_ANIME_PIC.Size = new System.Drawing.Size(514, 349);
            this.X_ANIME_PIC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.X_ANIME_PIC.TabIndex = 171;
            this.X_ANIME_PIC.TabStop = false;
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
            this.AddressPanel.Location = new System.Drawing.Point(277, 9);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(884, 30);
            this.AddressPanel.TabIndex = 191;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(317, 2);
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
            this.label3.Location = new System.Drawing.Point(233, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(534, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(407, 1);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(709, -1);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(94, 4);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 8;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(1, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 30);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImageRomAnimeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1177, 735);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.AddressPanel);
            this.Name = "ImageRomAnimeForm";
            this.Text = "ROM内の魔法アニメ";
            this.Load += new System.EventHandler(this.ImageRomAnimeForm_Load);
            this.panel9.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShowFrameUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_ANIME_PIC)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel AddressPanel;
        private TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label23;
        private InterpolatedPictureBox X_ANIME_PIC;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.NumericUpDown ShowFrameUpDown;
        private TextBoxEx X_INFO;
        private System.Windows.Forms.Button X_JumpGraphicsTool;
        private System.Windows.Forms.Button AnimeExportButton;
        private System.Windows.Forms.Button AnimeImportButton;
    }
}