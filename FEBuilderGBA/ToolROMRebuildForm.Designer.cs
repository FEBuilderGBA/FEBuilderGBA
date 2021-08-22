namespace FEBuilderGBA
{
    partial class ToolROMRebuildForm
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
            this.MakeROMRebuildPatchButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.X_FreeAreaDef = new System.Windows.Forms.Panel();
            this.FreeAreaStartAddress = new System.Windows.Forms.NumericUpDown();
            this.FreeAreaMinimumSize = new System.Windows.Forms.NumericUpDown();
            this.X_FreeAreaStartAddress = new System.Windows.Forms.Label();
            this.X_FreeAreaMinimumSize = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.UseFreeAreaComboBox = new System.Windows.Forms.ComboBox();
            this.X_UseFreeArea = new System.Windows.Forms.Label();
            this.X_RebuildAddress = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RebuildAddress = new System.Windows.Forms.NumericUpDown();
            this.DefragCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1.SuspendLayout();
            this.X_FreeAreaDef.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FreeAreaStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreeAreaMinimumSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RebuildAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // MakeROMRebuildPatchButton
            // 
            this.MakeROMRebuildPatchButton.Location = new System.Drawing.Point(209, 484);
            this.MakeROMRebuildPatchButton.Name = "MakeROMRebuildPatchButton";
            this.MakeROMRebuildPatchButton.Size = new System.Drawing.Size(939, 32);
            this.MakeROMRebuildPatchButton.TabIndex = 0;
            this.MakeROMRebuildPatchButton.Text = "変更点をファイルに書きだす";
            this.MakeROMRebuildPatchButton.UseVisualStyleBackColor = true;
            this.MakeROMRebuildPatchButton.Click += new System.EventHandler(this.MakeROMRebuildButton_Click);
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.X_FreeAreaDef);
            this.customColorGroupBox1.Controls.Add(this.UseFreeAreaComboBox);
            this.customColorGroupBox1.Controls.Add(this.X_UseFreeArea);
            this.customColorGroupBox1.Controls.Add(this.X_RebuildAddress);
            this.customColorGroupBox1.Controls.Add(this.label3);
            this.customColorGroupBox1.Controls.Add(this.RebuildAddress);
            this.customColorGroupBox1.Controls.Add(this.DefragCheckBox);
            this.customColorGroupBox1.Controls.Add(this.label1);
            this.customColorGroupBox1.Controls.Add(this.MakeROMRebuildPatchButton);
            this.customColorGroupBox1.Controls.Add(this.label2);
            this.customColorGroupBox1.Controls.Add(this.OrignalFilename);
            this.customColorGroupBox1.Controls.Add(this.OrignalSelectButton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(1153, 523);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "ROMリビルド";
            // 
            // X_FreeAreaDef
            // 
            this.X_FreeAreaDef.Controls.Add(this.FreeAreaStartAddress);
            this.X_FreeAreaDef.Controls.Add(this.FreeAreaMinimumSize);
            this.X_FreeAreaDef.Controls.Add(this.X_FreeAreaStartAddress);
            this.X_FreeAreaDef.Controls.Add(this.X_FreeAreaMinimumSize);
            this.X_FreeAreaDef.Controls.Add(this.label7);
            this.X_FreeAreaDef.Location = new System.Drawing.Point(7, 384);
            this.X_FreeAreaDef.Name = "X_FreeAreaDef";
            this.X_FreeAreaDef.Size = new System.Drawing.Size(1140, 62);
            this.X_FreeAreaDef.TabIndex = 105;
            // 
            // FreeAreaStartAddress
            // 
            this.FreeAreaStartAddress.Hexadecimal = true;
            this.FreeAreaStartAddress.Location = new System.Drawing.Point(450, 34);
            this.FreeAreaStartAddress.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.FreeAreaStartAddress.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.FreeAreaStartAddress.Name = "FreeAreaStartAddress";
            this.FreeAreaStartAddress.Size = new System.Drawing.Size(128, 25);
            this.FreeAreaStartAddress.TabIndex = 107;
            this.FreeAreaStartAddress.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // FreeAreaMinimumSize
            // 
            this.FreeAreaMinimumSize.Location = new System.Drawing.Point(450, 4);
            this.FreeAreaMinimumSize.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.FreeAreaMinimumSize.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.FreeAreaMinimumSize.Name = "FreeAreaMinimumSize";
            this.FreeAreaMinimumSize.Size = new System.Drawing.Size(128, 25);
            this.FreeAreaMinimumSize.TabIndex = 106;
            this.FreeAreaMinimumSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // X_FreeAreaStartAddress
            // 
            this.X_FreeAreaStartAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_FreeAreaStartAddress.Location = new System.Drawing.Point(200, 30);
            this.X_FreeAreaStartAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X_FreeAreaStartAddress.Name = "X_FreeAreaStartAddress";
            this.X_FreeAreaStartAddress.Size = new System.Drawing.Size(243, 31);
            this.X_FreeAreaStartAddress.TabIndex = 105;
            this.X_FreeAreaStartAddress.Text = "探索開始アドレス";
            this.X_FreeAreaStartAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_FreeAreaMinimumSize
            // 
            this.X_FreeAreaMinimumSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_FreeAreaMinimumSize.Location = new System.Drawing.Point(200, 0);
            this.X_FreeAreaMinimumSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X_FreeAreaMinimumSize.Name = "X_FreeAreaMinimumSize";
            this.X_FreeAreaMinimumSize.Size = new System.Drawing.Size(243, 31);
            this.X_FreeAreaMinimumSize.TabIndex = 104;
            this.X_FreeAreaMinimumSize.Text = "nullの連続数";
            this.X_FreeAreaMinimumSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(1, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(196, 31);
            this.label7.TabIndex = 103;
            this.label7.Text = "フリー領域の定義";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UseFreeAreaComboBox
            // 
            this.UseFreeAreaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UseFreeAreaComboBox.FormattingEnabled = true;
            this.UseFreeAreaComboBox.Items.AddRange(new object[] {
            "0=フリー領域を再利用しません",
            "1=再構築アドレスより下のアドレスで、空いている領域があれば利用する",
            "2=0x01000000より下のアドレスで、空いている領域があれば利用する"});
            this.UseFreeAreaComboBox.Location = new System.Drawing.Point(207, 351);
            this.UseFreeAreaComboBox.Name = "UseFreeAreaComboBox";
            this.UseFreeAreaComboBox.Size = new System.Drawing.Size(936, 26);
            this.UseFreeAreaComboBox.TabIndex = 100;
            this.UseFreeAreaComboBox.SelectedIndexChanged += new System.EventHandler(this.UseFreeAreaComboBox_SelectedIndexChanged);
            // 
            // X_UseFreeArea
            // 
            this.X_UseFreeArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_UseFreeArea.Location = new System.Drawing.Point(7, 348);
            this.X_UseFreeArea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X_UseFreeArea.Name = "X_UseFreeArea";
            this.X_UseFreeArea.Size = new System.Drawing.Size(196, 31);
            this.X_UseFreeArea.TabIndex = 98;
            this.X_UseFreeArea.Text = "フリー領域の利用";
            this.X_UseFreeArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_RebuildAddress
            // 
            this.X_RebuildAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_RebuildAddress.Location = new System.Drawing.Point(7, 310);
            this.X_RebuildAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X_RebuildAddress.Name = "X_RebuildAddress";
            this.X_RebuildAddress.Size = new System.Drawing.Size(196, 31);
            this.X_RebuildAddress.TabIndex = 96;
            this.X_RebuildAddress.Text = "再構築アドレス";
            this.X_RebuildAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(7, 261);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 31);
            this.label3.TabIndex = 95;
            this.label3.Text = "自動適応";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RebuildAddress
            // 
            this.RebuildAddress.Hexadecimal = true;
            this.RebuildAddress.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.RebuildAddress.Location = new System.Drawing.Point(209, 313);
            this.RebuildAddress.Margin = new System.Windows.Forms.Padding(2);
            this.RebuildAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.RebuildAddress.Name = "RebuildAddress";
            this.RebuildAddress.Size = new System.Drawing.Size(160, 25);
            this.RebuildAddress.TabIndex = 94;
            // 
            // DefragCheckBox
            // 
            this.DefragCheckBox.AutoSize = true;
            this.DefragCheckBox.Checked = true;
            this.DefragCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DefragCheckBox.Location = new System.Drawing.Point(208, 265);
            this.DefragCheckBox.Name = "DefragCheckBox";
            this.DefragCheckBox.Size = new System.Drawing.Size(422, 22);
            this.DefragCheckBox.TabIndex = 93;
            this.DefragCheckBox.Text = "書出しが終わったらインポートを行う(デフラグを実行する)";
            this.DefragCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(207, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(939, 166);
            this.label1.TabIndex = 92;
            this.label1.Text = "変更されている設定をファイルに書きだします。\r\n\r\nUPSパッチと比較して以下のメリットがあります。\r\n無改造ROMにこのデータをインポートすることで、断片化した" +
    "無駄な領域を節約できます。\r\n\r\n逆に以下のデメリットがあります。\r\nFEBuilderGBAが知らないデータは、ダンプされません。\r\nたとえばバイナリエディタ" +
    "で変更した内容などは保存されません。";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = "@VanillaROM";
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(7, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 31);
            this.label2.TabIndex = 91;
            this.label2.Text = "無改造ROM";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OrignalFilename
            // 
            this.OrignalFilename.ErrorMessage = "";
            this.OrignalFilename.Location = new System.Drawing.Point(345, 32);
            this.OrignalFilename.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalFilename.Name = "OrignalFilename";
            this.OrignalFilename.Placeholder = "";
            this.OrignalFilename.Size = new System.Drawing.Size(801, 25);
            this.OrignalFilename.TabIndex = 3;
            this.OrignalFilename.DoubleClick += new System.EventHandler(this.OrignalFilename_DoubleClick);
            // 
            // OrignalSelectButton
            // 
            this.OrignalSelectButton.Location = new System.Drawing.Point(207, 29);
            this.OrignalSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalSelectButton.Name = "OrignalSelectButton";
            this.OrignalSelectButton.Size = new System.Drawing.Size(130, 31);
            this.OrignalSelectButton.TabIndex = 1;
            this.OrignalSelectButton.Text = "ファイル選択";
            this.OrignalSelectButton.UseVisualStyleBackColor = true;
            this.OrignalSelectButton.Click += new System.EventHandler(this.OrignalSelectButton_Click);
            // 
            // ToolROMRebuildForm
            // 
            this.AcceptButton = this.MakeROMRebuildPatchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1177, 540);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "ToolROMRebuildForm";
            this.Text = "ROMリビルド";
            this.Load += new System.EventHandler(this.ROMRebuildForm_Load);
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            this.X_FreeAreaDef.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FreeAreaStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreeAreaMinimumSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RebuildAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomColorGroupBox customColorGroupBox1;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx OrignalFilename;
        private System.Windows.Forms.Button OrignalSelectButton;
        private System.Windows.Forms.Button MakeROMRebuildPatchButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DefragCheckBox;
        private System.Windows.Forms.NumericUpDown RebuildAddress;
        private System.Windows.Forms.Label X_RebuildAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label X_UseFreeArea;
        private System.Windows.Forms.ComboBox UseFreeAreaComboBox;
        private System.Windows.Forms.Panel X_FreeAreaDef;
        private System.Windows.Forms.NumericUpDown FreeAreaStartAddress;
        private System.Windows.Forms.NumericUpDown FreeAreaMinimumSize;
        private System.Windows.Forms.Label X_FreeAreaStartAddress;
        private System.Windows.Forms.Label X_FreeAreaMinimumSize;
        private System.Windows.Forms.Label label7;
    }
}