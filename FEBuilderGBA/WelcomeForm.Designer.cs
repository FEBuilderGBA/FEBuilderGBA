namespace FEBuilderGBA
{
    partial class WelcomeForm
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
            this.ManButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.OpenLastROMButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.UpdateCheckButton = new System.Windows.Forms.Button();
            this.OpenROMButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.VersionLabel = new System.Windows.Forms.TextBox();
            this.LANG_ZH_Label = new System.Windows.Forms.Label();
            this.LANG_JP_Label = new System.Windows.Forms.Label();
            this.LANG_EN_Label = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LANG_ZH_Label);
            this.panel1.Controls.Add(this.LANG_JP_Label);
            this.panel1.Controls.Add(this.LANG_EN_Label);
            this.panel1.Controls.Add(this.VersionLabel);
            this.panel1.Controls.Add(this.ManButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.OpenLastROMButton);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.UpdateCheckButton);
            this.panel1.Controls.Add(this.OpenROMButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1128, 529);
            this.panel1.TabIndex = 1;
            // 
            // ManButton
            // 
            this.ManButton.Location = new System.Drawing.Point(214, 474);
            this.ManButton.Name = "ManButton";
            this.ManButton.Size = new System.Drawing.Size(851, 50);
            this.ManButton.TabIndex = 8;
            this.ManButton.Text = "FEBuilderGBAの使い方の説明書を見る";
            this.ManButton.UseVisualStyleBackColor = true;
            this.ManButton.Click += new System.EventHandler(this.ManButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 490);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "オンラインマニュアル";
            // 
            // OpenLastROMButton
            // 
            this.OpenLastROMButton.Location = new System.Drawing.Point(214, 281);
            this.OpenLastROMButton.Name = "OpenLastROMButton";
            this.OpenLastROMButton.Size = new System.Drawing.Size(851, 47);
            this.OpenLastROMButton.TabIndex = 0;
            this.OpenLastROMButton.Text = "最後に開いたROM";
            this.OpenLastROMButton.UseVisualStyleBackColor = true;
            this.OpenLastROMButton.Click += new System.EventHandler(this.OpenLastROMButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 294);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "最後に開いたROM";
            // 
            // UpdateCheckButton
            // 
            this.UpdateCheckButton.Location = new System.Drawing.Point(214, 411);
            this.UpdateCheckButton.Name = "UpdateCheckButton";
            this.UpdateCheckButton.Size = new System.Drawing.Size(851, 50);
            this.UpdateCheckButton.TabIndex = 2;
            this.UpdateCheckButton.Text = "FEBuilderGBAを最新版へ更新する";
            this.UpdateCheckButton.UseVisualStyleBackColor = true;
            this.UpdateCheckButton.Click += new System.EventHandler(this.UpdateCheckButton_Click);
            // 
            // OpenROMButton
            // 
            this.OpenROMButton.Location = new System.Drawing.Point(214, 346);
            this.OpenROMButton.Name = "OpenROMButton";
            this.OpenROMButton.Size = new System.Drawing.Size(851, 47);
            this.OpenROMButton.TabIndex = 1;
            this.OpenROMButton.Text = "他のFE ROMを開く";
            this.OpenROMButton.UseVisualStyleBackColor = true;
            this.OpenROMButton.Click += new System.EventHandler(this.OpenROMButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 424);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "アップデートチェック";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "ROMを開く";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FEBuilderGBA.Properties.Resources.FEBuilderGBA_logo1;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1123, 278);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // VersionLabel
            // 
            this.VersionLabel.BackColor = System.Drawing.SystemColors.Control;
            this.VersionLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VersionLabel.Location = new System.Drawing.Point(924, 225);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.ReadOnly = true;
            this.VersionLabel.Size = new System.Drawing.Size(193, 18);
            this.VersionLabel.TabIndex = 9;
            this.VersionLabel.Text = "Ver:";
            // 
            // LANG_ZH_Label
            // 
            this.LANG_ZH_Label.AutoSize = true;
            this.LANG_ZH_Label.Location = new System.Drawing.Point(1060, 255);
            this.LANG_ZH_Label.Name = "LANG_ZH_Label";
            this.LANG_ZH_Label.Size = new System.Drawing.Size(30, 18);
            this.LANG_ZH_Label.TabIndex = 29;
            this.LANG_ZH_Label.Text = "ZH";
            this.LANG_ZH_Label.Click += new System.EventHandler(this.LANG_ZH_Label_Click);
            // 
            // LANG_JP_Label
            // 
            this.LANG_JP_Label.AutoSize = true;
            this.LANG_JP_Label.Location = new System.Drawing.Point(995, 255);
            this.LANG_JP_Label.Name = "LANG_JP_Label";
            this.LANG_JP_Label.Size = new System.Drawing.Size(29, 18);
            this.LANG_JP_Label.TabIndex = 28;
            this.LANG_JP_Label.Text = "JP";
            this.LANG_JP_Label.Click += new System.EventHandler(this.LANG_JP_Label_Click);
            // 
            // LANG_EN_Label
            // 
            this.LANG_EN_Label.AutoSize = true;
            this.LANG_EN_Label.Location = new System.Drawing.Point(923, 255);
            this.LANG_EN_Label.Name = "LANG_EN_Label";
            this.LANG_EN_Label.Size = new System.Drawing.Size(30, 18);
            this.LANG_EN_Label.TabIndex = 27;
            this.LANG_EN_Label.Text = "EN";
            this.LANG_EN_Label.Click += new System.EventHandler(this.LANG_EN_Label_Click);
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1156, 550);
            this.Controls.Add(this.panel1);
            this.Name = "WelcomeForm";
            this.Text = "Welcome to the FEBuilderGBA";
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OpenROMButton;
        private System.Windows.Forms.Button UpdateCheckButton;
        private System.Windows.Forms.Button OpenLastROMButton;
        private System.Windows.Forms.Button ManButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox VersionLabel;
        private System.Windows.Forms.Label LANG_ZH_Label;
        private System.Windows.Forms.Label LANG_JP_Label;
        private System.Windows.Forms.Label LANG_EN_Label;
    }
}