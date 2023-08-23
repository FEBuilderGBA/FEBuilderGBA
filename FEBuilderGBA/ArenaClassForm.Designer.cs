﻿namespace FEBuilderGBA
{
    partial class ArenaClassForm
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
            this.L_0_CLASSICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.X_FE8J_COMMENT = new System.Windows.Forms.Label();
            this.L_0_CLASS = new FEBuilderGBA.TextBoxEx();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.J_0_CLASS = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_CLASSICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.L_0_CLASSICON);
            this.panel4.Controls.Add(this.X_FE8J_COMMENT);
            this.panel4.Controls.Add(this.L_0_CLASS);
            this.panel4.Controls.Add(this.B0);
            this.panel4.Controls.Add(this.J_0_CLASS);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 34);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(878, 632);
            this.panel4.TabIndex = 88;
            // 
            // L_0_CLASSICON
            // 
            this.L_0_CLASSICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_CLASSICON.Location = new System.Drawing.Point(363, 2);
            this.L_0_CLASSICON.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_CLASSICON.Name = "L_0_CLASSICON";
            this.L_0_CLASSICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_CLASSICON.TabIndex = 102;
            this.L_0_CLASSICON.TabStop = false;
            // 
            // X_FE8J_COMMENT
            // 
            this.X_FE8J_COMMENT.AutoSize = true;
            this.X_FE8J_COMMENT.Location = new System.Drawing.Point(48, 139);
            this.X_FE8J_COMMENT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.X_FE8J_COMMENT.Name = "X_FE8J_COMMENT";
            this.X_FE8J_COMMENT.Size = new System.Drawing.Size(447, 54);
            this.X_FE8J_COMMENT.TabIndex = 42;
            this.X_FE8J_COMMENT.Text = "上級職、下級職は自動で調整されます。\r\n闘技場で敵として使用されるユニットは、0xFD 対戦相手です。\r\nほかのユニットが出ることはないようです。";
            this.X_FE8J_COMMENT.Visible = false;
            // 
            // L_0_CLASS
            // 
            this.L_0_CLASS.ErrorMessage = "";
            this.L_0_CLASS.Location = new System.Drawing.Point(212, 7);
            this.L_0_CLASS.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_CLASS.Name = "L_0_CLASS";
            this.L_0_CLASS.Placeholder = "";
            this.L_0_CLASS.ReadOnly = true;
            this.L_0_CLASS.Size = new System.Drawing.Size(147, 25);
            this.L_0_CLASS.TabIndex = 41;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(118, 7);
            this.B0.Margin = new System.Windows.Forms.Padding(2);
            this.B0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(79, 25);
            this.B0.TabIndex = 0;
            // 
            // J_0_CLASS
            // 
            this.J_0_CLASS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_CLASS.Location = new System.Drawing.Point(-2, 2);
            this.J_0_CLASS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_CLASS.Name = "J_0_CLASS";
            this.J_0_CLASS.Size = new System.Drawing.Size(114, 30);
            this.J_0_CLASS.TabIndex = 24;
            this.J_0_CLASS.Text = "クラス";
            this.J_0_CLASS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(306, -3);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 36);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(689, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(168, 30);
            this.WriteButton.TabIndex = 55;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(729, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 3;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 34);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(307, 666);
            this.panel6.TabIndex = 89;
            // 
            // AddressList
            // 
            this.AddressList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(0, 26);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(305, 608);
            this.AddressList.TabIndex = 0;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AddressListExpandsButton.Location = new System.Drawing.Point(0, 634);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(305, 30);
            this.AddressListExpandsButton.TabIndex = 114;
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
            this.LabelFilter.Size = new System.Drawing.Size(305, 26);
            this.LabelFilter.TabIndex = 55;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(408, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 34);
            this.label22.TabIndex = 56;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(557, -2);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 34);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.FilterComboBox);
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1185, 34);
            this.panel3.TabIndex = 87;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(-1, -1);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 34);
            this.label2.TabIndex = 30;
            this.label2.Text = "条件:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Items.AddRange(new object[] {
            "近距離武器",
            "魔法武器",
            "遠距離武器"});
            this.FilterComboBox.Location = new System.Drawing.Point(80, 2);
            this.FilterComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(223, 26);
            this.FilterComboBox.TabIndex = 2;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(647, 3);
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
            this.ReadStartAddress.Location = new System.Drawing.Point(422, 3);
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
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, -1);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(84, 33);
            this.label23.TabIndex = 53;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(878, 34);
            this.panel5.TabIndex = 86;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(314, 4);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 58;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(532, 2);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(150, 25);
            this.SelectAddress.TabIndex = 57;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(307, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(878, 666);
            this.panel1.TabIndex = 90;
            // 
            // ArenaClassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1185, 700);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ArenaClassForm";
            this.Text = "闘技場 出現クラス";
            this.Load += new System.EventHandler(this.ArenaClassForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_CLASSICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.Label J_0_CLASS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.Label LabelFilter;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panel5;
        private FEBuilderGBA.TextBoxEx L_0_CLASS;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.Label X_FE8J_COMMENT;
        private System.Windows.Forms.Label label2;
        private InterpolatedPictureBox L_0_CLASSICON;
        private System.Windows.Forms.Panel panel1;
    }
}