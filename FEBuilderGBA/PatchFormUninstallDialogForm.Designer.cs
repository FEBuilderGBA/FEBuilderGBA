namespace FEBuilderGBA
{
    partial class PatchFormUninstallDialogForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.UninstallPatchButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.customColorGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(208, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(939, 159);
            this.label1.TabIndex = 92;
            this.label1.Text = "復旧用に、このパッチをインストールする前のROMを選択してください。\r\n\r\nこの機能は、確実なアンインストールを約束するものではありません。\r\n失敗するかもしれな" +
    "いので、事前にバックアップを取ってください。\r\nまた、パッチ本体は消せますが、付属するデータは消せない時があります。\r\nその場合、数百バイト程度のロスになります" +
    "が、ご了承ください。";
            // 
            // UninstallPatchButton
            // 
            this.UninstallPatchButton.Location = new System.Drawing.Point(207, 247);
            this.UninstallPatchButton.Name = "UninstallPatchButton";
            this.UninstallPatchButton.Size = new System.Drawing.Size(939, 32);
            this.UninstallPatchButton.TabIndex = 0;
            this.UninstallPatchButton.Text = "アンインストール";
            this.UninstallPatchButton.UseVisualStyleBackColor = true;
            this.UninstallPatchButton.Click += new System.EventHandler(this.UninstallPatchButton_Click);
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.label1);
            this.customColorGroupBox1.Controls.Add(this.UninstallPatchButton);
            this.customColorGroupBox1.Controls.Add(this.label2);
            this.customColorGroupBox1.Controls.Add(this.OrignalFilename);
            this.customColorGroupBox1.Controls.Add(this.OrignalSelectButton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(1153, 296);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "パッチのアンインストール";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(7, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 79);
            this.label2.TabIndex = 91;
            this.label2.Text = "パッチを含んでいないROM";
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
            // PatchFormUninstallDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1177, 317);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "PatchFormUninstallDialogForm";
            this.Text = "パッチのアンインストール";
            this.Load += new System.EventHandler(this.PatchFormUninstallDialogForm_Load);
            this.Shown += new System.EventHandler(this.UPSOpenSimpleForm_Shown);
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UninstallPatchButton;
        private System.Windows.Forms.Label label1;
        private CustomColorGroupBox customColorGroupBox1;
        private System.Windows.Forms.Label label2;
        private TextBoxEx OrignalFilename;
        private System.Windows.Forms.Button OrignalSelectButton;
    }
}