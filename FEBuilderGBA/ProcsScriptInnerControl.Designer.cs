namespace FEBuilderGBA
{
    partial class ProcsScriptInnerControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.ListBoxPanel = new System.Windows.Forms.Panel();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.ParamValue2 = new FEBuilderGBA.TextBoxEx();
            this.ParamValue1 = new FEBuilderGBA.TextBoxEx();
            this.ControlPanelCommand = new System.Windows.Forms.Panel();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.AddressTextBox = new System.Windows.Forms.TextBox();
            this.CommentTextBox = new FEBuilderGBA.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.ASMTextBox = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.ScriptChangeButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ScriptCodeName = new FEBuilderGBA.TextBoxEx();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ParamSrc2 = new System.Windows.Forms.NumericUpDown();
            this.ParamLabel2 = new System.Windows.Forms.Label();
            this.ParamSrc1 = new System.Windows.Forms.NumericUpDown();
            this.ParamLabel1 = new System.Windows.Forms.Label();
            this.Script = new FEBuilderGBA.ListBoxEx();
            this.panel5 = new System.Windows.Forms.Panel();
            this.N_ReloadListButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.N_ReadCount = new System.Windows.Forms.NumericUpDown();
            this.AllWriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.EventToFileButton = new System.Windows.Forms.Button();
            this.FileToEventButton = new System.Windows.Forms.Button();
            this.ListBoxPanel.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.ControlPanelCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.SuspendLayout();
            // 
            // ListBoxPanel
            // 
            this.ListBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ListBoxPanel.Controls.Add(this.EventToFileButton);
            this.ListBoxPanel.Controls.Add(this.FileToEventButton);
            this.ListBoxPanel.Controls.Add(this.ControlPanel);
            this.ListBoxPanel.Controls.Add(this.Script);
            this.ListBoxPanel.Location = new System.Drawing.Point(3, 31);
            this.ListBoxPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ListBoxPanel.Name = "ListBoxPanel";
            this.ListBoxPanel.Size = new System.Drawing.Size(1089, 798);
            this.ListBoxPanel.TabIndex = 151;
            // 
            // ControlPanel
            // 
            this.ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ControlPanel.Controls.Add(this.ParamValue2);
            this.ControlPanel.Controls.Add(this.ParamValue1);
            this.ControlPanel.Controls.Add(this.ControlPanelCommand);
            this.ControlPanel.Controls.Add(this.CloseButton);
            this.ControlPanel.Controls.Add(this.ParamSrc2);
            this.ControlPanel.Controls.Add(this.ParamLabel2);
            this.ControlPanel.Controls.Add(this.ParamSrc1);
            this.ControlPanel.Controls.Add(this.ParamLabel1);
            this.ControlPanel.Location = new System.Drawing.Point(-1, 554);
            this.ControlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(1082, 205);
            this.ControlPanel.TabIndex = 2;
            this.ControlPanel.Visible = false;
            // 
            // ParamValue2
            // 
            this.ParamValue2.ErrorMessage = "";
            this.ParamValue2.Location = new System.Drawing.Point(301, 31);
            this.ParamValue2.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue2.Name = "ParamValue2";
            this.ParamValue2.Placeholder = "";
            this.ParamValue2.ReadOnly = true;
            this.ParamValue2.Size = new System.Drawing.Size(218, 25);
            this.ParamValue2.TabIndex = 204;
            // 
            // ParamValue1
            // 
            this.ParamValue1.ErrorMessage = "";
            this.ParamValue1.Location = new System.Drawing.Point(301, 3);
            this.ParamValue1.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue1.Name = "ParamValue1";
            this.ParamValue1.Placeholder = "";
            this.ParamValue1.ReadOnly = true;
            this.ParamValue1.Size = new System.Drawing.Size(218, 25);
            this.ParamValue1.TabIndex = 203;
            // 
            // ControlPanelCommand
            // 
            this.ControlPanelCommand.Controls.Add(this.AddressLabel);
            this.ControlPanelCommand.Controls.Add(this.AddressTextBox);
            this.ControlPanelCommand.Controls.Add(this.CommentTextBox);
            this.ControlPanelCommand.Controls.Add(this.label1);
            this.ControlPanelCommand.Controls.Add(this.ASMTextBox);
            this.ControlPanelCommand.Controls.Add(this.label3);
            this.ControlPanelCommand.Controls.Add(this.ScriptChangeButton);
            this.ControlPanelCommand.Controls.Add(this.label4);
            this.ControlPanelCommand.Controls.Add(this.ScriptCodeName);
            this.ControlPanelCommand.Controls.Add(this.UpdateButton);
            this.ControlPanelCommand.Controls.Add(this.NewButton);
            this.ControlPanelCommand.Controls.Add(this.RemoveButton);
            this.ControlPanelCommand.Location = new System.Drawing.Point(0, 61);
            this.ControlPanelCommand.Margin = new System.Windows.Forms.Padding(4);
            this.ControlPanelCommand.Name = "ControlPanelCommand";
            this.ControlPanelCommand.Size = new System.Drawing.Size(1076, 138);
            this.ControlPanelCommand.TabIndex = 202;
            // 
            // AddressLabel
            // 
            this.AddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressLabel.Location = new System.Drawing.Point(663, 58);
            this.AddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(130, 24);
            this.AddressLabel.TabIndex = 204;
            this.AddressLabel.Text = "アドレス";
            this.AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AddressLabel.Click += new System.EventHandler(this.AddressLabel_Click);
            // 
            // AddressTextBox
            // 
            this.AddressTextBox.Location = new System.Drawing.Point(799, 57);
            this.AddressTextBox.Name = "AddressTextBox";
            this.AddressTextBox.ReadOnly = true;
            this.AddressTextBox.Size = new System.Drawing.Size(136, 25);
            this.AddressTextBox.TabIndex = 203;
            this.AddressTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AddressTextBox_MouseDoubleClick);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.ErrorMessage = "";
            this.CommentTextBox.Location = new System.Drawing.Point(144, 3);
            this.CommentTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Placeholder = "";
            this.CommentTextBox.Size = new System.Drawing.Size(792, 25);
            this.CommentTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(7, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 201;
            this.label1.Text = "コメント";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ASMTextBox
            // 
            this.ASMTextBox.ErrorMessage = "";
            this.ASMTextBox.Location = new System.Drawing.Point(144, 57);
            this.ASMTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ASMTextBox.Name = "ASMTextBox";
            this.ASMTextBox.Placeholder = "";
            this.ASMTextBox.Size = new System.Drawing.Size(514, 25);
            this.ASMTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(7, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 58;
            this.label3.Text = "イベント命令";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScriptChangeButton
            // 
            this.ScriptChangeButton.Location = new System.Drawing.Point(950, 2);
            this.ScriptChangeButton.Margin = new System.Windows.Forms.Padding(2);
            this.ScriptChangeButton.Name = "ScriptChangeButton";
            this.ScriptChangeButton.Size = new System.Drawing.Size(122, 55);
            this.ScriptChangeButton.TabIndex = 2;
            this.ScriptChangeButton.Text = "命令変更";
            this.ScriptChangeButton.UseVisualStyleBackColor = true;
            this.ScriptChangeButton.Click += new System.EventHandler(this.ScriptChangeButton_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(7, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 24);
            this.label4.TabIndex = 61;
            this.label4.Text = "バイナリコード";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScriptCodeName
            // 
            this.ScriptCodeName.ErrorMessage = "";
            this.ScriptCodeName.Location = new System.Drawing.Point(144, 30);
            this.ScriptCodeName.Margin = new System.Windows.Forms.Padding(2);
            this.ScriptCodeName.Name = "ScriptCodeName";
            this.ScriptCodeName.Placeholder = "";
            this.ScriptCodeName.ReadOnly = true;
            this.ScriptCodeName.Size = new System.Drawing.Size(791, 25);
            this.ScriptCodeName.TabIndex = 1;
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(14, 87);
            this.UpdateButton.Margin = new System.Windows.Forms.Padding(2);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(475, 50);
            this.UpdateButton.TabIndex = 4;
            this.UpdateButton.Text = "変更";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // NewButton
            // 
            this.NewButton.Location = new System.Drawing.Point(654, 88);
            this.NewButton.Margin = new System.Windows.Forms.Padding(2);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(166, 50);
            this.NewButton.TabIndex = 6;
            this.NewButton.Text = "新規挿入";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(538, 88);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(2);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(112, 50);
            this.RemoveButton.TabIndex = 5;
            this.RemoveButton.Text = "削除";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(995, 0);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(2);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(82, 32);
            this.CloseButton.TabIndex = 201;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ParamSrc2
            // 
            this.ParamSrc2.Hexadecimal = true;
            this.ParamSrc2.Location = new System.Drawing.Point(180, 32);
            this.ParamSrc2.Margin = new System.Windows.Forms.Padding(2);
            this.ParamSrc2.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ParamSrc2.Name = "ParamSrc2";
            this.ParamSrc2.Size = new System.Drawing.Size(117, 25);
            this.ParamSrc2.TabIndex = 1;
            // 
            // ParamLabel2
            // 
            this.ParamLabel2.Location = new System.Drawing.Point(-1, 34);
            this.ParamLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ParamLabel2.Name = "ParamLabel2";
            this.ParamLabel2.Size = new System.Drawing.Size(174, 18);
            this.ParamLabel2.TabIndex = 43;
            this.ParamLabel2.Text = "パラメータ2";
            this.ParamLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ParamSrc1
            // 
            this.ParamSrc1.Hexadecimal = true;
            this.ParamSrc1.Location = new System.Drawing.Point(180, 4);
            this.ParamSrc1.Margin = new System.Windows.Forms.Padding(2);
            this.ParamSrc1.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ParamSrc1.Name = "ParamSrc1";
            this.ParamSrc1.Size = new System.Drawing.Size(117, 25);
            this.ParamSrc1.TabIndex = 0;
            // 
            // ParamLabel1
            // 
            this.ParamLabel1.Location = new System.Drawing.Point(8, 7);
            this.ParamLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ParamLabel1.Name = "ParamLabel1";
            this.ParamLabel1.Size = new System.Drawing.Size(165, 18);
            this.ParamLabel1.TabIndex = 1;
            this.ParamLabel1.Text = "パラメータ1";
            this.ParamLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Script
            // 
            this.Script.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Script.FormattingEnabled = true;
            this.Script.ItemHeight = 18;
            this.Script.Location = new System.Drawing.Point(-1, -1);
            this.Script.Margin = new System.Windows.Forms.Padding(4);
            this.Script.Name = "Script";
            this.Script.Size = new System.Drawing.Size(1089, 760);
            this.Script.TabIndex = 0;
            this.Script.SelectedIndexChanged += new System.EventHandler(this.Script_SelectedIndexChanged);
            this.Script.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Script_KeyDown);
            this.Script.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Script_MouseDoubleClick);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.N_ReloadListButton);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.N_ReadCount);
            this.panel5.Controls.Add(this.AllWriteButton);
            this.panel5.Controls.Add(this.Address);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1089, 29);
            this.panel5.TabIndex = 150;
            // 
            // N_ReloadListButton
            // 
            this.N_ReloadListButton.Location = new System.Drawing.Point(490, -2);
            this.N_ReloadListButton.Name = "N_ReloadListButton";
            this.N_ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.N_ReloadListButton.TabIndex = 62;
            this.N_ReloadListButton.Text = "再取得";
            this.N_ReloadListButton.UseVisualStyleBackColor = true;
            this.N_ReloadListButton.Click += new System.EventHandler(this.N_ReloadListButton_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(259, -3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 32);
            this.label2.TabIndex = 61;
            this.label2.Text = "読込バイト数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_ReadCount
            // 
            this.N_ReadCount.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.N_ReadCount.Location = new System.Drawing.Point(395, 0);
            this.N_ReadCount.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.N_ReadCount.Name = "N_ReadCount";
            this.N_ReadCount.Size = new System.Drawing.Size(91, 25);
            this.N_ReadCount.TabIndex = 63;
            this.N_ReadCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Address_KeyDown);
            // 
            // AllWriteButton
            // 
            this.AllWriteButton.Location = new System.Drawing.Point(920, -2);
            this.AllWriteButton.Name = "AllWriteButton";
            this.AllWriteButton.Size = new System.Drawing.Size(167, 30);
            this.AllWriteButton.TabIndex = 60;
            this.AllWriteButton.Text = "書き込み";
            this.AllWriteButton.UseVisualStyleBackColor = true;
            this.AllWriteButton.Click += new System.EventHandler(this.AllWriteButton_Click);
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.Address.Location = new System.Drawing.Point(90, 4);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 55;
            this.Address.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Address_KeyDown);
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(-1, -2);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "アドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EventToFileButton
            // 
            this.EventToFileButton.Location = new System.Drawing.Point(857, 764);
            this.EventToFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.EventToFileButton.Name = "EventToFileButton";
            this.EventToFileButton.Size = new System.Drawing.Size(219, 30);
            this.EventToFileButton.TabIndex = 15;
            this.EventToFileButton.Text = "ファイルへエクスポート";
            this.EventToFileButton.UseVisualStyleBackColor = true;
            this.EventToFileButton.Click += new System.EventHandler(this.EventToFileButton_Click);
            // 
            // FileToEventButton
            // 
            this.FileToEventButton.Location = new System.Drawing.Point(634, 764);
            this.FileToEventButton.Margin = new System.Windows.Forms.Padding(2);
            this.FileToEventButton.Name = "FileToEventButton";
            this.FileToEventButton.Size = new System.Drawing.Size(219, 30);
            this.FileToEventButton.TabIndex = 16;
            this.FileToEventButton.Text = "ファイルからインポート";
            this.FileToEventButton.UseVisualStyleBackColor = true;
            this.FileToEventButton.Click += new System.EventHandler(this.FileToEventButton_Click);
            // 
            // ProcsScriptInnerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.Controls.Add(this.ListBoxPanel);
            this.Controls.Add(this.panel5);
            this.Name = "ProcsScriptInnerControl";
            this.Size = new System.Drawing.Size(1097, 833);
            this.Load += new System.EventHandler(this.ProcsScriptInnerControl_Load);
            this.ListBoxPanel.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.ControlPanelCommand.ResumeLayout(false);
            this.ControlPanelCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ListBoxPanel;
        private System.Windows.Forms.Panel ControlPanel;
        private TextBoxEx ParamValue2;
        private TextBoxEx ParamValue1;
        private System.Windows.Forms.Panel ControlPanelCommand;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.TextBox AddressTextBox;
        private TextBoxEx CommentTextBox;
        private System.Windows.Forms.Label label1;
        private TextBoxEx ASMTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ScriptChangeButton;
        private System.Windows.Forms.Label label4;
        private TextBoxEx ScriptCodeName;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.NumericUpDown ParamSrc2;
        private System.Windows.Forms.Label ParamLabel2;
        private System.Windows.Forms.NumericUpDown ParamSrc1;
        private System.Windows.Forms.Label ParamLabel1;
        private ListBoxEx Script;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button N_ReloadListButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown N_ReadCount;
        private System.Windows.Forms.Button AllWriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button EventToFileButton;
        private System.Windows.Forms.Button FileToEventButton;
    }
}
