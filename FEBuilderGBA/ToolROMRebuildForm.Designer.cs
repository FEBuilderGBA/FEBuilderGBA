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
            this.UseFreeAreaComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RebuildAddress = new System.Windows.Forms.NumericUpDown();
            this.DefragCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RebuildAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // MakeROMRebuildPatchButton
            // 
            this.MakeROMRebuildPatchButton.Location = new System.Drawing.Point(209, 537);
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
            this.customColorGroupBox1.Controls.Add(this.UseFreeAreaComboBox);
            this.customColorGroupBox1.Controls.Add(this.label6);
            this.customColorGroupBox1.Controls.Add(this.label5);
            this.customColorGroupBox1.Controls.Add(this.label4);
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
            this.customColorGroupBox1.Size = new System.Drawing.Size(1153, 576);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "ROMリビルド";
            // 
            // UseFreeAreaComboBox
            // 
            this.UseFreeAreaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UseFreeAreaComboBox.FormattingEnabled = true;
            this.UseFreeAreaComboBox.Items.AddRange(new object[] {
            "0=フリー領域を再利用しません",
            "1=再構築アドレスより下のアドレスで、空いている領域があれば利用する",
            "2=0x01000000より下のアドレスで、空いている領域があれば利用する"});
            this.UseFreeAreaComboBox.Location = new System.Drawing.Point(210, 440);
            this.UseFreeAreaComboBox.Name = "UseFreeAreaComboBox";
            this.UseFreeAreaComboBox.Size = new System.Drawing.Size(936, 26);
            this.UseFreeAreaComboBox.TabIndex = 100;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(7, 436);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(196, 31);
            this.label6.TabIndex = 98;
            this.label6.Text = "フリー領域の利用";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(207, 347);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(939, 63);
            this.label5.TabIndex = 97;
            this.label5.Text = "通称は変更しないでください。\r\n再構築するエリアを指定します。";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(7, 310);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 31);
            this.label4.TabIndex = 96;
            this.label4.Text = "再構築アドレス";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ClientSize = new System.Drawing.Size(1177, 600);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "ToolROMRebuildForm";
            this.Text = "ROMリビルド";
            this.Load += new System.EventHandler(this.ROMRebuildForm_Load);
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox UseFreeAreaComboBox;
    }
}