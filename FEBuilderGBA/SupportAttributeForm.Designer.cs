namespace FEBuilderGBA
{
    partial class SupportAttributeForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Comment = new FEBuilderGBA.TextBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.L_0_ATTRIBUTEICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.B7 = new System.Windows.Forms.NumericUpDown();
            this.J_7 = new System.Windows.Forms.Label();
            this.B6 = new System.Windows.Forms.NumericUpDown();
            this.B3 = new System.Windows.Forms.NumericUpDown();
            this.J_6 = new System.Windows.Forms.Label();
            this.J_3 = new System.Windows.Forms.Label();
            this.B5 = new System.Windows.Forms.NumericUpDown();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.B4 = new System.Windows.Forms.NumericUpDown();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.J_5 = new System.Windows.Forms.Label();
            this.J_2 = new System.Windows.Forms.Label();
            this.J_4 = new System.Windows.Forms.Label();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.L_0_ATTRIBUTE = new System.Windows.Forms.Label();
            this.J_1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_ATTRIBUTEICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(13, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1133, 30);
            this.panel1.TabIndex = 57;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(509, 0);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(277, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(371, 2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(120, 4);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
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
            this.AddressPanel.Location = new System.Drawing.Point(291, 42);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(855, 30);
            this.AddressPanel.TabIndex = 56;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(321, 3);
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
            this.label3.Location = new System.Drawing.Point(233, -1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(530, 3);
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
            this.label22.Location = new System.Drawing.Point(407, -1);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(675, -3);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(179, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(94, 3);
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
            this.label23.Location = new System.Drawing.Point(-1, -2);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 30);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.Comment);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.L_0_ATTRIBUTEICON);
            this.panel2.Controls.Add(this.B7);
            this.panel2.Controls.Add(this.J_7);
            this.panel2.Controls.Add(this.B6);
            this.panel2.Controls.Add(this.B3);
            this.panel2.Controls.Add(this.J_6);
            this.panel2.Controls.Add(this.J_3);
            this.panel2.Controls.Add(this.B5);
            this.panel2.Controls.Add(this.B2);
            this.panel2.Controls.Add(this.B4);
            this.panel2.Controls.Add(this.B1);
            this.panel2.Controls.Add(this.J_5);
            this.panel2.Controls.Add(this.J_2);
            this.panel2.Controls.Add(this.J_4);
            this.panel2.Controls.Add(this.B0);
            this.panel2.Controls.Add(this.L_0_ATTRIBUTE);
            this.panel2.Controls.Add(this.J_1);
            this.panel2.Location = new System.Drawing.Point(291, 75);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(855, 382);
            this.panel2.TabIndex = 54;
            // 
            // Comment
            // 
            this.Comment.ErrorMessage = "";
            this.Comment.Location = new System.Drawing.Point(180, 190);
            this.Comment.Name = "Comment";
            this.Comment.Placeholder = "";
            this.Comment.Size = new System.Drawing.Size(256, 25);
            this.Comment.TabIndex = 198;
            // 
            // label4
            // 
            this.label4.AccessibleDescription = "@COMMENT";
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(44, 187);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 31);
            this.label4.TabIndex = 197;
            this.label4.Text = "コメント";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_0_ATTRIBUTEICON
            // 
            this.L_0_ATTRIBUTEICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_ATTRIBUTEICON.Location = new System.Drawing.Point(267, 13);
            this.L_0_ATTRIBUTEICON.Name = "L_0_ATTRIBUTEICON";
            this.L_0_ATTRIBUTEICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_ATTRIBUTEICON.TabIndex = 47;
            this.L_0_ATTRIBUTEICON.TabStop = false;
            // 
            // B7
            // 
            this.B7.Hexadecimal = true;
            this.B7.Location = new System.Drawing.Point(180, 128);
            this.B7.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B7.Name = "B7";
            this.B7.Size = new System.Drawing.Size(77, 25);
            this.B7.TabIndex = 38;
            // 
            // J_7
            // 
            this.J_7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_7.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.J_7.Location = new System.Drawing.Point(44, 128);
            this.J_7.Name = "J_7";
            this.J_7.Size = new System.Drawing.Size(127, 32);
            this.J_7.TabIndex = 37;
            this.J_7.Text = "??";
            this.J_7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B6
            // 
            this.B6.Location = new System.Drawing.Point(669, 83);
            this.B6.Name = "B6";
            this.B6.Size = new System.Drawing.Size(78, 25);
            this.B6.TabIndex = 36;
            // 
            // B3
            // 
            this.B3.Location = new System.Drawing.Point(669, 49);
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(78, 25);
            this.B3.TabIndex = 35;
            // 
            // J_6
            // 
            this.J_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_6.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.J_6.Location = new System.Drawing.Point(504, 79);
            this.J_6.Name = "J_6";
            this.J_6.Size = new System.Drawing.Size(159, 30);
            this.J_6.TabIndex = 34;
            this.J_6.Text = "必殺回避";
            this.J_6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_3
            // 
            this.J_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_3.Location = new System.Drawing.Point(504, 49);
            this.J_3.Name = "J_3";
            this.J_3.Size = new System.Drawing.Size(159, 29);
            this.J_3.TabIndex = 33;
            this.J_3.Text = "命中";
            this.J_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B5
            // 
            this.B5.Location = new System.Drawing.Point(419, 83);
            this.B5.Name = "B5";
            this.B5.Size = new System.Drawing.Size(78, 25);
            this.B5.TabIndex = 32;
            // 
            // B2
            // 
            this.B2.Location = new System.Drawing.Point(419, 49);
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(78, 25);
            this.B2.TabIndex = 31;
            // 
            // B4
            // 
            this.B4.Location = new System.Drawing.Point(180, 82);
            this.B4.Name = "B4";
            this.B4.Size = new System.Drawing.Size(78, 25);
            this.B4.TabIndex = 30;
            // 
            // B1
            // 
            this.B1.Location = new System.Drawing.Point(179, 47);
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(78, 25);
            this.B1.TabIndex = 29;
            // 
            // J_5
            // 
            this.J_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_5.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.J_5.Location = new System.Drawing.Point(267, 79);
            this.J_5.Name = "J_5";
            this.J_5.Size = new System.Drawing.Size(146, 30);
            this.J_5.TabIndex = 28;
            this.J_5.Text = "必殺";
            this.J_5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(267, 49);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(146, 29);
            this.J_2.TabIndex = 25;
            this.J_2.Text = "防御";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(44, 79);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(129, 30);
            this.J_4.TabIndex = 24;
            this.J_4.Text = "回避";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(179, 13);
            this.B0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(78, 25);
            this.B0.TabIndex = 20;
            // 
            // L_0_ATTRIBUTE
            // 
            this.L_0_ATTRIBUTE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.L_0_ATTRIBUTE.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.L_0_ATTRIBUTE.Location = new System.Drawing.Point(0, 9);
            this.L_0_ATTRIBUTE.Name = "L_0_ATTRIBUTE";
            this.L_0_ATTRIBUTE.Size = new System.Drawing.Size(174, 32);
            this.L_0_ATTRIBUTE.TabIndex = 19;
            this.L_0_ATTRIBUTE.Text = "属性:-";
            this.L_0_ATTRIBUTE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_1
            // 
            this.J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_1.Location = new System.Drawing.Point(44, 46);
            this.J_1.Name = "J_1";
            this.J_1.Size = new System.Drawing.Size(129, 32);
            this.J_1.TabIndex = 18;
            this.J_1.Text = "攻撃";
            this.J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.AddressList);
            this.panel3.Controls.Add(this.AddressListExpandsButton);
            this.panel3.Controls.Add(this.LabelFilter);
            this.panel3.Location = new System.Drawing.Point(13, 40);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 417);
            this.panel3.TabIndex = 82;
            // 
            // AddressList
            // 
            this.AddressList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(0, 33);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(270, 352);
            this.AddressList.TabIndex = 108;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AddressListExpandsButton.Location = new System.Drawing.Point(0, 385);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(270, 30);
            this.AddressListExpandsButton.TabIndex = 115;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabelFilter.Location = new System.Drawing.Point(0, 0);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(270, 33);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SupportAttributeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1153, 459);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressPanel);
            this.Controls.Add(this.panel2);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "SupportAttributeForm";
            this.Text = "支援効果";
            this.Load += new System.EventHandler(this.SupportAttributeForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_ATTRIBUTEICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel AddressPanel;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label J_5;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.Label J_4;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.Label L_0_ATTRIBUTE;
        private System.Windows.Forms.Label J_1;
        private System.Windows.Forms.NumericUpDown B6;
        private System.Windows.Forms.NumericUpDown B3;
        private System.Windows.Forms.Label J_6;
        private System.Windows.Forms.Label J_3;
        private System.Windows.Forms.NumericUpDown B5;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.NumericUpDown B4;
        private System.Windows.Forms.NumericUpDown B1;
        private System.Windows.Forms.NumericUpDown B7;
        private System.Windows.Forms.Label J_7;
        private System.Windows.Forms.Panel panel3;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label LabelFilter;
        private InterpolatedPictureBox L_0_ATTRIBUTEICON;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private TextBoxEx Comment;
        private System.Windows.Forms.Label label4;
    }
}