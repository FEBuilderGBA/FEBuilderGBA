namespace FEBuilderGBA
{
    partial class EDSensekiCommentForm
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
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.L_0_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.L_12_TEXT_CONVERSATION = new FEBuilderGBA.TextBoxEx();
            this.D12 = new System.Windows.Forms.NumericUpDown();
            this.J_12_TEXT = new System.Windows.Forms.Label();
            this.L_8_TEXT_CONVERSATION = new FEBuilderGBA.TextBoxEx();
            this.D8 = new System.Windows.Forms.NumericUpDown();
            this.J_8_TEXT = new System.Windows.Forms.Label();
            this.L_4_TEXT_CONVERSATION = new FEBuilderGBA.TextBoxEx();
            this.D4 = new System.Windows.Forms.NumericUpDown();
            this.J_4_TEXT = new System.Windows.Forms.Label();
            this.D0 = new System.Windows.Forms.NumericUpDown();
            this.L_0_UNIT = new FEBuilderGBA.TextBoxEx();
            this.J_0_UNIT = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.label55 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.AddressPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(323, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(371, 1);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(14, 42);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(269, 393);
            this.panel6.TabIndex = 148;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(3, 366);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(222, 23);
            this.AddressListExpandsButton.TabIndex = 112;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(0, 0);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(267, 26);
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
            this.AddressList.Location = new System.Drawing.Point(-1, 26);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(268, 328);
            this.AddressList.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.L_0_UNITICON);
            this.panel2.Controls.Add(this.L_12_TEXT_CONVERSATION);
            this.panel2.Controls.Add(this.D12);
            this.panel2.Controls.Add(this.J_12_TEXT);
            this.panel2.Controls.Add(this.L_8_TEXT_CONVERSATION);
            this.panel2.Controls.Add(this.D8);
            this.panel2.Controls.Add(this.J_8_TEXT);
            this.panel2.Controls.Add(this.L_4_TEXT_CONVERSATION);
            this.panel2.Controls.Add(this.D4);
            this.panel2.Controls.Add(this.J_4_TEXT);
            this.panel2.Controls.Add(this.D0);
            this.panel2.Controls.Add(this.L_0_UNIT);
            this.panel2.Controls.Add(this.J_0_UNIT);
            this.panel2.Location = new System.Drawing.Point(289, 71);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(908, 362);
            this.panel2.TabIndex = 147;
            // 
            // L_0_UNITICON
            // 
            this.L_0_UNITICON.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.L_0_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_UNITICON.Location = new System.Drawing.Point(406, 7);
            this.L_0_UNITICON.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_UNITICON.Name = "L_0_UNITICON";
            this.L_0_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_UNITICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_0_UNITICON.TabIndex = 189;
            this.L_0_UNITICON.TabStop = false;
            // 
            // L_12_TEXT_CONVERSATION
            // 
            this.L_12_TEXT_CONVERSATION.ErrorMessage = "";
            this.L_12_TEXT_CONVERSATION.Location = new System.Drawing.Point(142, 263);
            this.L_12_TEXT_CONVERSATION.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.L_12_TEXT_CONVERSATION.Multiline = true;
            this.L_12_TEXT_CONVERSATION.Name = "L_12_TEXT_CONVERSATION";
            this.L_12_TEXT_CONVERSATION.Placeholder = "";
            this.L_12_TEXT_CONVERSATION.ReadOnly = true;
            this.L_12_TEXT_CONVERSATION.Size = new System.Drawing.Size(746, 84);
            this.L_12_TEXT_CONVERSATION.TabIndex = 183;
            // 
            // D12
            // 
            this.D12.Hexadecimal = true;
            this.D12.Location = new System.Drawing.Point(12, 298);
            this.D12.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.D12.Name = "D12";
            this.D12.Size = new System.Drawing.Size(124, 25);
            this.D12.TabIndex = 182;
            // 
            // J_12_TEXT
            // 
            this.J_12_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_12_TEXT.Location = new System.Drawing.Point(12, 263);
            this.J_12_TEXT.Name = "J_12_TEXT";
            this.J_12_TEXT.Size = new System.Drawing.Size(124, 32);
            this.J_12_TEXT.TabIndex = 181;
            this.J_12_TEXT.Text = "戦績悪";
            this.J_12_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_8_TEXT_CONVERSATION
            // 
            this.L_8_TEXT_CONVERSATION.ErrorMessage = "";
            this.L_8_TEXT_CONVERSATION.Location = new System.Drawing.Point(142, 147);
            this.L_8_TEXT_CONVERSATION.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.L_8_TEXT_CONVERSATION.Multiline = true;
            this.L_8_TEXT_CONVERSATION.Name = "L_8_TEXT_CONVERSATION";
            this.L_8_TEXT_CONVERSATION.Placeholder = "";
            this.L_8_TEXT_CONVERSATION.ReadOnly = true;
            this.L_8_TEXT_CONVERSATION.Size = new System.Drawing.Size(746, 84);
            this.L_8_TEXT_CONVERSATION.TabIndex = 180;
            // 
            // D8
            // 
            this.D8.Hexadecimal = true;
            this.D8.Location = new System.Drawing.Point(12, 182);
            this.D8.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.D8.Name = "D8";
            this.D8.Size = new System.Drawing.Size(124, 25);
            this.D8.TabIndex = 179;
            // 
            // J_8_TEXT
            // 
            this.J_8_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8_TEXT.Location = new System.Drawing.Point(12, 147);
            this.J_8_TEXT.Name = "J_8_TEXT";
            this.J_8_TEXT.Size = new System.Drawing.Size(124, 32);
            this.J_8_TEXT.TabIndex = 178;
            this.J_8_TEXT.Text = "戦績普通";
            this.J_8_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_4_TEXT_CONVERSATION
            // 
            this.L_4_TEXT_CONVERSATION.ErrorMessage = "";
            this.L_4_TEXT_CONVERSATION.Location = new System.Drawing.Point(142, 41);
            this.L_4_TEXT_CONVERSATION.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.L_4_TEXT_CONVERSATION.Multiline = true;
            this.L_4_TEXT_CONVERSATION.Name = "L_4_TEXT_CONVERSATION";
            this.L_4_TEXT_CONVERSATION.Placeholder = "";
            this.L_4_TEXT_CONVERSATION.ReadOnly = true;
            this.L_4_TEXT_CONVERSATION.Size = new System.Drawing.Size(746, 84);
            this.L_4_TEXT_CONVERSATION.TabIndex = 177;
            // 
            // D4
            // 
            this.D4.Hexadecimal = true;
            this.D4.Location = new System.Drawing.Point(12, 76);
            this.D4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.D4.Name = "D4";
            this.D4.Size = new System.Drawing.Size(122, 25);
            this.D4.TabIndex = 176;
            // 
            // J_4_TEXT
            // 
            this.J_4_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_TEXT.Location = new System.Drawing.Point(12, 41);
            this.J_4_TEXT.Name = "J_4_TEXT";
            this.J_4_TEXT.Size = new System.Drawing.Size(124, 32);
            this.J_4_TEXT.TabIndex = 175;
            this.J_4_TEXT.Text = "戦績良";
            this.J_4_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // D0
            // 
            this.D0.Hexadecimal = true;
            this.D0.Location = new System.Drawing.Point(145, 10);
            this.D0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.D0.Name = "D0";
            this.D0.Size = new System.Drawing.Size(67, 25);
            this.D0.TabIndex = 174;
            // 
            // L_0_UNIT
            // 
            this.L_0_UNIT.ErrorMessage = "";
            this.L_0_UNIT.Location = new System.Drawing.Point(222, 10);
            this.L_0_UNIT.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.L_0_UNIT.Name = "L_0_UNIT";
            this.L_0_UNIT.Placeholder = "";
            this.L_0_UNIT.ReadOnly = true;
            this.L_0_UNIT.Size = new System.Drawing.Size(181, 25);
            this.L_0_UNIT.TabIndex = 172;
            // 
            // J_0_UNIT
            // 
            this.J_0_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_UNIT.Location = new System.Drawing.Point(12, 10);
            this.J_0_UNIT.Name = "J_0_UNIT";
            this.J_0_UNIT.Size = new System.Drawing.Size(124, 32);
            this.J_0_UNIT.TabIndex = 2;
            this.J_0_UNIT.Text = "ユニット";
            this.J_0_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(121, 1);
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
            this.panel1.Location = new System.Drawing.Point(14, 13);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 30);
            this.panel1.TabIndex = 145;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(483, -2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 70;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(274, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(240, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(545, 3);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(414, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(754, 0);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(96, 3);
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
            this.AddressPanel.Controls.Add(this.label55);
            this.AddressPanel.Location = new System.Drawing.Point(289, 42);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(908, 30);
            this.AddressPanel.TabIndex = 146;
            // 
            // label55
            // 
            this.label55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label55.Location = new System.Drawing.Point(-2, -2);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(85, 30);
            this.label55.TabIndex = 1;
            this.label55.Text = "アドレス";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EDSensekiCommentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1210, 447);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressPanel);
            this.Name = "EDSensekiCommentForm";
            this.Text = "戦績コメント";
            this.Load += new System.EventHandler(this.EDSensekiCommentForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown D0;
        private FEBuilderGBA.TextBoxEx L_0_UNIT;
        private System.Windows.Forms.Label J_0_UNIT;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.Label label55;
        private FEBuilderGBA.TextBoxEx L_12_TEXT_CONVERSATION;
        private System.Windows.Forms.NumericUpDown D12;
        private System.Windows.Forms.Label J_12_TEXT;
        private FEBuilderGBA.TextBoxEx L_8_TEXT_CONVERSATION;
        private System.Windows.Forms.NumericUpDown D8;
        private System.Windows.Forms.Label J_8_TEXT;
        private FEBuilderGBA.TextBoxEx L_4_TEXT_CONVERSATION;
        private System.Windows.Forms.NumericUpDown D4;
        private System.Windows.Forms.Label J_4_TEXT;
        private InterpolatedPictureBox L_0_UNITICON;
    }
}