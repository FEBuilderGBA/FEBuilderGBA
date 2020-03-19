namespace FEBuilderGBA
{
    partial class ToolCustomBuildForm
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
            this.Explain = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TargetFilenameTextBox = new System.Windows.Forms.TextBox();
            this.TargetFilenameSelectButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.OrignalROMTextArea = new System.Windows.Forms.TextBox();
            this.OrignalROMSelectButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TakeoverSkillAssignmentComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Explain
            // 
            this.Explain.Location = new System.Drawing.Point(16, 238);
            this.Explain.Multiline = true;
            this.Explain.Name = "Explain";
            this.Explain.ReadOnly = true;
            this.Explain.Size = new System.Drawing.Size(989, 156);
            this.Explain.TabIndex = 0;
            this.Explain.Text = "スキル拡張をソースコードからビルドしてインストールします。\r\nこの機能は危険なので、実行前に必ずバックアップを取ってください。\r\nまた、変更後に必ず動作テストをし" +
    "てください。\r\nもし、動作テストに不合格だった場合は、危険なので保全せずにFEBuilderGBAを終了してください。\r\n\r\n詳細な説明と、スキル拡張のソースコ" +
    "ードのダウンロードは、以下のURLを参照してください。\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "ターゲット:ビルドするスキル拡張";
            // 
            // TargetFilenameTextBox
            // 
            this.TargetFilenameTextBox.Location = new System.Drawing.Point(16, 59);
            this.TargetFilenameTextBox.Name = "TargetFilenameTextBox";
            this.TargetFilenameTextBox.Size = new System.Drawing.Size(840, 25);
            this.TargetFilenameTextBox.TabIndex = 0;
            this.TargetFilenameTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TargetFilenameTextBox_MouseDoubleClick);
            // 
            // TargetFilenameSelectButton
            // 
            this.TargetFilenameSelectButton.Location = new System.Drawing.Point(873, 56);
            this.TargetFilenameSelectButton.Name = "TargetFilenameSelectButton";
            this.TargetFilenameSelectButton.Size = new System.Drawing.Size(130, 31);
            this.TargetFilenameSelectButton.TabIndex = 1;
            this.TargetFilenameSelectButton.Text = "別ファイル選択";
            this.TargetFilenameSelectButton.UseVisualStyleBackColor = true;
            this.TargetFilenameSelectButton.Click += new System.EventHandler(this.TargetFilenameSelectButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(760, 400);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(242, 79);
            this.RunButton.TabIndex = 5;
            this.RunButton.Text = "ビルド開始";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "無改造ROM";
            // 
            // OrignalROMTextArea
            // 
            this.OrignalROMTextArea.Location = new System.Drawing.Point(16, 125);
            this.OrignalROMTextArea.Name = "OrignalROMTextArea";
            this.OrignalROMTextArea.Size = new System.Drawing.Size(840, 25);
            this.OrignalROMTextArea.TabIndex = 2;
            this.OrignalROMTextArea.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OrignalROMTextArea_MouseDoubleClick);
            // 
            // OrignalROMSelectButton
            // 
            this.OrignalROMSelectButton.Location = new System.Drawing.Point(872, 122);
            this.OrignalROMSelectButton.Name = "OrignalROMSelectButton";
            this.OrignalROMSelectButton.Size = new System.Drawing.Size(130, 31);
            this.OrignalROMSelectButton.TabIndex = 3;
            this.OrignalROMSelectButton.Text = "別ファイル選択";
            this.OrignalROMSelectButton.UseVisualStyleBackColor = true;
            this.OrignalROMSelectButton.Click += new System.EventHandler(this.OrignalROMSelectButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "スキル割り当ての引き継ぎ";
            // 
            // TakeoverSkillAssignmentComboBox
            // 
            this.TakeoverSkillAssignmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TakeoverSkillAssignmentComboBox.FormattingEnabled = true;
            this.TakeoverSkillAssignmentComboBox.Items.AddRange(new object[] {
            "0=引き継がない",
            "1=引き継ぐ"});
            this.TakeoverSkillAssignmentComboBox.Location = new System.Drawing.Point(16, 193);
            this.TakeoverSkillAssignmentComboBox.Name = "TakeoverSkillAssignmentComboBox";
            this.TakeoverSkillAssignmentComboBox.Size = new System.Drawing.Size(840, 26);
            this.TakeoverSkillAssignmentComboBox.TabIndex = 4;
            // 
            // ToolCustomBuildForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1021, 491);
            this.Controls.Add(this.TakeoverSkillAssignmentComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OrignalROMSelectButton);
            this.Controls.Add(this.OrignalROMTextArea);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.TargetFilenameSelectButton);
            this.Controls.Add(this.TargetFilenameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Explain);
            this.Name = "ToolCustomBuildForm";
            this.Text = "カスタムビルド";
            this.Load += new System.EventHandler(this.ToolCustomBuildForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Explain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TargetFilenameTextBox;
        private System.Windows.Forms.Button TargetFilenameSelectButton;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox OrignalROMTextArea;
        private System.Windows.Forms.Button OrignalROMSelectButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox TakeoverSkillAssignmentComboBox;
    }
}