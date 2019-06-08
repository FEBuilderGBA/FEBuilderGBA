namespace FEBuilderGBA
{
    partial class ToolProblemReportSearchBackupForm
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
            this.BackupFilename = new FEBuilderGBA.TextBoxEx();
            this.BackupSelectButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BackupFilename
            // 
            this.BackupFilename.ErrorMessage = "";
            this.BackupFilename.Location = new System.Drawing.Point(189, 121);
            this.BackupFilename.Margin = new System.Windows.Forms.Padding(4);
            this.BackupFilename.Name = "BackupFilename";
            this.BackupFilename.Placeholder = "";
            this.BackupFilename.Size = new System.Drawing.Size(537, 25);
            this.BackupFilename.TabIndex = 94;
            // 
            // BackupSelectButton
            // 
            this.BackupSelectButton.Location = new System.Drawing.Point(734, 115);
            this.BackupSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.BackupSelectButton.Name = "BackupSelectButton";
            this.BackupSelectButton.Size = new System.Drawing.Size(130, 31);
            this.BackupSelectButton.TabIndex = 93;
            this.BackupSelectButton.Text = "参照";
            this.BackupSelectButton.UseVisualStyleBackColor = true;
            this.BackupSelectButton.Click += new System.EventHandler(this.BackupSelectButton_Click);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(15, 121);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 31);
            this.label9.TabIndex = 92;
            this.label9.Text = "backup.7z";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(591, 54);
            this.label1.TabIndex = 91;
            this.label1.Text = "過去のバックアップが1つもありません。\r\n過去のバックアップが別名で保存されていますか？\r\n問題が発生ずに、正しく動作するバックアップがある場合は、パスを指定して" +
    "ください。";
            // 
            // ToolProblemReportSearchBackupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 188);
            this.Controls.Add(this.BackupFilename);
            this.Controls.Add(this.BackupSelectButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Name = "ToolProblemReportSearchBackupForm";
            this.Text = "過去のバックアップがありません";
            this.Load += new System.EventHandler(this.ToolProblemReportSearchBackupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBoxEx BackupFilename;
        private System.Windows.Forms.Button BackupSelectButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
    }
}