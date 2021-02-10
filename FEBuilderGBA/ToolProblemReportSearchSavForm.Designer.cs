namespace FEBuilderGBA
{
    partial class ToolProblemReportSearchSavForm
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
            this.SavSelectButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.SavFilename = new FEBuilderGBA.TextBoxEx();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(413, 54);
            this.label1.TabIndex = 0;
            this.label1.Text = "savファイルが含まれていないようです。\r\n問題を確実に再現するためには、savファイルが必要です。\r\n対応するsavがある場合は、パスを指定してください。";
            // 
            // SavSelectButton
            // 
            this.SavSelectButton.Location = new System.Drawing.Point(750, 121);
            this.SavSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.SavSelectButton.Name = "SavSelectButton";
            this.SavSelectButton.Size = new System.Drawing.Size(130, 31);
            this.SavSelectButton.TabIndex = 89;
            this.SavSelectButton.Text = "参照";
            this.SavSelectButton.UseVisualStyleBackColor = true;
            this.SavSelectButton.Click += new System.EventHandler(this.SavSelectButton_Click);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(13, 127);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 31);
            this.label9.TabIndex = 88;
            this.label9.Text = "sav";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SavFilename
            // 
            this.SavFilename.ErrorMessage = "";
            this.SavFilename.Location = new System.Drawing.Point(187, 127);
            this.SavFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SavFilename.Name = "SavFilename";
            this.SavFilename.Placeholder = "";
            this.SavFilename.Size = new System.Drawing.Size(542, 25);
            this.SavFilename.TabIndex = 90;
            // 
            // ToolProblemReportSearchSavForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 215);
            this.Controls.Add(this.SavFilename);
            this.Controls.Add(this.SavSelectButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Name = "ToolProblemReportSearchSavForm";
            this.Text = "savファイルがありません";
            this.Load += new System.EventHandler(this.ToolProblemReportSearchSavForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private TextBoxEx SavFilename;
        private System.Windows.Forms.Button SavSelectButton;
        private System.Windows.Forms.Label label9;
    }
}