namespace FEBuilderGBA
{
    partial class VersionForm
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
            this.Version = new FEBuilderGBA.TextBoxEx();
            this.DevTranslateButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Version
            // 
            this.Version.Location = new System.Drawing.Point(18, 12);
            this.Version.Margin = new System.Windows.Forms.Padding(2);
            this.Version.Multiline = true;
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            this.Version.Size = new System.Drawing.Size(557, 242);
            this.Version.TabIndex = 0;
            this.Version.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Version_KeyDown);
            // 
            // DevTranslateButton
            // 
            this.DevTranslateButton.Location = new System.Drawing.Point(331, 204);
            this.DevTranslateButton.Margin = new System.Windows.Forms.Padding(2);
            this.DevTranslateButton.Name = "DevTranslateButton";
            this.DevTranslateButton.Size = new System.Drawing.Size(244, 69);
            this.DevTranslateButton.TabIndex = 70;
            this.DevTranslateButton.Text = "開発者機能: 翻訳";
            this.DevTranslateButton.UseVisualStyleBackColor = true;
            this.DevTranslateButton.Visible = false;
            this.DevTranslateButton.Click += new System.EventHandler(this.DevTranslateButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(171, 102);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(2);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(56, 30);
            this.CloseButton.TabIndex = 71;
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Visible = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // VersionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(586, 272);
            this.Controls.Add(this.DevTranslateButton);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.CloseButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VersionForm";
            this.Text = "バージョン";
            this.Load += new System.EventHandler(this.VersionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FEBuilderGBA.TextBoxEx Version;
        private System.Windows.Forms.Button DevTranslateButton;
        private System.Windows.Forms.Button CloseButton;
    }
}