namespace FEBuilderGBA
{
    partial class HexEditorSearch
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
            this.OKButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.RevCheckBox = new System.Windows.Forms.CheckBox();
            this.LittleEndianCheckBox = new System.Windows.Forms.CheckBox();
            this.Align4 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // AddrComboBox
            // 
            this.AddrComboBox.FormattingEnabled = true;
            this.AddrComboBox.Location = new System.Drawing.Point(150, 25);
            this.AddrComboBox.Name = "AddrComboBox";
            this.AddrComboBox.Size = new System.Drawing.Size(337, 26);
            this.AddrComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "検索";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(307, 153);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(180, 39);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "検索する";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(472, 36);
            this.label2.TabIndex = 6;
            this.label2.Text = "バイトごとにワイルドカードが利用できます。\r\n12 XX 33 44 --> 11 FF 33 44 / 11 BB 33 44 とかにマッチします。";
            // 
            // RevCheckBox
            // 
            this.RevCheckBox.AutoSize = true;
            this.RevCheckBox.Location = new System.Drawing.Point(16, 82);
            this.RevCheckBox.Name = "RevCheckBox";
            this.RevCheckBox.Size = new System.Drawing.Size(131, 22);
            this.RevCheckBox.TabIndex = 1;
            this.RevCheckBox.Text = "逆に検索する";
            this.RevCheckBox.UseVisualStyleBackColor = true;
            // 
            // LittleEndianCheckBox
            // 
            this.LittleEndianCheckBox.AutoSize = true;
            this.LittleEndianCheckBox.Location = new System.Drawing.Point(16, 119);
            this.LittleEndianCheckBox.Name = "LittleEndianCheckBox";
            this.LittleEndianCheckBox.Size = new System.Drawing.Size(152, 22);
            this.LittleEndianCheckBox.TabIndex = 2;
            this.LittleEndianCheckBox.Text = "リトルエンディアン";
            this.LittleEndianCheckBox.UseVisualStyleBackColor = true;
            // 
            // Align4
            // 
            this.Align4.AutoSize = true;
            this.Align4.Location = new System.Drawing.Point(16, 153);
            this.Align4.Name = "Align4";
            this.Align4.Size = new System.Drawing.Size(120, 22);
            this.Align4.TabIndex = 3;
            this.Align4.Text = "4バイト単位";
            this.Align4.UseVisualStyleBackColor = true;
            // 
            // HexEditorSearch
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(499, 270);
            this.Controls.Add(this.Align4);
            this.Controls.Add(this.LittleEndianCheckBox);
            this.Controls.Add(this.RevCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AddrComboBox);
            this.Name = "HexEditorSearch";
            this.Text = "バイナリエディタ - 検索";
            this.Load += new System.EventHandler(this.HexEditorSearch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OKButton;
        public System.Windows.Forms.ComboBox AddrComboBox;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox RevCheckBox;
        public System.Windows.Forms.CheckBox LittleEndianCheckBox;
        public System.Windows.Forms.CheckBox Align4;
    }
}