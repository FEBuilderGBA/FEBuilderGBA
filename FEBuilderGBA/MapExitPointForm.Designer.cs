namespace FEBuilderGBA
{
    partial class MapExitPointForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.N_B1 = new System.Windows.Forms.NumericUpDown();
            this.N_B0 = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.N_B3 = new System.Windows.Forms.NumericUpDown();
            this.N_J_3 = new System.Windows.Forms.Label();
            this.N_L_2_COMBO = new System.Windows.Forms.ComboBox();
            this.N_B2 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.N_L_0_MAPXY_1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.N_WriteButton = new System.Windows.Forms.Button();
            this.N_Address = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.N_ReloadListButton = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.N_ReadCount = new System.Windows.Forms.NumericUpDown();
            this.WriteButton = new System.Windows.Forms.Button();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.panel22 = new System.Windows.Forms.Panel();
            this.N_AddressListExpandsButton = new System.Windows.Forms.Button();
            this.NewListAlloc = new System.Windows.Forms.Button();
            this.N_LabelFilter = new System.Windows.Forms.Label();
            this.X_Filter_Note_Message = new System.Windows.Forms.Panel();
            this.X_Filter_Note_Message_Picture = new System.Windows.Forms.PictureBox();
            this.X_Filter_Note_Message_Label = new System.Windows.Forms.Label();
            this.MapPictureBox = new FEBuilderGBA.MapPictureBox();
            this.N_AddressList = new FEBuilderGBA.ListBoxEx();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.N_BlockSize = new FEBuilderGBA.TextBoxEx();
            this.N_SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.N_L_3_FLAG = new FEBuilderGBA.TextBoxEx();
            this.FilterComboBox = new FEBuilderGBA.ComboBoxEx();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B2)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Address)).BeginInit();
            this.panel14.SuspendLayout();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            this.panel22.SuspendLayout();
            this.X_Filter_Note_Message.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_Filter_Note_Message_Picture)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.FilterComboBox);
            this.panel1.Controls.Add(this.Address);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(7, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1578, 34);
            this.panel1.TabIndex = 67;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(942, 3);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 159;
            this.Address.Visible = false;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(-1, -1);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 34);
            this.label3.TabIndex = 158;
            this.label3.Text = "条件:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(817, -2);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 34);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(536, -2);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 34);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(657, 3);
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
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.X_Filter_Note_Message);
            this.panel3.Controls.Add(this.N_L_3_FLAG);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.N_B1);
            this.panel3.Controls.Add(this.N_B0);
            this.panel3.Controls.Add(this.label30);
            this.panel3.Controls.Add(this.N_B3);
            this.panel3.Controls.Add(this.N_J_3);
            this.panel3.Controls.Add(this.N_L_2_COMBO);
            this.panel3.Controls.Add(this.N_B2);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.N_L_0_MAPXY_1);
            this.panel3.Location = new System.Drawing.Point(436, 162);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1154, 555);
            this.panel3.TabIndex = 179;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(265, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 26);
            this.label5.TabIndex = 190;
            this.label5.Text = "Y:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_B1
            // 
            this.N_B1.Location = new System.Drawing.Point(299, 9);
            this.N_B1.Margin = new System.Windows.Forms.Padding(2);
            this.N_B1.Name = "N_B1";
            this.N_B1.Size = new System.Drawing.Size(78, 25);
            this.N_B1.TabIndex = 189;
            // 
            // N_B0
            // 
            this.N_B0.Location = new System.Drawing.Point(184, 9);
            this.N_B0.Margin = new System.Windows.Forms.Padding(2);
            this.N_B0.Name = "N_B0";
            this.N_B0.Size = new System.Drawing.Size(78, 25);
            this.N_B0.TabIndex = 187;
            // 
            // label30
            // 
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Location = new System.Drawing.Point(152, 9);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(28, 26);
            this.label30.TabIndex = 188;
            this.label30.Text = "X:";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_B3
            // 
            this.N_B3.Hexadecimal = true;
            this.N_B3.Location = new System.Drawing.Point(152, 86);
            this.N_B3.Margin = new System.Windows.Forms.Padding(2);
            this.N_B3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N_B3.Name = "N_B3";
            this.N_B3.Size = new System.Drawing.Size(65, 25);
            this.N_B3.TabIndex = 185;
            // 
            // N_J_3
            // 
            this.N_J_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_3.Location = new System.Drawing.Point(2, 86);
            this.N_J_3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_3.Name = "N_J_3";
            this.N_J_3.Size = new System.Drawing.Size(143, 26);
            this.N_J_3.TabIndex = 184;
            this.N_J_3.Text = "00";
            this.N_J_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_L_2_COMBO
            // 
            this.N_L_2_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.N_L_2_COMBO.FormattingEnabled = true;
            this.N_L_2_COMBO.Items.AddRange(new object[] {
            "00=左2歩",
            "01=右2歩",
            "02=下2歩",
            "03=上2歩",
            "05=その場"});
            this.N_L_2_COMBO.Location = new System.Drawing.Point(223, 47);
            this.N_L_2_COMBO.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_L_2_COMBO.Name = "N_L_2_COMBO";
            this.N_L_2_COMBO.Size = new System.Drawing.Size(184, 26);
            this.N_L_2_COMBO.TabIndex = 181;
            // 
            // N_B2
            // 
            this.N_B2.Hexadecimal = true;
            this.N_B2.Location = new System.Drawing.Point(152, 48);
            this.N_B2.Margin = new System.Windows.Forms.Padding(2);
            this.N_B2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N_B2.Name = "N_B2";
            this.N_B2.Size = new System.Drawing.Size(65, 25);
            this.N_B2.TabIndex = 183;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(2, 48);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(143, 26);
            this.label11.TabIndex = 182;
            this.label11.Text = "消滅方法";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_L_0_MAPXY_1
            // 
            this.N_L_0_MAPXY_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_L_0_MAPXY_1.Location = new System.Drawing.Point(2, 8);
            this.N_L_0_MAPXY_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_L_0_MAPXY_1.Name = "N_L_0_MAPXY_1";
            this.N_L_0_MAPXY_1.Size = new System.Drawing.Size(143, 26);
            this.N_L_0_MAPXY_1.TabIndex = 181;
            this.N_L_0_MAPXY_1.Text = "離脱座標";
            this.N_L_0_MAPXY_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.N_BlockSize);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.N_SelectAddress);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.N_WriteButton);
            this.panel5.Controls.Add(this.N_Address);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Location = new System.Drawing.Point(436, 76);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1150, 87);
            this.panel5.TabIndex = 177;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(2, 52);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 30);
            this.label6.TabIndex = 56;
            this.label6.Text = "Size:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(154, 52);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 53;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_WriteButton
            // 
            this.N_WriteButton.Location = new System.Drawing.Point(242, 2);
            this.N_WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_WriteButton.Name = "N_WriteButton";
            this.N_WriteButton.Size = new System.Drawing.Size(167, 30);
            this.N_WriteButton.TabIndex = 9;
            this.N_WriteButton.Text = "書き込み";
            this.N_WriteButton.UseVisualStyleBackColor = true;
            // 
            // N_Address
            // 
            this.N_Address.Hexadecimal = true;
            this.N_Address.Location = new System.Drawing.Point(91, 10);
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
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(2, 8);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 30);
            this.label10.TabIndex = 1;
            this.label10.Text = "アドレス";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel14
            // 
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.LabelFilter);
            this.panel14.Controls.Add(this.AddressList);
            this.panel14.Location = new System.Drawing.Point(7, 45);
            this.panel14.Margin = new System.Windows.Forms.Padding(2);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(258, 673);
            this.panel14.TabIndex = 188;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(258, 26);
            this.LabelFilter.TabIndex = 106;
            this.LabelFilter.Text = "マップ名";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressPanel
            // 
            this.AddressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressPanel.Controls.Add(this.N_ReloadListButton);
            this.AddressPanel.Controls.Add(this.label15);
            this.AddressPanel.Controls.Add(this.N_ReadCount);
            this.AddressPanel.Controls.Add(this.WriteButton);
            this.AddressPanel.Controls.Add(this.P0);
            this.AddressPanel.Controls.Add(this.label46);
            this.AddressPanel.Location = new System.Drawing.Point(267, 45);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(1317, 30);
            this.AddressPanel.TabIndex = 189;
            // 
            // N_ReloadListButton
            // 
            this.N_ReloadListButton.Location = new System.Drawing.Point(481, -1);
            this.N_ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_ReloadListButton.Name = "N_ReloadListButton";
            this.N_ReloadListButton.Size = new System.Drawing.Size(188, 32);
            this.N_ReloadListButton.TabIndex = 29;
            this.N_ReloadListButton.Text = "離脱ポイント再取得";
            this.N_ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Location = new System.Drawing.Point(307, -1);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(84, 32);
            this.label15.TabIndex = 24;
            this.label15.Text = "読込数";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_ReadCount
            // 
            this.N_ReadCount.Location = new System.Drawing.Point(397, 5);
            this.N_ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.N_ReadCount.Name = "N_ReadCount";
            this.N_ReadCount.Size = new System.Drawing.Size(78, 25);
            this.N_ReadCount.TabIndex = 28;
            this.N_ReadCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(1069, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(247, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "離脱ポインタ書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(172, 3);
            this.P0.Margin = new System.Windows.Forms.Padding(2);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 8;
            // 
            // label46
            // 
            this.label46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label46.Location = new System.Drawing.Point(-1, -1);
            this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(170, 32);
            this.label46.TabIndex = 1;
            this.label46.Text = "離脱ポインタ";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel22
            // 
            this.panel22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel22.Controls.Add(this.N_AddressListExpandsButton);
            this.panel22.Controls.Add(this.NewListAlloc);
            this.panel22.Controls.Add(this.N_AddressList);
            this.panel22.Controls.Add(this.N_LabelFilter);
            this.panel22.Location = new System.Drawing.Point(266, 76);
            this.panel22.Margin = new System.Windows.Forms.Padding(2);
            this.panel22.Name = "panel22";
            this.panel22.Size = new System.Drawing.Size(169, 642);
            this.panel22.TabIndex = 192;
            // 
            // N_AddressListExpandsButton
            // 
            this.N_AddressListExpandsButton.Location = new System.Drawing.Point(1, 611);
            this.N_AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.N_AddressListExpandsButton.Name = "N_AddressListExpandsButton";
            this.N_AddressListExpandsButton.Size = new System.Drawing.Size(166, 30);
            this.N_AddressListExpandsButton.TabIndex = 114;
            this.N_AddressListExpandsButton.Text = "リストの拡張";
            this.N_AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // NewListAlloc
            // 
            this.NewListAlloc.Location = new System.Drawing.Point(3, 611);
            this.NewListAlloc.Margin = new System.Windows.Forms.Padding(2);
            this.NewListAlloc.Name = "NewListAlloc";
            this.NewListAlloc.Size = new System.Drawing.Size(152, 30);
            this.NewListAlloc.TabIndex = 115;
            this.NewListAlloc.Text = "新規領域の確保";
            this.NewListAlloc.UseVisualStyleBackColor = true;
            this.NewListAlloc.Visible = false;
            this.NewListAlloc.Click += new System.EventHandler(this.NewListAlloc_Click);
            // 
            // N_LabelFilter
            // 
            this.N_LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.N_LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_LabelFilter.Location = new System.Drawing.Point(0, -1);
            this.N_LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_LabelFilter.Name = "N_LabelFilter";
            this.N_LabelFilter.Size = new System.Drawing.Size(170, 26);
            this.N_LabelFilter.TabIndex = 110;
            this.N_LabelFilter.Text = "名前";
            this.N_LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // X_Filter_Note_Message
            // 
            this.X_Filter_Note_Message.Controls.Add(this.X_Filter_Note_Message_Label);
            this.X_Filter_Note_Message.Controls.Add(this.X_Filter_Note_Message_Picture);
            this.X_Filter_Note_Message.Location = new System.Drawing.Point(7, 478);
            this.X_Filter_Note_Message.Name = "X_Filter_Note_Message";
            this.X_Filter_Note_Message.Size = new System.Drawing.Size(403, 68);
            this.X_Filter_Note_Message.TabIndex = 226;
            // 
            // X_Filter_Note_Message_Picture
            // 
            this.X_Filter_Note_Message_Picture.Location = new System.Drawing.Point(3, 2);
            this.X_Filter_Note_Message_Picture.Name = "X_Filter_Note_Message_Picture";
            this.X_Filter_Note_Message_Picture.Size = new System.Drawing.Size(64, 64);
            this.X_Filter_Note_Message_Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.X_Filter_Note_Message_Picture.TabIndex = 0;
            this.X_Filter_Note_Message_Picture.TabStop = false;
            // 
            // X_Filter_Note_Message_Label
            // 
            this.X_Filter_Note_Message_Label.Location = new System.Drawing.Point(73, 4);
            this.X_Filter_Note_Message_Label.Name = "X_Filter_Note_Message_Label";
            this.X_Filter_Note_Message_Label.Size = new System.Drawing.Size(324, 64);
            this.X_Filter_Note_Message_Label.TabIndex = 1;
            this.X_Filter_Note_Message_Label.Text = "これは敵のエスケープポイントです。\r\nNPC用は、左上のコンボボックスを切り替えてください。";
            // 
            // MapPictureBox
            // 
            this.MapPictureBox.AutoScroll = true;
            this.MapPictureBox.Location = new System.Drawing.Point(851, 80);
            this.MapPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.MapPictureBox.Name = "MapPictureBox";
            this.MapPictureBox.Size = new System.Drawing.Size(743, 649);
            this.MapPictureBox.TabIndex = 224;
            // 
            // N_AddressList
            // 
            this.N_AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.N_AddressList.FormattingEnabled = true;
            this.N_AddressList.IntegralHeight = false;
            this.N_AddressList.ItemHeight = 18;
            this.N_AddressList.Location = new System.Drawing.Point(1, 24);
            this.N_AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.N_AddressList.Name = "N_AddressList";
            this.N_AddressList.Size = new System.Drawing.Size(169, 580);
            this.N_AddressList.TabIndex = 109;
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
            this.AddressList.Size = new System.Drawing.Size(258, 634);
            this.AddressList.TabIndex = 1;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // N_BlockSize
            // 
            this.N_BlockSize.ErrorMessage = "";
            this.N_BlockSize.Location = new System.Drawing.Point(91, 52);
            this.N_BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_BlockSize.Name = "N_BlockSize";
            this.N_BlockSize.Placeholder = "";
            this.N_BlockSize.ReadOnly = true;
            this.N_BlockSize.Size = new System.Drawing.Size(54, 25);
            this.N_BlockSize.TabIndex = 55;
            // 
            // N_SelectAddress
            // 
            this.N_SelectAddress.ErrorMessage = "";
            this.N_SelectAddress.Location = new System.Drawing.Point(281, 52);
            this.N_SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_SelectAddress.Name = "N_SelectAddress";
            this.N_SelectAddress.Placeholder = "";
            this.N_SelectAddress.ReadOnly = true;
            this.N_SelectAddress.Size = new System.Drawing.Size(129, 25);
            this.N_SelectAddress.TabIndex = 54;
            // 
            // N_L_3_FLAG
            // 
            this.N_L_3_FLAG.ErrorMessage = "";
            this.N_L_3_FLAG.Location = new System.Drawing.Point(223, 85);
            this.N_L_3_FLAG.Margin = new System.Windows.Forms.Padding(2);
            this.N_L_3_FLAG.Name = "N_L_3_FLAG";
            this.N_L_3_FLAG.Placeholder = "";
            this.N_L_3_FLAG.ReadOnly = true;
            this.N_L_3_FLAG.Size = new System.Drawing.Size(186, 25);
            this.N_L_3_FLAG.TabIndex = 225;
            this.N_L_3_FLAG.Visible = false;
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Location = new System.Drawing.Point(79, 1);
            this.FilterComboBox.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(450, 26);
            this.FilterComboBox.TabIndex = 226;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // MapExitPointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1591, 739);
            this.Controls.Add(this.MapPictureBox);
            this.Controls.Add(this.panel22);
            this.Controls.Add(this.AddressPanel);
            this.Controls.Add(this.panel14);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "MapExitPointForm";
            this.Text = "離脱ポイント";
            this.Load += new System.EventHandler(this.MapExitPointForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B2)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Address)).EndInit();
            this.panel14.ResumeLayout(false);
            this.AddressPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            this.panel22.ResumeLayout(false);
            this.X_Filter_Note_Message.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_Filter_Note_Message_Picture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button N_WriteButton;
        private System.Windows.Forms.NumericUpDown N_Address;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown N_B2;
        private System.Windows.Forms.ComboBox N_L_2_COMBO;
        private System.Windows.Forms.NumericUpDown N_B3;
        private System.Windows.Forms.Label N_J_3;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown N_ReadCount;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Panel panel22;
        private ListBoxEx N_AddressList;
        private System.Windows.Forms.Label N_LabelFilter;
        private System.Windows.Forms.Button N_AddressListExpandsButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown N_B1;
        private System.Windows.Forms.NumericUpDown N_B0;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label N_L_0_MAPXY_1;
        private FEBuilderGBA.TextBoxEx N_BlockSize;
        private System.Windows.Forms.Label label6;
        private FEBuilderGBA.TextBoxEx N_SelectAddress;
        private System.Windows.Forms.Label label22;
        private MapPictureBox MapPictureBox;
        private System.Windows.Forms.Button N_ReloadListButton;
        private System.Windows.Forms.Button NewListAlloc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Address;
        private TextBoxEx N_L_3_FLAG;
        private ComboBoxEx FilterComboBox;
        private System.Windows.Forms.Panel X_Filter_Note_Message;
        private System.Windows.Forms.Label X_Filter_Note_Message_Label;
        private System.Windows.Forms.PictureBox X_Filter_Note_Message_Picture;
    }
}