namespace FEBuilderGBA
{
    partial class ToolUPSPatchSimpleForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.MakeUPSPatchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.label1);
            this.customColorGroupBox1.Controls.Add(this.MakeUPSPatchButton);
            this.customColorGroupBox1.Controls.Add(this.label2);
            this.customColorGroupBox1.Controls.Add(this.OrignalFilename);
            this.customColorGroupBox1.Controls.Add(this.OrignalSelectButton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(1153, 234);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "UPSで保存する";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(207, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(939, 73);
            this.label1.TabIndex = 92;
            this.label1.Text = "現在のデータをUPSパッチとして保存します。\r\n特別な理由がない限り、通常の保存をしたあとで利用してください。";
            // 
            // MakeUPSPatchButton
            // 
            this.MakeUPSPatchButton.Location = new System.Drawing.Point(208, 187);
            this.MakeUPSPatchButton.Name = "MakeUPSPatchButton";
            this.MakeUPSPatchButton.Size = new System.Drawing.Size(939, 32);
            this.MakeUPSPatchButton.TabIndex = 0;
            this.MakeUPSPatchButton.Text = "差分をUPSパッチとして作成する";
            this.MakeUPSPatchButton.UseVisualStyleBackColor = true;
            this.MakeUPSPatchButton.Click += new System.EventHandler(this.MakeUPSPatchButton_Click);
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
            // ToolUPSPatchSimpleForm
            // 
            this.AcceptButton = this.MakeUPSPatchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1177, 260);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "ToolUPSPatchSimpleForm";
            this.Text = "UPSで保存する";
            this.Load += new System.EventHandler(this.UPSPatchForm_Load);
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomColorGroupBox customColorGroupBox1;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx OrignalFilename;
        private System.Windows.Forms.Button OrignalSelectButton;
        private System.Windows.Forms.Button MakeUPSPatchButton;
        private System.Windows.Forms.Label label1;
    }
}