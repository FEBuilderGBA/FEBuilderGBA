namespace FEBuilderGBA
{
    partial class ToolUPSOpenSimpleForm
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
            this.IsSaveFileCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ApplyUPSPatchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.IsSaveFileCheckBox);
            this.customColorGroupBox1.Controls.Add(this.label1);
            this.customColorGroupBox1.Controls.Add(this.ApplyUPSPatchButton);
            this.customColorGroupBox1.Controls.Add(this.label2);
            this.customColorGroupBox1.Controls.Add(this.OrignalFilename);
            this.customColorGroupBox1.Controls.Add(this.OrignalSelectButton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(1153, 234);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "UPSパッチ作成";
            // 
            // IsSaveFileCheckBox
            // 
            this.IsSaveFileCheckBox.AutoSize = true;
            this.IsSaveFileCheckBox.Checked = true;
            this.IsSaveFileCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsSaveFileCheckBox.Location = new System.Drawing.Point(207, 83);
            this.IsSaveFileCheckBox.Name = "IsSaveFileCheckBox";
            this.IsSaveFileCheckBox.Size = new System.Drawing.Size(483, 32);
            this.IsSaveFileCheckBox.TabIndex = 93;
            this.IsSaveFileCheckBox.Text = "UPSパッチを適応したROMを、UPSファイル名.gbaとして保存する";
            this.IsSaveFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(208, 122);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(939, 29);
            this.label1.TabIndex = 92;
            this.label1.Text = "UPSパッチを開くために無改造のROMを選択してください";
            // 
            // ApplyUPSPatchButton
            // 
            this.ApplyUPSPatchButton.Location = new System.Drawing.Point(208, 187);
            this.ApplyUPSPatchButton.Name = "ApplyUPSPatchButton";
            this.ApplyUPSPatchButton.Size = new System.Drawing.Size(939, 32);
            this.ApplyUPSPatchButton.TabIndex = 0;
            this.ApplyUPSPatchButton.Text = "UPSパッチを開く";
            this.ApplyUPSPatchButton.UseVisualStyleBackColor = true;
            this.ApplyUPSPatchButton.Click += new System.EventHandler(this.ApplyUPSPatchButton_Click);
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
            this.OrignalFilename.Location = new System.Drawing.Point(345, 32);
            this.OrignalFilename.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalFilename.Name = "OrignalFilename";
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
            // UPSOpenSimpleForm
            // 
            this.AcceptButton = this.ApplyUPSPatchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1177, 260);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "UPSOpenSimpleForm";
            this.Text = "UPS差分で保存";
            this.Load += new System.EventHandler(this.UPSPatchForm_Load);
            this.Shown += new System.EventHandler(this.UPSOpenSimpleForm_Shown);
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomColorGroupBox customColorGroupBox1;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx OrignalFilename;
        private System.Windows.Forms.Button OrignalSelectButton;
        private System.Windows.Forms.Button ApplyUPSPatchButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox IsSaveFileCheckBox;
    }
}