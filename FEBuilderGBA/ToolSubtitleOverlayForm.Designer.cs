namespace FEBuilderGBA
{
    partial class ToolSubtitleOverlayForm
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
            this.Subtile = new FEBuilderGBA.RichTextBoxEx();
            this.SuspendLayout();
            // 
            // Subtile
            // 
            this.Subtile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Subtile.ErrorMessage = "";
            this.Subtile.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Subtile.Location = new System.Drawing.Point(0, 0);
            this.Subtile.Name = "Subtile";
            this.Subtile.Placeholder = "";
            this.Subtile.Size = new System.Drawing.Size(976, 192);
            this.Subtile.TabIndex = 0;
            this.Subtile.Text = "";
            this.Subtile.DoubleClick += new System.EventHandler(this.Subtile_DoubleClick);
            this.Subtile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Subtile_MouseDown);
            this.Subtile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Subtile_MouseMove);
            // 
            // ToolSubtitleOverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 192);
            this.Controls.Add(this.Subtile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ToolSubtitleOverlayForm";
            this.Text = "字幕";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.ToolSubtitleOverlayForm_Activated);
            this.Load += new System.EventHandler(this.ToolSubtitleOverlayForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBoxEx Subtile;
    }
}