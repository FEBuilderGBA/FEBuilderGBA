namespace FEBuilderGBA
{
    partial class HexEditorForm
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
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.Hint = new FEBuilderGBA.TextBoxEx();
            this.MarkListButton = new System.Windows.Forms.Button();
            this.SetMarkButton = new System.Windows.Forms.Button();
            this.WriteButton = new System.Windows.Forms.Button();
            this.DisASMButton = new System.Windows.Forms.Button();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.SearchButton = new System.Windows.Forms.Button();
            this.JumpButton = new System.Windows.Forms.Button();
            this.HexBox = new FEBuilderGBA.HexBox();
            this.ControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.Hint);
            this.ControlPanel.Controls.Add(this.MarkListButton);
            this.ControlPanel.Controls.Add(this.SetMarkButton);
            this.ControlPanel.Controls.Add(this.WriteButton);
            this.ControlPanel.Controls.Add(this.DisASMButton);
            this.ControlPanel.Controls.Add(this.SelectAddress);
            this.ControlPanel.Controls.Add(this.SearchButton);
            this.ControlPanel.Controls.Add(this.JumpButton);
            this.ControlPanel.Location = new System.Drawing.Point(13, 730);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(1236, 80);
            this.ControlPanel.TabIndex = 1;
            // 
            // Hint
            // 
            this.Hint.Location = new System.Drawing.Point(4, 52);
            this.Hint.Name = "Hint";
            this.Hint.ReadOnly = true;
            this.Hint.Size = new System.Drawing.Size(1227, 25);
            this.Hint.TabIndex = 9;
            // 
            // MarkListButton
            // 
            this.MarkListButton.Location = new System.Drawing.Point(377, 3);
            this.MarkListButton.Name = "MarkListButton";
            this.MarkListButton.Size = new System.Drawing.Size(100, 38);
            this.MarkListButton.TabIndex = 8;
            this.MarkListButton.Text = "Mark&List";
            this.MarkListButton.UseVisualStyleBackColor = true;
            this.MarkListButton.Click += new System.EventHandler(this.MarkListButton_Click);
            // 
            // SetMarkButton
            // 
            this.SetMarkButton.Location = new System.Drawing.Point(272, 3);
            this.SetMarkButton.Name = "SetMarkButton";
            this.SetMarkButton.Size = new System.Drawing.Size(100, 38);
            this.SetMarkButton.TabIndex = 7;
            this.SetMarkButton.Text = "Set&Mark";
            this.SetMarkButton.UseVisualStyleBackColor = true;
            this.SetMarkButton.Click += new System.EventHandler(this.SetMarkButton_Click);
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(1107, 3);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(126, 38);
            this.WriteButton.TabIndex = 6;
            this.WriteButton.Text = "Write";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // DisASMButton
            // 
            this.DisASMButton.Location = new System.Drawing.Point(566, 3);
            this.DisASMButton.Name = "DisASMButton";
            this.DisASMButton.Size = new System.Drawing.Size(126, 38);
            this.DisASMButton.TabIndex = 5;
            this.DisASMButton.Text = "DisASM";
            this.DisASMButton.UseVisualStyleBackColor = true;
            this.DisASMButton.Click += new System.EventHandler(this.DisASMButton_Click);
            // 
            // SelectAddress
            // 
            this.SelectAddress.Location = new System.Drawing.Point(698, 10);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Size = new System.Drawing.Size(403, 25);
            this.SelectAddress.TabIndex = 4;
            this.SelectAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectAddress_KeyDown);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(110, 3);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(100, 38);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "&Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // JumpButton
            // 
            this.JumpButton.Location = new System.Drawing.Point(4, 3);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(100, 38);
            this.JumpButton.TabIndex = 0;
            this.JumpButton.Text = "&Jump";
            this.JumpButton.UseVisualStyleBackColor = true;
            this.JumpButton.Click += new System.EventHandler(this.JumpButton_Click);
            // 
            // HexBox
            // 
            this.HexBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HexBox.Location = new System.Drawing.Point(13, 13);
            this.HexBox.Name = "HexBox";
            this.HexBox.Size = new System.Drawing.Size(1236, 711);
            this.HexBox.TabIndex = 0;
            this.HexBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HexBox_KeyDown);
            // 
            // HexEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1256, 822);
            this.Controls.Add(this.ControlPanel);
            this.Controls.Add(this.HexBox);
            this.Name = "HexEditorForm";
            this.Text = "バイナリエディタ";
            this.Load += new System.EventHandler(this.HexEditorForm_Load);
            this.Resize += new System.EventHandler(this.HexEditorForm_Resize);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HexBox HexBox;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button JumpButton;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Button DisASMButton;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Button MarkListButton;
        private System.Windows.Forms.Button SetMarkButton;
        private FEBuilderGBA.TextBoxEx Hint;

    }
}