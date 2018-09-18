namespace FEBuilderGBA
{
    partial class ToolClickWriteFloatControlPanelButtonForm
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
            this.FormIcon = new System.Windows.Forms.PictureBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.Message = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.FormIcon);
            this.panel1.Controls.Add(this.UpdateButton);
            this.panel1.Controls.Add(this.NewButton);
            this.panel1.Controls.Add(this.Message);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(879, 164);
            this.panel1.TabIndex = 1;
            // 
            // FormIcon
            // 
            this.FormIcon.Location = new System.Drawing.Point(18, 24);
            this.FormIcon.Name = "FormIcon";
            this.FormIcon.Size = new System.Drawing.Size(128, 128);
            this.FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FormIcon.TabIndex = 4;
            this.FormIcon.TabStop = false;
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(184, 102);
            this.UpdateButton.Margin = new System.Windows.Forms.Padding(2);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(435, 50);
            this.UpdateButton.TabIndex = 0;
            this.UpdateButton.Text = "変更";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // NewButton
            // 
            this.NewButton.Location = new System.Drawing.Point(654, 102);
            this.NewButton.Margin = new System.Windows.Forms.Padding(2);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(223, 50);
            this.NewButton.TabIndex = 1;
            this.NewButton.Text = "新規挿入";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(181, 24);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(236, 18);
            this.Message.TabIndex = 0;
            this.Message.Text = "どちらのボタンをクリックしますか？";
            // 
            // ToolClickWriteFloatControlPanelButtonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(904, 188);
            this.Controls.Add(this.panel1);
            this.Name = "ToolClickWriteFloatControlPanelButtonForm";
            this.Text = "どちらのボタンをクリックしますか？";
            this.Load += new System.EventHandler(this.UpdateDialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox FormIcon;
        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button NewButton;
    }
}