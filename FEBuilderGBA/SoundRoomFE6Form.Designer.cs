namespace FEBuilderGBA
{
    partial class SoundRoomFE6Form
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
            this.D4 = new System.Windows.Forms.NumericUpDown();
            this.J_4_TEXT = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.L_0_SONG = new FEBuilderGBA.TextBoxEx();
            this.L_8_TEXT_SOUND1 = new FEBuilderGBA.TextBoxEx();
            this.D8 = new System.Windows.Forms.NumericUpDown();
            this.J_8_TEXT = new System.Windows.Forms.Label();
            this.L_4_TEXT_SOUND1 = new FEBuilderGBA.TextBoxEx();
            this.D0 = new System.Windows.Forms.NumericUpDown();
            this.J_0_SONG = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton_255 = new System.Windows.Forms.Button();
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
            this.X_RoomPosstionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.D8)).BeginInit();
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
            this.L_0_SONGPLAY.Location = new System.Drawing.Point(704, 30);
            this.L_0_SONGPLAY.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_0_SONGPLAY.Name = "L_0_SONGPLAY";
            this.L_0_SONGPLAY.Size = new System.Drawing.Size(60, 62);
            this.L_0_SONGPLAY.TabIndex = 181;
            this.L_0_SONGPLAY.Text = "♪";
            this.L_0_SONGPLAY.UseVisualStyleBackColor = true;
            // 
            // D4
            // 
            this.D4.Hexadecimal = true;
            this.D4.Location = new System.Drawing.Point(230, 36);
            this.D4.Margin = new System.Windows.Forms.Padding(2);
            this.D4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D4.Name = "D4";
            this.D4.Size = new System.Drawing.Size(130, 25);
            this.D4.TabIndex = 179;
            // 
            // J_4_TEXT
            // 
            this.J_4_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_TEXT.Location = new System.Drawing.Point(-1, 32);
            this.J_4_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_TEXT.Name = "J_4_TEXT";
            this.J_4_TEXT.Size = new System.Drawing.Size(221, 30);
            this.J_4_TEXT.TabIndex = 178;
            this.J_4_TEXT.Text = "曲名";
            this.J_4_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.X_RoomPosstionLabel);
            this.panel6.Controls.Add(this.L_0_SONG);
            this.panel6.Controls.Add(this.L_8_TEXT_SOUND1);
            this.panel6.Controls.Add(this.D8);
            this.panel6.Controls.Add(this.J_8_TEXT);
            this.panel6.Controls.Add(this.L_0_SONGPLAY);
            this.panel6.Controls.Add(this.L_4_TEXT_SOUND1);
            this.panel6.Controls.Add(this.D4);
            this.panel6.Controls.Add(this.J_4_TEXT);
            this.panel6.Controls.Add(this.D0);
            this.panel6.Controls.Add(this.J_0_SONG);
            this.panel6.Location = new System.Drawing.Point(416, 78);
            this.panel6.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(839, 690);
            this.panel6.TabIndex = 80;
            // 
            // L_0_SONG
            // 
            this.L_0_SONG.ErrorMessage = "";
            this.L_0_SONG.Location = new System.Drawing.Point(361, 3);
            this.L_0_SONG.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_SONG.Name = "L_0_SONG";
            this.L_0_SONG.Placeholder = "";
            this.L_0_SONG.ReadOnly = true;
            this.L_0_SONG.Size = new System.Drawing.Size(337, 25);
            this.L_0_SONG.TabIndex = 186;
            // 
            // L_8_TEXT_SOUND1
            // 
            this.L_8_TEXT_SOUND1.ErrorMessage = "";
            this.L_8_TEXT_SOUND1.Location = new System.Drawing.Point(361, 68);
            this.L_8_TEXT_SOUND1.Margin = new System.Windows.Forms.Padding(2);
            this.L_8_TEXT_SOUND1.Name = "L_8_TEXT_SOUND1";
            this.L_8_TEXT_SOUND1.Placeholder = "";
            this.L_8_TEXT_SOUND1.ReadOnly = true;
            this.L_8_TEXT_SOUND1.Size = new System.Drawing.Size(337, 25);
            this.L_8_TEXT_SOUND1.TabIndex = 184;
            // 
            // D8
            // 
            this.D8.Hexadecimal = true;
            this.D8.Location = new System.Drawing.Point(230, 67);
            this.D8.Margin = new System.Windows.Forms.Padding(2);
            this.D8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D8.Name = "D8";
            this.D8.Size = new System.Drawing.Size(130, 25);
            this.D8.TabIndex = 183;
            // 
            // J_8_TEXT
            // 
            this.J_8_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_8_TEXT.Location = new System.Drawing.Point(-1, 62);
            this.J_8_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_8_TEXT.Name = "J_8_TEXT";
            this.J_8_TEXT.Size = new System.Drawing.Size(221, 30);
            this.J_8_TEXT.TabIndex = 182;
            this.J_8_TEXT.Text = "説明";
            this.J_8_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_4_TEXT_SOUND1
            // 
            this.L_4_TEXT_SOUND1.ErrorMessage = "";
            this.L_4_TEXT_SOUND1.Location = new System.Drawing.Point(361, 37);
            this.L_4_TEXT_SOUND1.Margin = new System.Windows.Forms.Padding(2);
            this.L_4_TEXT_SOUND1.Name = "L_4_TEXT_SOUND1";
            this.L_4_TEXT_SOUND1.Placeholder = "";
            this.L_4_TEXT_SOUND1.ReadOnly = true;
            this.L_4_TEXT_SOUND1.Size = new System.Drawing.Size(337, 25);
            this.L_4_TEXT_SOUND1.TabIndex = 180;
            // 
            // D0
            // 
            this.D0.Hexadecimal = true;
            this.D0.Location = new System.Drawing.Point(230, 2);
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
            this.J_0_SONG.AccessibleDescription = "@SOUNDROOM_SONGID";
            this.J_0_SONG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_SONG.Location = new System.Drawing.Point(-1, 0);
            this.J_0_SONG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_SONG.Name = "J_0_SONG";
            this.J_0_SONG.Size = new System.Drawing.Size(221, 30);
            this.J_0_SONG.TabIndex = 62;
            this.J_0_SONG.Text = "BGMID";
            this.J_0_SONG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.AddressListExpandsButton_255);
            this.panel1.Controls.Add(this.AddressList);
            this.panel1.Controls.Add(this.LabelFilter);
            this.panel1.Location = new System.Drawing.Point(20, 44);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 724);
            this.panel1.TabIndex = 81;
            // 
            // AddressListExpandsButton_255
            // 
            this.AddressListExpandsButton_255.Location = new System.Drawing.Point(2, 687);
            this.AddressListExpandsButton_255.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton_255.Name = "AddressListExpandsButton_255";
            this.AddressListExpandsButton_255.Size = new System.Drawing.Size(386, 30);
            this.AddressListExpandsButton_255.TabIndex = 114;
            this.AddressListExpandsButton_255.Text = "リストの拡張";
            this.AddressListExpandsButton_255.UseVisualStyleBackColor = true;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 31);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(394, 652);
            this.AddressList.TabIndex = 108;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(394, 33);
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
            this.panel2.Size = new System.Drawing.Size(1237, 30);
            this.panel2.TabIndex = 82;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(596, -1);
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
            this.label4.Location = new System.Drawing.Point(-1, -1);
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
            this.label5.Location = new System.Drawing.Point(397, -1);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 30);
            this.label5.TabIndex = 24;
            this.label5.Text = "読込数";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(488, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(120, 4);
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
            this.panel5.Location = new System.Drawing.Point(418, 45);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(837, 30);
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
            this.SelectAddress.Location = new System.Drawing.Point(534, 1);
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
            this.WriteButton.Location = new System.Drawing.Point(688, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(150, 30);
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
            // X_RoomPosstionLabel
            // 
            this.X_RoomPosstionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_RoomPosstionLabel.Location = new System.Drawing.Point(-1, 119);
            this.X_RoomPosstionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.X_RoomPosstionLabel.Name = "X_RoomPosstionLabel";
            this.X_RoomPosstionLabel.Size = new System.Drawing.Size(365, 30);
            this.X_RoomPosstionLabel.TabIndex = 188;
            this.X_RoomPosstionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SoundRoomFE6Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1269, 773);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel6);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "SoundRoomFE6Form";
            this.Text = "サウンドルーム";
            this.Load += new System.EventHandler(this.SoundRoomForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.D4)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.D8)).EndInit();
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
        private System.Windows.Forms.NumericUpDown D4;
        private System.Windows.Forms.Label J_4_TEXT;
        private System.Windows.Forms.Panel panel6;
        private FEBuilderGBA.TextBoxEx L_4_TEXT_SOUND1;
        private System.Windows.Forms.NumericUpDown D0;
        private System.Windows.Forms.Label J_0_SONG;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button AddressListExpandsButton_255;
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
        private FEBuilderGBA.TextBoxEx L_8_TEXT_SOUND1;
        private System.Windows.Forms.NumericUpDown D8;
        private System.Windows.Forms.Label J_8_TEXT;
        private TextBoxEx L_0_SONG;
        private System.Windows.Forms.Label X_RoomPosstionLabel;

    }
}