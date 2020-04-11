namespace FEBuilderGBA
{
    partial class ItemShopForm
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
            this.J_0_ITEM = new System.Windows.Forms.Label();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.W = new System.Windows.Forms.Panel();
            this.L_0_ITEMICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label14 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label15 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton_21 = new System.Windows.Forms.Button();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.J_1 = new System.Windows.Forms.Label();
            this.L_0_ITEM = new FEBuilderGBA.TextBoxEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SHOP_LIST = new FEBuilderGBA.ListBoxEx();
            this.label30 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.W.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_ITEMICON)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // J_0_ITEM
            // 
            this.J_0_ITEM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_ITEM.Location = new System.Drawing.Point(282, 90);
            this.J_0_ITEM.Name = "J_0_ITEM";
            this.J_0_ITEM.Size = new System.Drawing.Size(90, 30);
            this.J_0_ITEM.TabIndex = 57;
            this.J_0_ITEM.Text = "アイテム ";
            this.J_0_ITEM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(380, 90);
            this.B0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(63, 25);
            this.B0.TabIndex = 30;
            // 
            // W
            // 
            this.W.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.W.Controls.Add(this.L_0_ITEMICON);
            this.W.Controls.Add(this.panel5);
            this.W.Controls.Add(this.B1);
            this.W.Controls.Add(this.panel2);
            this.W.Controls.Add(this.J_1);
            this.W.Controls.Add(this.J_0_ITEM);
            this.W.Controls.Add(this.B0);
            this.W.Controls.Add(this.L_0_ITEM);
            this.W.Location = new System.Drawing.Point(384, 42);
            this.W.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.W.Name = "W";
            this.W.Size = new System.Drawing.Size(794, 589);
            this.W.TabIndex = 94;
            // 
            // L_0_ITEMICON
            // 
            this.L_0_ITEMICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_ITEMICON.Location = new System.Drawing.Point(616, 91);
            this.L_0_ITEMICON.Name = "L_0_ITEMICON";
            this.L_0_ITEMICON.Size = new System.Drawing.Size(32, 32);
            this.L_0_ITEMICON.TabIndex = 181;
            this.L_0_ITEMICON.TabStop = false;
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
            this.panel5.Location = new System.Drawing.Point(282, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(510, 76);
            this.panel5.TabIndex = 96;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(97, 45);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(-1, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 30);
            this.label14.TabIndex = 52;
            this.label14.Text = "Size:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(324, 46);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(169, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Location = new System.Drawing.Point(194, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(122, 30);
            this.label15.TabIndex = 39;
            this.label15.Text = "選択アドレス:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(342, 1);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(97, 3);
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
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(90, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "アドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B1
            // 
            this.B1.Location = new System.Drawing.Point(380, 120);
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(63, 25);
            this.B1.TabIndex = 61;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.AddressListExpandsButton_21);
            this.panel2.Controls.Add(this.AddressList);
            this.panel2.Controls.Add(this.LabelFilter);
            this.panel2.Location = new System.Drawing.Point(-1, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(280, 586);
            this.panel2.TabIndex = 180;
            // 
            // AddressListExpandsButton_21
            // 
            this.AddressListExpandsButton_21.Location = new System.Drawing.Point(1, 553);
            this.AddressListExpandsButton_21.Name = "AddressListExpandsButton_21";
            this.AddressListExpandsButton_21.Size = new System.Drawing.Size(270, 30);
            this.AddressListExpandsButton_21.TabIndex = 146;
            this.AddressListExpandsButton_21.Text = "リストの拡張";
            this.AddressListExpandsButton_21.UseVisualStyleBackColor = true;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 25);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(279, 526);
            this.AddressList.TabIndex = 108;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(279, 26);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // J_1
            // 
            this.J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_1.Location = new System.Drawing.Point(282, 119);
            this.J_1.Name = "J_1";
            this.J_1.Size = new System.Drawing.Size(90, 30);
            this.J_1.TabIndex = 60;
            this.J_1.Text = "00";
            this.J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_0_ITEM
            // 
            this.L_0_ITEM.ErrorMessage = "";
            this.L_0_ITEM.Location = new System.Drawing.Point(451, 91);
            this.L_0_ITEM.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_0_ITEM.Name = "L_0_ITEM";
            this.L_0_ITEM.Placeholder = "";
            this.L_0_ITEM.ReadOnly = true;
            this.L_0_ITEM.Size = new System.Drawing.Size(156, 25);
            this.L_0_ITEM.TabIndex = 55;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SHOP_LIST);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(367, 620);
            this.panel1.TabIndex = 95;
            // 
            // SHOP_LIST
            // 
            this.SHOP_LIST.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SHOP_LIST.FormattingEnabled = true;
            this.SHOP_LIST.IntegralHeight = false;
            this.SHOP_LIST.ItemHeight = 18;
            this.SHOP_LIST.Location = new System.Drawing.Point(-1, 25);
            this.SHOP_LIST.Margin = new System.Windows.Forms.Padding(4);
            this.SHOP_LIST.Name = "SHOP_LIST";
            this.SHOP_LIST.Size = new System.Drawing.Size(367, 580);
            this.SHOP_LIST.TabIndex = 108;
            this.SHOP_LIST.SelectedIndexChanged += new System.EventHandler(this.SHOP_LIST_SelectedIndexChanged);
            // 
            // label30
            // 
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Location = new System.Drawing.Point(-1, -1);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(367, 26);
            this.label30.TabIndex = 107;
            this.label30.Text = "店の名前";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(384, 12);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(793, 30);
            this.panel3.TabIndex = 97;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(507, -1);
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
            this.label9.Location = new System.Drawing.Point(282, -1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 30);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(379, 3);
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
            this.ReadStartAddress.Location = new System.Drawing.Point(149, 2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // ItemShopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1191, 644);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.W);
            this.Name = "ItemShopForm";
            this.Text = "お店";
            this.Load += new System.EventHandler(this.ItemShopForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.W.ResumeLayout(false);
            this.W.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_ITEMICON)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label J_0_ITEM;
        private FEBuilderGBA.TextBoxEx L_0_ITEM;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.Panel W;
        private System.Windows.Forms.Label J_1;
        private System.Windows.Forms.NumericUpDown B1;
        private System.Windows.Forms.Panel panel1;
        private ListBoxEx SHOP_LIST;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Panel panel5;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label14;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel2;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label LabelFilter;
        private System.Windows.Forms.Button AddressListExpandsButton_21;
        private InterpolatedPictureBox L_0_ITEMICON;
    }
}