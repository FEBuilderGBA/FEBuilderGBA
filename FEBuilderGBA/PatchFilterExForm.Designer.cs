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
            this.label1 = new System.Windows.Forms.Label();
            this.SortFilterComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // InstalledButton
            // 
            this.InstalledButton.Location = new System.Drawing.Point(12, 78);
            this.InstalledButton.Name = "InstalledButton";
            this.InstalledButton.Size = new System.Drawing.Size(435, 43);
            this.InstalledButton.TabIndex = 1;
            this.InstalledButton.Text = "インストール済みのパッチ(!)";
            this.InstalledButton.UseVisualStyleBackColor = true;
            this.InstalledButton.Click += new System.EventHandler(this.InstalledButton_Click);
            // 
            // ImageButton
            // 
            this.ImageButton.Location = new System.Drawing.Point(12, 134);
            this.ImageButton.Name = "ImageButton";
            this.ImageButton.Size = new System.Drawing.Size(435, 43);
            this.ImageButton.TabIndex = 2;
            this.ImageButton.Text = "画像(#IMAGE)";
            this.ImageButton.UseVisualStyleBackColor = true;
            this.ImageButton.Click += new System.EventHandler(this.ImageButton_Click);
            // 
            // SoundButton
            // 
            this.SoundButton.Location = new System.Drawing.Point(12, 188);
            this.SoundButton.Name = "SoundButton";
            this.SoundButton.Size = new System.Drawing.Size(435, 43);
            this.SoundButton.TabIndex = 3;
            this.SoundButton.Text = "音楽(#SOUND)";
            this.SoundButton.UseVisualStyleBackColor = true;
            this.SoundButton.Click += new System.EventHandler(this.SoundButton_Click);
            // 
            // EngineButton
            // 
            this.EngineButton.Location = new System.Drawing.Point(12, 241);
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
            this.EventButton.Location = new System.Drawing.Point(12, 293);
            this.EventButton.Name = "EventButton";
            this.EventButton.Size = new System.Drawing.Size(435, 43);
            this.EventButton.TabIndex = 5;
            this.EventButton.Text = "イベント命令(#EVENT)";
            this.EventButton.UseVisualStyleBackColor = true;
            this.EventButton.Click += new System.EventHandler(this.EventButton_Click);
            // 
            // ESSENTIALFIXESButton
            // 
            this.ESSENTIALFIXESButton.Location = new System.Drawing.Point(12, 345);
            this.ESSENTIALFIXESButton.Name = "ESSENTIALFIXESButton";
            this.ESSENTIALFIXESButton.Size = new System.Drawing.Size(435, 43);
            this.ESSENTIALFIXESButton.TabIndex = 6;
            this.ESSENTIALFIXESButton.Text = "定番の修正(#ESSENTIALFIXES)";
            this.ESSENTIALFIXESButton.UseVisualStyleBackColor = true;
            this.ESSENTIALFIXESButton.Click += new System.EventHandler(this.ESSENTIALFIXESButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 419);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "ソート";
            // 
            // SortFilterComboBox
            // 
            this.SortFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SortFilterComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SortFilterComboBox.FormattingEnabled = true;
            this.SortFilterComboBox.Items.AddRange(new object[] {
            "ソートしない",
            "新しい日付でソート",
            "古い日付でソート",
            "名前でソート"});
            this.SortFilterComboBox.Location = new System.Drawing.Point(12, 444);
            this.SortFilterComboBox.Name = "SortFilterComboBox";
            this.SortFilterComboBox.Size = new System.Drawing.Size(435, 32);
            this.SortFilterComboBox.TabIndex = 8;
            this.SortFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.SortFilterComboBox_SelectedIndexChanged);
            // 
            // PatchFilterExForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(469, 494);
            this.Controls.Add(this.SortFilterComboBox);
            this.Controls.Add(this.label1);
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InstalledButton;
        private System.Windows.Forms.Button ImageButton;
        private System.Windows.Forms.Button SoundButton;
        private System.Windows.Forms.Button EngineButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button EventButton;
        private System.Windows.Forms.Button ESSENTIALFIXESButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SortFilterComboBox;
    }
}