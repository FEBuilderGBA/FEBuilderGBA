namespace FEBuilderGBA
{
    partial class ItemWeaponTriangleForm
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
            this.WriteButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.L_1_WEAPONTYPEICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.L_0_WEAPONTYPEICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.J_1 = new System.Windows.Forms.Label();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.L_1_COMBO = new System.Windows.Forms.ComboBox();
            this.J_0 = new System.Windows.Forms.Label();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.L_0_COMBO = new System.Windows.Forms.ComboBox();
            this.b3 = new System.Windows.Forms.NumericUpDown();
            this.b2 = new System.Windows.Forms.NumericUpDown();
            this.J_3 = new System.Windows.Forms.Label();
            this.J_2 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_1_WEAPONTYPEICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_WEAPONTYPEICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.b3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.b2)).BeginInit();
            this.panel3.SuspendLayout();
            this.AddressPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(738, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(168, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(14, 13);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1255, 30);
            this.panel1.TabIndex = 85;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(568, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -1);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(347, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(438, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(215, 2);
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
            this.Address.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.L_1_WEAPONTYPEICON);
            this.panel2.Controls.Add(this.L_0_WEAPONTYPEICON);
            this.panel2.Controls.Add(this.J_1);
            this.panel2.Controls.Add(this.B1);
            this.panel2.Controls.Add(this.L_1_COMBO);
            this.panel2.Controls.Add(this.J_0);
            this.panel2.Controls.Add(this.B0);
            this.panel2.Controls.Add(this.L_0_COMBO);
            this.panel2.Controls.Add(this.b3);
            this.panel2.Controls.Add(this.b2);
            this.panel2.Controls.Add(this.J_3);
            this.panel2.Controls.Add(this.J_2);
            this.panel2.Location = new System.Drawing.Point(362, 71);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(907, 528);
            this.panel2.TabIndex = 83;
            // 
            // L_1_WEAPONTYPEICON
            // 
            this.L_1_WEAPONTYPEICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_1_WEAPONTYPEICON.Location = new System.Drawing.Point(498, 47);
            this.L_1_WEAPONTYPEICON.Name = "L_1_WEAPONTYPEICON";
            this.L_1_WEAPONTYPEICON.Size = new System.Drawing.Size(32, 32);
            this.L_1_WEAPONTYPEICON.TabIndex = 49;
            this.L_1_WEAPONTYPEICON.TabStop = false;
            // 
            // L_0_WEAPONTYPEICON
            // 
            this.L_0_WEAPONTYPEICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_WEAPONTYPEICON.Location = new System.Drawing.Point(498, 13);
            this.L_0_WEAPONTYPEICON.Name = "L_0_WEAPONTYPEICON";
            this.L_0_WEAPONTYPEICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_WEAPONTYPEICON.TabIndex = 48;
            this.L_0_WEAPONTYPEICON.TabStop = false;
            // 
            // J_1
            // 
            this.J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_1.Location = new System.Drawing.Point(0, 41);
            this.J_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_1.Name = "J_1";
            this.J_1.Size = new System.Drawing.Size(244, 32);
            this.J_1.TabIndex = 35;
            this.J_1.Text = "防御武器";
            this.J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B1
            // 
            this.B1.Hexadecimal = true;
            this.B1.Location = new System.Drawing.Point(250, 46);
            this.B1.Margin = new System.Windows.Forms.Padding(2);
            this.B1.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(62, 25);
            this.B1.TabIndex = 36;
            // 
            // L_1_COMBO
            // 
            this.L_1_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_1_COMBO.FormattingEnabled = true;
            this.L_1_COMBO.Items.AddRange(new object[] {
            "00=剣",
            "01=槍",
            "02=斧",
            "03=弓",
            "04=杖",
            "05=理",
            "06=光",
            "07=闇",
            "09=道具",
            "0B=竜石・魔物専用",
            "0C=指輪",
            "11=火竜石",
            "12=踊る用指輪"});
            this.L_1_COMBO.Location = new System.Drawing.Point(319, 46);
            this.L_1_COMBO.Margin = new System.Windows.Forms.Padding(2);
            this.L_1_COMBO.Name = "L_1_COMBO";
            this.L_1_COMBO.Size = new System.Drawing.Size(174, 26);
            this.L_1_COMBO.TabIndex = 37;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(0, 10);
            this.J_0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(244, 32);
            this.J_0.TabIndex = 32;
            this.J_0.Text = "攻撃武器";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(250, 14);
            this.B0.Margin = new System.Windows.Forms.Padding(2);
            this.B0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(62, 25);
            this.B0.TabIndex = 33;
            // 
            // L_0_COMBO
            // 
            this.L_0_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_0_COMBO.FormattingEnabled = true;
            this.L_0_COMBO.Items.AddRange(new object[] {
            "00=剣",
            "01=槍",
            "02=斧",
            "03=弓",
            "04=杖",
            "05=理",
            "06=光",
            "07=闇",
            "09=道具",
            "0B=竜石・魔物専用",
            "0C=指輪",
            "11=火竜石",
            "12=踊る用指輪"});
            this.L_0_COMBO.Location = new System.Drawing.Point(319, 14);
            this.L_0_COMBO.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_COMBO.Name = "L_0_COMBO";
            this.L_0_COMBO.Size = new System.Drawing.Size(174, 26);
            this.L_0_COMBO.TabIndex = 34;
            // 
            // b3
            // 
            this.b3.Location = new System.Drawing.Point(689, 48);
            this.b3.Margin = new System.Windows.Forms.Padding(2);
            this.b3.Name = "b3";
            this.b3.Size = new System.Drawing.Size(78, 25);
            this.b3.TabIndex = 30;
            // 
            // b2
            // 
            this.b2.Location = new System.Drawing.Point(689, 13);
            this.b2.Margin = new System.Windows.Forms.Padding(2);
            this.b2.Name = "b2";
            this.b2.Size = new System.Drawing.Size(78, 25);
            this.b2.TabIndex = 29;
            // 
            // J_3
            // 
            this.J_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_3.Location = new System.Drawing.Point(548, 47);
            this.J_3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_3.Name = "J_3";
            this.J_3.Size = new System.Drawing.Size(136, 30);
            this.J_3.TabIndex = 24;
            this.J_3.Text = "攻撃補正";
            this.J_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(548, 14);
            this.J_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(136, 32);
            this.J_2.TabIndex = 18;
            this.J_2.Text = "命中補正";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, -1);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(84, 30);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 24);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(347, 502);
            this.AddressList.TabIndex = 108;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.AddressListExpandsButton);
            this.panel3.Controls.Add(this.AddressList);
            this.panel3.Controls.Add(this.LabelFilter);
            this.panel3.Location = new System.Drawing.Point(14, 44);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(347, 556);
            this.panel3.TabIndex = 86;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(-1, 526);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(344, 30);
            this.AddressListExpandsButton.TabIndex = 145;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(0, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(346, 26);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(424, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(548, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(150, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(335, 2);
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
            this.label3.Location = new System.Drawing.Point(250, -1);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
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
            this.AddressPanel.Controls.Add(this.label23);
            this.AddressPanel.Location = new System.Drawing.Point(362, 42);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(907, 30);
            this.AddressPanel.TabIndex = 84;
            // 
            // ItemWeaponTriangleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1274, 610);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.AddressPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ItemWeaponTriangleForm";
            this.Text = "すくみ";
            this.Load += new System.EventHandler(this.ItemWeaponTriangleForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.L_1_WEAPONTYPEICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_WEAPONTYPEICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.b3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.b2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown b3;
        private System.Windows.Forms.NumericUpDown b2;
        private System.Windows.Forms.Label J_3;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.Label label23;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label LabelFilter;
        private System.Windows.Forms.Label label22;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.Label J_1;
        private System.Windows.Forms.NumericUpDown B1;
        private System.Windows.Forms.ComboBox L_1_COMBO;
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.ComboBox L_0_COMBO;
        private InterpolatedPictureBox L_0_WEAPONTYPEICON;
        private InterpolatedPictureBox L_1_WEAPONTYPEICON;
        private System.Windows.Forms.Button AddressListExpandsButton;
    }
}