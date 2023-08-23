namespace FEBuilderGBA
{
    partial class EventForceSortieForm
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
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.L_0_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.L_2_COMBO = new System.Windows.Forms.ComboBox();
            this.L_3_MAP_ANYFF = new FEBuilderGBA.TextBoxEx();
            this.L_0_UNIT = new FEBuilderGBA.TextBoxEx();
            this.B3 = new System.Windows.Forms.NumericUpDown();
            this.J_2 = new System.Windows.Forms.Label();
            this.J_3_MAP = new System.Windows.Forms.Label();
            this.J_0_UNIT = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(322, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -2);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.AddressPanel.Controls.Add(this.label55);
            this.AddressPanel.Location = new System.Drawing.Point(324, 35);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(835, 30);
            this.AddressPanel.TabIndex = 77;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(240, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(539, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(150, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(414, 0);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(689, 0);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(145, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(91, 1);
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
            // label55
            // 
            this.label55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label55.Location = new System.Drawing.Point(-2, -1);
            this.label55.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(85, 30);
            this.label55.TabIndex = 1;
            this.label55.Text = "アドレス";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B2
            // 
            this.B2.Hexadecimal = true;
            this.B2.Location = new System.Drawing.Point(161, 42);
            this.B2.Margin = new System.Windows.Forms.Padding(2);
            this.B2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(68, 25);
            this.B2.TabIndex = 175;
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(161, 6);
            this.W0.Margin = new System.Windows.Forms.Padding(2);
            this.W0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(68, 25);
            this.W0.TabIndex = 174;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(552, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 70;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.L_0_UNITICON);
            this.panel2.Controls.Add(this.L_2_COMBO);
            this.panel2.Controls.Add(this.L_3_MAP_ANYFF);
            this.panel2.Controls.Add(this.B2);
            this.panel2.Controls.Add(this.W0);
            this.panel2.Controls.Add(this.L_0_UNIT);
            this.panel2.Controls.Add(this.B3);
            this.panel2.Controls.Add(this.J_2);
            this.panel2.Controls.Add(this.J_3_MAP);
            this.panel2.Controls.Add(this.J_0_UNIT);
            this.panel2.Location = new System.Drawing.Point(322, 64);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(836, 666);
            this.panel2.TabIndex = 78;
            // 
            // L_0_UNITICON
            // 
            this.L_0_UNITICON.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.L_0_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_UNITICON.Location = new System.Drawing.Point(441, 4);
            this.L_0_UNITICON.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_UNITICON.Name = "L_0_UNITICON";
            this.L_0_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_UNITICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_0_UNITICON.TabIndex = 191;
            this.L_0_UNITICON.TabStop = false;
            // 
            // L_2_COMBO
            // 
            this.L_2_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_2_COMBO.FormattingEnabled = true;
            this.L_2_COMBO.Items.AddRange(new object[] {
            "01=序盤",
            "02=エイリーク編",
            "03=エフラム編",
            "FF=無条件"});
            this.L_2_COMBO.Location = new System.Drawing.Point(244, 44);
            this.L_2_COMBO.Margin = new System.Windows.Forms.Padding(2);
            this.L_2_COMBO.Name = "L_2_COMBO";
            this.L_2_COMBO.Size = new System.Drawing.Size(293, 26);
            this.L_2_COMBO.TabIndex = 177;
            // 
            // L_3_MAP_ANYFF
            // 
            this.L_3_MAP_ANYFF.ErrorMessage = "";
            this.L_3_MAP_ANYFF.Location = new System.Drawing.Point(244, 83);
            this.L_3_MAP_ANYFF.Margin = new System.Windows.Forms.Padding(2);
            this.L_3_MAP_ANYFF.Name = "L_3_MAP_ANYFF";
            this.L_3_MAP_ANYFF.Placeholder = "";
            this.L_3_MAP_ANYFF.ReadOnly = true;
            this.L_3_MAP_ANYFF.Size = new System.Drawing.Size(182, 25);
            this.L_3_MAP_ANYFF.TabIndex = 176;
            // 
            // L_0_UNIT
            // 
            this.L_0_UNIT.ErrorMessage = "";
            this.L_0_UNIT.Location = new System.Drawing.Point(244, 6);
            this.L_0_UNIT.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_UNIT.Name = "L_0_UNIT";
            this.L_0_UNIT.Placeholder = "";
            this.L_0_UNIT.ReadOnly = true;
            this.L_0_UNIT.Size = new System.Drawing.Size(182, 25);
            this.L_0_UNIT.TabIndex = 172;
            // 
            // B3
            // 
            this.B3.Hexadecimal = true;
            this.B3.Location = new System.Drawing.Point(161, 78);
            this.B3.Margin = new System.Windows.Forms.Padding(2);
            this.B3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(68, 25);
            this.B3.TabIndex = 160;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(4, 41);
            this.J_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(153, 32);
            this.J_2.TabIndex = 5;
            this.J_2.Text = "編";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_3_MAP
            // 
            this.J_3_MAP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_3_MAP.Location = new System.Drawing.Point(4, 78);
            this.J_3_MAP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_3_MAP.Name = "J_3_MAP";
            this.J_3_MAP.Size = new System.Drawing.Size(153, 32);
            this.J_3_MAP.TabIndex = 3;
            this.J_3_MAP.Text = "章ID";
            this.J_3_MAP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_0_UNIT
            // 
            this.J_0_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_UNIT.Location = new System.Drawing.Point(4, 6);
            this.J_0_UNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_UNIT.Name = "J_0_UNIT";
            this.J_0_UNIT.Size = new System.Drawing.Size(153, 32);
            this.J_0_UNIT.TabIndex = 2;
            this.J_0_UNIT.Text = "ユニット";
            this.J_0_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(406, 1);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(178, 1);
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
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(9, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1149, 30);
            this.panel1.TabIndex = 76;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(315, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(8, 32);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(311, 697);
            this.panel6.TabIndex = 144;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(5, 667);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(304, 26);
            this.AddressListExpandsButton.TabIndex = 112;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(0, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(310, 26);
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
            this.AddressList.Location = new System.Drawing.Point(0, 24);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(310, 634);
            this.AddressList.TabIndex = 0;
            // 
            // EventForceSortieForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1169, 740);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.AddressPanel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "EventForceSortieForm";
            this.Text = "強制出撃";
            this.Load += new System.EventHandler(this.EventForceSortieForm_Load);
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.NumericUpDown W0;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Panel panel2;
        private FEBuilderGBA.TextBoxEx L_0_UNIT;
        private System.Windows.Forms.NumericUpDown B3;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.Label J_3_MAP;
        private System.Windows.Forms.Label J_0_UNIT;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx L_3_MAP_ANYFF;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.ComboBox L_2_COMBO;
        private InterpolatedPictureBox L_0_UNITICON;
    }
}