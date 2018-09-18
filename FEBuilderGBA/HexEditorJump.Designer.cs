namespace FEBuilderGBA
{
    partial class HexEditorJump
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
            this.AddrComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LittleEndianCheckBox = new System.Windows.Forms.CheckBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddrComboBox
            // 
            this.AddrComboBox.FormattingEnabled = true;
            this.AddrComboBox.Location = new System.Drawing.Point(151, 25);
            this.AddrComboBox.Name = "AddrComboBox";
            this.AddrComboBox.Size = new System.Drawing.Size(337, 26);
            this.AddrComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "アドレス移動";
            // 
            // LittleEndianCheckBox
            // 
            this.LittleEndianCheckBox.AutoSize = true;
            this.LittleEndianCheckBox.Location = new System.Drawing.Point(16, 81);
            this.LittleEndianCheckBox.Name = "LittleEndianCheckBox";
            this.LittleEndianCheckBox.Size = new System.Drawing.Size(188, 22);
            this.LittleEndianCheckBox.TabIndex = 3;
            this.LittleEndianCheckBox.Text = "強制リトルエンディアン";
            this.LittleEndianCheckBox.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(330, 126);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(158, 39);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "移動する";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // HexEditorJump
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(499, 176);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.LittleEndianCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AddrComboBox);
            this.Name = "HexEditorJump";
            this.Text = "バイナリエディタ 移動";
            this.Load += new System.EventHandler(this.HexEditorJump_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OKButton;
        public System.Windows.Forms.ComboBox AddrComboBox;
        public System.Windows.Forms.CheckBox LittleEndianCheckBox;
    }
}