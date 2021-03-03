namespace FEBuilderGBA
{
    partial class WorldMapEventPointerFE7Form
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.EventWriteButton = new System.Windows.Forms.Button();
            this.ENDING2_EVENT = new System.Windows.Forms.NumericUpDown();
            this.ENDING1_EVENT = new System.Windows.Forms.NumericUpDown();
            this.JUMP_ENDING2_EVENT = new System.Windows.Forms.Label();
            this.JUMP_ENDING1_EVENT = new System.Windows.Forms.Label();
            this.N_L_0_EVENT = new FEBuilderGBA.TextBoxEx();
            this.N_P0 = new System.Windows.Forms.NumericUpDown();
            this.N_J_0_EVENT = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.N_ReloadListButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.N_ReadCount = new System.Windows.Forms.NumericUpDown();
            this.N_ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel5 = new System.Windows.Forms.Panel();
            this.N_BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label7 = new System.Windows.Forms.Label();
            this.N_WriteButton = new System.Windows.Forms.Button();
            this.N_SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label8 = new System.Windows.Forms.Label();
            this.N_Address = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.N_AddressListExpandsButton_255 = new System.Windows.Forms.Button();
            this.N_AddressList = new FEBuilderGBA.ListBoxEx();
            this.N_LabelFilter = new System.Windows.Forms.Label();
            this.N_L_0_NEWALLOC_EVENT = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ENDING2_EVENT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ENDING1_EVENT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_P0)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadStartAddress)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Address)).BeginInit();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.N_L_0_NEWALLOC_EVENT);
            this.panel3.Controls.Add(this.panel8);
            this.panel3.Controls.Add(this.N_L_0_EVENT);
            this.panel3.Controls.Add(this.N_P0);
            this.panel3.Controls.Add(this.N_J_0_EVENT);
            this.panel3.Location = new System.Drawing.Point(359, 70);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(908, 650);
            this.panel3.TabIndex = 188;
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.EventWriteButton);
            this.panel8.Controls.Add(this.ENDING2_EVENT);
            this.panel8.Controls.Add(this.ENDING1_EVENT);
            this.panel8.Controls.Add(this.JUMP_ENDING2_EVENT);
            this.panel8.Controls.Add(this.JUMP_ENDING1_EVENT);
            this.panel8.Location = new System.Drawing.Point(273, 558);
            this.panel8.Margin = new System.Windows.Forms.Padding(2);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(631, 88);
            this.panel8.TabIndex = 193;
            // 
            // EventWriteButton
            // 
            this.EventWriteButton.Location = new System.Drawing.Point(451, 7);
            this.EventWriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.EventWriteButton.Name = "EventWriteButton";
            this.EventWriteButton.Size = new System.Drawing.Size(167, 30);
            this.EventWriteButton.TabIndex = 15;
            this.EventWriteButton.Text = "書き込み";
            this.EventWriteButton.UseVisualStyleBackColor = true;
            this.EventWriteButton.Click += new System.EventHandler(this.EventWriteButton_Click);
            // 
            // ENDING2_EVENT
            // 
            this.ENDING2_EVENT.Hexadecimal = true;
            this.ENDING2_EVENT.Location = new System.Drawing.Point(242, 37);
            this.ENDING2_EVENT.Margin = new System.Windows.Forms.Padding(2);
            this.ENDING2_EVENT.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ENDING2_EVENT.Name = "ENDING2_EVENT";
            this.ENDING2_EVENT.Size = new System.Drawing.Size(130, 25);
            this.ENDING2_EVENT.TabIndex = 14;
            // 
            // ENDING1_EVENT
            // 
            this.ENDING1_EVENT.Hexadecimal = true;
            this.ENDING1_EVENT.Location = new System.Drawing.Point(242, 10);
            this.ENDING1_EVENT.Margin = new System.Windows.Forms.Padding(2);
            this.ENDING1_EVENT.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ENDING1_EVENT.Name = "ENDING1_EVENT";
            this.ENDING1_EVENT.Size = new System.Drawing.Size(130, 25);
            this.ENDING1_EVENT.TabIndex = 13;
            // 
            // JUMP_ENDING2_EVENT
            // 
            this.JUMP_ENDING2_EVENT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.JUMP_ENDING2_EVENT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JUMP_ENDING2_EVENT.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.JUMP_ENDING2_EVENT.Location = new System.Drawing.Point(2, 37);
            this.JUMP_ENDING2_EVENT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.JUMP_ENDING2_EVENT.Name = "JUMP_ENDING2_EVENT";
            this.JUMP_ENDING2_EVENT.Size = new System.Drawing.Size(234, 30);
            this.JUMP_ENDING2_EVENT.TabIndex = 12;
            this.JUMP_ENDING2_EVENT.Text = "ヘクトルエンディング";
            this.JUMP_ENDING2_EVENT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.JUMP_ENDING2_EVENT.Click += new System.EventHandler(this.JUMP_ENDING2_EVENT_Click);
            // 
            // JUMP_ENDING1_EVENT
            // 
            this.JUMP_ENDING1_EVENT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.JUMP_ENDING1_EVENT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JUMP_ENDING1_EVENT.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.JUMP_ENDING1_EVENT.Location = new System.Drawing.Point(2, 7);
            this.JUMP_ENDING1_EVENT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.JUMP_ENDING1_EVENT.Name = "JUMP_ENDING1_EVENT";
            this.JUMP_ENDING1_EVENT.Size = new System.Drawing.Size(234, 30);
            this.JUMP_ENDING1_EVENT.TabIndex = 11;
            this.JUMP_ENDING1_EVENT.Text = "エリウッドエンディング";
            this.JUMP_ENDING1_EVENT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.JUMP_ENDING1_EVENT.Click += new System.EventHandler(this.JUMP_ENDING1_EVENT_Click);
            // 
            // N_L_0_EVENT
            // 
            this.N_L_0_EVENT.ErrorMessage = "";
            this.N_L_0_EVENT.Location = new System.Drawing.Point(323, 33);
            this.N_L_0_EVENT.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_L_0_EVENT.Name = "N_L_0_EVENT";
            this.N_L_0_EVENT.Placeholder = "";
            this.N_L_0_EVENT.ReadOnly = true;
            this.N_L_0_EVENT.Size = new System.Drawing.Size(438, 25);
            this.N_L_0_EVENT.TabIndex = 245;
            this.N_L_0_EVENT.Visible = false;
            // 
            // N_P0
            // 
            this.N_P0.Hexadecimal = true;
            this.N_P0.Location = new System.Drawing.Point(323, 2);
            this.N_P0.Margin = new System.Windows.Forms.Padding(2);
            this.N_P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.N_P0.Name = "N_P0";
            this.N_P0.Size = new System.Drawing.Size(130, 25);
            this.N_P0.TabIndex = 211;
            // 
            // N_J_0_EVENT
            // 
            this.N_J_0_EVENT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_0_EVENT.Location = new System.Drawing.Point(2, 1);
            this.N_J_0_EVENT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_0_EVENT.Name = "N_J_0_EVENT";
            this.N_J_0_EVENT.Size = new System.Drawing.Size(312, 26);
            this.N_J_0_EVENT.TabIndex = 210;
            this.N_J_0_EVENT.Text = "ワールドマップイベント";
            this.N_J_0_EVENT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.N_ReloadListButton);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.N_ReadCount);
            this.panel4.Controls.Add(this.N_ReadStartAddress);
            this.panel4.Location = new System.Drawing.Point(14, 13);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1252, 30);
            this.panel4.TabIndex = 186;
            // 
            // N_ReloadListButton
            // 
            this.N_ReloadListButton.Location = new System.Drawing.Point(584, -2);
            this.N_ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_ReloadListButton.Name = "N_ReloadListButton";
            this.N_ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.N_ReloadListButton.TabIndex = 25;
            this.N_ReloadListButton.Text = "再取得";
            this.N_ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(-1, -1);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 30);
            this.label5.TabIndex = 23;
            this.label5.Text = "先頭アドレス";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(344, -1);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 30);
            this.label6.TabIndex = 24;
            this.label6.Text = "読込数";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_ReadCount
            // 
            this.N_ReadCount.Location = new System.Drawing.Point(435, 4);
            this.N_ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.N_ReadCount.Name = "N_ReadCount";
            this.N_ReadCount.Size = new System.Drawing.Size(78, 25);
            this.N_ReadCount.TabIndex = 28;
            // 
            // N_ReadStartAddress
            // 
            this.N_ReadStartAddress.Hexadecimal = true;
            this.N_ReadStartAddress.Location = new System.Drawing.Point(120, 2);
            this.N_ReadStartAddress.Margin = new System.Windows.Forms.Padding(2);
            this.N_ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.N_ReadStartAddress.Name = "N_ReadStartAddress";
            this.N_ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.N_ReadStartAddress.TabIndex = 27;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.N_BlockSize);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.N_WriteButton);
            this.panel5.Controls.Add(this.N_SelectAddress);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.N_Address);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Location = new System.Drawing.Point(359, 42);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(908, 30);
            this.panel5.TabIndex = 185;
            // 
            // N_BlockSize
            // 
            this.N_BlockSize.ErrorMessage = "";
            this.N_BlockSize.Location = new System.Drawing.Point(325, 4);
            this.N_BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_BlockSize.Name = "N_BlockSize";
            this.N_BlockSize.Placeholder = "";
            this.N_BlockSize.ReadOnly = true;
            this.N_BlockSize.Size = new System.Drawing.Size(82, 25);
            this.N_BlockSize.TabIndex = 52;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(239, -2);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 30);
            this.label7.TabIndex = 52;
            this.label7.Text = "Size:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_WriteButton
            // 
            this.N_WriteButton.Location = new System.Drawing.Point(739, -1);
            this.N_WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_WriteButton.Name = "N_WriteButton";
            this.N_WriteButton.Size = new System.Drawing.Size(167, 30);
            this.N_WriteButton.TabIndex = 9;
            this.N_WriteButton.Text = "書き込み";
            this.N_WriteButton.UseVisualStyleBackColor = true;
            // 
            // N_SelectAddress
            // 
            this.N_SelectAddress.ErrorMessage = "";
            this.N_SelectAddress.Location = new System.Drawing.Point(542, -2);
            this.N_SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_SelectAddress.Name = "N_SelectAddress";
            this.N_SelectAddress.Placeholder = "";
            this.N_SelectAddress.ReadOnly = true;
            this.N_SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.N_SelectAddress.TabIndex = 40;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(413, -1);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 30);
            this.label8.TabIndex = 39;
            this.label8.Text = "選択アドレス:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_Address
            // 
            this.N_Address.Hexadecimal = true;
            this.N_Address.Location = new System.Drawing.Point(90, 2);
            this.N_Address.Margin = new System.Windows.Forms.Padding(2);
            this.N_Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.N_Address.Name = "N_Address";
            this.N_Address.Size = new System.Drawing.Size(130, 25);
            this.N_Address.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(-1, -1);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 30);
            this.label9.TabIndex = 1;
            this.label9.Text = "アドレス";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.N_AddressListExpandsButton_255);
            this.panel7.Controls.Add(this.N_AddressList);
            this.panel7.Controls.Add(this.N_LabelFilter);
            this.panel7.Location = new System.Drawing.Point(14, 42);
            this.panel7.Margin = new System.Windows.Forms.Padding(2);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(344, 678);
            this.panel7.TabIndex = 190;
            // 
            // N_AddressListExpandsButton_255
            // 
            this.N_AddressListExpandsButton_255.Location = new System.Drawing.Point(-1, 648);
            this.N_AddressListExpandsButton_255.Margin = new System.Windows.Forms.Padding(2);
            this.N_AddressListExpandsButton_255.Name = "N_AddressListExpandsButton_255";
            this.N_AddressListExpandsButton_255.Size = new System.Drawing.Size(331, 30);
            this.N_AddressListExpandsButton_255.TabIndex = 154;
            this.N_AddressListExpandsButton_255.Text = "リストの拡張";
            this.N_AddressListExpandsButton_255.UseVisualStyleBackColor = true;
            // 
            // N_AddressList
            // 
            this.N_AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.N_AddressList.FormattingEnabled = true;
            this.N_AddressList.IntegralHeight = false;
            this.N_AddressList.ItemHeight = 18;
            this.N_AddressList.Location = new System.Drawing.Point(-1, 26);
            this.N_AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.N_AddressList.Name = "N_AddressList";
            this.N_AddressList.Size = new System.Drawing.Size(348, 616);
            this.N_AddressList.TabIndex = 108;
            // 
            // N_LabelFilter
            // 
            this.N_LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.N_LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.N_LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_LabelFilter.Name = "N_LabelFilter";
            this.N_LabelFilter.Size = new System.Drawing.Size(346, 26);
            this.N_LabelFilter.TabIndex = 107;
            this.N_LabelFilter.Text = "名前";
            this.N_LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // N_L_0_NEWALLOC_EVENT
            // 
            this.N_L_0_NEWALLOC_EVENT.Location = new System.Drawing.Point(325, 65);
            this.N_L_0_NEWALLOC_EVENT.Name = "N_L_0_NEWALLOC_EVENT";
            this.N_L_0_NEWALLOC_EVENT.Size = new System.Drawing.Size(300, 28);
            this.N_L_0_NEWALLOC_EVENT.TabIndex = 249;
            this.N_L_0_NEWALLOC_EVENT.Text = "新規イベント";
            this.N_L_0_NEWALLOC_EVENT.UseVisualStyleBackColor = true;
            // 
            // WorldMapEventPointerFE7Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1272, 732);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "WorldMapEventPointerFE7Form";
            this.Text = "ワールドマップイベント";
            this.Load += new System.EventHandler(this.WorldMapEventPointerFE7Form_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ENDING2_EVENT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ENDING1_EVENT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_P0)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadStartAddress)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Address)).EndInit();
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown N_P0;
        private System.Windows.Forms.Label N_J_0_EVENT;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button N_ReloadListButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown N_ReadCount;
        private System.Windows.Forms.NumericUpDown N_ReadStartAddress;
        private System.Windows.Forms.Panel panel5;
        private FEBuilderGBA.TextBoxEx N_BlockSize;
        private System.Windows.Forms.Label label7;
        private FEBuilderGBA.TextBoxEx N_SelectAddress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button N_WriteButton;
        private System.Windows.Forms.NumericUpDown N_Address;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel7;
        private ListBoxEx N_AddressList;
        private System.Windows.Forms.Label N_LabelFilter;
        private TextBoxEx N_L_0_EVENT;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button EventWriteButton;
        private System.Windows.Forms.NumericUpDown ENDING2_EVENT;
        private System.Windows.Forms.NumericUpDown ENDING1_EVENT;
        private System.Windows.Forms.Label JUMP_ENDING2_EVENT;
        private System.Windows.Forms.Label JUMP_ENDING1_EVENT;
        private System.Windows.Forms.Button N_AddressListExpandsButton_255;
        private System.Windows.Forms.Button N_L_0_NEWALLOC_EVENT;
    }
}