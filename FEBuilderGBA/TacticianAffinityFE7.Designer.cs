namespace FEBuilderGBA
{
    partial class TacticianAffinityFE7
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
            this.Explain = new FEBuilderGBA.TextBoxEx();
            this.L_0_ATTRIBUTEICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.D0 = new System.Windows.Forms.NumericUpDown();
            this.L_0_ATTRIBUTE = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.L_4_ID_PLUS1 = new FEBuilderGBA.PanelEx();
            this.B4 = new System.Windows.Forms.NumericUpDown();
            this.L_5_CLASS = new FEBuilderGBA.TextBoxEx();
            this.L_0_TEXT_NAME1 = new FEBuilderGBA.TextBoxEx();
            this.L_2_TEXT_DETAIL3 = new FEBuilderGBA.TextBoxEx();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_ATTRIBUTEICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D0)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.L_4_ID_PLUS1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.Explain);
            this.panel4.Controls.Add(this.L_0_ATTRIBUTEICON);
            this.panel4.Controls.Add(this.D0);
            this.panel4.Controls.Add(this.L_0_ATTRIBUTE);
            this.panel4.Location = new System.Drawing.Point(257, 74);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(843, 610);
            this.panel4.TabIndex = 92;
            // 
            // Explain
            // 
            this.Explain.ErrorMessage = "";
            this.Explain.Location = new System.Drawing.Point(10, 78);
            this.Explain.Multiline = true;
            this.Explain.Name = "Explain";
            this.Explain.Placeholder = "";
            this.Explain.ReadOnly = true;
            this.Explain.Size = new System.Drawing.Size(800, 527);
            this.Explain.TabIndex = 111;
            // 
            // L_0_ATTRIBUTEICON
            // 
            this.L_0_ATTRIBUTEICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_ATTRIBUTEICON.Location = new System.Drawing.Point(297, 11);
            this.L_0_ATTRIBUTEICON.Name = "L_0_ATTRIBUTEICON";
            this.L_0_ATTRIBUTEICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_ATTRIBUTEICON.TabIndex = 110;
            this.L_0_ATTRIBUTEICON.TabStop = false;
            // 
            // D0
            // 
            this.D0.Hexadecimal = true;
            this.D0.Location = new System.Drawing.Point(231, 13);
            this.D0.Margin = new System.Windows.Forms.Padding(2);
            this.D0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.D0.Name = "D0";
            this.D0.Size = new System.Drawing.Size(61, 25);
            this.D0.TabIndex = 108;
            this.D0.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // L_0_ATTRIBUTE
            // 
            this.L_0_ATTRIBUTE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.L_0_ATTRIBUTE.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.L_0_ATTRIBUTE.Location = new System.Drawing.Point(10, 13);
            this.L_0_ATTRIBUTE.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.L_0_ATTRIBUTE.Name = "L_0_ATTRIBUTE";
            this.L_0_ATTRIBUTE.Size = new System.Drawing.Size(214, 26);
            this.L_0_ATTRIBUTE.TabIndex = 109;
            this.L_0_ATTRIBUTE.Text = "属性:-";
            this.L_0_ATTRIBUTE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(7, 42);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(245, 641);
            this.panel6.TabIndex = 93;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(2, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(244, 26);
            this.LabelFilter.TabIndex = 55;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 24);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(246, 616);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(7, 7);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1093, 34);
            this.panel3.TabIndex = 91;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(480, -3);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 3;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-2, -2);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 36);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(249, -1);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 35);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(339, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 1;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(114, 4);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.BlockSize);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.SelectAddress);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.WriteButton);
            this.panel5.Controls.Add(this.Address);
            this.panel5.Controls.Add(this.label23);
            this.panel5.Location = new System.Drawing.Point(257, 41);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(843, 34);
            this.panel5.TabIndex = 90;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(314, 4);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(87, 25);
            this.BlockSize.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(231, -1);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 34);
            this.label3.TabIndex = 59;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(532, 2);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(133, 25);
            this.SelectAddress.TabIndex = 57;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(408, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 34);
            this.label22.TabIndex = 56;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(672, 0);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(168, 30);
            this.WriteButton.TabIndex = 55;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(90, 4);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 0;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, -1);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(86, 34);
            this.label23.TabIndex = 53;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_4_ID_PLUS1
            // 
            this.L_4_ID_PLUS1.Controls.Add(this.B4);
            this.L_4_ID_PLUS1.ErrorMessage = "";
            this.L_4_ID_PLUS1.Location = new System.Drawing.Point(389, 1);
            this.L_4_ID_PLUS1.Name = "L_4_ID_PLUS1";
            this.L_4_ID_PLUS1.Size = new System.Drawing.Size(84, 32);
            this.L_4_ID_PLUS1.TabIndex = 105;
            // 
            // B4
            // 
            this.B4.Hexadecimal = true;
            this.B4.Location = new System.Drawing.Point(4, 4);
            this.B4.Margin = new System.Windows.Forms.Padding(2);
            this.B4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B4.Name = "B4";
            this.B4.Size = new System.Drawing.Size(76, 25);
            this.B4.TabIndex = 0;
            // 
            // L_5_CLASS
            // 
            this.L_5_CLASS.ErrorMessage = "";
            this.L_5_CLASS.Location = new System.Drawing.Point(723, 6);
            this.L_5_CLASS.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_5_CLASS.Name = "L_5_CLASS";
            this.L_5_CLASS.Placeholder = "";
            this.L_5_CLASS.ReadOnly = true;
            this.L_5_CLASS.Size = new System.Drawing.Size(117, 25);
            this.L_5_CLASS.TabIndex = 23;
            // 
            // L_0_TEXT_NAME1
            // 
            this.L_0_TEXT_NAME1.ErrorMessage = "";
            this.L_0_TEXT_NAME1.Location = new System.Drawing.Point(178, 7);
            this.L_0_TEXT_NAME1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_TEXT_NAME1.Name = "L_0_TEXT_NAME1";
            this.L_0_TEXT_NAME1.Placeholder = "";
            this.L_0_TEXT_NAME1.ReadOnly = true;
            this.L_0_TEXT_NAME1.Size = new System.Drawing.Size(162, 25);
            this.L_0_TEXT_NAME1.TabIndex = 14;
            // 
            // L_2_TEXT_DETAIL3
            // 
            this.L_2_TEXT_DETAIL3.ErrorMessage = "";
            this.L_2_TEXT_DETAIL3.Location = new System.Drawing.Point(178, 34);
            this.L_2_TEXT_DETAIL3.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_2_TEXT_DETAIL3.Multiline = true;
            this.L_2_TEXT_DETAIL3.Name = "L_2_TEXT_DETAIL3";
            this.L_2_TEXT_DETAIL3.Placeholder = "";
            this.L_2_TEXT_DETAIL3.ReadOnly = true;
            this.L_2_TEXT_DETAIL3.Size = new System.Drawing.Size(297, 65);
            this.L_2_TEXT_DETAIL3.TabIndex = 15;
            // 
            // TacticianAffinityFE7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1105, 697);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Name = "TacticianAffinityFE7";
            this.Text = "軍師属性";
            this.Load += new System.EventHandler(this.TacticianAffinityFE7_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_ATTRIBUTEICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D0)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.L_4_ID_PLUS1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.B4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel5;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label23;
        private PanelEx L_4_ID_PLUS1;
        private System.Windows.Forms.NumericUpDown B4;
        private TextBoxEx L_5_CLASS;
        private TextBoxEx L_0_TEXT_NAME1;
        private TextBoxEx L_2_TEXT_DETAIL3;
        private TextBoxEx Explain;
        private InterpolatedPictureBox L_0_ATTRIBUTEICON;
        private System.Windows.Forms.NumericUpDown D0;
        private System.Windows.Forms.Label L_0_ATTRIBUTE;

    }
}