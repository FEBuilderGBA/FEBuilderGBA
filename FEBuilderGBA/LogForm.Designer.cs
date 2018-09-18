namespace FEBuilderGBA
{
    partial class LogForm
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.ClipbordButton = new System.Windows.Forms.Button();
            this.openLogDirButton = new System.Windows.Forms.Button();
            this.LogList = new ListBoxEx();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(896, 580);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(2);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(154, 28);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "ファイルに保存";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ClipbordButton
            // 
            this.ClipbordButton.Location = new System.Drawing.Point(738, 580);
            this.ClipbordButton.Margin = new System.Windows.Forms.Padding(2);
            this.ClipbordButton.Name = "ClipbordButton";
            this.ClipbordButton.Size = new System.Drawing.Size(154, 28);
            this.ClipbordButton.TabIndex = 4;
            this.ClipbordButton.Text = "クリップボードへ";
            this.ClipbordButton.UseVisualStyleBackColor = true;
            this.ClipbordButton.Click += new System.EventHandler(this.ClipbordButton_Click);
            // 
            // openLogDirButton
            // 
            this.openLogDirButton.Location = new System.Drawing.Point(15, 580);
            this.openLogDirButton.Margin = new System.Windows.Forms.Padding(2);
            this.openLogDirButton.Name = "openLogDirButton";
            this.openLogDirButton.Size = new System.Drawing.Size(314, 28);
            this.openLogDirButton.TabIndex = 5;
            this.openLogDirButton.Text = "ログディレクトリを開く";
            this.openLogDirButton.UseVisualStyleBackColor = true;
            this.openLogDirButton.Click += new System.EventHandler(this.openLogDirButton_Click);
            // 
            // LogList
            // 
            this.LogList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogList.FormattingEnabled = true;
            this.LogList.ItemHeight = 18;
            this.LogList.Location = new System.Drawing.Point(15, 12);
            this.LogList.Margin = new System.Windows.Forms.Padding(2);
            this.LogList.Name = "LogList";
            this.LogList.Size = new System.Drawing.Size(1030, 562);
            this.LogList.TabIndex = 2;
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1058, 620);
            this.Controls.Add(this.openLogDirButton);
            this.Controls.Add(this.ClipbordButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LogList);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LogForm";
            this.Text = "Log";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LogForm_FormClosed);
            this.Load += new System.EventHandler(this.LogForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ClipbordButton;
        private System.Windows.Forms.Button openLogDirButton;
        private ListBoxEx LogList;
    }
}