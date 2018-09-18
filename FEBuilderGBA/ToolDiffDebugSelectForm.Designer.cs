namespace FEBuilderGBA
{
    partial class ToolDiffDebugSelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolDiffDebugSelectForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.SelectButton = new System.Windows.Forms.Button();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.PrefixTextBox = new FEBuilderGBA.TextBoxEx();
            this.AppendBackupDirectoryButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.BackupList = new FEBuilderGBA.ListBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TestPlayButton = new System.Windows.Forms.Button();
            this.SelectROMButton = new System.Windows.Forms.Button();
            this.ThisFileInfo = new FEBuilderGBA.TextBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SelectButton);
            this.panel1.Controls.Add(this.OrignalFilename);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.PrefixTextBox);
            this.panel1.Controls.Add(this.AppendBackupDirectoryButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Controls.Add(this.BackupList);
            this.panel1.Location = new System.Drawing.Point(12, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(712, 828);
            this.panel1.TabIndex = 2;
            // 
            // SelectButton
            // 
            this.SelectButton.Location = new System.Drawing.Point(653, 726);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(58, 36);
            this.SelectButton.TabIndex = 2;
            this.SelectButton.Text = "..";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // OrignalFilename
            // 
            this.OrignalFilename.ErrorMessage = "";
            this.OrignalFilename.Location = new System.Drawing.Point(217, 731);
            this.OrignalFilename.Name = "OrignalFilename";
            this.OrignalFilename.Placeholder = "";
            this.OrignalFilename.Size = new System.Drawing.Size(430, 25);
            this.OrignalFilename.TabIndex = 1;
            this.OrignalFilename.DoubleClick += new System.EventHandler(this.SelectButton_Click);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(4, 731);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(208, 26);
            this.label6.TabIndex = 62;
            this.label6.Text = "無改造ROM";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(4, 766);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(208, 26);
            this.label5.TabIndex = 61;
            this.label5.Text = "探索するprefix";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PrefixTextBox
            // 
            this.PrefixTextBox.ErrorMessage = "";
            this.PrefixTextBox.Location = new System.Drawing.Point(217, 764);
            this.PrefixTextBox.Name = "PrefixTextBox";
            this.PrefixTextBox.Placeholder = "";
            this.PrefixTextBox.Size = new System.Drawing.Size(492, 25);
            this.PrefixTextBox.TabIndex = 3;
            this.PrefixTextBox.TextChanged += new System.EventHandler(this.PrefixTextBox_TextChanged);
            // 
            // AppendBackupDirectoryButton
            // 
            this.AppendBackupDirectoryButton.Location = new System.Drawing.Point(3, 795);
            this.AppendBackupDirectoryButton.Name = "AppendBackupDirectoryButton";
            this.AppendBackupDirectoryButton.Size = new System.Drawing.Size(706, 29);
            this.AppendBackupDirectoryButton.TabIndex = 4;
            this.AppendBackupDirectoryButton.Text = "バックアップROMがある場所を追加";
            this.AppendBackupDirectoryButton.UseVisualStyleBackColor = true;
            this.AppendBackupDirectoryButton.Click += new System.EventHandler(this.AppendBackupDirectoryButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(4, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 181);
            this.label1.TabIndex = 57;
            this.label1.Text = "↑最新";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(5, 504);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 216);
            this.label2.TabIndex = 58;
            this.label2.Text = "より古い↓";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Location = new System.Drawing.Point(4, 2);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(704, 26);
            this.label30.TabIndex = 56;
            this.label30.Text = "バックアップ履歴(上が最新) ";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BackupList
            // 
            this.BackupList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BackupList.FormattingEnabled = true;
            this.BackupList.ItemHeight = 18;
            this.BackupList.Location = new System.Drawing.Point(34, 30);
            this.BackupList.Margin = new System.Windows.Forms.Padding(4);
            this.BackupList.Name = "BackupList";
            this.BackupList.Size = new System.Drawing.Size(674, 688);
            this.BackupList.TabIndex = 0;
            this.BackupList.SelectedIndexChanged += new System.EventHandler(this.BackupList_SelectedIndexChanged);
            this.BackupList.DoubleClick += new System.EventHandler(this.BackupList_DoubleClick);
            this.BackupList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BackupList_KeyDown);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(727, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(430, 345);
            this.label3.TabIndex = 59;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.TestPlayButton);
            this.panel2.Controls.Add(this.SelectROMButton);
            this.panel2.Controls.Add(this.ThisFileInfo);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(727, 353);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(428, 480);
            this.panel2.TabIndex = 60;
            // 
            // TestPlayButton
            // 
            this.TestPlayButton.Location = new System.Drawing.Point(0, 249);
            this.TestPlayButton.Name = "TestPlayButton";
            this.TestPlayButton.Size = new System.Drawing.Size(425, 80);
            this.TestPlayButton.TabIndex = 58;
            this.TestPlayButton.Text = "選択しているROMをエミュレータでテストプレイする";
            this.TestPlayButton.UseVisualStyleBackColor = true;
            this.TestPlayButton.Click += new System.EventHandler(this.TestPlayButton_Click);
            // 
            // SelectROMButton
            // 
            this.SelectROMButton.Location = new System.Drawing.Point(2, 383);
            this.SelectROMButton.Name = "SelectROMButton";
            this.SelectROMButton.Size = new System.Drawing.Size(425, 93);
            this.SelectROMButton.TabIndex = 1;
            this.SelectROMButton.Text = "このROMが最後に安定していたROMとして、\r\n相違点を取得する";
            this.SelectROMButton.UseVisualStyleBackColor = true;
            this.SelectROMButton.Click += new System.EventHandler(this.SelectROMButton_Click);
            // 
            // ThisFileInfo
            // 
            this.ThisFileInfo.ErrorMessage = "";
            this.ThisFileInfo.Location = new System.Drawing.Point(3, 29);
            this.ThisFileInfo.Multiline = true;
            this.ThisFileInfo.Name = "ThisFileInfo";
            this.ThisFileInfo.Placeholder = "";
            this.ThisFileInfo.ReadOnly = true;
            this.ThisFileInfo.Size = new System.Drawing.Size(421, 214);
            this.ThisFileInfo.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(2, -1);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(424, 27);
            this.label4.TabIndex = 57;
            this.label4.Text = "選択されているROM情報";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToolDiffDebugSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1161, 842);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Name = "ToolDiffDebugSelectForm";
            this.Text = "比較デバッグツール";
            this.Load += new System.EventHandler(this.ToolDiffDebugSelectForm_Load);
            this.Shown += new System.EventHandler(this.ToolDiffDebugSelectForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label30;
        private ListBoxEx BackupList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button AppendBackupDirectoryButton;
        private FEBuilderGBA.TextBoxEx ThisFileInfo;
        private System.Windows.Forms.Button SelectROMButton;
        private System.Windows.Forms.Label label5;
        private FEBuilderGBA.TextBoxEx PrefixTextBox;
        private System.Windows.Forms.Label label6;
        private FEBuilderGBA.TextBoxEx OrignalFilename;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button TestPlayButton;
    }
}