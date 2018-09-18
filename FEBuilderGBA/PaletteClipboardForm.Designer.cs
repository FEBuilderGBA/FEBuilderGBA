namespace FEBuilderGBA
{
    partial class PaletteClipboardForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.ChangeButton = new System.Windows.Forms.Button();
            this.PAL_TEXT = new FEBuilderGBA.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.MyCancelButton);
            this.panel1.Controls.Add(this.ChangeButton);
            this.panel1.Controls.Add(this.PAL_TEXT);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 239);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(523, 54);
            this.label2.TabIndex = 4;
            this.label2.Text = "現在のパレットの設定をテキストデータとして保存できます。\r\nこの数字をメモしておけば、いつでも同じパレットに復元することができます。\r\nこの文字列は、FE Rec" +
    "olorのカラー文字列と互換性があります。";
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(616, 190);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(123, 36);
            this.MyCancelButton.TabIndex = 3;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ChangeButton
            // 
            this.ChangeButton.Location = new System.Drawing.Point(472, 190);
            this.ChangeButton.Name = "ChangeButton";
            this.ChangeButton.Size = new System.Drawing.Size(123, 36);
            this.ChangeButton.TabIndex = 2;
            this.ChangeButton.Text = "変更";
            this.ChangeButton.UseVisualStyleBackColor = true;
            this.ChangeButton.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // PAL_TEXT
            // 
            this.PAL_TEXT.ErrorMessage = "";
            this.PAL_TEXT.Location = new System.Drawing.Point(14, 61);
            this.PAL_TEXT.Name = "PAL_TEXT";
            this.PAL_TEXT.Placeholder = "";
            this.PAL_TEXT.Size = new System.Drawing.Size(731, 25);
            this.PAL_TEXT.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "パレット値";
            // 
            // PaletteClipboardForm
            // 
            this.AcceptButton = this.ChangeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(788, 266);
            this.Controls.Add(this.panel1);
            this.Name = "PaletteClipboardForm";
            this.Text = "パレットの数字化";
            this.Load += new System.EventHandler(this.PaletteClipboardForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private TextBoxEx PAL_TEXT;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Button ChangeButton;
        private System.Windows.Forms.Label label2;
    }
}