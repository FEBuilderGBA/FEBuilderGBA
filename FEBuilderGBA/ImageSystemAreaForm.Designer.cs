namespace FEBuilderGBA
{
    partial class ImageSystemAreaForm
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
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.L_0_PALETTE_COLOR = new System.Windows.Forms.Label();
            this.L_0_PALETTE_B = new System.Windows.Forms.NumericUpDown();
            this.L_0_PALETTE_G = new System.Windows.Forms.NumericUpDown();
            this.L_0_PALETTE_R = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label11 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_PALETTE_B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_PALETTE_G)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_PALETTE_R)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(909, -2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.W0);
            this.panel4.Controls.Add(this.label50);
            this.panel4.Controls.Add(this.label49);
            this.panel4.Controls.Add(this.label48);
            this.panel4.Controls.Add(this.L_0_PALETTE_COLOR);
            this.panel4.Controls.Add(this.L_0_PALETTE_B);
            this.panel4.Controls.Add(this.L_0_PALETTE_G);
            this.panel4.Controls.Add(this.L_0_PALETTE_R);
            this.panel4.Location = new System.Drawing.Point(295, 76);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(916, 488);
            this.panel4.TabIndex = 149;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 18);
            this.label1.TabIndex = 266;
            this.label1.Text = "GBAカラー";
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(47, 44);
            this.W0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(91, 25);
            this.W0.TabIndex = 265;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(15, 214);
            this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(19, 18);
            this.label50.TabIndex = 264;
            this.label50.Text = "B";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(15, 182);
            this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(20, 18);
            this.label49.TabIndex = 263;
            this.label49.Text = "G";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(15, 152);
            this.label48.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(19, 18);
            this.label48.TabIndex = 262;
            this.label48.Text = "R";
            // 
            // L_0_PALETTE_COLOR
            // 
            this.L_0_PALETTE_COLOR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.L_0_PALETTE_COLOR.Location = new System.Drawing.Point(69, 85);
            this.L_0_PALETTE_COLOR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.L_0_PALETTE_COLOR.Name = "L_0_PALETTE_COLOR";
            this.L_0_PALETTE_COLOR.Size = new System.Drawing.Size(69, 58);
            this.L_0_PALETTE_COLOR.TabIndex = 261;
            this.L_0_PALETTE_COLOR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_0_PALETTE_B
            // 
            this.L_0_PALETTE_B.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.L_0_PALETTE_B.Location = new System.Drawing.Point(47, 208);
            this.L_0_PALETTE_B.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_PALETTE_B.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.L_0_PALETTE_B.Name = "L_0_PALETTE_B";
            this.L_0_PALETTE_B.Size = new System.Drawing.Size(91, 25);
            this.L_0_PALETTE_B.TabIndex = 259;
            // 
            // L_0_PALETTE_G
            // 
            this.L_0_PALETTE_G.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.L_0_PALETTE_G.Location = new System.Drawing.Point(47, 176);
            this.L_0_PALETTE_G.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_PALETTE_G.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.L_0_PALETTE_G.Name = "L_0_PALETTE_G";
            this.L_0_PALETTE_G.Size = new System.Drawing.Size(91, 25);
            this.L_0_PALETTE_G.TabIndex = 258;
            // 
            // L_0_PALETTE_R
            // 
            this.L_0_PALETTE_R.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.L_0_PALETTE_R.Location = new System.Drawing.Point(47, 146);
            this.L_0_PALETTE_R.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_PALETTE_R.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.L_0_PALETTE_R.Name = "L_0_PALETTE_R";
            this.L_0_PALETTE_R.Size = new System.Drawing.Size(91, 25);
            this.L_0_PALETTE_R.TabIndex = 257;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-1, -2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.BlockSize);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.SelectAddress);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.WriteButton);
            this.panel5.Controls.Add(this.Address);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(295, 45);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(917, 30);
            this.panel5.TabIndex = 147;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(326, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 63;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(240, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 30);
            this.label11.TabIndex = 64;
            this.label11.Text = "Size:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(540, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 62;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(414, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 61;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(749, -1);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 60;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(90, 4);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 55;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(-1, -2);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "アドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.label30);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(12, 45);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(277, 519);
            this.panel6.TabIndex = 150;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(-2, 489);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(274, 30);
            this.AddressListExpandsButton.TabIndex = 115;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Location = new System.Drawing.Point(0, -1);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(276, 26);
            this.label30.TabIndex = 106;
            this.label30.Text = "名前";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 25);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(277, 454);
            this.AddressList.TabIndex = 0;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(372, 4);
            this.ReadCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.FilterComboBox);
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1200, 30);
            this.panel3.TabIndex = 148;
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Items.AddRange(new object[] {
            "移動範囲",
            "攻撃範囲",
            "杖範囲"});
            this.FilterComboBox.Location = new System.Drawing.Point(456, 2);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(447, 26);
            this.FilterComboBox.TabIndex = 30;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(282, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 30);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(146, 3);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // ImageSystemAreaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1222, 566);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Name = "ImageSystemAreaForm";
            this.Text = "移動範囲";
            this.Load += new System.EventHandler(this.ImageSystemAreaForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_PALETTE_B)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_PALETTE_G)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_PALETTE_R)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label30;
        private ListBoxEx AddressList;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.NumericUpDown Address;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label11;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown W0;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label L_0_PALETTE_COLOR;
        private System.Windows.Forms.NumericUpDown L_0_PALETTE_B;
        private System.Windows.Forms.NumericUpDown L_0_PALETTE_G;
        private System.Windows.Forms.NumericUpDown L_0_PALETTE_R;
    }
}