namespace FEBuilderGBA
{
    partial class MapTerrainNameEngForm
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.L_0_TEXT_TERRAINNAME1 = new FEBuilderGBA.TextBoxEx();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.J_0_TEXT = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.TextWriteButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.AddressPointer = new FEBuilderGBA.TextBoxEx();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.panel6 = new System.Windows.Forms.Panel();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.L_0_TEXT_TERRAINNAME1);
            this.panel4.Controls.Add(this.W0);
            this.panel4.Controls.Add(this.J_0_TEXT);
            this.panel4.Location = new System.Drawing.Point(286, 79);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(933, 278);
            this.panel4.TabIndex = 80;
            // 
            // L_0_TEXT_TERRAINNAME1
            // 
            this.L_0_TEXT_TERRAINNAME1.ErrorMessage = "";
            this.L_0_TEXT_TERRAINNAME1.Location = new System.Drawing.Point(235, 5);
            this.L_0_TEXT_TERRAINNAME1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_0_TEXT_TERRAINNAME1.Name = "L_0_TEXT_TERRAINNAME1";
            this.L_0_TEXT_TERRAINNAME1.Placeholder = "";
            this.L_0_TEXT_TERRAINNAME1.ReadOnly = true;
            this.L_0_TEXT_TERRAINNAME1.Size = new System.Drawing.Size(169, 25);
            this.L_0_TEXT_TERRAINNAME1.TabIndex = 80;
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(147, 4);
            this.W0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(80, 25);
            this.W0.TabIndex = 79;
            // 
            // J_0_TEXT
            // 
            this.J_0_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_TEXT.Location = new System.Drawing.Point(-1, 0);
            this.J_0_TEXT.Name = "J_0_TEXT";
            this.J_0_TEXT.Size = new System.Drawing.Size(142, 30);
            this.J_0_TEXT.TabIndex = 78;
            this.J_0_TEXT.Text = "名前";
            this.J_0_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(303, -2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 30);
            this.label14.TabIndex = 52;
            this.label14.Text = "Size:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextWriteButton
            // 
            this.TextWriteButton.Location = new System.Drawing.Point(765, -2);
            this.TextWriteButton.Name = "TextWriteButton";
            this.TextWriteButton.Size = new System.Drawing.Size(167, 30);
            this.TextWriteButton.TabIndex = 9;
            this.TextWriteButton.Text = "書き込み";
            this.TextWriteButton.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(20, 18);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1199, 30);
            this.panel3.TabIndex = 79;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(460, -1);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-1, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(265, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 30);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(356, 3);
            this.ReadCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(129, 4);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(-1, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "アドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.AddressPointer);
            this.panel5.Controls.Add(this.BlockSize);
            this.panel5.Controls.Add(this.TextWriteButton);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(286, 49);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(934, 30);
            this.panel5.TabIndex = 78;
            // 
            // AddressPointer
            // 
            this.AddressPointer.ErrorMessage = "";
            this.AddressPointer.Location = new System.Drawing.Point(92, 3);
            this.AddressPointer.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.AddressPointer.Name = "AddressPointer";
            this.AddressPointer.Placeholder = "";
            this.AddressPointer.ReadOnly = true;
            this.AddressPointer.Size = new System.Drawing.Size(169, 25);
            this.AddressPointer.TabIndex = 54;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(386, -2);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 53;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(20, 53);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(258, 304);
            this.panel6.TabIndex = 146;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-3, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(260, 26);
            this.LabelFilter.TabIndex = 106;
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
            this.AddressList.Size = new System.Drawing.Size(258, 274);
            this.AddressList.TabIndex = 0;
            // 
            // MapTerrainNameEngForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1237, 369);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "MapTerrainNameEngForm";
            this.Text = "地形名(英語)";
            this.Load += new System.EventHandler(this.MapTerrainNameEngForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button TextWriteButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label J_0_TEXT;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private FEBuilderGBA.TextBoxEx AddressPointer;
        private FEBuilderGBA.TextBoxEx L_0_TEXT_TERRAINNAME1;
        private System.Windows.Forms.NumericUpDown W0;
    }
}