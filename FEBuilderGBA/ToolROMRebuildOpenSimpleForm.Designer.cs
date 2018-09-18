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
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.IsSaveFileCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ApplyROMRebuildButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.UseFreeAreaComboBox = new System.Windows.Forms.ComboBox();
            this.customColorGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.UseFreeAreaComboBox);
            this.customColorGroupBox1.Controls.Add(this.label6);
            this.customColorGroupBox1.Controls.Add(this.IsSaveFileCheckBox);
            this.customColorGroupBox1.Controls.Add(this.label1);
            this.customColorGroupBox1.Controls.Add(this.ApplyROMRebuildButton);
            this.customColorGroupBox1.Controls.Add(this.label2);
            this.customColorGroupBox1.Controls.Add(this.OrignalFilename);
            this.customColorGroupBox1.Controls.Add(this.OrignalSelectButton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(1153, 317);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "ROMRebuildを開く";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(7, 212);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(196, 31);
            this.label6.TabIndex = 101;
            this.label6.Text = "フリー領域の利用";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.label1.Size = new System.Drawing.Size(939, 85);
            this.label1.TabIndex = 92;
            this.label1.Text = "ROMRebuildを開くために無改造のROMを選択してください。\r\n適応後には、動作を確認するプレイテストを行ってください。";
            // 
            // ApplyROMRebuildButton
            // 
            this.ApplyROMRebuildButton.Location = new System.Drawing.Point(207, 272);
            this.ApplyROMRebuildButton.Name = "ApplyROMRebuildButton";
            this.ApplyROMRebuildButton.Size = new System.Drawing.Size(939, 32);
            this.ApplyROMRebuildButton.TabIndex = 0;
            this.ApplyROMRebuildButton.Text = "ROMRebuildを開く";
            this.ApplyROMRebuildButton.UseVisualStyleBackColor = true;
            this.ApplyROMRebuildButton.Click += new System.EventHandler(this.ApplyROMRebuildButton_Click);
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
            // UseFreeAreaComboBox
            // 
            this.UseFreeAreaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UseFreeAreaComboBox.FormattingEnabled = true;
            this.UseFreeAreaComboBox.Items.AddRange(new object[] {
            "0=フリー領域を再利用しません",
            "1=再構築アドレスより下のアドレスで、空いている領域があれば利用する",
            "2=0x01000000より下のアドレスで、空いている領域があれば利用する"});
            this.UseFreeAreaComboBox.Location = new System.Drawing.Point(207, 212);
            this.UseFreeAreaComboBox.Name = "UseFreeAreaComboBox";
            this.UseFreeAreaComboBox.Size = new System.Drawing.Size(936, 26);
            this.UseFreeAreaComboBox.TabIndex = 102;
            // 
            // ToolROMRebuildOpenSimpleForm
            // 
            this.AcceptButton = this.ApplyROMRebuildButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1177, 335);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "ToolROMRebuildOpenSimpleForm";
            this.Text = "ROMRebuildを開く";
            this.Load += new System.EventHandler(this.ToolROMRebuildOpenSimpleForm_Load);
            this.Shown += new System.EventHandler(this.ROMRebuildOpenSimpleForm_Shown);
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox UseFreeAreaComboBox;
    }
}