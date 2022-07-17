namespace FEBuilderGBA
{
    partial class SkillConfigSkillSystemForm
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
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
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
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton_255 = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.IconAddr = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ExportAllButton = new System.Windows.Forms.Button();
            this.ImportAllButton = new System.Windows.Forms.Button();
            this.AnimationExportButton = new System.Windows.Forms.Button();
            this.AnimationInportButton = new System.Windows.Forms.Button();
            this.AnimationPanel = new System.Windows.Forms.Panel();
            this.X_N_JumpEditor = new System.Windows.Forms.Button();
            this.ShowZoomComboBox = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.ShowFrameUpDown = new System.Windows.Forms.NumericUpDown();
            this.AnimationPictureBox = new FEBuilderGBA.InterpolatedPictureBox();
            this.ANIMATION = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.L_0_TEXT = new FEBuilderGBA.TextBoxEx();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.J_0_TEXT = new System.Windows.Forms.Label();
            this.SKILLICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.panel10 = new System.Windows.Forms.Label();
            this.BinInfo = new FEBuilderGBA.TextBoxEx();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconAddr)).BeginInit();
            this.panel2.SuspendLayout();
            this.AnimationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShowFrameUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ANIMATION)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SKILLICON)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(11, 7);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1155, 34);
            this.panel3.TabIndex = 95;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(480, 1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 3;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(-2, -2);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 36);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(249, -1);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 34);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(341, 3);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 1;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(114, 4);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 0;
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
            this.panel5.Location = new System.Drawing.Point(261, 40);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(905, 34);
            this.panel5.TabIndex = 94;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(314, 4);
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
            this.label3.Location = new System.Drawing.Point(231, 0);
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
            this.SelectAddress.Location = new System.Drawing.Point(532, 2);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(150, 25);
            this.SelectAddress.TabIndex = 57;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(408, 1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 56;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(716, 0);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(168, 30);
            this.WriteButton.TabIndex = 55;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(90, 4);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 0;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, 0);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(87, 33);
            this.label23.TabIndex = 53;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton_255);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(11, 43);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(245, 802);
            this.panel6.TabIndex = 96;
            // 
            // AddressListExpandsButton_255
            // 
            this.AddressListExpandsButton_255.Location = new System.Drawing.Point(1, 768);
            this.AddressListExpandsButton_255.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton_255.Name = "AddressListExpandsButton_255";
            this.AddressListExpandsButton_255.Size = new System.Drawing.Size(239, 30);
            this.AddressListExpandsButton_255.TabIndex = 114;
            this.AddressListExpandsButton_255.Text = "リストの拡張";
            this.AddressListExpandsButton_255.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-2, -3);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(247, 26);
            this.LabelFilter.TabIndex = 55;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-2, 22);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(246, 742);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.IconAddr);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.AnimationExportButton);
            this.panel1.Controls.Add(this.AnimationInportButton);
            this.panel1.Controls.Add(this.AnimationPanel);
            this.panel1.Controls.Add(this.ANIMATION);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ImportButton);
            this.panel1.Controls.Add(this.ExportButton);
            this.panel1.Controls.Add(this.L_0_TEXT);
            this.panel1.Controls.Add(this.W0);
            this.panel1.Controls.Add(this.J_0_TEXT);
            this.panel1.Controls.Add(this.SKILLICON);
            this.panel1.Controls.Add(this.panel10);
            this.panel1.Location = new System.Drawing.Point(261, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(904, 762);
            this.panel1.TabIndex = 97;
            // 
            // IconAddr
            // 
            this.IconAddr.Hexadecimal = true;
            this.IconAddr.Location = new System.Drawing.Point(234, 44);
            this.IconAddr.Margin = new System.Windows.Forms.Padding(2);
            this.IconAddr.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.IconAddr.Name = "IconAddr";
            this.IconAddr.ReadOnly = true;
            this.IconAddr.Size = new System.Drawing.Size(167, 25);
            this.IconAddr.TabIndex = 154;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.ExportAllButton);
            this.panel2.Controls.Add(this.ImportAllButton);
            this.panel2.Location = new System.Drawing.Point(242, 721);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(658, 37);
            this.panel2.TabIndex = 153;
            // 
            // ExportAllButton
            // 
            this.ExportAllButton.Location = new System.Drawing.Point(377, 3);
            this.ExportAllButton.Name = "ExportAllButton";
            this.ExportAllButton.Size = new System.Drawing.Size(274, 30);
            this.ExportAllButton.TabIndex = 117;
            this.ExportAllButton.Text = "一括エクスポート";
            this.ExportAllButton.UseVisualStyleBackColor = true;
            this.ExportAllButton.Click += new System.EventHandler(this.ExportAllButton_Click);
            // 
            // ImportAllButton
            // 
            this.ImportAllButton.Location = new System.Drawing.Point(98, 3);
            this.ImportAllButton.Name = "ImportAllButton";
            this.ImportAllButton.Size = new System.Drawing.Size(274, 30);
            this.ImportAllButton.TabIndex = 116;
            this.ImportAllButton.Text = "一括インポート";
            this.ImportAllButton.UseVisualStyleBackColor = true;
            this.ImportAllButton.Click += new System.EventHandler(this.ImportAllButton_Click);
            // 
            // AnimationExportButton
            // 
            this.AnimationExportButton.Location = new System.Drawing.Point(611, 195);
            this.AnimationExportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.AnimationExportButton.Name = "AnimationExportButton";
            this.AnimationExportButton.Size = new System.Drawing.Size(230, 30);
            this.AnimationExportButton.TabIndex = 122;
            this.AnimationExportButton.Text = "アニメーション取出";
            this.AnimationExportButton.UseVisualStyleBackColor = true;
            this.AnimationExportButton.Click += new System.EventHandler(this.AnimationExportButton_Click);
            // 
            // AnimationInportButton
            // 
            this.AnimationInportButton.Location = new System.Drawing.Point(344, 195);
            this.AnimationInportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.AnimationInportButton.Name = "AnimationInportButton";
            this.AnimationInportButton.Size = new System.Drawing.Size(230, 30);
            this.AnimationInportButton.TabIndex = 121;
            this.AnimationInportButton.Text = "アニメーション読込";
            this.AnimationInportButton.UseVisualStyleBackColor = true;
            this.AnimationInportButton.Click += new System.EventHandler(this.AnimationImportButton_Click);
            // 
            // AnimationPanel
            // 
            this.AnimationPanel.Controls.Add(this.BinInfo);
            this.AnimationPanel.Controls.Add(this.X_N_JumpEditor);
            this.AnimationPanel.Controls.Add(this.ShowZoomComboBox);
            this.AnimationPanel.Controls.Add(this.label25);
            this.AnimationPanel.Controls.Add(this.label26);
            this.AnimationPanel.Controls.Add(this.label24);
            this.AnimationPanel.Controls.Add(this.ShowFrameUpDown);
            this.AnimationPanel.Controls.Add(this.AnimationPictureBox);
            this.AnimationPanel.Location = new System.Drawing.Point(18, 242);
            this.AnimationPanel.Name = "AnimationPanel";
            this.AnimationPanel.Size = new System.Drawing.Size(864, 409);
            this.AnimationPanel.TabIndex = 120;
            this.AnimationPanel.Visible = false;
            // 
            // X_N_JumpEditor
            // 
            this.X_N_JumpEditor.Location = new System.Drawing.Point(169, 121);
            this.X_N_JumpEditor.Name = "X_N_JumpEditor";
            this.X_N_JumpEditor.Size = new System.Drawing.Size(134, 30);
            this.X_N_JumpEditor.TabIndex = 188;
            this.X_N_JumpEditor.Text = "エディタ";
            this.X_N_JumpEditor.UseVisualStyleBackColor = true;
            this.X_N_JumpEditor.Click += new System.EventHandler(this.X_N_JumpEditor_Click);
            // 
            // ShowZoomComboBox
            // 
            this.ShowZoomComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ShowZoomComboBox.FormattingEnabled = true;
            this.ShowZoomComboBox.Items.AddRange(new object[] {
            "拡大して描画",
            "拡大しないで描画"});
            this.ShowZoomComboBox.Location = new System.Drawing.Point(128, 35);
            this.ShowZoomComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.ShowZoomComboBox.Name = "ShowZoomComboBox";
            this.ShowZoomComboBox.Size = new System.Drawing.Size(175, 26);
            this.ShowZoomComboBox.TabIndex = 187;
            this.ShowZoomComboBox.SelectedIndexChanged += new System.EventHandler(this.ShowZoomComboBox_SelectedIndexChanged);
            // 
            // label25
            // 
            this.label25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label25.Location = new System.Drawing.Point(4, 34);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(116, 32);
            this.label25.TabIndex = 186;
            this.label25.Text = "拡大";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label26
            // 
            this.label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label26.Location = new System.Drawing.Point(4, 65);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(116, 30);
            this.label26.TabIndex = 185;
            this.label26.Text = "フレーム";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label24.Location = new System.Drawing.Point(4, 2);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(299, 30);
            this.label24.TabIndex = 184;
            this.label24.Text = "表示例";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShowFrameUpDown
            // 
            this.ShowFrameUpDown.Hexadecimal = true;
            this.ShowFrameUpDown.Location = new System.Drawing.Point(128, 68);
            this.ShowFrameUpDown.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ShowFrameUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ShowFrameUpDown.Name = "ShowFrameUpDown";
            this.ShowFrameUpDown.Size = new System.Drawing.Size(66, 25);
            this.ShowFrameUpDown.TabIndex = 183;
            this.ShowFrameUpDown.ValueChanged += new System.EventHandler(this.ShowFrameUpDown_ValueChanged);
            // 
            // AnimationPictureBox
            // 
            this.AnimationPictureBox.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.AnimationPictureBox.Location = new System.Drawing.Point(326, 2);
            this.AnimationPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.AnimationPictureBox.Name = "AnimationPictureBox";
            this.AnimationPictureBox.Size = new System.Drawing.Size(536, 360);
            this.AnimationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AnimationPictureBox.TabIndex = 114;
            this.AnimationPictureBox.TabStop = false;
            // 
            // ANIMATION
            // 
            this.ANIMATION.Hexadecimal = true;
            this.ANIMATION.Location = new System.Drawing.Point(141, 200);
            this.ANIMATION.Margin = new System.Windows.Forms.Padding(2);
            this.ANIMATION.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ANIMATION.Name = "ANIMATION";
            this.ANIMATION.Size = new System.Drawing.Size(167, 25);
            this.ANIMATION.TabIndex = 119;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(2, 195);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 30);
            this.label1.TabIndex = 118;
            this.label1.Text = "アニメーション";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(234, 6);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(160, 30);
            this.ImportButton.TabIndex = 117;
            this.ImportButton.Text = "画像読込";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(425, 6);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(160, 30);
            this.ExportButton.TabIndex = 116;
            this.ExportButton.Text = "画像取出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // L_0_TEXT
            // 
            this.L_0_TEXT.ErrorMessage = "";
            this.L_0_TEXT.Location = new System.Drawing.Point(234, 73);
            this.L_0_TEXT.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_TEXT.Multiline = true;
            this.L_0_TEXT.Name = "L_0_TEXT";
            this.L_0_TEXT.Placeholder = "";
            this.L_0_TEXT.ReadOnly = true;
            this.L_0_TEXT.Size = new System.Drawing.Size(348, 108);
            this.L_0_TEXT.TabIndex = 115;
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(141, 74);
            this.W0.Margin = new System.Windows.Forms.Padding(2);
            this.W0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(79, 25);
            this.W0.TabIndex = 111;
            // 
            // J_0_TEXT
            // 
            this.J_0_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_TEXT.Location = new System.Drawing.Point(2, 69);
            this.J_0_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_TEXT.Name = "J_0_TEXT";
            this.J_0_TEXT.Size = new System.Drawing.Size(135, 30);
            this.J_0_TEXT.TabIndex = 114;
            this.J_0_TEXT.Text = "詳細";
            this.J_0_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SKILLICON
            // 
            this.SKILLICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.SKILLICON.Location = new System.Drawing.Point(141, 6);
            this.SKILLICON.Margin = new System.Windows.Forms.Padding(2);
            this.SKILLICON.Name = "SKILLICON";
            this.SKILLICON.Size = new System.Drawing.Size(64, 64);
            this.SKILLICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SKILLICON.TabIndex = 113;
            this.SKILLICON.TabStop = false;
            // 
            // panel10
            // 
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Location = new System.Drawing.Point(2, 6);
            this.panel10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(135, 30);
            this.panel10.TabIndex = 112;
            this.panel10.Text = "アイコン";
            this.panel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BinInfo
            // 
            this.BinInfo.ErrorMessage = "";
            this.BinInfo.Location = new System.Drawing.Point(324, 367);
            this.BinInfo.Name = "BinInfo";
            this.BinInfo.Placeholder = "";
            this.BinInfo.ReadOnly = true;
            this.BinInfo.Size = new System.Drawing.Size(537, 25);
            this.BinInfo.TabIndex = 197;
            // 
            // SkillConfigSkillSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1178, 859);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Name = "SkillConfigSkillSystemForm";
            this.Text = "スキル拡張設定";
            this.Load += new System.EventHandler(this.SkillConfigSkillSystemForm_Load);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconAddr)).EndInit();
            this.panel2.ResumeLayout(false);
            this.AnimationPanel.ResumeLayout(false);
            this.AnimationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShowFrameUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ANIMATION)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SKILLICON)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
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
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button AddressListExpandsButton_255;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel1;
        private FEBuilderGBA.TextBoxEx L_0_TEXT;
        private System.Windows.Forms.NumericUpDown W0;
        private System.Windows.Forms.Label J_0_TEXT;
        private InterpolatedPictureBox SKILLICON;
        private System.Windows.Forms.Label panel10;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ANIMATION;
        private System.Windows.Forms.Button AnimationExportButton;
        private System.Windows.Forms.Button AnimationInportButton;
        private System.Windows.Forms.Panel AnimationPanel;
        private InterpolatedPictureBox AnimationPictureBox;
        private System.Windows.Forms.ComboBox ShowZoomComboBox;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.NumericUpDown ShowFrameUpDown;
        private System.Windows.Forms.Button X_N_JumpEditor;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button ExportAllButton;
        private System.Windows.Forms.Button ImportAllButton;
        private System.Windows.Forms.NumericUpDown IconAddr;
        private TextBoxEx BinInfo;
    }
}