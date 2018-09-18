namespace FEBuilderGBA
{
    partial class PointerToolForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LoadOtherROMButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LittleEndian = new FEBuilderGBA.TextBoxEx();
            this.RefPointer = new FEBuilderGBA.TextBoxEx();
            this.OtherROMRefPointer2 = new FEBuilderGBA.TextBoxEx();
            this.OtherROMAddress2 = new FEBuilderGBA.TextBoxEx();
            this.Pointer = new FEBuilderGBA.TextBoxEx();
            this.label8 = new System.Windows.Forms.Label();
            this.DataAddress = new FEBuilderGBA.TextBoxEx();
            this.label9 = new System.Windows.Forms.Label();
            this.ERROR_ZERO1 = new System.Windows.Forms.Label();
            this.Address = new FEBuilderGBA.TextBoxEx();
            this.WhatIsButton = new System.Windows.Forms.Button();
            this.BatchButton = new System.Windows.Forms.Button();
            this.OtherROMAddressWithLDRRef = new FEBuilderGBA.TextBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.OtherROMAddressWithLDR = new FEBuilderGBA.TextBoxEx();
            this.label5 = new System.Windows.Forms.Label();
            this.OtherLoadName = new System.Windows.Forms.Label();
            this.ERROR_VERYFAR1 = new System.Windows.Forms.Label();
            this.ERROR_VERYFAR3 = new System.Windows.Forms.Label();
            this.ERROR_ZERO3 = new System.Windows.Forms.Label();
            this.groupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.WarningLevelComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.AutomaticTrackingComboBox = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.SlideComboBox = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.GrepType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.DataType = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TestMatchDataSizeComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(12, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(374, 30);
            this.label3.TabIndex = 36;
            this.label3.Text = "アドレス";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Click += new System.EventHandler(this.Address_ValueChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 83);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(374, 30);
            this.label1.TabIndex = 37;
            this.label1.Text = "リトルエンディアン";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(12, 113);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(374, 30);
            this.label2.TabIndex = 38;
            this.label2.Text = "この場所への最初の参照";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadOtherROMButton
            // 
            this.LoadOtherROMButton.Location = new System.Drawing.Point(333, 222);
            this.LoadOtherROMButton.Margin = new System.Windows.Forms.Padding(2);
            this.LoadOtherROMButton.Name = "LoadOtherROMButton";
            this.LoadOtherROMButton.Size = new System.Drawing.Size(187, 32);
            this.LoadOtherROMButton.TabIndex = 5;
            this.LoadOtherROMButton.Text = "別ROM読込";
            this.LoadOtherROMButton.UseVisualStyleBackColor = true;
            this.LoadOtherROMButton.Click += new System.EventHandler(this.LoadOtherROMButton_Click);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(13, 307);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(374, 30);
            this.label6.TabIndex = 43;
            this.label6.Text = "上記データの参照場所";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(13, 261);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(374, 46);
            this.label7.TabIndex = 42;
            this.label7.Text = "アドレス値の参照先の\r\nデータがある場所";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LittleEndian
            // 
            this.LittleEndian.ErrorMessage = "";
            this.LittleEndian.Location = new System.Drawing.Point(396, 89);
            this.LittleEndian.Margin = new System.Windows.Forms.Padding(2);
            this.LittleEndian.Name = "LittleEndian";
            this.LittleEndian.Placeholder = "";
            this.LittleEndian.ReadOnly = true;
            this.LittleEndian.Size = new System.Drawing.Size(124, 25);
            this.LittleEndian.TabIndex = 2;
            this.LittleEndian.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortCut_KeyDown);
            this.LittleEndian.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMAddress_MouseDoubleClick);
            // 
            // RefPointer
            // 
            this.RefPointer.ErrorMessage = "";
            this.RefPointer.Location = new System.Drawing.Point(396, 116);
            this.RefPointer.Margin = new System.Windows.Forms.Padding(2);
            this.RefPointer.Name = "RefPointer";
            this.RefPointer.Placeholder = "";
            this.RefPointer.ReadOnly = true;
            this.RefPointer.Size = new System.Drawing.Size(124, 25);
            this.RefPointer.TabIndex = 3;
            this.RefPointer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortCut_KeyDown);
            this.RefPointer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMAddress_MouseDoubleClick);
            // 
            // OtherROMRefPointer2
            // 
            this.OtherROMRefPointer2.ErrorMessage = "";
            this.OtherROMRefPointer2.Location = new System.Drawing.Point(398, 307);
            this.OtherROMRefPointer2.Margin = new System.Windows.Forms.Padding(2);
            this.OtherROMRefPointer2.Name = "OtherROMRefPointer2";
            this.OtherROMRefPointer2.Placeholder = "";
            this.OtherROMRefPointer2.ReadOnly = true;
            this.OtherROMRefPointer2.Size = new System.Drawing.Size(124, 25);
            this.OtherROMRefPointer2.TabIndex = 7;
            this.OtherROMRefPointer2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortCut_KeyDown);
            this.OtherROMRefPointer2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMAddress_MouseDoubleClick);
            // 
            // OtherROMAddress2
            // 
            this.OtherROMAddress2.ErrorMessage = "";
            this.OtherROMAddress2.Location = new System.Drawing.Point(398, 261);
            this.OtherROMAddress2.Margin = new System.Windows.Forms.Padding(2);
            this.OtherROMAddress2.Name = "OtherROMAddress2";
            this.OtherROMAddress2.Placeholder = "";
            this.OtherROMAddress2.ReadOnly = true;
            this.OtherROMAddress2.Size = new System.Drawing.Size(124, 25);
            this.OtherROMAddress2.TabIndex = 6;
            this.OtherROMAddress2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortCut_KeyDown);
            this.OtherROMAddress2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMAddress_MouseDoubleClick);
            // 
            // Pointer
            // 
            this.Pointer.ErrorMessage = "";
            this.Pointer.Location = new System.Drawing.Point(396, 54);
            this.Pointer.Margin = new System.Windows.Forms.Padding(2);
            this.Pointer.Name = "Pointer";
            this.Pointer.Placeholder = "";
            this.Pointer.ReadOnly = true;
            this.Pointer.Size = new System.Drawing.Size(124, 25);
            this.Pointer.TabIndex = 1;
            this.Pointer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortCut_KeyDown);
            this.Pointer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMAddress_MouseDoubleClick);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(12, 49);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(374, 30);
            this.label8.TabIndex = 51;
            this.label8.Text = "ポインタ化";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataAddress
            // 
            this.DataAddress.ErrorMessage = "";
            this.DataAddress.Location = new System.Drawing.Point(396, 160);
            this.DataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.DataAddress.Name = "DataAddress";
            this.DataAddress.Placeholder = "";
            this.DataAddress.ReadOnly = true;
            this.DataAddress.Size = new System.Drawing.Size(124, 25);
            this.DataAddress.TabIndex = 4;
            this.DataAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortCut_KeyDown);
            this.DataAddress.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMAddress_MouseDoubleClick);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(12, 156);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(374, 46);
            this.label9.TabIndex = 53;
            this.label9.Text = "アドレスがポインタの場合の\r\n指されるデータ位置";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ERROR_ZERO1
            // 
            this.ERROR_ZERO1.AutoSize = true;
            this.ERROR_ZERO1.Location = new System.Drawing.Point(12, 358);
            this.ERROR_ZERO1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ERROR_ZERO1.Name = "ERROR_ZERO1";
            this.ERROR_ZERO1.Size = new System.Drawing.Size(123, 18);
            this.ERROR_ZERO1.TabIndex = 59;
            this.ERROR_ZERO1.Text = "警告:0地帯です";
            // 
            // Address
            // 
            this.Address.ErrorMessage = "";
            this.Address.Location = new System.Drawing.Point(396, 17);
            this.Address.Margin = new System.Windows.Forms.Padding(4);
            this.Address.Name = "Address";
            this.Address.Placeholder = "";
            this.Address.Size = new System.Drawing.Size(126, 25);
            this.Address.TabIndex = 0;
            this.Address.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Address_KeyDown);
            // 
            // WhatIsButton
            // 
            this.WhatIsButton.Location = new System.Drawing.Point(639, 589);
            this.WhatIsButton.Margin = new System.Windows.Forms.Padding(2);
            this.WhatIsButton.Name = "WhatIsButton";
            this.WhatIsButton.Size = new System.Drawing.Size(360, 32);
            this.WhatIsButton.TabIndex = 13;
            this.WhatIsButton.Text = "アドレスの種類判定";
            this.WhatIsButton.UseVisualStyleBackColor = true;
            this.WhatIsButton.Click += new System.EventHandler(this.WhatIsButton_Click);
            // 
            // BatchButton
            // 
            this.BatchButton.Location = new System.Drawing.Point(639, 538);
            this.BatchButton.Margin = new System.Windows.Forms.Padding(2);
            this.BatchButton.Name = "BatchButton";
            this.BatchButton.Size = new System.Drawing.Size(360, 32);
            this.BatchButton.TabIndex = 12;
            this.BatchButton.Text = "バッチ (一括処理)";
            this.BatchButton.UseVisualStyleBackColor = true;
            this.BatchButton.Click += new System.EventHandler(this.BatchButton_Click);
            // 
            // OtherROMAddressWithLDRRef
            // 
            this.OtherROMAddressWithLDRRef.ErrorMessage = "";
            this.OtherROMAddressWithLDRRef.Location = new System.Drawing.Point(396, 443);
            this.OtherROMAddressWithLDRRef.Margin = new System.Windows.Forms.Padding(2);
            this.OtherROMAddressWithLDRRef.Name = "OtherROMAddressWithLDRRef";
            this.OtherROMAddressWithLDRRef.Placeholder = "";
            this.OtherROMAddressWithLDRRef.ReadOnly = true;
            this.OtherROMAddressWithLDRRef.Size = new System.Drawing.Size(124, 25);
            this.OtherROMAddressWithLDRRef.TabIndex = 11;
            this.OtherROMAddressWithLDRRef.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortCut_KeyDown);
            this.OtherROMAddressWithLDRRef.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMAddress_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(11, 441);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(374, 30);
            this.label4.TabIndex = 69;
            this.label4.Text = "上記データの参照場所";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OtherROMAddressWithLDR
            // 
            this.OtherROMAddressWithLDR.ErrorMessage = "";
            this.OtherROMAddressWithLDR.Location = new System.Drawing.Point(396, 395);
            this.OtherROMAddressWithLDR.Margin = new System.Windows.Forms.Padding(2);
            this.OtherROMAddressWithLDR.Name = "OtherROMAddressWithLDR";
            this.OtherROMAddressWithLDR.Placeholder = "";
            this.OtherROMAddressWithLDR.ReadOnly = true;
            this.OtherROMAddressWithLDR.Size = new System.Drawing.Size(124, 25);
            this.OtherROMAddressWithLDR.TabIndex = 10;
            this.OtherROMAddressWithLDR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortCut_KeyDown);
            this.OtherROMAddressWithLDR.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMAddress_MouseDoubleClick);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(11, 395);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(374, 46);
            this.label5.TabIndex = 68;
            this.label5.Text = "参照値から追跡\r\nマッチアドレス";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OtherLoadName
            // 
            this.OtherLoadName.AutoSize = true;
            this.OtherLoadName.Location = new System.Drawing.Point(15, 233);
            this.OtherLoadName.Name = "OtherLoadName";
            this.OtherLoadName.Size = new System.Drawing.Size(128, 18);
            this.OtherLoadName.TabIndex = 70;
            this.OtherLoadName.Text = "LoadROMNAME";
            // 
            // ERROR_VERYFAR1
            // 
            this.ERROR_VERYFAR1.AutoSize = true;
            this.ERROR_VERYFAR1.Location = new System.Drawing.Point(12, 358);
            this.ERROR_VERYFAR1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ERROR_VERYFAR1.Name = "ERROR_VERYFAR1";
            this.ERROR_VERYFAR1.Size = new System.Drawing.Size(182, 18);
            this.ERROR_VERYFAR1.TabIndex = 61;
            this.ERROR_VERYFAR1.Text = "警告:元データと離れすぎ";
            // 
            // ERROR_VERYFAR3
            // 
            this.ERROR_VERYFAR3.AutoSize = true;
            this.ERROR_VERYFAR3.Location = new System.Drawing.Point(15, 481);
            this.ERROR_VERYFAR3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ERROR_VERYFAR3.Name = "ERROR_VERYFAR3";
            this.ERROR_VERYFAR3.Size = new System.Drawing.Size(182, 18);
            this.ERROR_VERYFAR3.TabIndex = 72;
            this.ERROR_VERYFAR3.Text = "警告:元データと離れすぎ";
            // 
            // ERROR_ZERO3
            // 
            this.ERROR_ZERO3.AutoSize = true;
            this.ERROR_ZERO3.Location = new System.Drawing.Point(15, 481);
            this.ERROR_ZERO3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ERROR_ZERO3.Name = "ERROR_ZERO3";
            this.ERROR_ZERO3.Size = new System.Drawing.Size(123, 18);
            this.ERROR_ZERO3.TabIndex = 71;
            this.ERROR_ZERO3.Text = "警告:0地帯です";
            // 
            // groupBox1
            // 
            this.groupBox1.BorderColor = System.Drawing.Color.Empty;
            this.groupBox1.Controls.Add(this.WarningLevelComboBox);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.AutomaticTrackingComboBox);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.SlideComboBox);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.GrepType);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.DataType);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.TestMatchDataSizeComboBox);
            this.groupBox1.Location = new System.Drawing.Point(537, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(467, 521);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "検索オプション";
            // 
            // WarningLevelComboBox
            // 
            this.WarningLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.WarningLevelComboBox.FormattingEnabled = true;
            this.WarningLevelComboBox.Items.AddRange(new object[] {
            "警告をエラーにする",
            "参照あれば警告を無視する",
            "警告を無視する"});
            this.WarningLevelComboBox.Location = new System.Drawing.Point(211, 245);
            this.WarningLevelComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.WarningLevelComboBox.Name = "WarningLevelComboBox";
            this.WarningLevelComboBox.Size = new System.Drawing.Size(250, 26);
            this.WarningLevelComboBox.TabIndex = 66;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(0, 241);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(206, 30);
            this.label10.TabIndex = 65;
            this.label10.Text = "警告システム";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Location = new System.Drawing.Point(0, 212);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(206, 30);
            this.label17.TabIndex = 64;
            this.label17.Text = "自動追跡システム";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AutomaticTrackingComboBox
            // 
            this.AutomaticTrackingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.AutomaticTrackingComboBox.FormattingEnabled = true;
            this.AutomaticTrackingComboBox.Items.AddRange(new object[] {
            "0000=自動追跡しない",
            "0500=サイズだけ追記1",
            "0A00=サイズだけ追記2",
            "0F00=サイズだけ追記3",
            "0502=追跡レベル2",
            "0505=追跡レベル3",
            "0A02=追跡レベル4",
            "0A05=追跡レベル5",
            "0A07=追跡レベル6",
            "0F07=追跡レベル7"});
            this.AutomaticTrackingComboBox.Location = new System.Drawing.Point(211, 215);
            this.AutomaticTrackingComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.AutomaticTrackingComboBox.Name = "AutomaticTrackingComboBox";
            this.AutomaticTrackingComboBox.Size = new System.Drawing.Size(250, 26);
            this.AutomaticTrackingComboBox.TabIndex = 4;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(0, 131);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(206, 30);
            this.label16.TabIndex = 62;
            this.label16.Text = "スライドして追加検索";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SlideComboBox
            // 
            this.SlideComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.SlideComboBox.FormattingEnabled = true;
            this.SlideComboBox.Items.AddRange(new object[] {
            "0=追加検索しない",
            "2=2byte",
            "4=4byte",
            "6=6byte",
            "8=8byte",
            "C=12byte",
            "10=16byte",
            "14=20byte"});
            this.SlideComboBox.Location = new System.Drawing.Point(211, 134);
            this.SlideComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.SlideComboBox.Name = "SlideComboBox";
            this.SlideComboBox.Size = new System.Drawing.Size(250, 26);
            this.SlideComboBox.TabIndex = 3;
            this.SlideComboBox.SelectedIndexChanged += new System.EventHandler(this.Address_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 322);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(303, 108);
            this.label15.TabIndex = 60;
            this.label15.Text = "現在のROMと、\r\n他のバージョンのROMをバイナリ比較して、\r\n特定のデータがどこにあるかを特定します。\r\nたとえば、画像やプログラムなどは\r\nFE7,FE8共" +
    "通して使われているので、\r\nどこに移動したか検索することができます。";
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(0, 100);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(206, 30);
            this.label14.TabIndex = 59;
            this.label14.Text = "比較方法";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GrepType
            // 
            this.GrepType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.GrepType.FormattingEnabled = true;
            this.GrepType.Items.AddRange(new object[] {
            "0=完全一致",
            "1=パターンマッチ"});
            this.GrepType.Location = new System.Drawing.Point(212, 103);
            this.GrepType.Margin = new System.Windows.Forms.Padding(2);
            this.GrepType.Name = "GrepType";
            this.GrepType.Size = new System.Drawing.Size(250, 26);
            this.GrepType.TabIndex = 2;
            this.GrepType.SelectedIndexChanged += new System.EventHandler(this.Address_ValueChanged);
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(0, 69);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(206, 30);
            this.label13.TabIndex = 57;
            this.label13.Text = "内容";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataType
            // 
            this.DataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.DataType.FormattingEnabled = true;
            this.DataType.Items.AddRange(new object[] {
            "0=DATA",
            "1=ASM"});
            this.DataType.Location = new System.Drawing.Point(212, 72);
            this.DataType.Margin = new System.Windows.Forms.Padding(2);
            this.DataType.Name = "DataType";
            this.DataType.Size = new System.Drawing.Size(250, 26);
            this.DataType.TabIndex = 1;
            this.DataType.SelectedIndexChanged += new System.EventHandler(this.Address_ValueChanged);
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Location = new System.Drawing.Point(0, 37);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(206, 30);
            this.label12.TabIndex = 55;
            this.label12.Text = "比較サイズ";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestMatchDataSizeComboBox
            // 
            this.TestMatchDataSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.TestMatchDataSizeComboBox.FormattingEnabled = true;
            this.TestMatchDataSizeComboBox.Items.AddRange(new object[] {
            "100=512byte",
            "FF=256byte",
            "80=128byte",
            "60=96byte",
            "40=64byte",
            "30=48byte",
            "20=32byte",
            "1E=30byte",
            "1C=28byte",
            "1A=26byte",
            "18=24byte",
            "16=22byte",
            "14=20byte",
            "12=18byte",
            "10=16byte",
            "0E=14byte",
            "0C=12byte",
            "0A=10byte",
            "08=8byte",
            "06=6byte",
            "04=4byte"});
            this.TestMatchDataSizeComboBox.Location = new System.Drawing.Point(212, 41);
            this.TestMatchDataSizeComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.TestMatchDataSizeComboBox.Name = "TestMatchDataSizeComboBox";
            this.TestMatchDataSizeComboBox.Size = new System.Drawing.Size(250, 26);
            this.TestMatchDataSizeComboBox.TabIndex = 0;
            this.TestMatchDataSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.Address_ValueChanged);
            // 
            // PointerToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1006, 632);
            this.Controls.Add(this.ERROR_VERYFAR3);
            this.Controls.Add(this.ERROR_ZERO3);
            this.Controls.Add(this.OtherLoadName);
            this.Controls.Add(this.OtherROMAddressWithLDRRef);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.OtherROMAddressWithLDR);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BatchButton);
            this.Controls.Add(this.WhatIsButton);
            this.Controls.Add(this.ERROR_VERYFAR1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Address);
            this.Controls.Add(this.ERROR_ZERO1);
            this.Controls.Add(this.DataAddress);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Pointer);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.OtherROMRefPointer2);
            this.Controls.Add(this.OtherROMAddress2);
            this.Controls.Add(this.RefPointer);
            this.Controls.Add(this.LittleEndian);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.LoadOtherROMButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PointerToolForm";
            this.Text = "ポインタ計算ツール";
            this.Load += new System.EventHandler(this.PointerToolForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button LoadOtherROMButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private FEBuilderGBA.TextBoxEx LittleEndian;
        private FEBuilderGBA.TextBoxEx RefPointer;
        private FEBuilderGBA.TextBoxEx OtherROMRefPointer2;
        private FEBuilderGBA.TextBoxEx OtherROMAddress2;
        private FEBuilderGBA.TextBoxEx Pointer;
        private System.Windows.Forms.Label label8;
        private FEBuilderGBA.TextBoxEx DataAddress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox TestMatchDataSizeComboBox;
        private System.Windows.Forms.Label ERROR_ZERO1;
        private FEBuilderGBA.TextBoxEx Address;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox GrepType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox DataType;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private CustomColorGroupBox groupBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox SlideComboBox;
        private System.Windows.Forms.Button WhatIsButton;
        private System.Windows.Forms.Button BatchButton;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox AutomaticTrackingComboBox;
        private FEBuilderGBA.TextBoxEx OtherROMAddressWithLDRRef;
        private System.Windows.Forms.Label label4;
        private FEBuilderGBA.TextBoxEx OtherROMAddressWithLDR;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label OtherLoadName;
        private System.Windows.Forms.Label ERROR_VERYFAR1;
        private System.Windows.Forms.Label ERROR_VERYFAR3;
        private System.Windows.Forms.Label ERROR_ZERO3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox WarningLevelComboBox;
    }
}