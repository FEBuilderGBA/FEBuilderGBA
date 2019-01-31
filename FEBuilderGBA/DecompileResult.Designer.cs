namespace FEBuilderGBA
{
    partial class DecompileResult
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
            this.code = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // code
            // 
            this.code.Location = new System.Drawing.Point(0, 2);
            this.code.Multiline = true;
            this.code.Name = "code";
            this.code.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.code.Size = new System.Drawing.Size(763, 408);
            this.code.TabIndex = 0;
            this.code.TextChanged += new System.EventHandler(this.code_TextChanged);
            // 
            // DecompileResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.code);
            this.Name = "DecompileResult";
            this.Text = "C Pseudocode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox code;
    }
}