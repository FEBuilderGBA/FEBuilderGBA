namespace FEBuilderGBA
{
    partial class ItemEffectivenessSkillSystemsReworkForm
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
            this.label10 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.N_ReloadListButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.N_L_2_CLASSTYPEICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.N_W2 = new System.Windows.Forms.NumericUpDown();
            this.N_J_2_CLASSTYPE = new System.Windows.Forms.Label();
            this.N_J_1 = new System.Windows.Forms.Label();
            this.N_B1 = new System.Windows.Forms.NumericUpDown();
            this.N_J_0 = new System.Windows.Forms.Label();
            this.N_L_2_CLASSTYPE = new FEBuilderGBA.TextBoxEx();
            this.N_B0 = new System.Windows.Forms.NumericUpDown();
            this.panel6 = new System.Windows.Forms.Panel();
            this.N_BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label6 = new System.Windows.Forms.Label();
            this.N_SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.N_WriteButton = new System.Windows.Forms.Button();
            this.N_Address = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.N_AddressListExpandsButton = new System.Windows.Forms.Button();
            this.N_AddressList = new FEBuilderGBA.ListBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.ItemListBox = new FEBuilderGBA.ListBoxEx();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_L_2_CLASSTYPEICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_W2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B0)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Address)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(716, 72);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(220, 26);
            this.label10.TabIndex = 109;
            this.label10.Text = "該当アイテム";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.N_ReloadListButton);
            this.panel5.Controls.Add(this.Address);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(336, 42);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(942, 30);
            this.panel5.TabIndex = 92;
            // 
            // N_ReloadListButton
            // 
            this.N_ReloadListButton.Location = new System.Drawing.Point(409, 0);
            this.N_ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_ReloadListButton.Name = "N_ReloadListButton";
            this.N_ReloadListButton.Size = new System.Drawing.Size(189, 30);
            this.N_ReloadListButton.TabIndex = 26;
            this.N_ReloadListButton.Text = "特効再取得";
            this.N_ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(273, 2);
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
            this.label16.Location = new System.Drawing.Point(-1, -1);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(266, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "特効ポインタアドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-3, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(326, 30);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LabelFilter);
            this.panel1.Controls.Add(this.AddressList);
            this.panel1.Location = new System.Drawing.Point(14, 43);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(322, 833);
            this.panel1.TabIndex = 91;
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
            this.AddressList.Size = new System.Drawing.Size(324, 800);
            this.AddressList.TabIndex = 108;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Controls.Add(this.ItemListBox);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Location = new System.Drawing.Point(336, 76);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(942, 801);
            this.panel4.TabIndex = 90;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.N_L_2_CLASSTYPEICON);
            this.panel7.Controls.Add(this.N_W2);
            this.panel7.Controls.Add(this.N_J_2_CLASSTYPE);
            this.panel7.Controls.Add(this.N_J_1);
            this.panel7.Controls.Add(this.N_B1);
            this.panel7.Controls.Add(this.N_J_0);
            this.panel7.Controls.Add(this.N_L_2_CLASSTYPE);
            this.panel7.Controls.Add(this.N_B0);
            this.panel7.Location = new System.Drawing.Point(269, 73);
            this.panel7.Margin = new System.Windows.Forms.Padding(2);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(444, 724);
            this.panel7.TabIndex = 179;
            // 
            // N_L_2_CLASSTYPEICON
            // 
            this.N_L_2_CLASSTYPEICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.N_L_2_CLASSTYPEICON.Location = new System.Drawing.Point(297, 71);
            this.N_L_2_CLASSTYPEICON.Margin = new System.Windows.Forms.Padding(2);
            this.N_L_2_CLASSTYPEICON.Name = "N_L_2_CLASSTYPEICON";
            this.N_L_2_CLASSTYPEICON.Size = new System.Drawing.Size(32, 32);
            this.N_L_2_CLASSTYPEICON.TabIndex = 110;
            this.N_L_2_CLASSTYPEICON.TabStop = false;
            // 
            // N_W2
            // 
            this.N_W2.Location = new System.Drawing.Point(186, 76);
            this.N_W2.Margin = new System.Windows.Forms.Padding(2);
            this.N_W2.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.N_W2.Name = "N_W2";
            this.N_W2.Size = new System.Drawing.Size(89, 25);
            this.N_W2.TabIndex = 109;
            // 
            // N_J_2_CLASSTYPE
            // 
            this.N_J_2_CLASSTYPE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_2_CLASSTYPE.Location = new System.Drawing.Point(4, 71);
            this.N_J_2_CLASSTYPE.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_2_CLASSTYPE.Name = "N_J_2_CLASSTYPE";
            this.N_J_2_CLASSTYPE.Size = new System.Drawing.Size(178, 30);
            this.N_J_2_CLASSTYPE.TabIndex = 108;
            this.N_J_2_CLASSTYPE.Text = "ClassType";
            this.N_J_2_CLASSTYPE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_J_1
            // 
            this.N_J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_1.Location = new System.Drawing.Point(4, 40);
            this.N_J_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_1.Name = "N_J_1";
            this.N_J_1.Size = new System.Drawing.Size(178, 30);
            this.N_J_1.TabIndex = 107;
            this.N_J_1.Text = "coefficient_times*2";
            this.N_J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_B1
            // 
            this.N_B1.Location = new System.Drawing.Point(186, 45);
            this.N_B1.Margin = new System.Windows.Forms.Padding(2);
            this.N_B1.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.N_B1.Name = "N_B1";
            this.N_B1.Size = new System.Drawing.Size(62, 25);
            this.N_B1.TabIndex = 105;
            // 
            // N_J_0
            // 
            this.N_J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_0.Location = new System.Drawing.Point(4, 8);
            this.N_J_0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_0.Name = "N_J_0";
            this.N_J_0.Size = new System.Drawing.Size(178, 30);
            this.N_J_0.TabIndex = 57;
            this.N_J_0.Text = "00";
            this.N_J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_L_2_CLASSTYPE
            // 
            this.N_L_2_CLASSTYPE.ErrorMessage = "";
            this.N_L_2_CLASSTYPE.Location = new System.Drawing.Point(159, 105);
            this.N_L_2_CLASSTYPE.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_L_2_CLASSTYPE.Name = "N_L_2_CLASSTYPE";
            this.N_L_2_CLASSTYPE.Placeholder = "";
            this.N_L_2_CLASSTYPE.ReadOnly = true;
            this.N_L_2_CLASSTYPE.Size = new System.Drawing.Size(280, 25);
            this.N_L_2_CLASSTYPE.TabIndex = 55;
            // 
            // N_B0
            // 
            this.N_B0.Hexadecimal = true;
            this.N_B0.Location = new System.Drawing.Point(186, 13);
            this.N_B0.Margin = new System.Windows.Forms.Padding(2);
            this.N_B0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.N_B0.Name = "N_B0";
            this.N_B0.Size = new System.Drawing.Size(62, 25);
            this.N_B0.TabIndex = 30;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.N_BlockSize);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Controls.Add(this.N_SelectAddress);
            this.panel6.Controls.Add(this.label22);
            this.panel6.Controls.Add(this.N_WriteButton);
            this.panel6.Controls.Add(this.N_Address);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Location = new System.Drawing.Point(269, 2);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(672, 64);
            this.panel6.TabIndex = 178;
            // 
            // N_BlockSize
            // 
            this.N_BlockSize.ErrorMessage = "";
            this.N_BlockSize.Location = new System.Drawing.Point(91, 34);
            this.N_BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_BlockSize.Name = "N_BlockSize";
            this.N_BlockSize.Placeholder = "";
            this.N_BlockSize.ReadOnly = true;
            this.N_BlockSize.Size = new System.Drawing.Size(72, 25);
            this.N_BlockSize.TabIndex = 55;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(0, 31);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 30);
            this.label6.TabIndex = 56;
            this.label6.Text = "Size:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_SelectAddress
            // 
            this.N_SelectAddress.ErrorMessage = "";
            this.N_SelectAddress.Location = new System.Drawing.Point(307, 32);
            this.N_SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_SelectAddress.Name = "N_SelectAddress";
            this.N_SelectAddress.Placeholder = "";
            this.N_SelectAddress.ReadOnly = true;
            this.N_SelectAddress.Size = new System.Drawing.Size(136, 25);
            this.N_SelectAddress.TabIndex = 54;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(177, 32);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 53;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_WriteButton
            // 
            this.N_WriteButton.Location = new System.Drawing.Point(449, 27);
            this.N_WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_WriteButton.Name = "N_WriteButton";
            this.N_WriteButton.Size = new System.Drawing.Size(214, 30);
            this.N_WriteButton.TabIndex = 9;
            this.N_WriteButton.Text = "書き込み";
            this.N_WriteButton.UseVisualStyleBackColor = true;
            // 
            // N_Address
            // 
            this.N_Address.Hexadecimal = true;
            this.N_Address.Location = new System.Drawing.Point(91, 2);
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
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "アドレス";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.N_AddressListExpandsButton);
            this.panel2.Controls.Add(this.N_AddressList);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(1, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(264, 795);
            this.panel2.TabIndex = 111;
            // 
            // N_AddressListExpandsButton
            // 
            this.N_AddressListExpandsButton.Location = new System.Drawing.Point(2, 762);
            this.N_AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_AddressListExpandsButton.Name = "N_AddressListExpandsButton";
            this.N_AddressListExpandsButton.Size = new System.Drawing.Size(256, 30);
            this.N_AddressListExpandsButton.TabIndex = 145;
            this.N_AddressListExpandsButton.Text = "リストの拡張";
            this.N_AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // N_AddressList
            // 
            this.N_AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.N_AddressList.FormattingEnabled = true;
            this.N_AddressList.IntegralHeight = false;
            this.N_AddressList.ItemHeight = 18;
            this.N_AddressList.Location = new System.Drawing.Point(-1, 33);
            this.N_AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.N_AddressList.Name = "N_AddressList";
            this.N_AddressList.Size = new System.Drawing.Size(262, 722);
            this.N_AddressList.TabIndex = 108;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -1);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 33);
            this.label1.TabIndex = 107;
            this.label1.Text = "特効クラス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ItemListBox
            // 
            this.ItemListBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ItemListBox.FormattingEnabled = true;
            this.ItemListBox.IntegralHeight = false;
            this.ItemListBox.ItemHeight = 18;
            this.ItemListBox.Location = new System.Drawing.Point(716, 97);
            this.ItemListBox.Margin = new System.Windows.Forms.Padding(4);
            this.ItemListBox.Name = "ItemListBox";
            this.ItemListBox.Size = new System.Drawing.Size(220, 698);
            this.ItemListBox.TabIndex = 110;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(149, 2);
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
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-1, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(146, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(320, -2);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(107, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(14, 13);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1264, 30);
            this.panel3.TabIndex = 93;
            // 
            // ItemEffectivenessSkillSystemsReworkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1281, 880);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ItemEffectivenessSkillSystemsReworkForm";
            this.Text = "特効指定";
            this.Load += new System.EventHandler(this.ItemCriticalForm_Load);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_L_2_CLASSTYPEICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_W2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B0)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Address)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListBoxEx ItemListBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private ListBoxEx N_AddressList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel6;
        private FEBuilderGBA.TextBoxEx N_BlockSize;
        private System.Windows.Forms.Label label6;
        private FEBuilderGBA.TextBoxEx N_SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button N_WriteButton;
        private System.Windows.Forms.NumericUpDown N_Address;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label N_J_0;
        private FEBuilderGBA.TextBoxEx N_L_2_CLASSTYPE;
        private System.Windows.Forms.NumericUpDown N_B0;
        private System.Windows.Forms.Button N_ReloadListButton;
        private System.Windows.Forms.Button N_AddressListExpandsButton;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label N_J_1;
        private System.Windows.Forms.NumericUpDown N_B1;
        private System.Windows.Forms.Label N_J_2_CLASSTYPE;
        private System.Windows.Forms.NumericUpDown N_W2;
        private InterpolatedPictureBox N_L_2_CLASSTYPEICON;
    }
}