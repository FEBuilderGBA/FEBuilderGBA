namespace FEBuilderGBA
{
    partial class ImageBattleTerrainForm
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
            this.J_16 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton_255 = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.GraphicsToolButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.D12 = new System.Windows.Forms.NumericUpDown();
            this.D16 = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.X_REF = new FEBuilderGBA.ListBoxEx();
            this.Comment = new FEBuilderGBA.TextBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.B11 = new System.Windows.Forms.NumericUpDown();
            this.B10 = new System.Windows.Forms.NumericUpDown();
            this.B9 = new System.Windows.Forms.NumericUpDown();
            this.B8 = new System.Windows.Forms.NumericUpDown();
            this.B7 = new System.Windows.Forms.NumericUpDown();
            this.B6 = new System.Windows.Forms.NumericUpDown();
            this.B5 = new System.Windows.Forms.NumericUpDown();
            this.B4 = new System.Windows.Forms.NumericUpDown();
            this.B3 = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.L_0_SPLITSTRING_11 = new FEBuilderGBA.TextBoxEx();
            this.J_0 = new System.Windows.Forms.Label();
            this.D20 = new System.Windows.Forms.NumericUpDown();
            this.J_20 = new System.Windows.Forms.Label();
            this.J_12 = new System.Windows.Forms.Label();
            this.X_BG_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D16)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_BG_PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.AddressPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(449, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(107, 31);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // J_16
            // 
            this.J_16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_16.Location = new System.Drawing.Point(6, 193);
            this.J_16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_16.Name = "J_16";
            this.J_16.Size = new System.Drawing.Size(148, 31);
            this.J_16.TabIndex = 69;
            this.J_16.Text = "パレット";
            this.J_16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton_255);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(14, 43);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(252, 727);
            this.panel6.TabIndex = 76;
            // 
            // AddressListExpandsButton_255
            // 
            this.AddressListExpandsButton_255.Location = new System.Drawing.Point(-1, 693);
            this.AddressListExpandsButton_255.Name = "AddressListExpandsButton_255";
            this.AddressListExpandsButton_255.Size = new System.Drawing.Size(244, 30);
            this.AddressListExpandsButton_255.TabIndex = 114;
            this.AddressListExpandsButton_255.Text = "リストの拡張";
            this.AddressListExpandsButton_255.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, 1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(252, 26);
            this.LabelFilter.TabIndex = 55;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 27);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(252, 659);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(299, -1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(80, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.GraphicsToolButton);
            this.panel3.Controls.Add(this.ImportButton);
            this.panel3.Controls.Add(this.ExportButton);
            this.panel3.Location = new System.Drawing.Point(271, 727);
            this.panel3.Margin = new System.Windows.Forms.Padding(5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(953, 43);
            this.panel3.TabIndex = 78;
            // 
            // GraphicsToolButton
            // 
            this.GraphicsToolButton.Location = new System.Drawing.Point(696, 8);
            this.GraphicsToolButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.GraphicsToolButton.Name = "GraphicsToolButton";
            this.GraphicsToolButton.Size = new System.Drawing.Size(212, 30);
            this.GraphicsToolButton.TabIndex = 204;
            this.GraphicsToolButton.Text = "グラフィックツール";
            this.GraphicsToolButton.UseVisualStyleBackColor = true;
            this.GraphicsToolButton.Click += new System.EventHandler(this.GraphicsToolButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(8, 9);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(160, 30);
            this.ImportButton.TabIndex = 65;
            this.ImportButton.Text = "画像読込";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(199, 9);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(160, 30);
            this.ExportButton.TabIndex = 64;
            this.ExportButton.Text = "画像取出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(510, -2);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(132, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(391, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(117, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -1);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 32);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(258, -1);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 32);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(14, 14);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1209, 30);
            this.panel1.TabIndex = 77;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(348, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(76, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(118, 2);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(125, 25);
            this.ReadStartAddress.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(225, -1);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, 0);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(83, 32);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // D12
            // 
            this.D12.Hexadecimal = true;
            this.D12.Location = new System.Drawing.Point(160, 158);
            this.D12.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.D12.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D12.Name = "D12";
            this.D12.Size = new System.Drawing.Size(125, 25);
            this.D12.TabIndex = 0;
            // 
            // D16
            // 
            this.D16.Hexadecimal = true;
            this.D16.Location = new System.Drawing.Point(160, 193);
            this.D16.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.D16.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D16.Name = "D16";
            this.D16.Size = new System.Drawing.Size(125, 25);
            this.D16.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.X_REF);
            this.panel2.Controls.Add(this.Comment);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.B11);
            this.panel2.Controls.Add(this.B10);
            this.panel2.Controls.Add(this.B9);
            this.panel2.Controls.Add(this.B8);
            this.panel2.Controls.Add(this.B7);
            this.panel2.Controls.Add(this.B6);
            this.panel2.Controls.Add(this.B5);
            this.panel2.Controls.Add(this.B4);
            this.panel2.Controls.Add(this.B3);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.B2);
            this.panel2.Controls.Add(this.B1);
            this.panel2.Controls.Add(this.B0);
            this.panel2.Controls.Add(this.L_0_SPLITSTRING_11);
            this.panel2.Controls.Add(this.J_0);
            this.panel2.Controls.Add(this.D12);
            this.panel2.Controls.Add(this.D16);
            this.panel2.Controls.Add(this.D20);
            this.panel2.Controls.Add(this.J_20);
            this.panel2.Controls.Add(this.J_16);
            this.panel2.Controls.Add(this.J_12);
            this.panel2.Controls.Add(this.X_BG_PIC);
            this.panel2.Location = new System.Drawing.Point(273, 72);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(950, 645);
            this.panel2.TabIndex = 74;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(692, 16);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(257, 26);
            this.label8.TabIndex = 201;
            this.label8.Text = "参照箇所";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // X_REF
            // 
            this.X_REF.FormattingEnabled = true;
            this.X_REF.IntegralHeight = false;
            this.X_REF.ItemHeight = 18;
            this.X_REF.Location = new System.Drawing.Point(694, 41);
            this.X_REF.Margin = new System.Windows.Forms.Padding(4);
            this.X_REF.Name = "X_REF";
            this.X_REF.Size = new System.Drawing.Size(256, 603);
            this.X_REF.TabIndex = 199;
            this.X_REF.KeyDown += new System.Windows.Forms.KeyEventHandler(this.X_REF_KeyDown);
            this.X_REF.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.X_REF_MouseDoubleClick);
            // 
            // Comment
            // 
            this.Comment.ErrorMessage = "";
            this.Comment.Location = new System.Drawing.Point(161, 284);
            this.Comment.Name = "Comment";
            this.Comment.Placeholder = "";
            this.Comment.Size = new System.Drawing.Size(290, 25);
            this.Comment.TabIndex = 198;
            // 
            // label4
            // 
            this.label4.AccessibleDescription = "@COMMENT";
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(6, 281);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 31);
            this.label4.TabIndex = 197;
            this.label4.Text = "コメント";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B11
            // 
            this.B11.Hexadecimal = true;
            this.B11.Location = new System.Drawing.Point(194, 118);
            this.B11.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B11.Name = "B11";
            this.B11.Size = new System.Drawing.Size(60, 25);
            this.B11.TabIndex = 191;
            // 
            // B10
            // 
            this.B10.Hexadecimal = true;
            this.B10.Location = new System.Drawing.Point(126, 118);
            this.B10.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B10.Name = "B10";
            this.B10.Size = new System.Drawing.Size(60, 25);
            this.B10.TabIndex = 190;
            // 
            // B9
            // 
            this.B9.Hexadecimal = true;
            this.B9.Location = new System.Drawing.Point(389, 87);
            this.B9.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B9.Name = "B9";
            this.B9.Size = new System.Drawing.Size(60, 25);
            this.B9.TabIndex = 189;
            // 
            // B8
            // 
            this.B8.Hexadecimal = true;
            this.B8.Location = new System.Drawing.Point(327, 87);
            this.B8.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B8.Name = "B8";
            this.B8.Size = new System.Drawing.Size(60, 25);
            this.B8.TabIndex = 188;
            // 
            // B7
            // 
            this.B7.Hexadecimal = true;
            this.B7.Location = new System.Drawing.Point(261, 87);
            this.B7.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B7.Name = "B7";
            this.B7.Size = new System.Drawing.Size(60, 25);
            this.B7.TabIndex = 187;
            // 
            // B6
            // 
            this.B6.Hexadecimal = true;
            this.B6.Location = new System.Drawing.Point(193, 87);
            this.B6.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B6.Name = "B6";
            this.B6.Size = new System.Drawing.Size(60, 25);
            this.B6.TabIndex = 186;
            // 
            // B5
            // 
            this.B5.Hexadecimal = true;
            this.B5.Location = new System.Drawing.Point(125, 87);
            this.B5.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B5.Name = "B5";
            this.B5.Size = new System.Drawing.Size(60, 25);
            this.B5.TabIndex = 185;
            // 
            // B4
            // 
            this.B4.Hexadecimal = true;
            this.B4.Location = new System.Drawing.Point(391, 56);
            this.B4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B4.Name = "B4";
            this.B4.Size = new System.Drawing.Size(60, 25);
            this.B4.TabIndex = 184;
            // 
            // B3
            // 
            this.B3.Hexadecimal = true;
            this.B3.Location = new System.Drawing.Point(329, 56);
            this.B3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(60, 25);
            this.B3.TabIndex = 183;
            // 
            // label21
            // 
            this.label21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label21.Location = new System.Drawing.Point(328, 16);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(130, 31);
            this.label21.TabIndex = 182;
            this.label21.Text = "↓文字列内訳";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B2
            // 
            this.B2.Hexadecimal = true;
            this.B2.Location = new System.Drawing.Point(263, 56);
            this.B2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(60, 25);
            this.B2.TabIndex = 181;
            // 
            // B1
            // 
            this.B1.Hexadecimal = true;
            this.B1.Location = new System.Drawing.Point(195, 56);
            this.B1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(60, 25);
            this.B1.TabIndex = 180;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(125, 56);
            this.B0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(60, 25);
            this.B0.TabIndex = 179;
            // 
            // L_0_SPLITSTRING_11
            // 
            this.L_0_SPLITSTRING_11.ErrorMessage = "";
            this.L_0_SPLITSTRING_11.Location = new System.Drawing.Point(128, 20);
            this.L_0_SPLITSTRING_11.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_0_SPLITSTRING_11.Name = "L_0_SPLITSTRING_11";
            this.L_0_SPLITSTRING_11.Placeholder = "";
            this.L_0_SPLITSTRING_11.Size = new System.Drawing.Size(191, 25);
            this.L_0_SPLITSTRING_11.TabIndex = 178;
            // 
            // J_0
            // 
            this.J_0.AccessibleDescription = "@INNERNAME";
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(6, 18);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(115, 30);
            this.J_0.TabIndex = 177;
            this.J_0.Text = "識別子";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // D20
            // 
            this.D20.Hexadecimal = true;
            this.D20.Location = new System.Drawing.Point(162, 234);
            this.D20.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.D20.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.D20.Name = "D20";
            this.D20.Size = new System.Drawing.Size(125, 25);
            this.D20.TabIndex = 2;
            // 
            // J_20
            // 
            this.J_20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_20.Location = new System.Drawing.Point(6, 234);
            this.J_20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_20.Name = "J_20";
            this.J_20.Size = new System.Drawing.Size(148, 31);
            this.J_20.TabIndex = 70;
            this.J_20.Text = "00";
            this.J_20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_12
            // 
            this.J_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_12.Location = new System.Drawing.Point(6, 154);
            this.J_12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_12.Name = "J_12";
            this.J_12.Size = new System.Drawing.Size(148, 31);
            this.J_12.TabIndex = 68;
            this.J_12.Text = "画像";
            this.J_12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_BG_PIC
            // 
            this.X_BG_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_BG_PIC.Location = new System.Drawing.Point(6, 350);
            this.X_BG_PIC.Margin = new System.Windows.Forms.Padding(5);
            this.X_BG_PIC.Name = "X_BG_PIC";
            this.X_BG_PIC.Size = new System.Drawing.Size(685, 288);
            this.X_BG_PIC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.X_BG_PIC.TabIndex = 67;
            this.X_BG_PIC.TabStop = false;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(696, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(251, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(89, 4);
            this.Address.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(125, 25);
            this.Address.TabIndex = 0;
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
            this.AddressPanel.Location = new System.Drawing.Point(273, 43);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(951, 30);
            this.AddressPanel.TabIndex = 75;
            // 
            // ImageBattleTerrainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1235, 773);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.AddressPanel);
            this.Name = "ImageBattleTerrainForm";
            this.Text = "戦闘地形";
            this.Load += new System.EventHandler(this.ImageBattleTerrainForm_Load);
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D16)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_BG_PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label J_16;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button AddressListExpandsButton_255;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Panel panel3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown D12;
        private System.Windows.Forms.NumericUpDown D16;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown D20;
        private System.Windows.Forms.Label J_20;
        private System.Windows.Forms.Label J_12;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.NumericUpDown B1;
        private System.Windows.Forms.NumericUpDown B0;
        private FEBuilderGBA.TextBoxEx L_0_SPLITSTRING_11;
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.NumericUpDown B11;
        private System.Windows.Forms.NumericUpDown B10;
        private System.Windows.Forms.NumericUpDown B9;
        private System.Windows.Forms.NumericUpDown B8;
        private System.Windows.Forms.NumericUpDown B7;
        private System.Windows.Forms.NumericUpDown B6;
        private System.Windows.Forms.NumericUpDown B5;
        private System.Windows.Forms.NumericUpDown B4;
        private System.Windows.Forms.NumericUpDown B3;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportButton;
        private InterpolatedPictureBox X_BG_PIC;
        private TextBoxEx Comment;
        private System.Windows.Forms.Label label4;
        private ListBoxEx X_REF;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button GraphicsToolButton;
    }
}