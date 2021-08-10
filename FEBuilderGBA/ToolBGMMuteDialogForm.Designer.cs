namespace FEBuilderGBA
{
    partial class ToolBGMMuteDialogForm
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
            this.ToggleButton = new System.Windows.Forms.Button();
            this.OnlyPlayButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // ToggleButton
            // 
            this.ToggleButton.Location = new System.Drawing.Point(15, 99);
            this.ToggleButton.Name = "ToggleButton";
            this.ToggleButton.Size = new System.Drawing.Size(534, 42);
            this.ToggleButton.TabIndex = 1;
            this.ToggleButton.Text = "toggle";
            this.ToggleButton.UseVisualStyleBackColor = true;
            this.ToggleButton.Click += new System.EventHandler(this.ToggleButton_Click);
            // 
            // OnlyPlayButton
            // 
            this.OnlyPlayButton.Location = new System.Drawing.Point(15, 160);
            this.OnlyPlayButton.Name = "OnlyPlayButton";
            this.OnlyPlayButton.Size = new System.Drawing.Size(534, 42);
            this.OnlyPlayButton.TabIndex = 2;
            this.OnlyPlayButton.Text = "このトラックだけを再生する";
            this.OnlyPlayButton.UseVisualStyleBackColor = true;
            this.OnlyPlayButton.Click += new System.EventHandler(this.OnlyPlayButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 221);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(534, 42);
            this.button1.TabIndex = 3;
            this.button1.Text = "すべてのトラックを再生する";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ToolBGMMuteDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 287);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OnlyPlayButton);
            this.Controls.Add(this.ToggleButton);
            this.Controls.Add(this.label1);
            this.Name = "ToolBGMMuteDialogForm";
            this.Load += new System.EventHandler(this.ToolBGMMuteDialogForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ToggleButton;
        private System.Windows.Forms.Button OnlyPlayButton;
        private System.Windows.Forms.Button button1;
    }
}