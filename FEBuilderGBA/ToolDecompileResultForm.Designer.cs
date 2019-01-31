namespace FEBuilderGBA
{
    partial class ToolDecompileResultForm
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
            this.MainTab = new FEBuilderGBA.TabControlEx();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTab.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.MainTab.ItemSize = new System.Drawing.Size(200, 18);
            this.MainTab.Location = new System.Drawing.Point(0, 0);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(786, 807);
            this.MainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTab.TabIndex = 1;
            this.MainTab.Resize += new System.EventHandler(this.ToolDecompileResultForm_Resize);
            // 
            // ToolDecompileResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(786, 807);
            this.Controls.Add(this.MainTab);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "ToolDecompileResultForm";
            this.Text = "C Pseudocode";
            this.Load += new System.EventHandler(this.DecompileResult_Load);
            this.Resize += new System.EventHandler(this.ToolDecompileResultForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControlEx MainTab;

    }
}