namespace FEBuilderGBA
{
    partial class ToolFELintForm
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
            this.MainTab = new System.Windows.Forms.TabControl();
            this.tabPageScan = new System.Windows.Forms.TabPage();
            this.tabPageNoError = new System.Windows.Forms.TabPage();
            this.ShowAllError2 = new System.Windows.Forms.CheckBox();
            this.DiffDebugToolButton = new System.Windows.Forms.Button();
            this.NoError_ReloadButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.X_SUCCESSMESSAGE = new System.Windows.Forms.Label();
            this.tabPageError = new System.Windows.Forms.TabPage();
            this.Error_ReloadButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ShowAllError1 = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.X_ERRORMESSAGE = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MainTab.SuspendLayout();
            this.tabPageNoError.SuspendLayout();
            this.tabPageError.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.tabPageScan);
            this.MainTab.Controls.Add(this.tabPageNoError);
            this.MainTab.Controls.Add(this.tabPageError);
            this.MainTab.Location = new System.Drawing.Point(3, 3);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(1052, 707);
            this.MainTab.TabIndex = 0;
            // 
            // tabPageScan
            // 
            this.tabPageScan.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageScan.Location = new System.Drawing.Point(4, 28);
            this.tabPageScan.Name = "tabPageScan";
            this.tabPageScan.Size = new System.Drawing.Size(1044, 675);
            this.tabPageScan.TabIndex = 2;
            this.tabPageScan.Text = "Scan";
            // 
            // tabPageNoError
            // 
            this.tabPageNoError.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageNoError.Controls.Add(this.ShowAllError2);
            this.tabPageNoError.Controls.Add(this.DiffDebugToolButton);
            this.tabPageNoError.Controls.Add(this.NoError_ReloadButton);
            this.tabPageNoError.Controls.Add(this.label2);
            this.tabPageNoError.Controls.Add(this.X_SUCCESSMESSAGE);
            this.tabPageNoError.Location = new System.Drawing.Point(4, 28);
            this.tabPageNoError.Name = "tabPageNoError";
            this.tabPageNoError.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNoError.Size = new System.Drawing.Size(1044, 675);
            this.tabPageNoError.TabIndex = 0;
            this.tabPageNoError.Text = "NoError";
            // 
            // ShowAllError2
            // 
            this.ShowAllError2.AutoSize = true;
            this.ShowAllError2.Location = new System.Drawing.Point(808, 75);
            this.ShowAllError2.Name = "ShowAllError2";
            this.ShowAllError2.Size = new System.Drawing.Size(194, 22);
            this.ShowAllError2.TabIndex = 116;
            this.ShowAllError2.Text = "非表示のエラーも表示";
            this.ShowAllError2.UseVisualStyleBackColor = true;
            this.ShowAllError2.CheckedChanged += new System.EventHandler(this.NoError_ReloadButton_Click);
            // 
            // DiffDebugToolButton
            // 
            this.DiffDebugToolButton.Location = new System.Drawing.Point(10, 352);
            this.DiffDebugToolButton.Margin = new System.Windows.Forms.Padding(2);
            this.DiffDebugToolButton.Name = "DiffDebugToolButton";
            this.DiffDebugToolButton.Size = new System.Drawing.Size(277, 36);
            this.DiffDebugToolButton.TabIndex = 115;
            this.DiffDebugToolButton.Text = "比較デバッグツール";
            this.DiffDebugToolButton.UseVisualStyleBackColor = true;
            this.DiffDebugToolButton.Click += new System.EventHandler(this.DiffDebugToolButton_Click);
            // 
            // NoError_ReloadButton
            // 
            this.NoError_ReloadButton.Location = new System.Drawing.Point(808, 30);
            this.NoError_ReloadButton.Name = "NoError_ReloadButton";
            this.NoError_ReloadButton.Size = new System.Drawing.Size(230, 39);
            this.NoError_ReloadButton.TabIndex = 114;
            this.NoError_ReloadButton.Text = "再取得(Ctrl+R)";
            this.NoError_ReloadButton.UseVisualStyleBackColor = true;
            this.NoError_ReloadButton.Click += new System.EventHandler(this.NoError_ReloadButton_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(10, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(879, 209);
            this.label2.TabIndex = 1;
            this.label2.Text = "もし、原因不明のエラーに悩んでいる場合は、「ツール」->「比較デバッグツール」を使ってみてください。\r\nバックアップデータから、正しく動くROMと、正しく動かない" +
    "ROMを比較して自動的にバグを検出します。";
            // 
            // X_SUCCESSMESSAGE
            // 
            this.X_SUCCESSMESSAGE.AutoSize = true;
            this.X_SUCCESSMESSAGE.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.X_SUCCESSMESSAGE.Location = new System.Drawing.Point(6, 30);
            this.X_SUCCESSMESSAGE.Name = "X_SUCCESSMESSAGE";
            this.X_SUCCESSMESSAGE.Size = new System.Drawing.Size(563, 24);
            this.X_SUCCESSMESSAGE.TabIndex = 0;
            this.X_SUCCESSMESSAGE.Text = "このROMには、自動的に検出できるエラーは存在しません。";
            // 
            // tabPageError
            // 
            this.tabPageError.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageError.Controls.Add(this.Error_ReloadButton);
            this.tabPageError.Controls.Add(this.panel2);
            this.tabPageError.Controls.Add(this.X_ERRORMESSAGE);
            this.tabPageError.Location = new System.Drawing.Point(4, 28);
            this.tabPageError.Name = "tabPageError";
            this.tabPageError.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageError.Size = new System.Drawing.Size(1044, 675);
            this.tabPageError.TabIndex = 1;
            this.tabPageError.Text = "Error";
            // 
            // Error_ReloadButton
            // 
            this.Error_ReloadButton.Location = new System.Drawing.Point(806, 32);
            this.Error_ReloadButton.Name = "Error_ReloadButton";
            this.Error_ReloadButton.Size = new System.Drawing.Size(219, 39);
            this.Error_ReloadButton.TabIndex = 113;
            this.Error_ReloadButton.Text = "再取得(Ctrl+R)";
            this.Error_ReloadButton.UseVisualStyleBackColor = true;
            this.Error_ReloadButton.Click += new System.EventHandler(this.Error_ReloadButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ShowAllError1);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.AddressList);
            this.panel2.Location = new System.Drawing.Point(10, 77);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1018, 598);
            this.panel2.TabIndex = 112;
            // 
            // ShowAllError1
            // 
            this.ShowAllError1.AutoSize = true;
            this.ShowAllError1.Location = new System.Drawing.Point(796, 3);
            this.ShowAllError1.Name = "ShowAllError1";
            this.ShowAllError1.Size = new System.Drawing.Size(194, 22);
            this.ShowAllError1.TabIndex = 114;
            this.ShowAllError1.Text = "非表示のエラーも表示";
            this.ShowAllError1.UseVisualStyleBackColor = true;
            this.ShowAllError1.CheckedChanged += new System.EventHandler(this.NoError_ReloadButton_Click);
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Location = new System.Drawing.Point(2, 0);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(1013, 31);
            this.label19.TabIndex = 111;
            this.label19.Text = "名前";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.FormattingEnabled = true;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(2, 31);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(1013, 562);
            this.AddressList.TabIndex = 2;
            this.AddressList.DoubleClick += new System.EventHandler(this.AddressList_DoubleClick);
            this.AddressList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressList_KeyDown);
            // 
            // X_ERRORMESSAGE
            // 
            this.X_ERRORMESSAGE.AutoSize = true;
            this.X_ERRORMESSAGE.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.X_ERRORMESSAGE.Location = new System.Drawing.Point(6, 32);
            this.X_ERRORMESSAGE.Name = "X_ERRORMESSAGE";
            this.X_ERRORMESSAGE.Size = new System.Drawing.Size(341, 24);
            this.X_ERRORMESSAGE.TabIndex = 1;
            this.X_ERRORMESSAGE.Text = "以下のマップにエラーが存在します。";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.MainTab);
            this.panel1.Location = new System.Drawing.Point(13, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1058, 712);
            this.panel1.TabIndex = 1;
            // 
            // ToolFELintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1083, 750);
            this.Controls.Add(this.panel1);
            this.Name = "ToolFELintForm";
            this.Text = "Lint";
            this.Load += new System.EventHandler(this.ToolFELintForm_Load);
            this.Shown += new System.EventHandler(this.ToolFELintForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ToolFELintForm_KeyDown);
            this.MainTab.ResumeLayout(false);
            this.tabPageNoError.ResumeLayout(false);
            this.tabPageNoError.PerformLayout();
            this.tabPageError.ResumeLayout(false);
            this.tabPageError.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage tabPageNoError;
        private System.Windows.Forms.TabPage tabPageError;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label X_SUCCESSMESSAGE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label X_ERRORMESSAGE;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TabPage tabPageScan;
        private System.Windows.Forms.Button Error_ReloadButton;
        private System.Windows.Forms.Button NoError_ReloadButton;
        private System.Windows.Forms.Button DiffDebugToolButton;
        private System.Windows.Forms.CheckBox ShowAllError1;
        private System.Windows.Forms.CheckBox ShowAllError2;
    }
}