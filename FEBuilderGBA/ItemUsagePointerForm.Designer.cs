namespace FEBuilderGBA
{
    partial class ItemUsagePointerForm
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
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.StatBoosterItemExplain = new System.Windows.Forms.GroupBox();
            this.StatBoosterItemLink = new System.Windows.Forms.Label();
            this.PromotionItemExplain = new System.Windows.Forms.GroupBox();
            this.PromotionItemLink = new System.Windows.Forms.Label();
            this.L_0_COMBO = new System.Windows.Forms.ComboBox();
            this.ERROR_NOT_FOUND = new System.Windows.Forms.Label();
            this.J_0_ASM_THUMB = new System.Windows.Forms.Label();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.SwitchListExpandsButton = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.L_0_ASM_SWITCH = new FEBuilderGBA.TextBoxEx();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel4.SuspendLayout();
            this.StatBoosterItemExplain.SuspendLayout();
            this.PromotionItemExplain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(1082, -1);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.StatBoosterItemExplain);
            this.panel4.Controls.Add(this.PromotionItemExplain);
            this.panel4.Controls.Add(this.L_0_ASM_SWITCH);
            this.panel4.Controls.Add(this.L_0_COMBO);
            this.panel4.Controls.Add(this.ERROR_NOT_FOUND);
            this.panel4.Controls.Add(this.J_0_ASM_THUMB);
            this.panel4.Controls.Add(this.P0);
            this.panel4.Location = new System.Drawing.Point(295, 72);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(916, 678);
            this.panel4.TabIndex = 149;
            // 
            // StatBoosterItemExplain
            // 
            this.StatBoosterItemExplain.Controls.Add(this.StatBoosterItemLink);
            this.StatBoosterItemExplain.Location = new System.Drawing.Point(8, 399);
            this.StatBoosterItemExplain.Name = "StatBoosterItemExplain";
            this.StatBoosterItemExplain.Size = new System.Drawing.Size(893, 82);
            this.StatBoosterItemExplain.TabIndex = 121;
            this.StatBoosterItemExplain.TabStop = false;
            this.StatBoosterItemExplain.Text = "関連項目";
            // 
            // StatBoosterItemLink
            // 
            this.StatBoosterItemLink.AutoSize = true;
            this.StatBoosterItemLink.Location = new System.Drawing.Point(24, 45);
            this.StatBoosterItemLink.Name = "StatBoosterItemLink";
            this.StatBoosterItemLink.Size = new System.Drawing.Size(80, 18);
            this.StatBoosterItemLink.TabIndex = 0;
            this.StatBoosterItemLink.Text = "能力補正";
            this.StatBoosterItemLink.Click += new System.EventHandler(this.StatBoosterItemLink_Click);
            // 
            // PromotionItemExplain
            // 
            this.PromotionItemExplain.Controls.Add(this.PromotionItemLink);
            this.PromotionItemExplain.Location = new System.Drawing.Point(8, 399);
            this.PromotionItemExplain.Name = "PromotionItemExplain";
            this.PromotionItemExplain.Size = new System.Drawing.Size(893, 82);
            this.PromotionItemExplain.TabIndex = 120;
            this.PromotionItemExplain.TabStop = false;
            this.PromotionItemExplain.Text = "関連項目";
            // 
            // PromotionItemLink
            // 
            this.PromotionItemLink.AutoSize = true;
            this.PromotionItemLink.Location = new System.Drawing.Point(24, 45);
            this.PromotionItemLink.Name = "PromotionItemLink";
            this.PromotionItemLink.Size = new System.Drawing.Size(88, 18);
            this.PromotionItemLink.TabIndex = 0;
            this.PromotionItemLink.Text = "CCアイテム";
            this.PromotionItemLink.Click += new System.EventHandler(this.PromotionItemLink_Click);
            // 
            // L_0_COMBO
            // 
            this.L_0_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_0_COMBO.FormattingEnabled = true;
            this.L_0_COMBO.Location = new System.Drawing.Point(3, 36);
            this.L_0_COMBO.Name = "L_0_COMBO";
            this.L_0_COMBO.Size = new System.Drawing.Size(908, 26);
            this.L_0_COMBO.TabIndex = 117;
            // 
            // ERROR_NOT_FOUND
            // 
            this.ERROR_NOT_FOUND.AutoSize = true;
            this.ERROR_NOT_FOUND.ForeColor = System.Drawing.Color.Red;
            this.ERROR_NOT_FOUND.Location = new System.Drawing.Point(3, 163);
            this.ERROR_NOT_FOUND.Name = "ERROR_NOT_FOUND";
            this.ERROR_NOT_FOUND.Size = new System.Drawing.Size(272, 18);
            this.ERROR_NOT_FOUND.TabIndex = 118;
            this.ERROR_NOT_FOUND.Text = "このバージョンのFEでは利用しません。";
            // 
            // J_0_ASM_THUMB
            // 
            this.J_0_ASM_THUMB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_ASM_THUMB.Location = new System.Drawing.Point(-1, 3);
            this.J_0_ASM_THUMB.Name = "J_0_ASM_THUMB";
            this.J_0_ASM_THUMB.Size = new System.Drawing.Size(220, 30);
            this.J_0_ASM_THUMB.TabIndex = 114;
            this.J_0_ASM_THUMB.Text = "ASMポインタ";
            this.J_0_ASM_THUMB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(240, 8);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(142, 25);
            this.P0.TabIndex = 85;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(609, -1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.BlockSize);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.SelectAddress);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.WriteButton);
            this.panel5.Controls.Add(this.Address);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(295, 42);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(917, 30);
            this.panel5.TabIndex = 147;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(240, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 30);
            this.label11.TabIndex = 64;
            this.label11.Text = "Size:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(414, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 61;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(746, -1);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 60;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(90, 3);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 55;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(-1, -1);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "アドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Controls.Add(this.SwitchListExpandsButton);
            this.panel6.Controls.Add(this.label30);
            this.panel6.Location = new System.Drawing.Point(12, 45);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(277, 709);
            this.panel6.TabIndex = 150;
            // 
            // SwitchListExpandsButton
            // 
            this.SwitchListExpandsButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SwitchListExpandsButton.Location = new System.Drawing.Point(0, 677);
            this.SwitchListExpandsButton.Name = "SwitchListExpandsButton";
            this.SwitchListExpandsButton.Size = new System.Drawing.Size(275, 30);
            this.SwitchListExpandsButton.TabIndex = 115;
            this.SwitchListExpandsButton.Text = "リストの拡張";
            this.SwitchListExpandsButton.UseVisualStyleBackColor = true;
            this.SwitchListExpandsButton.Click += new System.EventHandler(this.SwitchListExpandsButton_Click);
            // 
            // label30
            // 
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Dock = System.Windows.Forms.DockStyle.Top;
            this.label30.Location = new System.Drawing.Point(0, 0);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(275, 26);
            this.label30.TabIndex = 106;
            this.label30.Text = "名前";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(983, 2);
            this.ReadCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.FilterComboBox);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1200, 30);
            this.panel3.TabIndex = 148;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(-1, -3);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 34);
            this.label4.TabIndex = 159;
            this.label4.Text = "条件:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Items.AddRange(new object[] {
            "0=アイテムを利用できるか判定する",
            "1=アイテムを利用した場合の効果を定義する",
            "2=CCアイテムを使った場合の処理を定義する",
            "3=CCアイテムかどうかを定義する(FE7のみ)",
            "4=アイテムのターゲット選択の方法を定義する",
            "5=杖の種類を定義する",
            "6=ドーピングアイテムを利用した時のメッセージを定義する",
            "7=ドーピングアイテムとCCアイテムかどうかを定義する",
            "8=利用時のエラーメッセージを定義する",
            "9=アイテムの前置詞 A|An|The(FE8Uのみ)"});
            this.FilterComboBox.Location = new System.Drawing.Point(82, 1);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(521, 26);
            this.FilterComboBox.TabIndex = 30;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(892, -1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 30);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(756, 2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // L_0_ASM_SWITCH
            // 
            this.L_0_ASM_SWITCH.ErrorMessage = "";
            this.L_0_ASM_SWITCH.Location = new System.Drawing.Point(414, 8);
            this.L_0_ASM_SWITCH.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.L_0_ASM_SWITCH.Name = "L_0_ASM_SWITCH";
            this.L_0_ASM_SWITCH.Placeholder = "";
            this.L_0_ASM_SWITCH.ReadOnly = true;
            this.L_0_ASM_SWITCH.Size = new System.Drawing.Size(404, 25);
            this.L_0_ASM_SWITCH.TabIndex = 119;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(326, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 63;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(540, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 62;
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
            this.AddressList.Size = new System.Drawing.Size(275, 651);
            this.AddressList.TabIndex = 0;
            // 
            // ItemUsagePointerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1222, 763);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Name = "ItemUsagePointerForm";
            this.Text = "アイテム利用効果";
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.StatBoosterItemExplain.ResumeLayout(false);
            this.StatBoosterItemExplain.PerformLayout();
            this.PromotionItemExplain.ResumeLayout(false);
            this.PromotionItemExplain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label30;
        private ListBoxEx AddressList;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.NumericUpDown Address;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label11;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.Button SwitchListExpandsButton;
        private System.Windows.Forms.Label J_0_ASM_THUMB;
        private System.Windows.Forms.ComboBox L_0_COMBO;
        private System.Windows.Forms.Label ERROR_NOT_FOUND;
        private TextBoxEx L_0_ASM_SWITCH;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox PromotionItemExplain;
        private System.Windows.Forms.Label PromotionItemLink;
        private System.Windows.Forms.GroupBox StatBoosterItemExplain;
        private System.Windows.Forms.Label StatBoosterItemLink;
    }
}