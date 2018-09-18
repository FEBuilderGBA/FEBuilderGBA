namespace FEBuilderGBA
{
    partial class OpenLastSelectedFileForm
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
            this.OpenDirButton = new System.Windows.Forms.Button();
            this.RunAsButton = new System.Windows.Forms.Button();
            this.LastSelectedFilename = new FEBuilderGBA.TextBoxEx();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OpenDirButton);
            this.panel1.Controls.Add(this.RunAsButton);
            this.panel1.Controls.Add(this.LastSelectedFilename);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(645, 227);
            this.panel1.TabIndex = 0;
            // 
            // OpenDirButton
            // 
            this.OpenDirButton.Location = new System.Drawing.Point(30, 141);
            this.OpenDirButton.Name = "OpenDirButton";
            this.OpenDirButton.Size = new System.Drawing.Size(601, 56);
            this.OpenDirButton.TabIndex = 92;
            this.OpenDirButton.Text = "ディレクトリの場所を開く";
            this.OpenDirButton.UseVisualStyleBackColor = true;
            this.OpenDirButton.Click += new System.EventHandler(this.OpenDirButton_Click);
            // 
            // RunAsButton
            // 
            this.RunAsButton.Location = new System.Drawing.Point(30, 61);
            this.RunAsButton.Name = "RunAsButton";
            this.RunAsButton.Size = new System.Drawing.Size(601, 56);
            this.RunAsButton.TabIndex = 91;
            this.RunAsButton.Text = "関連付けされたアプリケーションで開く";
            this.RunAsButton.UseVisualStyleBackColor = true;
            this.RunAsButton.Click += new System.EventHandler(this.RunAsButton_Click);
            // 
            // LastSelectedFilename
            // 
            this.LastSelectedFilename.ErrorMessage = "";
            this.LastSelectedFilename.Location = new System.Drawing.Point(301, 5);
            this.LastSelectedFilename.Name = "LastSelectedFilename";
            this.LastSelectedFilename.Placeholder = "";
            this.LastSelectedFilename.Size = new System.Drawing.Size(330, 25);
            this.LastSelectedFilename.TabIndex = 90;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 31);
            this.label2.TabIndex = 89;
            this.label2.Text = "最後に利用したファイル";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OpenLastSelectedFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(670, 255);
            this.Controls.Add(this.panel1);
            this.Name = "OpenLastSelectedFileForm";
            this.Text = "最後に利用したファイル";
            this.Load += new System.EventHandler(this.OpenLastSelectedFileForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FEBuilderGBA.TextBoxEx LastSelectedFilename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RunAsButton;
        private System.Windows.Forms.Button OpenDirButton;
    }
}