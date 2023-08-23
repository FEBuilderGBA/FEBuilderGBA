namespace FEBuilderGBA
{
    partial class StatusParamForm
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
            this.L_12_STATUSPARAM = new FEBuilderGBA.TextBoxEx();
            this.L_8_COMBO = new System.Windows.Forms.ComboBox();
            this.J_9 = new System.Windows.Forms.Label();
            this.J_8 = new System.Windows.Forms.Label();
            this.P12 = new System.Windows.Forms.NumericUpDown();
            this.J_12_HEX = new System.Windows.Forms.Label();
            this.B11 = new System.Windows.Forms.NumericUpDown();
            this.B10 = new System.Windows.Forms.NumericUpDown();
            this.J_10 = new System.Windows.Forms.Label();
            this.B9 = new System.Windows.Forms.NumericUpDown();
            this.B8 = new System.Windows.Forms.NumericUpDown();
            this.P4 = new System.Windows.Forms.NumericUpDown();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.J_4 = new System.Windows.Forms.Label();
            this.J_0 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.P12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
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
            this.panel4.Controls.Add(this.L_12_STATUSPARAM);
            this.panel4.Controls.Add(this.L_8_COMBO);
            this.panel4.Controls.Add(this.J_9);
            this.panel4.Controls.Add(this.J_8);
            this.panel4.Controls.Add(this.P12);
            this.panel4.Controls.Add(this.J_12_HEX);
            this.panel4.Controls.Add(this.B11);
            this.panel4.Controls.Add(this.B10);
            this.panel4.Controls.Add(this.J_10);
            this.panel4.Controls.Add(this.B9);
            this.panel4.Controls.Add(this.B8);
            this.panel4.Controls.Add(this.P4);
            this.panel4.Controls.Add(this.P0);
            this.panel4.Controls.Add(this.J_4);
            this.panel4.Controls.Add(this.J_0);
            this.panel4.Location = new System.Drawing.Point(295, 76);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(916, 488);
            this.panel4.TabIndex = 149;
            // 
            // L_12_STATUSPARAM
            // 
            this.L_12_STATUSPARAM.ErrorMessage = "";
            this.L_12_STATUSPARAM.Location = new System.Drawing.Point(555, 148);
            this.L_12_STATUSPARAM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_12_STATUSPARAM.Name = "L_12_STATUSPARAM";
            this.L_12_STATUSPARAM.Placeholder = "";
            this.L_12_STATUSPARAM.ReadOnly = true;
            this.L_12_STATUSPARAM.Size = new System.Drawing.Size(342, 25);
            this.L_12_STATUSPARAM.TabIndex = 118;
            // 
            // L_8_COMBO
            // 
            this.L_8_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_8_COMBO.FormattingEnabled = true;
            this.L_8_COMBO.Items.AddRange(new object[] {
            "00=普通",
            "01=灰色",
            "02=青",
            "03=黄色",
            "04=緑"});
            this.L_8_COMBO.Location = new System.Drawing.Point(497, 66);
            this.L_8_COMBO.Name = "L_8_COMBO";
            this.L_8_COMBO.Size = new System.Drawing.Size(243, 26);
            this.L_8_COMBO.TabIndex = 117;
            // 
            // J_9
            // 
            this.J_9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_9.Location = new System.Drawing.Point(-1, 89);
            this.J_9.Name = "J_9";
            this.J_9.Size = new System.Drawing.Size(409, 30);
            this.J_9.TabIndex = 116;
            this.J_9.Text = "字下げ";
            this.J_9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_8
            // 
            this.J_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8.Location = new System.Drawing.Point(-1, 60);
            this.J_8.Name = "J_8";
            this.J_8.Size = new System.Drawing.Size(409, 30);
            this.J_8.TabIndex = 114;
            this.J_8.Text = "色";
            this.J_8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P12
            // 
            this.P12.Hexadecimal = true;
            this.P12.Location = new System.Drawing.Point(414, 148);
            this.P12.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P12.Name = "P12";
            this.P12.Size = new System.Drawing.Size(130, 25);
            this.P12.TabIndex = 91;
            // 
            // J_12_HEX
            // 
            this.J_12_HEX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_12_HEX.Location = new System.Drawing.Point(-1, 147);
            this.J_12_HEX.Name = "J_12_HEX";
            this.J_12_HEX.Size = new System.Drawing.Size(409, 30);
            this.J_12_HEX.TabIndex = 90;
            this.J_12_HEX.Text = "文字列ポインタ";
            this.J_12_HEX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B11
            // 
            this.B11.Hexadecimal = true;
            this.B11.Location = new System.Drawing.Point(482, 120);
            this.B11.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B11.Name = "B11";
            this.B11.Size = new System.Drawing.Size(62, 25);
            this.B11.TabIndex = 89;
            // 
            // B10
            // 
            this.B10.Hexadecimal = true;
            this.B10.Location = new System.Drawing.Point(414, 120);
            this.B10.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B10.Name = "B10";
            this.B10.Size = new System.Drawing.Size(62, 25);
            this.B10.TabIndex = 88;
            // 
            // J_10
            // 
            this.J_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_10.Location = new System.Drawing.Point(-1, 118);
            this.J_10.Name = "J_10";
            this.J_10.Size = new System.Drawing.Size(409, 30);
            this.J_10.TabIndex = 87;
            this.J_10.Text = "00";
            this.J_10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B9
            // 
            this.B9.Hexadecimal = true;
            this.B9.Location = new System.Drawing.Point(414, 91);
            this.B9.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B9.Name = "B9";
            this.B9.Size = new System.Drawing.Size(62, 25);
            this.B9.TabIndex = 86;
            // 
            // B8
            // 
            this.B8.Hexadecimal = true;
            this.B8.Location = new System.Drawing.Point(414, 62);
            this.B8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B8.Name = "B8";
            this.B8.Size = new System.Drawing.Size(62, 25);
            this.B8.TabIndex = 85;
            // 
            // P4
            // 
            this.P4.Hexadecimal = true;
            this.P4.Location = new System.Drawing.Point(414, 33);
            this.P4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P4.Name = "P4";
            this.P4.Size = new System.Drawing.Size(130, 25);
            this.P4.TabIndex = 81;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(414, 4);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 80;
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(-1, 30);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(409, 30);
            this.J_4.TabIndex = 79;
            this.J_4.Text = "?? Bitmap";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(-1, 0);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(409, 30);
            this.J_0.TabIndex = 78;
            this.J_0.Text = "StatusMenuTextStruct";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-1, -2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 31);
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
            this.label16.Size = new System.Drawing.Size(85, 32);
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
            this.AddressList.Size = new System.Drawing.Size(276, 454);
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
            // StatusParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1222, 566);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Name = "StatusParamForm";
            this.Text = "ステータスパラメータ";
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
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
        private System.Windows.Forms.Label J_0;
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
        private System.Windows.Forms.NumericUpDown P4;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.Label J_4;
        private System.Windows.Forms.NumericUpDown B11;
        private System.Windows.Forms.NumericUpDown B10;
        private System.Windows.Forms.Label J_10;
        private System.Windows.Forms.NumericUpDown B9;
        private System.Windows.Forms.NumericUpDown B8;
        private System.Windows.Forms.NumericUpDown P12;
        private System.Windows.Forms.Label J_12_HEX;
        private System.Windows.Forms.NumericUpDown Address;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label11;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.Label J_8;
        private System.Windows.Forms.Label J_9;
        private System.Windows.Forms.ComboBox L_8_COMBO;
        private FEBuilderGBA.TextBoxEx L_12_STATUSPARAM;
    }
}