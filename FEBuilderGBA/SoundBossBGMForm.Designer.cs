namespace FEBuilderGBA
{
    partial class SoundBossBGMForm
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel4 = new System.Windows.Forms.Panel();
            this.L_0_UNITICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.B3 = new System.Windows.Forms.NumericUpDown();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.L_4_SONG = new FEBuilderGBA.TextBoxEx();
            this.L_0_UNIT = new FEBuilderGBA.TextBoxEx();
            this.L_4_SONGPLAY = new System.Windows.Forms.Button();
            this.D4 = new System.Windows.Forms.NumericUpDown();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.J_4_SONG = new System.Windows.Forms.Label();
            this.J_0_UNIT = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label15 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(337, 2);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(22, 18);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1351, 30);
            this.panel3.TabIndex = 83;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(721, 0);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
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
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(166, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(466, -1);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 30);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(587, 3);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
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
            this.ReadStartAddress.Location = new System.Drawing.Point(169, 1);
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
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.L_0_UNITICON);
            this.panel4.Controls.Add(this.B3);
            this.panel4.Controls.Add(this.B2);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.L_4_SONG);
            this.panel4.Controls.Add(this.L_0_UNIT);
            this.panel4.Controls.Add(this.L_4_SONGPLAY);
            this.panel4.Controls.Add(this.D4);
            this.panel4.Controls.Add(this.W0);
            this.panel4.Controls.Add(this.J_4_SONG);
            this.panel4.Controls.Add(this.J_0_UNIT);
            this.panel4.Location = new System.Drawing.Point(489, 84);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(884, 166);
            this.panel4.TabIndex = 84;
            // 
            // L_0_UNITICON
            // 
            this.L_0_UNITICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_UNITICON.Location = new System.Drawing.Point(487, 2);
            this.L_0_UNITICON.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_UNITICON.Name = "L_0_UNITICON";
            this.L_0_UNITICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_UNITICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.L_0_UNITICON.TabIndex = 189;
            this.L_0_UNITICON.TabStop = false;
            // 
            // B3
            // 
            this.B3.Hexadecimal = true;
            this.B3.Location = new System.Drawing.Point(181, 32);
            this.B3.Margin = new System.Windows.Forms.Padding(2);
            this.B3.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(55, 25);
            this.B3.TabIndex = 188;
            // 
            // B2
            // 
            this.B2.Hexadecimal = true;
            this.B2.Location = new System.Drawing.Point(123, 32);
            this.B2.Margin = new System.Windows.Forms.Padding(2);
            this.B2.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(50, 25);
            this.B2.TabIndex = 187;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(0, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 30);
            this.label2.TabIndex = 186;
            this.label2.Text = "??";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_4_SONG
            // 
            this.L_4_SONG.ErrorMessage = "";
            this.L_4_SONG.Location = new System.Drawing.Point(261, 64);
            this.L_4_SONG.Margin = new System.Windows.Forms.Padding(2);
            this.L_4_SONG.Name = "L_4_SONG";
            this.L_4_SONG.Placeholder = "";
            this.L_4_SONG.ReadOnly = true;
            this.L_4_SONG.Size = new System.Drawing.Size(216, 25);
            this.L_4_SONG.TabIndex = 185;
            // 
            // L_0_UNIT
            // 
            this.L_0_UNIT.ErrorMessage = "";
            this.L_0_UNIT.Location = new System.Drawing.Point(268, 0);
            this.L_0_UNIT.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_UNIT.Name = "L_0_UNIT";
            this.L_0_UNIT.Placeholder = "";
            this.L_0_UNIT.ReadOnly = true;
            this.L_0_UNIT.Size = new System.Drawing.Size(216, 25);
            this.L_0_UNIT.TabIndex = 85;
            // 
            // L_4_SONGPLAY
            // 
            this.L_4_SONGPLAY.Location = new System.Drawing.Point(485, 59);
            this.L_4_SONGPLAY.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_4_SONGPLAY.Name = "L_4_SONGPLAY";
            this.L_4_SONGPLAY.Size = new System.Drawing.Size(32, 34);
            this.L_4_SONGPLAY.TabIndex = 184;
            this.L_4_SONGPLAY.Text = "♪";
            this.L_4_SONGPLAY.UseVisualStyleBackColor = true;
            // 
            // D4
            // 
            this.D4.Hexadecimal = true;
            this.D4.Location = new System.Drawing.Point(123, 62);
            this.D4.Margin = new System.Windows.Forms.Padding(2);
            this.D4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D4.Name = "D4";
            this.D4.Size = new System.Drawing.Size(130, 25);
            this.D4.TabIndex = 183;
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(123, 2);
            this.W0.Margin = new System.Windows.Forms.Padding(2);
            this.W0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(72, 25);
            this.W0.TabIndex = 76;
            // 
            // J_4_SONG
            // 
            this.J_4_SONG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_SONG.Location = new System.Drawing.Point(0, 61);
            this.J_4_SONG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_SONG.Name = "J_4_SONG";
            this.J_4_SONG.Size = new System.Drawing.Size(115, 30);
            this.J_4_SONG.TabIndex = 182;
            this.J_4_SONG.Text = "曲番号";
            this.J_4_SONG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_0_UNIT
            // 
            this.J_0_UNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_UNIT.Location = new System.Drawing.Point(0, 0);
            this.J_0_UNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_UNIT.Name = "J_0_UNIT";
            this.J_0_UNIT.Size = new System.Drawing.Size(115, 30);
            this.J_0_UNIT.TabIndex = 62;
            this.J_0_UNIT.Text = "ユニット";
            this.J_0_UNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.BlockSize);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.SelectAddress);
            this.panel5.Controls.Add(this.label15);
            this.panel5.Controls.Add(this.WriteButton);
            this.panel5.Controls.Add(this.Address);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(489, 50);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(884, 30);
            this.panel5.TabIndex = 82;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(254, 2);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 30);
            this.label14.TabIndex = 52;
            this.label14.Text = "Size:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(551, 2);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(168, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label15
            // 
            this.label15.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Location = new System.Drawing.Point(421, 2);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(122, 30);
            this.label15.TabIndex = 39;
            this.label15.Text = "選択アドレス:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(720, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(120, 2);
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
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(-2, -1);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(116, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "アドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(22, 50);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(464, 623);
            this.panel6.TabIndex = 148;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(0, 591);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(443, 28);
            this.AddressListExpandsButton.TabIndex = 114;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(464, 30);
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
            this.AddressList.Location = new System.Drawing.Point(-1, 28);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(464, 562);
            this.AddressList.TabIndex = 0;
            // 
            // SoundBossBGMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1376, 684);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "SoundBossBGMForm";
            this.Text = "ボスBGM";
            this.Load += new System.EventHandler(this.SoundBossBGMForm_Load);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_UNITICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button L_4_SONGPLAY;
        private System.Windows.Forms.NumericUpDown D4;
        private System.Windows.Forms.NumericUpDown W0;
        private System.Windows.Forms.Label J_4_SONG;
        private System.Windows.Forms.Label J_0_UNIT;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label14;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label16;
        private FEBuilderGBA.TextBoxEx L_0_UNIT;
        private FEBuilderGBA.TextBoxEx L_4_SONG;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private System.Windows.Forms.NumericUpDown B3;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.Label label2;
        private InterpolatedPictureBox L_0_UNITICON;
    }
}