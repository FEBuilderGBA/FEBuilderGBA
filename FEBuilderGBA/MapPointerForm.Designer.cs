namespace FEBuilderGBA
{
    partial class MapPointerForm
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
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.PListSplitsExpandsButton = new System.Windows.Forms.Button();
            this.PListSplitsExpandsPanel = new System.Windows.Forms.Panel();
            this.PListSplitsExpandsExplainLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.PListSplitsExpandsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(537, 1);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(79, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(122, 4);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
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
            this.panel1.Controls.Add(this.FilterComboBox);
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(11, 11);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1274, 30);
            this.panel1.TabIndex = 80;
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Location = new System.Drawing.Point(631, 1);
            this.FilterComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(373, 26);
            this.FilterComboBox.TabIndex = 149;
            this.FilterComboBox.Visible = false;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(1041, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(111, 31);
            this.ReloadListButton.TabIndex = 70;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -1);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 32);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(445, -4);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 32);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(302, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(83, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.P0);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(459, 74);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(826, 44);
            this.panel2.TabIndex = 82;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(176, 11);
            this.P0.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(2, 6);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 32);
            this.label4.TabIndex = 2;
            this.label4.Text = "ポインタ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(530, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(138, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(224, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.AddressPanel.Location = new System.Drawing.Point(458, 43);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(826, 30);
            this.AddressPanel.TabIndex = 81;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(398, 0);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(676, 0);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(150, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(92, 1);
            this.Address.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
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
            this.label55.Location = new System.Drawing.Point(-1, -2);
            this.label55.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(86, 32);
            this.label55.TabIndex = 1;
            this.label55.Text = "アドレス";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label56
            // 
            this.label56.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label56.Location = new System.Drawing.Point(12, 41);
            this.label56.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(440, 26);
            this.label56.TabIndex = 85;
            this.label56.Text = "名前";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(11, 67);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(442, 418);
            this.AddressList.TabIndex = 84;
            // 
            // PListSplitsExpandsButton
            // 
            this.PListSplitsExpandsButton.Location = new System.Drawing.Point(2, 195);
            this.PListSplitsExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.PListSplitsExpandsButton.Name = "PListSplitsExpandsButton";
            this.PListSplitsExpandsButton.Size = new System.Drawing.Size(803, 30);
            this.PListSplitsExpandsButton.TabIndex = 116;
            this.PListSplitsExpandsButton.Text = "PLIST分割";
            this.PListSplitsExpandsButton.UseVisualStyleBackColor = true;
            this.PListSplitsExpandsButton.Click += new System.EventHandler(this.PListSplitsExpandsButton_Click);
            // 
            // PListSplitsExpandsPanel
            // 
            this.PListSplitsExpandsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PListSplitsExpandsPanel.Controls.Add(this.PListSplitsExpandsExplainLabel);
            this.PListSplitsExpandsPanel.Controls.Add(this.PListSplitsExpandsButton);
            this.PListSplitsExpandsPanel.Location = new System.Drawing.Point(462, 250);
            this.PListSplitsExpandsPanel.Margin = new System.Windows.Forms.Padding(5);
            this.PListSplitsExpandsPanel.Name = "PListSplitsExpandsPanel";
            this.PListSplitsExpandsPanel.Size = new System.Drawing.Size(822, 229);
            this.PListSplitsExpandsPanel.TabIndex = 117;
            // 
            // PListSplitsExpandsExplainLabel
            // 
            this.PListSplitsExpandsExplainLabel.AutoSize = true;
            this.PListSplitsExpandsExplainLabel.Location = new System.Drawing.Point(4, 4);
            this.PListSplitsExpandsExplainLabel.Name = "PListSplitsExpandsExplainLabel";
            this.PListSplitsExpandsExplainLabel.Size = new System.Drawing.Size(42, 18);
            this.PListSplitsExpandsExplainLabel.TabIndex = 117;
            this.PListSplitsExpandsExplainLabel.Text = "Text";
            // 
            // MapPointerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1289, 494);
            this.Controls.Add(this.PListSplitsExpandsPanel);
            this.Controls.Add(this.label56);
            this.Controls.Add(this.AddressList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.AddressPanel);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MapPointerForm";
            this.Text = "マップポインタ";
            this.Load += new System.EventHandler(this.MapPointerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.PListSplitsExpandsPanel.ResumeLayout(false);
            this.PListSplitsExpandsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.Label label56;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button PListSplitsExpandsButton;
        private System.Windows.Forms.Panel PListSplitsExpandsPanel;
        private System.Windows.Forms.Label PListSplitsExpandsExplainLabel;
    }
}