namespace FEBuilderGBA
{
    partial class PointerToolAlienForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.RunButton = new System.Windows.Forms.Button();
            this.SRCFilename = new FEBuilderGBA.TextBoxEx();
            this.SRCSelectButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.J_2_TEXT = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RunButton);
            this.panel1.Controls.Add(this.SRCFilename);
            this.panel1.Controls.Add(this.SRCSelectButton);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.J_2_TEXT);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(882, 230);
            this.panel1.TabIndex = 97;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(618, 164);
            this.RunButton.Margin = new System.Windows.Forms.Padding(4);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(264, 61);
            this.RunButton.TabIndex = 94;
            this.RunButton.Text = "変換開始";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // SRCFilename
            // 
            this.SRCFilename.ErrorMessage = "";
            this.SRCFilename.Location = new System.Drawing.Point(307, 91);
            this.SRCFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SRCFilename.Name = "SRCFilename";
            this.SRCFilename.Placeholder = "";
            this.SRCFilename.Size = new System.Drawing.Size(572, 25);
            this.SRCFilename.TabIndex = 93;
            // 
            // SRCSelectButton
            // 
            this.SRCSelectButton.Location = new System.Drawing.Point(169, 87);
            this.SRCSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.SRCSelectButton.Name = "SRCSelectButton";
            this.SRCSelectButton.Size = new System.Drawing.Size(130, 31);
            this.SRCSelectButton.TabIndex = 92;
            this.SRCSelectButton.Text = "別ファイル選択";
            this.SRCSelectButton.UseVisualStyleBackColor = true;
            this.SRCSelectButton.Click += new System.EventHandler(this.SRCSelectButton_Click);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(4, 90);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 31);
            this.label9.TabIndex = 91;
            this.label9.Text = "スクリプト";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_2_TEXT
            // 
            this.J_2_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2_TEXT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.J_2_TEXT.Location = new System.Drawing.Point(2, 1);
            this.J_2_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2_TEXT.Name = "J_2_TEXT";
            this.J_2_TEXT.Size = new System.Drawing.Size(879, 67);
            this.J_2_TEXT.TabIndex = 13;
            this.J_2_TEXT.Text = "ASMファイルを他のゲーム用に自動変換します";
            this.J_2_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PointerToolAlienForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(902, 250);
            this.Controls.Add(this.panel1);
            this.Name = "PointerToolAlienForm";
            this.Text = "PointerToolAlienForm";
            this.Load += new System.EventHandler(this.PointerToolAlienForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private TextBoxEx SRCFilename;
        private System.Windows.Forms.Button SRCSelectButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label J_2_TEXT;
        private System.Windows.Forms.Button RunButton;

    }
}