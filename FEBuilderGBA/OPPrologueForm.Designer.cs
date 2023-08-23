namespace FEBuilderGBA
{
    partial class OPPrologueForm
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
            this.J_4 = new System.Windows.Forms.Label();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.P4 = new System.Windows.Forms.NumericUpDown();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel4 = new System.Windows.Forms.Panel();
            this.J_11 = new System.Windows.Forms.Label();
            this.J_10 = new System.Windows.Forms.Label();
            this.J_9 = new System.Windows.Forms.Label();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.B11 = new System.Windows.Forms.NumericUpDown();
            this.B10 = new System.Windows.Forms.NumericUpDown();
            this.B9 = new System.Windows.Forms.NumericUpDown();
            this.B8 = new System.Windows.Forms.NumericUpDown();
            this.J_8 = new System.Windows.Forms.Label();
            this.X_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.J_0 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(-1, 32);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(115, 30);
            this.J_4.TabIndex = 30;
            this.J_4.Text = "TSA";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(379, 0);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(296, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 59;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(593, -2);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 57;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(470, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 56;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(97, 2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 54;
            // 
            // P4
            // 
            this.P4.Hexadecimal = true;
            this.P4.Location = new System.Drawing.Point(121, 37);
            this.P4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P4.Name = "P4";
            this.P4.Size = new System.Drawing.Size(130, 25);
            this.P4.TabIndex = 31;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(0, 26);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(277, 310);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(10, 41);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(278, 375);
            this.panel6.TabIndex = 85;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(3, 343);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(269, 28);
            this.AddressListExpandsButton.TabIndex = 114;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(0, 1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(276, 26);
            this.LabelFilter.TabIndex = 55;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, -1);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(88, 30);
            this.label23.TabIndex = 53;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(9, 9);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1189, 30);
            this.panel3.TabIndex = 83;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(470, -1);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-1, -1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(142, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(281, -1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 30);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(379, 2);
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
            this.ReadStartAddress.Location = new System.Drawing.Point(147, 2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.J_11);
            this.panel4.Controls.Add(this.J_10);
            this.panel4.Controls.Add(this.J_9);
            this.panel4.Controls.Add(this.ImportButton);
            this.panel4.Controls.Add(this.ExportButton);
            this.panel4.Controls.Add(this.B11);
            this.panel4.Controls.Add(this.B10);
            this.panel4.Controls.Add(this.B9);
            this.panel4.Controls.Add(this.B8);
            this.panel4.Controls.Add(this.J_8);
            this.panel4.Controls.Add(this.P4);
            this.panel4.Controls.Add(this.J_4);
            this.panel4.Controls.Add(this.X_PIC);
            this.panel4.Controls.Add(this.P0);
            this.panel4.Controls.Add(this.J_0);
            this.panel4.Location = new System.Drawing.Point(291, 73);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(908, 343);
            this.panel4.TabIndex = 84;
            // 
            // J_11
            // 
            this.J_11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_11.Location = new System.Drawing.Point(-1, 158);
            this.J_11.Name = "J_11";
            this.J_11.Size = new System.Drawing.Size(115, 30);
            this.J_11.TabIndex = 99;
            this.J_11.Text = "??";
            this.J_11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_10
            // 
            this.J_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_10.Location = new System.Drawing.Point(-1, 126);
            this.J_10.Name = "J_10";
            this.J_10.Size = new System.Drawing.Size(115, 30);
            this.J_10.TabIndex = 98;
            this.J_10.Text = "??";
            this.J_10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_9
            // 
            this.J_9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_9.Location = new System.Drawing.Point(-1, 94);
            this.J_9.Name = "J_9";
            this.J_9.Size = new System.Drawing.Size(115, 30);
            this.J_9.TabIndex = 97;
            this.J_9.Text = "??";
            this.J_9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(24, 210);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(160, 30);
            this.ImportButton.TabIndex = 96;
            this.ImportButton.Text = "画像読込";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(24, 254);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(160, 30);
            this.ExportButton.TabIndex = 95;
            this.ExportButton.Text = "画像取出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // B11
            // 
            this.B11.Hexadecimal = true;
            this.B11.Location = new System.Drawing.Point(121, 159);
            this.B11.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B11.Name = "B11";
            this.B11.Size = new System.Drawing.Size(63, 25);
            this.B11.TabIndex = 36;
            // 
            // B10
            // 
            this.B10.Hexadecimal = true;
            this.B10.Location = new System.Drawing.Point(121, 127);
            this.B10.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B10.Name = "B10";
            this.B10.Size = new System.Drawing.Size(63, 25);
            this.B10.TabIndex = 35;
            // 
            // B9
            // 
            this.B9.Hexadecimal = true;
            this.B9.Location = new System.Drawing.Point(121, 95);
            this.B9.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B9.Name = "B9";
            this.B9.Size = new System.Drawing.Size(63, 25);
            this.B9.TabIndex = 34;
            // 
            // B8
            // 
            this.B8.Hexadecimal = true;
            this.B8.Location = new System.Drawing.Point(121, 64);
            this.B8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B8.Name = "B8";
            this.B8.Size = new System.Drawing.Size(63, 25);
            this.B8.TabIndex = 33;
            // 
            // J_8
            // 
            this.J_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8.Location = new System.Drawing.Point(-1, 63);
            this.J_8.Name = "J_8";
            this.J_8.Size = new System.Drawing.Size(115, 30);
            this.J_8.TabIndex = 32;
            this.J_8.Text = "??";
            this.J_8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_PIC
            // 
            this.X_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_PIC.Location = new System.Drawing.Point(285, 7);
            this.X_PIC.Name = "X_PIC";
            this.X_PIC.Size = new System.Drawing.Size(469, 317);
            this.X_PIC.TabIndex = 29;
            this.X_PIC.TabStop = false;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(121, 6);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 28;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(-1, 2);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(115, 30);
            this.J_0.TabIndex = 24;
            this.J_0.Text = "画像";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(740, 1);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 55;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
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
            this.panel5.Location = new System.Drawing.Point(291, 41);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(907, 30);
            this.panel5.TabIndex = 82;
            // 
            // OPPrologueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1209, 433);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Name = "OPPrologueForm";
            this.Text = "OP字幕";
            this.Load += new System.EventHandler(this.OPPrologueForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.B11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label J_4;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.NumericUpDown P4;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.Label LabelFilter;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.NumericUpDown B11;
        private System.Windows.Forms.NumericUpDown B10;
        private System.Windows.Forms.NumericUpDown B9;
        private System.Windows.Forms.NumericUpDown B8;
        private System.Windows.Forms.Label J_8;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Label J_9;
        private System.Windows.Forms.Label J_11;
        private System.Windows.Forms.Label J_10;
        private InterpolatedPictureBox X_PIC;
    }
}