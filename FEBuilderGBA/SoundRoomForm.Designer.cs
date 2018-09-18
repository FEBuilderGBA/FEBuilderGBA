namespace FEBuilderGBA
{
    partial class SoundRoomForm
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
            this.L_0_SONGPLAY = new System.Windows.Forms.Button();
            this.D12 = new System.Windows.Forms.NumericUpDown();
            this.J_12_TEXT = new System.Windows.Forms.Label();
            this.P8 = new System.Windows.Forms.NumericUpDown();
            this.D4 = new System.Windows.Forms.NumericUpDown();
            this.J_4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.L_8_ASM = new FEBuilderGBA.TextBoxEx();
            this.J_8_ASM = new System.Windows.Forms.Label();
            this.L_12_TEXT_SOUND1 = new FEBuilderGBA.TextBoxEx();
            this.D0 = new System.Windows.Forms.NumericUpDown();
            this.J_0_SONG = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.D12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.D0)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.SuspendLayout();
            // 
            // L_0_SONGPLAY
            // 
            this.L_0_SONGPLAY.Location = new System.Drawing.Point(675, 98);
            this.L_0_SONGPLAY.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_0_SONGPLAY.Name = "L_0_SONGPLAY";
            this.L_0_SONGPLAY.Size = new System.Drawing.Size(32, 34);
            this.L_0_SONGPLAY.TabIndex = 181;
            this.L_0_SONGPLAY.Text = "♪";
            this.L_0_SONGPLAY.UseVisualStyleBackColor = true;
            // 
            // D12
            // 
            this.D12.Hexadecimal = true;
            this.D12.Location = new System.Drawing.Point(234, 104);
            this.D12.Margin = new System.Windows.Forms.Padding(2);
            this.D12.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D12.Name = "D12";
            this.D12.Size = new System.Drawing.Size(130, 25);
            this.D12.TabIndex = 179;
            // 
            // J_12_TEXT
            // 
            this.J_12_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_12_TEXT.Location = new System.Drawing.Point(-1, 101);
            this.J_12_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_12_TEXT.Name = "J_12_TEXT";
            this.J_12_TEXT.Size = new System.Drawing.Size(227, 30);
            this.J_12_TEXT.TabIndex = 178;
            this.J_12_TEXT.Text = "曲名";
            this.J_12_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P8
            // 
            this.P8.Hexadecimal = true;
            this.P8.Location = new System.Drawing.Point(234, 74);
            this.P8.Margin = new System.Windows.Forms.Padding(2);
            this.P8.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.P8.Name = "P8";
            this.P8.Size = new System.Drawing.Size(130, 25);
            this.P8.TabIndex = 173;
            // 
            // D4
            // 
            this.D4.Location = new System.Drawing.Point(234, 38);
            this.D4.Margin = new System.Windows.Forms.Padding(2);
            this.D4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.D4.Name = "D4";
            this.D4.Size = new System.Drawing.Size(130, 25);
            this.D4.TabIndex = 78;
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(-1, 34);
            this.J_4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(227, 30);
            this.J_4.TabIndex = 77;
            this.J_4.Text = "曲の長さ";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.L_8_ASM);
            this.panel6.Controls.Add(this.J_8_ASM);
            this.panel6.Controls.Add(this.L_0_SONGPLAY);
            this.panel6.Controls.Add(this.L_12_TEXT_SOUND1);
            this.panel6.Controls.Add(this.D12);
            this.panel6.Controls.Add(this.J_12_TEXT);
            this.panel6.Controls.Add(this.P8);
            this.panel6.Controls.Add(this.D4);
            this.panel6.Controls.Add(this.J_4);
            this.panel6.Controls.Add(this.D0);
            this.panel6.Controls.Add(this.J_0_SONG);
            this.panel6.Location = new System.Drawing.Point(437, 77);
            this.panel6.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(816, 570);
            this.panel6.TabIndex = 80;
            // 
            // L_8_ASM
            // 
            this.L_8_ASM.ErrorMessage = "";
            this.L_8_ASM.Location = new System.Drawing.Point(370, 75);
            this.L_8_ASM.Margin = new System.Windows.Forms.Padding(2);
            this.L_8_ASM.Name = "L_8_ASM";
            this.L_8_ASM.Placeholder = "";
            this.L_8_ASM.ReadOnly = true;
            this.L_8_ASM.Size = new System.Drawing.Size(337, 25);
            this.L_8_ASM.TabIndex = 183;
            this.L_8_ASM.Visible = false;
            // 
            // J_8_ASM
            // 
            this.J_8_ASM.AccessibleDescription = "@IFASM";
            this.J_8_ASM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8_ASM.Location = new System.Drawing.Point(-1, 69);
            this.J_8_ASM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_8_ASM.Name = "J_8_ASM";
            this.J_8_ASM.Size = new System.Drawing.Size(227, 30);
            this.J_8_ASM.TabIndex = 182;
            this.J_8_ASM.Text = "表示条件ASM";
            this.J_8_ASM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_12_TEXT_SOUND1
            // 
            this.L_12_TEXT_SOUND1.ErrorMessage = "";
            this.L_12_TEXT_SOUND1.Location = new System.Drawing.Point(370, 104);
            this.L_12_TEXT_SOUND1.Margin = new System.Windows.Forms.Padding(2);
            this.L_12_TEXT_SOUND1.Name = "L_12_TEXT_SOUND1";
            this.L_12_TEXT_SOUND1.Placeholder = "";
            this.L_12_TEXT_SOUND1.ReadOnly = true;
            this.L_12_TEXT_SOUND1.Size = new System.Drawing.Size(298, 25);
            this.L_12_TEXT_SOUND1.TabIndex = 180;
            // 
            // D0
            // 
            this.D0.Hexadecimal = true;
            this.D0.Location = new System.Drawing.Point(234, 2);
            this.D0.Margin = new System.Windows.Forms.Padding(2);
            this.D0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D0.Name = "D0";
            this.D0.Size = new System.Drawing.Size(130, 25);
            this.D0.TabIndex = 76;
            // 
            // J_0_SONG
            // 
            this.J_0_SONG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_SONG.Location = new System.Drawing.Point(-1, 0);
            this.J_0_SONG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_SONG.Name = "J_0_SONG";
            this.J_0_SONG.Size = new System.Drawing.Size(227, 30);
            this.J_0_SONG.TabIndex = 62;
            this.J_0_SONG.Text = "BGMID";
            this.J_0_SONG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.AddressListExpandsButton);
            this.panel1.Controls.Add(this.AddressList);
            this.panel1.Controls.Add(this.LabelFilter);
            this.panel1.Location = new System.Drawing.Point(20, 44);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 604);
            this.panel1.TabIndex = 81;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(2, 570);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(404, 30);
            this.AddressListExpandsButton.TabIndex = 114;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 28);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(410, 526);
            this.AddressList.TabIndex = 108;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -2);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(410, 31);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ReloadListButton);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.ReadCount);
            this.panel2.Controls.Add(this.ReadStartAddress);
            this.panel2.Location = new System.Drawing.Point(20, 12);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1234, 30);
            this.panel2.TabIndex = 82;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(642, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(-1, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 30);
            this.label4.TabIndex = 23;
            this.label4.Text = "先頭アドレス";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(416, -1);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 30);
            this.label5.TabIndex = 24;
            this.label5.Text = "読込数";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(507, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
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
            this.panel5.Location = new System.Drawing.Point(437, 44);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(816, 30);
            this.panel5.TabIndex = 83;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(314, 1);
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
            this.label3.Location = new System.Drawing.Point(226, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 59;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(534, -1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 57;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(410, 1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 56;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(673, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(143, 30);
            this.WriteButton.TabIndex = 55;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(90, 2);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 54;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, -2);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 30);
            this.label23.TabIndex = 53;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SoundRoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1259, 650);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel6);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "SoundRoomForm";
            this.Text = "サウンドルーム";
            this.Load += new System.EventHandler(this.SoundRoomForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.D12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.D0)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button L_0_SONGPLAY;
        private System.Windows.Forms.NumericUpDown D12;
        private System.Windows.Forms.Label J_12_TEXT;
        private System.Windows.Forms.NumericUpDown P8;
        private System.Windows.Forms.NumericUpDown D4;
        private System.Windows.Forms.Label J_4;
        private System.Windows.Forms.Panel panel6;
        private FEBuilderGBA.TextBoxEx L_12_TEXT_SOUND1;
        private System.Windows.Forms.NumericUpDown D0;
        private System.Windows.Forms.Label J_0_SONG;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
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
        private System.Windows.Forms.Label J_8_ASM;
        private FEBuilderGBA.TextBoxEx L_8_ASM;

    }
}