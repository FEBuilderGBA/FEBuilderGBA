namespace FEBuilderGBA
{
    partial class WorldMapBGMForm
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
            this.panel11 = new System.Windows.Forms.Panel();
            this.L_2_SONGPLAY = new System.Windows.Forms.Button();
            this.L_0_SONGPLAY = new System.Windows.Forms.Button();
            this.W2 = new System.Windows.Forms.NumericUpDown();
            this.J_2_SONG = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.J_0_SONG = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.L_2_SONG = new FEBuilderGBA.TextBoxEx();
            this.L_0_SONG = new FEBuilderGBA.TextBoxEx();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel11
            // 
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Controls.Add(this.label4);
            this.panel11.Controls.Add(this.L_2_SONGPLAY);
            this.panel11.Controls.Add(this.L_0_SONGPLAY);
            this.panel11.Controls.Add(this.L_2_SONG);
            this.panel11.Controls.Add(this.L_0_SONG);
            this.panel11.Controls.Add(this.W2);
            this.panel11.Controls.Add(this.J_2_SONG);
            this.panel11.Controls.Add(this.W0);
            this.panel11.Controls.Add(this.J_0_SONG);
            this.panel11.Location = new System.Drawing.Point(266, 72);
            this.panel11.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(874, 639);
            this.panel11.TabIndex = 179;
            // 
            // L_2_SONGPLAY
            // 
            this.L_2_SONGPLAY.Location = new System.Drawing.Point(633, 43);
            this.L_2_SONGPLAY.Margin = new System.Windows.Forms.Padding(5);
            this.L_2_SONGPLAY.Name = "L_2_SONGPLAY";
            this.L_2_SONGPLAY.Size = new System.Drawing.Size(30, 35);
            this.L_2_SONGPLAY.TabIndex = 235;
            this.L_2_SONGPLAY.Text = "♪";
            this.L_2_SONGPLAY.UseVisualStyleBackColor = true;
            // 
            // L_0_SONGPLAY
            // 
            this.L_0_SONGPLAY.Location = new System.Drawing.Point(633, 9);
            this.L_0_SONGPLAY.Margin = new System.Windows.Forms.Padding(5);
            this.L_0_SONGPLAY.Name = "L_0_SONGPLAY";
            this.L_0_SONGPLAY.Size = new System.Drawing.Size(30, 35);
            this.L_0_SONGPLAY.TabIndex = 234;
            this.L_0_SONGPLAY.Text = "♪";
            this.L_0_SONGPLAY.UseVisualStyleBackColor = true;
            // 
            // W2
            // 
            this.W2.Hexadecimal = true;
            this.W2.Location = new System.Drawing.Point(278, 43);
            this.W2.Margin = new System.Windows.Forms.Padding(2);
            this.W2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W2.Name = "W2";
            this.W2.Size = new System.Drawing.Size(65, 25);
            this.W2.TabIndex = 202;
            // 
            // J_2_SONG
            // 
            this.J_2_SONG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2_SONG.Location = new System.Drawing.Point(11, 45);
            this.J_2_SONG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2_SONG.Name = "J_2_SONG";
            this.J_2_SONG.Size = new System.Drawing.Size(253, 26);
            this.J_2_SONG.TabIndex = 201;
            this.J_2_SONG.Text = "エフラム編";
            this.J_2_SONG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(682, 1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(193, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(278, 15);
            this.W0.Margin = new System.Windows.Forms.Padding(2);
            this.W0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(65, 25);
            this.W0.TabIndex = 200;
            // 
            // J_0_SONG
            // 
            this.J_0_SONG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_SONG.Location = new System.Drawing.Point(10, 13);
            this.J_0_SONG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_SONG.Name = "J_0_SONG";
            this.J_0_SONG.Size = new System.Drawing.Size(253, 26);
            this.J_0_SONG.TabIndex = 199;
            this.J_0_SONG.Text = "エイリーク編";
            this.J_0_SONG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(14, 8);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1126, 30);
            this.panel1.TabIndex = 178;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(450, -2);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(119, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(251, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(344, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            this.ReadCount.Value = new decimal(new int[] {
            29,
            0,
            0,
            0});
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(120, 2);
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
            // AddressPanel
            // 
            this.AddressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressPanel.Controls.Add(this.BlockSize);
            this.AddressPanel.Controls.Add(this.label3);
            this.AddressPanel.Controls.Add(this.SelectAddress);
            this.AddressPanel.Controls.Add(this.label22);
            this.AddressPanel.Controls.Add(this.Address);
            this.AddressPanel.Controls.Add(this.label23);
            this.AddressPanel.Controls.Add(this.WriteButton);
            this.AddressPanel.Location = new System.Drawing.Point(264, 40);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(877, 30);
            this.AddressPanel.TabIndex = 177;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(239, -1);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(413, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(94, 4);
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
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(2, 0);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 30);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Location = new System.Drawing.Point(14, 40);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(251, 670);
            this.panel6.TabIndex = 190;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(0, 0);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(248, 26);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(11, 153);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(845, 47);
            this.label4.TabIndex = 236;
            this.label4.Text = "この拠点が、次の目的地となったときに再生するBGMを選択します。";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.AddressList.Size = new System.Drawing.Size(246, 634);
            this.AddressList.TabIndex = 108;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // L_2_SONG
            // 
            this.L_2_SONG.ErrorMessage = "";
            this.L_2_SONG.Location = new System.Drawing.Point(353, 47);
            this.L_2_SONG.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_2_SONG.Name = "L_2_SONG";
            this.L_2_SONG.Placeholder = "";
            this.L_2_SONG.ReadOnly = true;
            this.L_2_SONG.Size = new System.Drawing.Size(270, 25);
            this.L_2_SONG.TabIndex = 233;
            // 
            // L_0_SONG
            // 
            this.L_0_SONG.ErrorMessage = "";
            this.L_0_SONG.Location = new System.Drawing.Point(353, 15);
            this.L_0_SONG.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_0_SONG.Name = "L_0_SONG";
            this.L_0_SONG.Placeholder = "";
            this.L_0_SONG.ReadOnly = true;
            this.L_0_SONG.Size = new System.Drawing.Size(270, 25);
            this.L_0_SONG.TabIndex = 232;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(326, 0);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(538, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // WorldMapBGMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1147, 740);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "WorldMapBGMForm";
            this.Text = "ワールドマップBGM";
            this.Load += new System.EventHandler(this.WorldMapPointForm_Load);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel11;
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
        private System.Windows.Forms.NumericUpDown W2;
        private System.Windows.Forms.Label J_2_SONG;
        private System.Windows.Forms.NumericUpDown W0;
        private System.Windows.Forms.Label J_0_SONG;
        private FEBuilderGBA.TextBoxEx L_2_SONG;
        private FEBuilderGBA.TextBoxEx L_0_SONG;
        private System.Windows.Forms.Panel panel6;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label LabelFilter;
        private System.Windows.Forms.Button L_2_SONGPLAY;
        private System.Windows.Forms.Button L_0_SONGPLAY;
        private System.Windows.Forms.Label label4;
    }
}