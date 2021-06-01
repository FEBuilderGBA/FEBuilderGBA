namespace FEBuilderGBA
{
    partial class SongTableForm
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.D4 = new System.Windows.Forms.NumericUpDown();
            this.J_4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.L_0_CHECKADDRESS = new FEBuilderGBA.TextBoxEx();
            this.panel9 = new System.Windows.Forms.Panel();
            this.X_REF = new FEBuilderGBA.ListBoxEx();
            this.label8 = new System.Windows.Forms.Label();
            this.Comment = new FEBuilderGBA.TextBoxEx();
            this.label7 = new System.Windows.Forms.Label();
            this.L_4_COMBO = new System.Windows.Forms.ComboBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.J_ID_INSTRUMENT_SONGID = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.J_ID_SONGTRACK = new System.Windows.Forms.Label();
            this.SoundRommPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.J_ID_SOUNDROOM = new System.Windows.Forms.Label();
            this.SONGPLAY = new System.Windows.Forms.Button();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.J_ID_SONGTRACK_ADDR = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.AddressListExpandsButton_32766 = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SoundRommPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1489, 30);
            this.panel3.TabIndex = 79;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(785, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(524, -1);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 30);
            this.label5.TabIndex = 24;
            this.label5.Text = "読込数";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(-1, -1);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 30);
            this.label4.TabIndex = 23;
            this.label4.Text = "先頭アドレス";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(169, 3);
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
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(621, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // D4
            // 
            this.D4.Hexadecimal = true;
            this.D4.Location = new System.Drawing.Point(222, 37);
            this.D4.Margin = new System.Windows.Forms.Padding(2);
            this.D4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.D4.Name = "D4";
            this.D4.Size = new System.Drawing.Size(130, 25);
            this.D4.TabIndex = 78;
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(-1, 34);
            this.J_4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(216, 30);
            this.J_4.TabIndex = 77;
            this.J_4.Text = "Priority(PlayerType)";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.L_0_CHECKADDRESS);
            this.panel4.Controls.Add(this.panel9);
            this.panel4.Controls.Add(this.Comment);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.L_4_COMBO);
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.SoundRommPanel);
            this.panel4.Controls.Add(this.SONGPLAY);
            this.panel4.Controls.Add(this.D4);
            this.panel4.Controls.Add(this.J_4);
            this.panel4.Controls.Add(this.P0);
            this.panel4.Controls.Add(this.J_ID_SONGTRACK_ADDR);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 30);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(963, 711);
            this.panel4.TabIndex = 80;
            // 
            // L_0_CHECKADDRESS
            // 
            this.L_0_CHECKADDRESS.ErrorMessage = "";
            this.L_0_CHECKADDRESS.Location = new System.Drawing.Point(357, 2);
            this.L_0_CHECKADDRESS.Name = "L_0_CHECKADDRESS";
            this.L_0_CHECKADDRESS.Placeholder = "";
            this.L_0_CHECKADDRESS.Size = new System.Drawing.Size(316, 25);
            this.L_0_CHECKADDRESS.TabIndex = 203;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.X_REF);
            this.panel9.Controls.Add(this.label8);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(704, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(257, 709);
            this.panel9.TabIndex = 201;
            // 
            // X_REF
            // 
            this.X_REF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.X_REF.FormattingEnabled = true;
            this.X_REF.IntegralHeight = false;
            this.X_REF.ItemHeight = 18;
            this.X_REF.Location = new System.Drawing.Point(0, 26);
            this.X_REF.Margin = new System.Windows.Forms.Padding(4);
            this.X_REF.Name = "X_REF";
            this.X_REF.Size = new System.Drawing.Size(257, 683);
            this.X_REF.TabIndex = 199;
            this.X_REF.KeyDown += new System.Windows.Forms.KeyEventHandler(this.X_REF_KeyDown);
            this.X_REF.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.X_REF_MouseDoubleClick);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(257, 26);
            this.label8.TabIndex = 200;
            this.label8.Text = "参照箇所";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Comment
            // 
            this.Comment.ErrorMessage = "";
            this.Comment.Location = new System.Drawing.Point(226, 78);
            this.Comment.Name = "Comment";
            this.Comment.Placeholder = "";
            this.Comment.Size = new System.Drawing.Size(336, 25);
            this.Comment.TabIndex = 198;
            // 
            // label7
            // 
            this.label7.AccessibleDescription = "@COMMENT";
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(-1, 75);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(216, 31);
            this.label7.TabIndex = 197;
            this.label7.Text = "コメント";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_4_COMBO
            // 
            this.L_4_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.L_4_COMBO.FormattingEnabled = true;
            this.L_4_COMBO.Items.AddRange(new object[] {
            "00000000=Highest (BGM)",
            "00010001=High (Map Music)",
            "00020002=Mid-High",
            "00030003=Medium",
            "00040004=Medium",
            "00050005=Mid-Low",
            "00060006=Low (SFX)",
            "00070007=Lowest (SFX)",
            "00080008=Lowest2 (SFX)"});
            this.L_4_COMBO.Location = new System.Drawing.Point(357, 36);
            this.L_4_COMBO.Name = "L_4_COMBO";
            this.L_4_COMBO.Size = new System.Drawing.Size(313, 26);
            this.L_4_COMBO.TabIndex = 191;
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.label6);
            this.panel8.Controls.Add(this.J_ID_INSTRUMENT_SONGID);
            this.panel8.Location = new System.Drawing.Point(3, 479);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(667, 129);
            this.panel8.TabIndex = 190;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(433, 36);
            this.label6.TabIndex = 189;
            this.label6.Text = "曲を構成する楽器テーブルを表示します。\r\n通常は気にする必要はありません。開発者向けの機能です。";
            // 
            // J_ID_INSTRUMENT_SONGID
            // 
            this.J_ID_INSTRUMENT_SONGID.AutoSize = true;
            this.J_ID_INSTRUMENT_SONGID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.J_ID_INSTRUMENT_SONGID.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.J_ID_INSTRUMENT_SONGID.Location = new System.Drawing.Point(3, 10);
            this.J_ID_INSTRUMENT_SONGID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_ID_INSTRUMENT_SONGID.Name = "J_ID_INSTRUMENT_SONGID";
            this.J_ID_INSTRUMENT_SONGID.Size = new System.Drawing.Size(142, 22);
            this.J_ID_INSTRUMENT_SONGID.TabIndex = 2;
            this.J_ID_INSTRUMENT_SONGID.Text = "楽器テーブルへ";
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.J_ID_SONGTRACK);
            this.panel7.Location = new System.Drawing.Point(3, 339);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(667, 132);
            this.panel7.TabIndex = 188;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 36);
            this.label2.TabIndex = 189;
            this.label2.Text = "曲の楽譜を表示します。\r\nここからは、曲のインポート、エクスポートを行います。";
            // 
            // J_ID_SONGTRACK
            // 
            this.J_ID_SONGTRACK.AutoSize = true;
            this.J_ID_SONGTRACK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.J_ID_SONGTRACK.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.J_ID_SONGTRACK.Location = new System.Drawing.Point(3, 7);
            this.J_ID_SONGTRACK.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_ID_SONGTRACK.Name = "J_ID_SONGTRACK";
            this.J_ID_SONGTRACK.Size = new System.Drawing.Size(143, 22);
            this.J_ID_SONGTRACK.TabIndex = 1;
            this.J_ID_SONGTRACK.Text = "ソングヘッダーへ";
            // 
            // SoundRommPanel
            // 
            this.SoundRommPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SoundRommPanel.Controls.Add(this.label1);
            this.SoundRommPanel.Controls.Add(this.J_ID_SOUNDROOM);
            this.SoundRommPanel.Location = new System.Drawing.Point(3, 205);
            this.SoundRommPanel.Name = "SoundRommPanel";
            this.SoundRommPanel.Size = new System.Drawing.Size(667, 128);
            this.SoundRommPanel.TabIndex = 187;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 36);
            this.label1.TabIndex = 188;
            this.label1.Text = "サウンドルームに曲を登録すると、曲名をつけられます。\r\nFEには、曲と効果音の違いはありません。\r\n";
            // 
            // J_ID_SOUNDROOM
            // 
            this.J_ID_SOUNDROOM.AutoSize = true;
            this.J_ID_SOUNDROOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.J_ID_SOUNDROOM.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.J_ID_SOUNDROOM.Location = new System.Drawing.Point(2, 3);
            this.J_ID_SOUNDROOM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_ID_SOUNDROOM.Name = "J_ID_SOUNDROOM";
            this.J_ID_SOUNDROOM.Size = new System.Drawing.Size(148, 22);
            this.J_ID_SOUNDROOM.TabIndex = 0;
            this.J_ID_SOUNDROOM.Text = "サウンドルームへ";
            // 
            // SONGPLAY
            // 
            this.SONGPLAY.Location = new System.Drawing.Point(222, 138);
            this.SONGPLAY.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SONGPLAY.Name = "SONGPLAY";
            this.SONGPLAY.Size = new System.Drawing.Size(66, 60);
            this.SONGPLAY.TabIndex = 185;
            this.SONGPLAY.Text = "♪";
            this.SONGPLAY.UseVisualStyleBackColor = true;
            this.SONGPLAY.Click += new System.EventHandler(this.SONGPLAY_Click);
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(222, 2);
            this.P0.Margin = new System.Windows.Forms.Padding(2);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 76;
            // 
            // J_ID_SONGTRACK_ADDR
            // 
            this.J_ID_SONGTRACK_ADDR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_ID_SONGTRACK_ADDR.Location = new System.Drawing.Point(-1, 0);
            this.J_ID_SONGTRACK_ADDR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_ID_SONGTRACK_ADDR.Name = "J_ID_SONGTRACK_ADDR";
            this.J_ID_SONGTRACK_ADDR.Size = new System.Drawing.Size(216, 30);
            this.J_ID_SONGTRACK_ADDR.TabIndex = 62;
            this.J_ID_SONGTRACK_ADDR.Text = "ソングヘッダー";
            this.J_ID_SONGTRACK_ADDR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(963, 30);
            this.panel5.TabIndex = 78;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(347, 1);
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
            this.label3.Location = new System.Drawing.Point(260, -1);
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
            this.SelectAddress.Location = new System.Drawing.Point(565, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(108, 25);
            this.SelectAddress.TabIndex = 57;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(439, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 56;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(703, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(233, 30);
            this.WriteButton.TabIndex = 55;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(96, 1);
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
            this.label23.Location = new System.Drawing.Point(-1, -1);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 30);
            this.label23.TabIndex = 53;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Controls.Add(this.AddressListExpandsButton_32766);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 30);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(526, 741);
            this.panel6.TabIndex = 147;
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
            this.AddressList.Size = new System.Drawing.Size(524, 683);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // AddressListExpandsButton_32766
            // 
            this.AddressListExpandsButton_32766.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AddressListExpandsButton_32766.Location = new System.Drawing.Point(0, 709);
            this.AddressListExpandsButton_32766.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton_32766.Name = "AddressListExpandsButton_32766";
            this.AddressListExpandsButton_32766.Size = new System.Drawing.Size(524, 30);
            this.AddressListExpandsButton_32766.TabIndex = 116;
            this.AddressListExpandsButton_32766.Text = "リストの拡張";
            this.AddressListExpandsButton_32766.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabelFilter.Location = new System.Drawing.Point(0, 0);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(524, 26);
            this.LabelFilter.TabIndex = 106;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(526, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(963, 741);
            this.panel2.TabIndex = 148;
            // 
            // SongTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1489, 771);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "SongTableForm";
            this.Text = "ソングテーブル";
            this.Load += new System.EventHandler(this.SongTableForm_Load);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D4)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.SoundRommPanel.ResumeLayout(false);
            this.SoundRommPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown D4;
        private System.Windows.Forms.Label J_4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.Label J_ID_SONGTRACK_ADDR;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label J_ID_SOUNDROOM;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Button SONGPLAY;
        private System.Windows.Forms.Label J_ID_SONGTRACK;
        private System.Windows.Forms.Label J_ID_INSTRUMENT_SONGID;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel SoundRommPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox L_4_COMBO;
        private System.Windows.Forms.Button AddressListExpandsButton_32766;
        private TextBoxEx Comment;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private ListBoxEx X_REF;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel9;
        private TextBoxEx L_0_CHECKADDRESS;
    }
}