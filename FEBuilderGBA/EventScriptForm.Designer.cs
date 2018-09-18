namespace FEBuilderGBA
{
    partial class EventScriptForm
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
            this.MainTab.Size = new System.Drawing.Size(1547, 884);
            this.MainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTab.TabIndex = 1;
            // 
            // EventScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1547, 884);
            this.Controls.Add(this.MainTab);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "EventScriptForm";
            this.Text = "イベント命令";
            this.Load += new System.EventHandler(this.EventScriptForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EventScriptForm_KeyDown);
            this.Resize += new System.EventHandler(this.EventScriptForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControlEx MainTab;

    }
}