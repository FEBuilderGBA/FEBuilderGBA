namespace FEBuilderGBA
{
    partial class ToolAllWorkSupportForm
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
            this.Ctrl = new System.Windows.Forms.Panel();
            this.HEADER = new System.Windows.Forms.Panel();
            this.UpdateCheckButton = new System.Windows.Forms.Button();
            this.HEADER.SuspendLayout();
            this.SuspendLayout();
            // 
            // Ctrl
            // 
            this.Ctrl.AutoScroll = true;
            this.Ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ctrl.Location = new System.Drawing.Point(0, 39);
            this.Ctrl.Name = "Ctrl";
            this.Ctrl.Size = new System.Drawing.Size(1264, 717);
            this.Ctrl.TabIndex = 0;
            // 
            // HEADER
            // 
            this.HEADER.Controls.Add(this.UpdateCheckButton);
            this.HEADER.Dock = System.Windows.Forms.DockStyle.Top;
            this.HEADER.Location = new System.Drawing.Point(0, 0);
            this.HEADER.Name = "HEADER";
            this.HEADER.Size = new System.Drawing.Size(1264, 39);
            this.HEADER.TabIndex = 1;
            // 
            // UpdateCheckButton
            // 
            this.UpdateCheckButton.Location = new System.Drawing.Point(3, 0);
            this.UpdateCheckButton.Name = "UpdateCheckButton";
            this.UpdateCheckButton.Size = new System.Drawing.Size(296, 33);
            this.UpdateCheckButton.TabIndex = 0;
            this.UpdateCheckButton.Text = "更新チェック";
            this.UpdateCheckButton.UseVisualStyleBackColor = true;
            this.UpdateCheckButton.Click += new System.EventHandler(this.UpdateCheckButton_Click);
            // 
            // ToolAllWorkSupportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1264, 756);
            this.Controls.Add(this.Ctrl);
            this.Controls.Add(this.HEADER);
            this.Name = "ToolAllWorkSupportForm";
            this.Text = "すべての作品";
            this.Load += new System.EventHandler(this.ToolAllWorkSupportForm_Load);
            this.Resize += new System.EventHandler(this.ToolAllWorkSupportForm_Resize);
            this.HEADER.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Ctrl;
        private System.Windows.Forms.Panel HEADER;
        private System.Windows.Forms.Button UpdateCheckButton;
    }
}