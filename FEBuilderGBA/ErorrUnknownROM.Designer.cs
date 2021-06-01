namespace FEBuilderGBA
{
    partial class ErorrUnknownROM
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
            this.ROMVersion = new System.Windows.Forms.Label();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.FE6Button = new System.Windows.Forms.Button();
            this.FE7UButton = new System.Windows.Forms.Button();
            this.FE8UButton = new System.Windows.Forms.Button();
            this.FE8JButton = new System.Windows.Forms.Button();
            this.FE7JButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.FE0Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "不明なバージョンのROM";
            // 
            // ROMVersion
            // 
            this.ROMVersion.AutoSize = true;
            this.ROMVersion.Location = new System.Drawing.Point(14, 77);
            this.ROMVersion.Name = "ROMVersion";
            this.ROMVersion.Size = new System.Drawing.Size(167, 18);
            this.ROMVersion.TabIndex = 2;
            this.ROMVersion.Text = "ROMのバージョン情報:";
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(17, 216);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(821, 66);
            this.MyCancelButton.TabIndex = 0;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click_1);
            // 
            // FE6Button
            // 
            this.FE6Button.Location = new System.Drawing.Point(17, 585);
            this.FE6Button.Name = "FE6Button";
            this.FE6Button.Size = new System.Drawing.Size(821, 66);
            this.FE6Button.TabIndex = 7;
            this.FE6Button.Text = "FE6として読み込む";
            this.FE6Button.UseVisualStyleBackColor = true;
            this.FE6Button.Click += new System.EventHandler(this.FE6Button_Click);
            // 
            // FE7UButton
            // 
            this.FE7UButton.Location = new System.Drawing.Point(17, 441);
            this.FE7UButton.Name = "FE7UButton";
            this.FE7UButton.Size = new System.Drawing.Size(821, 66);
            this.FE7UButton.TabIndex = 4;
            this.FE7UButton.Text = "英語版FE7Uとして読み込む";
            this.FE7UButton.UseVisualStyleBackColor = true;
            this.FE7UButton.Click += new System.EventHandler(this.FE7UButton_Click);
            // 
            // FE8UButton
            // 
            this.FE8UButton.Location = new System.Drawing.Point(17, 297);
            this.FE8UButton.Name = "FE8UButton";
            this.FE8UButton.Size = new System.Drawing.Size(821, 66);
            this.FE8UButton.TabIndex = 1;
            this.FE8UButton.Text = "英語版FE8Uとして読み込む";
            this.FE8UButton.UseVisualStyleBackColor = true;
            this.FE8UButton.Click += new System.EventHandler(this.FE8UButton_Click);
            // 
            // FE8JButton
            // 
            this.FE8JButton.Location = new System.Drawing.Point(17, 369);
            this.FE8JButton.Name = "FE8JButton";
            this.FE8JButton.Size = new System.Drawing.Size(821, 66);
            this.FE8JButton.TabIndex = 2;
            this.FE8JButton.Text = "日本語版FE8Jとして読み込む";
            this.FE8JButton.UseVisualStyleBackColor = true;
            this.FE8JButton.Click += new System.EventHandler(this.FE8JButton_Click);
            // 
            // FE7JButton
            // 
            this.FE7JButton.Location = new System.Drawing.Point(17, 513);
            this.FE7JButton.Name = "FE7JButton";
            this.FE7JButton.Size = new System.Drawing.Size(821, 66);
            this.FE7JButton.TabIndex = 5;
            this.FE7JButton.Text = "日本語版FE7Jとして読み込む";
            this.FE7JButton.UseVisualStyleBackColor = true;
            this.FE7JButton.Click += new System.EventHandler(this.FE7JButton_Click);
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Location = new System.Drawing.Point(17, 108);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(821, 86);
            this.label12.TabIndex = 25;
            this.label12.Text = "このROMには規定のバージョン情報がありません。\r\nこれは正しいGBAFEのROMですか？\r\nもし、GBAFEのROMであれば、バージョンを指定してください。\r\n" +
    "";
            // 
            // FE0Button
            // 
            this.FE0Button.Location = new System.Drawing.Point(17, 657);
            this.FE0Button.Name = "FE0Button";
            this.FE0Button.Size = new System.Drawing.Size(821, 66);
            this.FE0Button.TabIndex = 26;
            this.FE0Button.Text = "それ以外として読み込む";
            this.FE0Button.UseVisualStyleBackColor = true;
            this.FE0Button.Click += new System.EventHandler(this.FE0Button_Click);
            // 
            // ErorrUnknownROM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(850, 785);
            this.Controls.Add(this.FE0Button);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.FE7JButton);
            this.Controls.Add(this.FE8JButton);
            this.Controls.Add(this.FE8UButton);
            this.Controls.Add(this.FE7UButton);
            this.Controls.Add(this.FE6Button);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.ROMVersion);
            this.Controls.Add(this.label1);
            this.Name = "ErorrUnknownROM";
            this.Text = "不明なバージョンのROM";
            this.Load += new System.EventHandler(this.ErorrUnknownROM_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ROMVersion;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Button FE6Button;
        private System.Windows.Forms.Button FE7UButton;
        private System.Windows.Forms.Button FE8UButton;
        private System.Windows.Forms.Button FE8JButton;
        private System.Windows.Forms.Button FE7JButton;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button FE0Button;
    }
}