namespace FEBuilderGBA
{
    partial class DisASMInnerControl
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
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.decompile_button = new System.Windows.Forms.Button();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.ControlPanelCommand = new System.Windows.Forms.Panel();
            this.AsmEditButton = new System.Windows.Forms.Button();
            this.DirectEditButton = new System.Windows.Forms.Button();
            this.ParamExplain1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ScriptCodeName = new FEBuilderGBA.TextBoxEx();
            this.ParamSrc1 = new System.Windows.Forms.NumericUpDown();
            this.ParamLabel1 = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.ASMToFileButton = new System.Windows.Forms.Button();
            this.ToClipBordButton = new System.Windows.Forms.Button();
            this.FooterPanel = new System.Windows.Forms.Panel();
            this.FileToASMButton = new System.Windows.Forms.Button();
            this.DumpAll = new System.Windows.Forms.Button();
            this.HexEditorButton = new System.Windows.Forms.Button();
            this.check_vanilla_srccode_button = new System.Windows.Forms.Button();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.MainPanel.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.ControlPanelCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc1)).BeginInit();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HeaderPanel.Controls.Add(this.check_vanilla_srccode_button);
            this.HeaderPanel.Controls.Add(this.decompile_button);
            this.HeaderPanel.Controls.Add(this.ReloadListButton);
            this.HeaderPanel.Controls.Add(this.label1);
            this.HeaderPanel.Controls.Add(this.label2);
            this.HeaderPanel.Controls.Add(this.ReadCount);
            this.HeaderPanel.Controls.Add(this.ReadStartAddress);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(4);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(1486, 30);
            this.HeaderPanel.TabIndex = 54;
            // 
            // decompile_button
            // 
            this.decompile_button.Location = new System.Drawing.Point(1088, -2);
            this.decompile_button.Name = "decompile_button";
            this.decompile_button.Size = new System.Drawing.Size(256, 30);
            this.decompile_button.TabIndex = 29;
            this.decompile_button.Text = "C言語へ逆コンパイル";
            this.decompile_button.UseVisualStyleBackColor = true;
            this.decompile_button.Click += new System.EventHandler(this.decompile_button_Click);
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(453, -2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            this.ReloadListButton.Click += new System.EventHandler(this.ReloadListButton_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 32);
            this.label1.TabIndex = 23;
            this.label1.Text = "アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(222, -2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 32);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込バイト数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ReadCount.Location = new System.Drawing.Point(358, 2);
            this.ReadCount.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(92, 25);
            this.ReadCount.TabIndex = 28;
            this.ReadCount.ValueChanged += new System.EventHandler(this.ReadCount_ValueChanged);
            this.ReadCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReadCount_KeyDown);
            this.ReadCount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ReadCount_MouseDown);
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ReadStartAddress.Location = new System.Drawing.Point(90, 3);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 0;
            this.ReadStartAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReadStartAddress_KeyDown);
            // 
            // MainPanel
            // 
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.ControlPanel);
            this.MainPanel.Controls.Add(this.AddressList);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 30);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1486, 770);
            this.MainPanel.TabIndex = 55;
            this.MainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainPanel_Paint);
            // 
            // ControlPanel
            // 
            this.ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ControlPanel.Controls.Add(this.ControlPanelCommand);
            this.ControlPanel.Controls.Add(this.CloseButton);
            this.ControlPanel.Location = new System.Drawing.Point(2, 566);
            this.ControlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(1466, 66);
            this.ControlPanel.TabIndex = 3;
            this.ControlPanel.Visible = false;
            // 
            // ControlPanelCommand
            // 
            this.ControlPanelCommand.Controls.Add(this.AsmEditButton);
            this.ControlPanelCommand.Controls.Add(this.DirectEditButton);
            this.ControlPanelCommand.Controls.Add(this.ParamExplain1);
            this.ControlPanelCommand.Controls.Add(this.label3);
            this.ControlPanelCommand.Controls.Add(this.ScriptCodeName);
            this.ControlPanelCommand.Controls.Add(this.ParamSrc1);
            this.ControlPanelCommand.Controls.Add(this.ParamLabel1);
            this.ControlPanelCommand.Location = new System.Drawing.Point(0, 2);
            this.ControlPanelCommand.Margin = new System.Windows.Forms.Padding(4);
            this.ControlPanelCommand.Name = "ControlPanelCommand";
            this.ControlPanelCommand.Size = new System.Drawing.Size(1366, 63);
            this.ControlPanelCommand.TabIndex = 202;
            // 
            // AsmEditButton
            // 
            this.AsmEditButton.Location = new System.Drawing.Point(985, 30);
            this.AsmEditButton.Name = "AsmEditButton";
            this.AsmEditButton.Size = new System.Drawing.Size(356, 30);
            this.AsmEditButton.TabIndex = 202;
            this.AsmEditButton.Text = "ASMコードの直編集(&N)";
            this.AsmEditButton.UseVisualStyleBackColor = true;
            this.AsmEditButton.Click += new System.EventHandler(this.DirectEditButton_Click);
            // 
            // DirectEditButton
            // 
            this.DirectEditButton.Location = new System.Drawing.Point(1600, 46);
            this.DirectEditButton.Margin = new System.Windows.Forms.Padding(4);
            this.DirectEditButton.Name = "DirectEditButton";
            this.DirectEditButton.Size = new System.Drawing.Size(416, 45);
            this.DirectEditButton.TabIndex = 201;
            this.DirectEditButton.Text = "ASMコードの直編集(&N)";
            this.DirectEditButton.UseVisualStyleBackColor = true;
            this.DirectEditButton.Click += new System.EventHandler(this.DirectEditButton_Click);
            // 
            // ParamExplain1
            // 
            this.ParamExplain1.AutoSize = true;
            this.ParamExplain1.Location = new System.Drawing.Point(356, 36);
            this.ParamExplain1.Name = "ParamExplain1";
            this.ParamExplain1.Size = new System.Drawing.Size(312, 18);
            this.ParamExplain1.TabIndex = 200;
            this.ParamExplain1.Text = "リンクをクリックするか、Jで関数に移動します";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(8, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 58;
            this.label3.Text = "Code";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScriptCodeName
            // 
            this.ScriptCodeName.ErrorMessage = "";
            this.ScriptCodeName.Location = new System.Drawing.Point(146, 4);
            this.ScriptCodeName.Margin = new System.Windows.Forms.Padding(2);
            this.ScriptCodeName.Name = "ScriptCodeName";
            this.ScriptCodeName.Placeholder = "";
            this.ScriptCodeName.ReadOnly = true;
            this.ScriptCodeName.Size = new System.Drawing.Size(1198, 25);
            this.ScriptCodeName.TabIndex = 199;
            // 
            // ParamSrc1
            // 
            this.ParamSrc1.Hexadecimal = true;
            this.ParamSrc1.Location = new System.Drawing.Point(146, 34);
            this.ParamSrc1.Margin = new System.Windows.Forms.Padding(2);
            this.ParamSrc1.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ParamSrc1.Name = "ParamSrc1";
            this.ParamSrc1.ReadOnly = true;
            this.ParamSrc1.Size = new System.Drawing.Size(183, 25);
            this.ParamSrc1.TabIndex = 0;
            // 
            // ParamLabel1
            // 
            this.ParamLabel1.Location = new System.Drawing.Point(4, 38);
            this.ParamLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ParamLabel1.Name = "ParamLabel1";
            this.ParamLabel1.Size = new System.Drawing.Size(134, 18);
            this.ParamLabel1.TabIndex = 1;
            this.ParamLabel1.Text = "&Jump";
            this.ParamLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ParamLabel1.Click += new System.EventHandler(this.ParamLabel1_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(1380, 8);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(2);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(82, 32);
            this.CloseButton.TabIndex = 201;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // AddressList
            // 
            this.AddressList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressList.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 24;
            this.AddressList.Location = new System.Drawing.Point(0, 0);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(1484, 768);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            this.AddressList.DoubleClick += new System.EventHandler(this.AddressList_DoubleClick);
            this.AddressList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressList_KeyDown);
            // 
            // ASMToFileButton
            // 
            this.ASMToFileButton.Location = new System.Drawing.Point(502, 4);
            this.ASMToFileButton.Name = "ASMToFileButton";
            this.ASMToFileButton.Size = new System.Drawing.Size(256, 30);
            this.ASMToFileButton.TabIndex = 58;
            this.ASMToFileButton.Text = "ファイルへエクスポート";
            this.ASMToFileButton.UseVisualStyleBackColor = true;
            this.ASMToFileButton.Click += new System.EventHandler(this.ToFileButton_Click);
            // 
            // ToClipBordButton
            // 
            this.ToClipBordButton.Location = new System.Drawing.Point(3, 3);
            this.ToClipBordButton.Name = "ToClipBordButton";
            this.ToClipBordButton.Size = new System.Drawing.Size(243, 30);
            this.ToClipBordButton.TabIndex = 56;
            this.ToClipBordButton.Text = "クリックボードへ(Ctrl+C)";
            this.ToClipBordButton.UseVisualStyleBackColor = true;
            this.ToClipBordButton.Click += new System.EventHandler(this.ToClipBordButton_Click);
            // 
            // FooterPanel
            // 
            this.FooterPanel.Controls.Add(this.FileToASMButton);
            this.FooterPanel.Controls.Add(this.DumpAll);
            this.FooterPanel.Controls.Add(this.HexEditorButton);
            this.FooterPanel.Controls.Add(this.ToClipBordButton);
            this.FooterPanel.Controls.Add(this.ASMToFileButton);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FooterPanel.Location = new System.Drawing.Point(0, 800);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Size = new System.Drawing.Size(1486, 38);
            this.FooterPanel.TabIndex = 60;
            // 
            // FileToASMButton
            // 
            this.FileToASMButton.Location = new System.Drawing.Point(254, 3);
            this.FileToASMButton.Name = "FileToASMButton";
            this.FileToASMButton.Size = new System.Drawing.Size(244, 30);
            this.FileToASMButton.TabIndex = 62;
            this.FileToASMButton.Text = "ファイルからインポート";
            this.FileToASMButton.UseVisualStyleBackColor = true;
            this.FileToASMButton.Click += new System.EventHandler(this.FileToASMButton_Click);
            // 
            // DumpAll
            // 
            this.DumpAll.Location = new System.Drawing.Point(764, 4);
            this.DumpAll.Name = "DumpAll";
            this.DumpAll.Size = new System.Drawing.Size(334, 30);
            this.DumpAll.TabIndex = 61;
            this.DumpAll.Text = "全部ファイルに保存";
            this.DumpAll.UseVisualStyleBackColor = true;
            this.DumpAll.Click += new System.EventHandler(this.DumpAllButton_Click);
            // 
            // HexEditorButton
            // 
            this.HexEditorButton.Location = new System.Drawing.Point(1102, 4);
            this.HexEditorButton.Name = "HexEditorButton";
            this.HexEditorButton.Size = new System.Drawing.Size(243, 30);
            this.HexEditorButton.TabIndex = 59;
            this.HexEditorButton.Text = "HexEditor";
            this.HexEditorButton.UseVisualStyleBackColor = true;
            this.HexEditorButton.Click += new System.EventHandler(this.HexEditorButton_Click);
            // 
            // check_vanilla_srccode_button
            // 
            this.check_vanilla_srccode_button.Location = new System.Drawing.Point(827, -1);
            this.check_vanilla_srccode_button.Name = "check_vanilla_srccode_button";
            this.check_vanilla_srccode_button.Size = new System.Drawing.Size(256, 30);
            this.check_vanilla_srccode_button.TabIndex = 30;
            this.check_vanilla_srccode_button.Text = "バニラのソースコードを確認";
            this.check_vanilla_srccode_button.UseVisualStyleBackColor = true;
            this.check_vanilla_srccode_button.Click += new System.EventHandler(this.check_vanilla_srccode_button_Click);
            // 
            // DisASMInnerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.HeaderPanel);
            this.Controls.Add(this.FooterPanel);
            this.Name = "DisASMInnerControl";
            this.Size = new System.Drawing.Size(1486, 838);
            this.Load += new System.EventHandler(this.DisASMForm_Load);
            this.Resize += new System.EventHandler(this.DisASMForm_Resize);
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.MainPanel.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanelCommand.ResumeLayout(false);
            this.ControlPanelCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc1)).EndInit();
            this.FooterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel MainPanel;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button ASMToFileButton;
        private System.Windows.Forms.Button ToClipBordButton;
        private System.Windows.Forms.Panel FooterPanel;
        private System.Windows.Forms.Button HexEditorButton;
        private System.Windows.Forms.Button DumpAll;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.Panel ControlPanelCommand;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx ScriptCodeName;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label ParamLabel1;
        private System.Windows.Forms.NumericUpDown ParamSrc1;
        private System.Windows.Forms.Label ParamExplain1;
        private System.Windows.Forms.Button FileToASMButton;
        private System.Windows.Forms.Button decompile_button;
        private System.Windows.Forms.Button DirectEditButton;
        private System.Windows.Forms.Button AsmEditButton;
        private System.Windows.Forms.Button check_vanilla_srccode_button;
    }
}