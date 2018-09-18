namespace FEBuilderGBA
{
    partial class PatchFilterExForm
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
            this.InstalledButton = new System.Windows.Forms.Button();
            this.ImageButton = new System.Windows.Forms.Button();
            this.SoundButton = new System.Windows.Forms.Button();
            this.EngineButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.EventButton = new System.Windows.Forms.Button();
            this.ESSENTIALFIXESButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InstalledButton
            // 
            this.InstalledButton.Location = new System.Drawing.Point(12, 90);
            this.InstalledButton.Name = "InstalledButton";
            this.InstalledButton.Size = new System.Drawing.Size(435, 43);
            this.InstalledButton.TabIndex = 1;
            this.InstalledButton.Text = "インストール済みのパッチ(!)";
            this.InstalledButton.UseVisualStyleBackColor = true;
            this.InstalledButton.Click += new System.EventHandler(this.InstalledButton_Click);
            // 
            // ImageButton
            // 
            this.ImageButton.Location = new System.Drawing.Point(12, 159);
            this.ImageButton.Name = "ImageButton";
            this.ImageButton.Size = new System.Drawing.Size(435, 43);
            this.ImageButton.TabIndex = 2;
            this.ImageButton.Text = "画像(#IMAGE)";
            this.ImageButton.UseVisualStyleBackColor = true;
            this.ImageButton.Click += new System.EventHandler(this.ImageButton_Click);
            // 
            // SoundButton
            // 
            this.SoundButton.Location = new System.Drawing.Point(12, 230);
            this.SoundButton.Name = "SoundButton";
            this.SoundButton.Size = new System.Drawing.Size(435, 43);
            this.SoundButton.TabIndex = 3;
            this.SoundButton.Text = "音楽(#SOUND)";
            this.SoundButton.UseVisualStyleBackColor = true;
            this.SoundButton.Click += new System.EventHandler(this.SoundButton_Click);
            // 
            // EngineButton
            // 
            this.EngineButton.Location = new System.Drawing.Point(12, 298);
            this.EngineButton.Name = "EngineButton";
            this.EngineButton.Size = new System.Drawing.Size(435, 43);
            this.EngineButton.TabIndex = 4;
            this.EngineButton.Text = "エンジン(#ENGINE)";
            this.EngineButton.UseVisualStyleBackColor = true;
            this.EngineButton.Click += new System.EventHandler(this.EngineButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(12, 23);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(435, 43);
            this.ClearButton.TabIndex = 0;
            this.ClearButton.Text = "フィルタ解除";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // EventButton
            // 
            this.EventButton.Location = new System.Drawing.Point(12, 365);
            this.EventButton.Name = "EventButton";
            this.EventButton.Size = new System.Drawing.Size(435, 43);
            this.EventButton.TabIndex = 5;
            this.EventButton.Text = "イベント命令(#EVENT)";
            this.EventButton.UseVisualStyleBackColor = true;
            this.EventButton.Click += new System.EventHandler(this.EventButton_Click);
            // 
            // ESSENTIALFIXESButton
            // 
            this.ESSENTIALFIXESButton.Location = new System.Drawing.Point(12, 437);
            this.ESSENTIALFIXESButton.Name = "ESSENTIALFIXESButton";
            this.ESSENTIALFIXESButton.Size = new System.Drawing.Size(435, 43);
            this.ESSENTIALFIXESButton.TabIndex = 6;
            this.ESSENTIALFIXESButton.Text = "定番の修正(#ESSENTIALFIXES)";
            this.ESSENTIALFIXESButton.UseVisualStyleBackColor = true;
            this.ESSENTIALFIXESButton.Click += new System.EventHandler(this.ESSENTIALFIXESButton_Click);
            // 
            // PatchFilterExForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(469, 540);
            this.Controls.Add(this.ESSENTIALFIXESButton);
            this.Controls.Add(this.EventButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.EngineButton);
            this.Controls.Add(this.SoundButton);
            this.Controls.Add(this.ImageButton);
            this.Controls.Add(this.InstalledButton);
            this.Name = "PatchFilterExForm";
            this.Text = "フィルタの適応";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button InstalledButton;
        private System.Windows.Forms.Button ImageButton;
        private System.Windows.Forms.Button SoundButton;
        private System.Windows.Forms.Button EngineButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button EventButton;
        private System.Windows.Forms.Button ESSENTIALFIXESButton;
    }
}