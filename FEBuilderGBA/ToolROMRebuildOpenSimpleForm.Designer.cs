namespace FEBuilderGBA
{
    partial class ToolROMRebuildOpenSimpleForm
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
            this.ApplyROMRebuildButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.X_FreeAreaDef = new System.Windows.Forms.Panel();
            this.FreeAreaStartAddress = new System.Windows.Forms.NumericUpDown();
            this.FreeAreaMinimumSize = new System.Windows.Forms.NumericUpDown();
            this.X_FreeAreaStartAddress = new System.Windows.Forms.Label();
            this.X_FreeAreaMinimumSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UseFreeAreaComboBox = new System.Windows.Forms.ComboBox();
            this.X_UseFreeArea = new System.Windows.Forms.Label();
            this.IsSaveFileCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1.SuspendLayout();
            this.X_FreeAreaDef.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FreeAreaStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreeAreaMinimumSize)).BeginInit();
            this.SuspendLayout();
            // 
            // ApplyROMRebuildButton
            // 
            this.ApplyROMRebuildButton.Location = new System.Drawing.Point(208, 300);
            this.ApplyROMRebuildButton.Name = "ApplyROMRebuildButton";
            this.ApplyROMRebuildButton.Size = new System.Drawing.Size(939, 32);
            this.ApplyROMRebuildButton.TabIndex = 0;
            this.ApplyROMRebuildButton.Text = "ROMRebuildを開く";
            this.ApplyROMRebuildButton.UseVisualStyleBackColor = true;
            this.ApplyROMRebuildButton.Click += new System.EventHandler(this.ApplyROMRebuildButton_Click);
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.X_FreeAreaDef);
            this.customColorGroupBox1.Controls.Add(this.UseFreeAreaComboBox);
            this.customColorGroupBox1.Controls.Add(this.X_UseFreeArea);
            this.customColorGroupBox1.Controls.Add(this.IsSaveFileCheckBox);
            this.customColorGroupBox1.Controls.Add(this.label1);
            this.customColorGroupBox1.Controls.Add(this.ApplyROMRebuildButton);
            this.customColorGroupBox1.Controls.Add(this.label2);
            this.customColorGroupBox1.Controls.Add(this.OrignalFilename);
            this.customColorGroupBox1.Controls.Add(this.OrignalSelectButton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(1153, 337);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "ROMRebuildを開く";
            // 
            // X_FreeAreaDef
            // 
            this.X_FreeAreaDef.Controls.Add(this.FreeAreaStartAddress);
            this.X_FreeAreaDef.Controls.Add(this.FreeAreaMinimumSize);
            this.X_FreeAreaDef.Controls.Add(this.X_FreeAreaStartAddress);
            this.X_FreeAreaDef.Controls.Add(this.X_FreeAreaMinimumSize);
            this.X_FreeAreaDef.Controls.Add(this.label3);
            this.X_FreeAreaDef.Location = new System.Drawing.Point(7, 222);
            this.X_FreeAreaDef.Name = "X_FreeAreaDef";
            this.X_FreeAreaDef.Size = new System.Drawing.Size(1140, 62);
            this.X_FreeAreaDef.TabIndex = 104;
            // 
            // FreeAreaStartAddress
            // 
            this.FreeAreaStartAddress.Hexadecimal = true;
            this.FreeAreaStartAddress.Location = new System.Drawing.Point(403, 34);
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
            this.FreeAreaMinimumSize.Location = new System.Drawing.Point(403, 4);
            this.FreeAreaMinimumSize.Maximum = new decimal(new int[] {
            9999999,
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
            this.X_FreeAreaStartAddress.Size = new System.Drawing.Size(196, 31);
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
            this.X_FreeAreaMinimumSize.Size = new System.Drawing.Size(196, 31);
            this.X_FreeAreaMinimumSize.TabIndex = 104;
            this.X_FreeAreaMinimumSize.Text = "nullの連続数";
            this.X_FreeAreaMinimumSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(1, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 31);
            this.label3.TabIndex = 103;
            this.label3.Text = "フリー領域の定義";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UseFreeAreaComboBox
            // 
            this.UseFreeAreaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UseFreeAreaComboBox.FormattingEnabled = true;
            this.UseFreeAreaComboBox.Items.AddRange(new object[] {
            "0=フリー領域を再利用しません",
            "1=再構築アドレスより下のアドレスで、空いている領域があれば利用する",
            "2=0x01000000より下のアドレスで、空いている領域があれば利用する"});
            this.UseFreeAreaComboBox.Location = new System.Drawing.Point(207, 188);
            this.UseFreeAreaComboBox.Name = "UseFreeAreaComboBox";
            this.UseFreeAreaComboBox.Size = new System.Drawing.Size(936, 26);
            this.UseFreeAreaComboBox.TabIndex = 102;
            this.UseFreeAreaComboBox.SelectedIndexChanged += new System.EventHandler(this.UseFreeAreaComboBox_SelectedIndexChanged);
            // 
            // X_UseFreeArea
            // 
            this.X_UseFreeArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_UseFreeArea.Location = new System.Drawing.Point(7, 188);
            this.X_UseFreeArea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X_UseFreeArea.Name = "X_UseFreeArea";
            this.X_UseFreeArea.Size = new System.Drawing.Size(196, 31);
            this.X_UseFreeArea.TabIndex = 101;
            this.X_UseFreeArea.Text = "フリー領域の利用";
            this.X_UseFreeArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IsSaveFileCheckBox
            // 
            this.IsSaveFileCheckBox.AutoSize = true;
            this.IsSaveFileCheckBox.Checked = true;
            this.IsSaveFileCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsSaveFileCheckBox.Location = new System.Drawing.Point(207, 83);
            this.IsSaveFileCheckBox.Name = "IsSaveFileCheckBox";
            this.IsSaveFileCheckBox.Size = new System.Drawing.Size(468, 22);
            this.IsSaveFileCheckBox.TabIndex = 93;
            this.IsSaveFileCheckBox.Text = "ROMRebuildを適応したROMを、ファイル名.gbaとして保存する";
            this.IsSaveFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(208, 122);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(939, 62);
            this.label1.TabIndex = 92;
            this.label1.Text = "ROMRebuildを開くために無改造のROMを選択してください。\r\n適応後には、動作を確認するプレイテストを行ってください。";
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
            // ToolROMRebuildOpenSimpleForm
            // 
            this.AcceptButton = this.ApplyROMRebuildButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1177, 358);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "ToolROMRebuildOpenSimpleForm";
            this.Text = "ROMRebuildを開く";
            this.Load += new System.EventHandler(this.ToolROMRebuildOpenSimpleForm_Load);
            this.Shown += new System.EventHandler(this.ROMRebuildOpenSimpleForm_Shown);
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            this.X_FreeAreaDef.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FreeAreaStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreeAreaMinimumSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomColorGroupBox customColorGroupBox1;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx OrignalFilename;
        private System.Windows.Forms.Button OrignalSelectButton;
        private System.Windows.Forms.Button ApplyROMRebuildButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox IsSaveFileCheckBox;
        private System.Windows.Forms.Label X_UseFreeArea;
        private System.Windows.Forms.ComboBox UseFreeAreaComboBox;
        private System.Windows.Forms.Panel X_FreeAreaDef;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label X_FreeAreaMinimumSize;
        private System.Windows.Forms.Label X_FreeAreaStartAddress;
        private System.Windows.Forms.NumericUpDown FreeAreaStartAddress;
        private System.Windows.Forms.NumericUpDown FreeAreaMinimumSize;
    }
}