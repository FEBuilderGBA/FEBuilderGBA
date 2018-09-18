namespace FEBuilderGBA
{
    partial class ToolUndoPopupDialogForm
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
            this.TestPlayButton = new System.Windows.Forms.Button();
            this.Info = new System.Windows.Forms.TextBox();
            this.FormIcon = new System.Windows.Forms.PictureBox();
            this.RunUndoButton = new System.Windows.Forms.Button();
            this.MyCancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // TestPlayButton
            // 
            this.TestPlayButton.Location = new System.Drawing.Point(114, 171);
            this.TestPlayButton.Name = "TestPlayButton";
            this.TestPlayButton.Size = new System.Drawing.Size(629, 41);
            this.TestPlayButton.TabIndex = 0;
            this.TestPlayButton.Text = "このバージョンをエミュレータでテストプレイ";
            this.TestPlayButton.UseVisualStyleBackColor = true;
            this.TestPlayButton.Click += new System.EventHandler(this.TestPlayButton_Click);
            // 
            // Info
            // 
            this.Info.Location = new System.Drawing.Point(114, 22);
            this.Info.Multiline = true;
            this.Info.Name = "Info";
            this.Info.ReadOnly = true;
            this.Info.Size = new System.Drawing.Size(629, 117);
            this.Info.TabIndex = 1;
            // 
            // FormIcon
            // 
            this.FormIcon.Location = new System.Drawing.Point(12, 22);
            this.FormIcon.Name = "FormIcon";
            this.FormIcon.Size = new System.Drawing.Size(80, 80);
            this.FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FormIcon.TabIndex = 5;
            this.FormIcon.TabStop = false;
            // 
            // RunUndoButton
            // 
            this.RunUndoButton.Location = new System.Drawing.Point(114, 218);
            this.RunUndoButton.Name = "RunUndoButton";
            this.RunUndoButton.Size = new System.Drawing.Size(629, 36);
            this.RunUndoButton.TabIndex = 1;
            this.RunUndoButton.Text = "このバージョンに戻す";
            this.RunUndoButton.UseVisualStyleBackColor = true;
            this.RunUndoButton.Click += new System.EventHandler(this.RunUndoButton_Click);
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(114, 260);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(629, 35);
            this.MyCancelButton.TabIndex = 2;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // ToolUndoPopupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(755, 317);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.RunUndoButton);
            this.Controls.Add(this.FormIcon);
            this.Controls.Add(this.Info);
            this.Controls.Add(this.TestPlayButton);
            this.Name = "ToolUndoPopupDialogForm";
            this.Text = "UNDO確認";
            this.Load += new System.EventHandler(this.ToolUndoPopupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TestPlayButton;
        private System.Windows.Forms.TextBox Info;
        private System.Windows.Forms.PictureBox FormIcon;
        private System.Windows.Forms.Button RunUndoButton;
        private System.Windows.Forms.Button MyCancelButton;
    }
}