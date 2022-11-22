namespace FEBuilderGBA
{
    partial class TextCharCodeForm
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
            this.panel6 = new System.Windows.Forms.Panel();
            this.SerifFontPictureBox = new FEBuilderGBA.InterpolatedPictureBox();
            this.ItemFontPictureBox = new FEBuilderGBA.InterpolatedPictureBox();
            this.J_0_FONT_SERIF = new System.Windows.Forms.Label();
            this.J_0_FONT_ITEM = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.SEARCH_COUNT_BUTTON = new System.Windows.Forms.Button();
            this.SEARCH_COUNT_LIST = new FEBuilderGBA.ListBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.SEARCH_COUNT = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.SEARCH_CHAR_BUTTON = new System.Windows.Forms.Button();
            this.SEARCH_CHAR = new FEBuilderGBA.TextBoxEx();
            this.label17 = new System.Windows.Forms.Label();
            this.L_0_WSPLITSTRING_0 = new FEBuilderGBA.TextBoxEx();
            this.W2 = new System.Windows.Forms.NumericUpDown();
            this.J_2 = new System.Windows.Forms.Label();
            this.W0 = new System.Windows.Forms.NumericUpDown();
            this.J_0 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel3 = new System.Windows.Forms.Panel();
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
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SerifFontPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemFontPictureBox)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SEARCH_COUNT)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).BeginInit();
            this.panel3.SuspendLayout();
            this.AddressPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(742, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
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
            this.panel1.Size = new System.Drawing.Size(1108, 30);
            this.panel1.TabIndex = 85;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(467, 0);
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
            this.label1.Location = new System.Drawing.Point(-1, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(281, -1);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(367, 2);
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
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.L_0_WSPLITSTRING_0);
            this.panel2.Controls.Add(this.W2);
            this.panel2.Controls.Add(this.J_2);
            this.panel2.Controls.Add(this.W0);
            this.panel2.Controls.Add(this.J_0);
            this.panel2.Location = new System.Drawing.Point(212, 73);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(907, 393);
            this.panel2.TabIndex = 83;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.SerifFontPictureBox);
            this.panel6.Controls.Add(this.ItemFontPictureBox);
            this.panel6.Controls.Add(this.J_0_FONT_SERIF);
            this.panel6.Controls.Add(this.J_0_FONT_ITEM);
            this.panel6.Location = new System.Drawing.Point(1, 139);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(340, 254);
            this.panel6.TabIndex = 91;
            // 
            // SerifFontPictureBox
            // 
            this.SerifFontPictureBox.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.SerifFontPictureBox.Location = new System.Drawing.Point(7, 110);
            this.SerifFontPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.SerifFontPictureBox.Name = "SerifFontPictureBox";
            this.SerifFontPictureBox.Size = new System.Drawing.Size(59, 64);
            this.SerifFontPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SerifFontPictureBox.TabIndex = 87;
            this.SerifFontPictureBox.TabStop = false;
            // 
            // ItemFontPictureBox
            // 
            this.ItemFontPictureBox.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.ItemFontPictureBox.Location = new System.Drawing.Point(7, 30);
            this.ItemFontPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.ItemFontPictureBox.Name = "ItemFontPictureBox";
            this.ItemFontPictureBox.Size = new System.Drawing.Size(59, 56);
            this.ItemFontPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ItemFontPictureBox.TabIndex = 2;
            this.ItemFontPictureBox.TabStop = false;
            // 
            // J_0_FONT_SERIF
            // 
            this.J_0_FONT_SERIF.AutoSize = true;
            this.J_0_FONT_SERIF.Location = new System.Drawing.Point(2, 90);
            this.J_0_FONT_SERIF.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_FONT_SERIF.Name = "J_0_FONT_SERIF";
            this.J_0_FONT_SERIF.Size = new System.Drawing.Size(96, 18);
            this.J_0_FONT_SERIF.TabIndex = 1;
            this.J_0_FONT_SERIF.Text = "セリフフォント";
            // 
            // J_0_FONT_ITEM
            // 
            this.J_0_FONT_ITEM.AutoSize = true;
            this.J_0_FONT_ITEM.Location = new System.Drawing.Point(4, 7);
            this.J_0_FONT_ITEM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0_FONT_ITEM.Name = "J_0_FONT_ITEM";
            this.J_0_FONT_ITEM.Size = new System.Drawing.Size(114, 18);
            this.J_0_FONT_ITEM.TabIndex = 0;
            this.J_0_FONT_ITEM.Text = "アイテムフォント";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.SEARCH_COUNT_BUTTON);
            this.panel5.Controls.Add(this.SEARCH_COUNT_LIST);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.SEARCH_COUNT);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Location = new System.Drawing.Point(348, 79);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(564, 314);
            this.panel5.TabIndex = 90;
            // 
            // SEARCH_COUNT_BUTTON
            // 
            this.SEARCH_COUNT_BUTTON.Location = new System.Drawing.Point(4, 36);
            this.SEARCH_COUNT_BUTTON.Margin = new System.Windows.Forms.Padding(2);
            this.SEARCH_COUNT_BUTTON.Name = "SEARCH_COUNT_BUTTON";
            this.SEARCH_COUNT_BUTTON.Size = new System.Drawing.Size(200, 61);
            this.SEARCH_COUNT_BUTTON.TabIndex = 110;
            this.SEARCH_COUNT_BUTTON.Text = "使用回数検索";
            this.SEARCH_COUNT_BUTTON.UseVisualStyleBackColor = true;
            this.SEARCH_COUNT_BUTTON.Click += new System.EventHandler(this.SEARCH_COUNT_BUTTON_Click);
            // 
            // SEARCH_COUNT_LIST
            // 
            this.SEARCH_COUNT_LIST.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SEARCH_COUNT_LIST.FormattingEnabled = true;
            this.SEARCH_COUNT_LIST.IntegralHeight = false;
            this.SEARCH_COUNT_LIST.ItemHeight = 18;
            this.SEARCH_COUNT_LIST.Location = new System.Drawing.Point(2, 143);
            this.SEARCH_COUNT_LIST.Margin = new System.Windows.Forms.Padding(4);
            this.SEARCH_COUNT_LIST.Name = "SEARCH_COUNT_LIST";
            this.SEARCH_COUNT_LIST.Size = new System.Drawing.Size(554, 166);
            this.SEARCH_COUNT_LIST.TabIndex = 109;
            this.SEARCH_COUNT_LIST.SelectedIndexChanged += new System.EventHandler(this.SEARCH_COUNT_LIST_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(283, 70);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(241, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "回以下しか出現しない文字取得";
            // 
            // SEARCH_COUNT
            // 
            this.SEARCH_COUNT.Location = new System.Drawing.Point(227, 68);
            this.SEARCH_COUNT.Margin = new System.Windows.Forms.Padding(2);
            this.SEARCH_COUNT.Name = "SEARCH_COUNT";
            this.SEARCH_COUNT.Size = new System.Drawing.Size(47, 25);
            this.SEARCH_COUNT.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(2, 116);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(556, 26);
            this.label5.TabIndex = 108;
            this.label5.Text = "番号 文字 回数　登場会話ID";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.SEARCH_CHAR_BUTTON);
            this.panel4.Controls.Add(this.SEARCH_CHAR);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Location = new System.Drawing.Point(348, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(564, 82);
            this.panel4.TabIndex = 89;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(222, 32);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 18);
            this.label10.TabIndex = 112;
            this.label10.Text = "文字文字";
            // 
            // SEARCH_CHAR_BUTTON
            // 
            this.SEARCH_CHAR_BUTTON.Location = new System.Drawing.Point(-1, 30);
            this.SEARCH_CHAR_BUTTON.Margin = new System.Windows.Forms.Padding(2);
            this.SEARCH_CHAR_BUTTON.Name = "SEARCH_CHAR_BUTTON";
            this.SEARCH_CHAR_BUTTON.Size = new System.Drawing.Size(200, 30);
            this.SEARCH_CHAR_BUTTON.TabIndex = 109;
            this.SEARCH_CHAR_BUTTON.Text = "文字検索";
            this.SEARCH_CHAR_BUTTON.UseVisualStyleBackColor = true;
            this.SEARCH_CHAR_BUTTON.Click += new System.EventHandler(this.SEARCH_CHAR_BUTTON_Click);
            // 
            // SEARCH_CHAR
            // 
            this.SEARCH_CHAR.ErrorMessage = "";
            this.SEARCH_CHAR.Location = new System.Drawing.Point(308, 30);
            this.SEARCH_CHAR.Margin = new System.Windows.Forms.Padding(2);
            this.SEARCH_CHAR.Name = "SEARCH_CHAR";
            this.SEARCH_CHAR.Placeholder = "";
            this.SEARCH_CHAR.Size = new System.Drawing.Size(47, 25);
            this.SEARCH_CHAR.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(4, 4);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 18);
            this.label17.TabIndex = 0;
            this.label17.Text = "文字検索";
            // 
            // L_0_WSPLITSTRING_0
            // 
            this.L_0_WSPLITSTRING_0.ErrorMessage = "";
            this.L_0_WSPLITSTRING_0.Location = new System.Drawing.Point(203, 11);
            this.L_0_WSPLITSTRING_0.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_WSPLITSTRING_0.Name = "L_0_WSPLITSTRING_0";
            this.L_0_WSPLITSTRING_0.Placeholder = "";
            this.L_0_WSPLITSTRING_0.Size = new System.Drawing.Size(100, 25);
            this.L_0_WSPLITSTRING_0.TabIndex = 40;
            // 
            // W2
            // 
            this.W2.Hexadecimal = true;
            this.W2.Location = new System.Drawing.Point(103, 47);
            this.W2.Margin = new System.Windows.Forms.Padding(2);
            this.W2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W2.Name = "W2";
            this.W2.Size = new System.Drawing.Size(94, 25);
            this.W2.TabIndex = 39;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(2, 43);
            this.J_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(94, 30);
            this.J_2.TabIndex = 24;
            this.J_2.Text = "FFFF";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // W0
            // 
            this.W0.Hexadecimal = true;
            this.W0.Location = new System.Drawing.Point(101, 11);
            this.W0.Margin = new System.Windows.Forms.Padding(2);
            this.W0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.W0.Name = "W0";
            this.W0.Size = new System.Drawing.Size(94, 25);
            this.W0.TabIndex = 20;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(2, 11);
            this.J_0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(94, 32);
            this.J_0.TabIndex = 18;
            this.J_0.Text = "ASCII";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, -2);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 30);
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
            this.AddressList.Location = new System.Drawing.Point(-2, 26);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(199, 395);
            this.AddressList.TabIndex = 108;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.AddressList);
            this.panel3.Controls.Add(this.LabelFilter);
            this.panel3.Location = new System.Drawing.Point(14, 43);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(199, 426);
            this.panel3.TabIndex = 86;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(198, 28);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(444, 0);
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
            this.SelectAddress.Location = new System.Drawing.Point(566, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(352, 1);
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
            this.label3.Location = new System.Drawing.Point(269, 0);
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
            this.AddressPanel.Location = new System.Drawing.Point(212, 44);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(910, 30);
            this.AddressPanel.TabIndex = 84;
            // 
            // TextCharCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1156, 481);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.AddressPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TextCharCodeForm";
            this.Text = "符号テーブル";
            this.Load += new System.EventHandler(this.TextCharCodeForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SerifFontPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemFontPictureBox)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SEARCH_COUNT)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W0)).EndInit();
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
        private FEBuilderGBA.TextBoxEx L_0_WSPLITSTRING_0;
        private System.Windows.Forms.NumericUpDown W2;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.NumericUpDown W0;
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.Label label23;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label LabelFilter;
        private System.Windows.Forms.Label label22;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.Panel panel4;
        private FEBuilderGBA.TextBoxEx SEARCH_CHAR;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label4;
        private ListBoxEx SEARCH_COUNT_LIST;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown SEARCH_COUNT;
        private System.Windows.Forms.Button SEARCH_CHAR_BUTTON;
        private System.Windows.Forms.Button SEARCH_COUNT_BUTTON;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label J_0_FONT_SERIF;
        private System.Windows.Forms.Label J_0_FONT_ITEM;
        private System.Windows.Forms.Label label10;
        private InterpolatedPictureBox SerifFontPictureBox;
        private InterpolatedPictureBox ItemFontPictureBox;
    }
}