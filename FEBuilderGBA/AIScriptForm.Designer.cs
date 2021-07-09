namespace FEBuilderGBA
{
    partial class AIScriptForm
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
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.ListBoxPanel = new System.Windows.Forms.Panel();
            this.EventToFileButton = new System.Windows.Forms.Button();
            this.FileToEventButton = new System.Windows.Forms.Button();
            this.ControlPanel = new System.Windows.Forms.Panel();
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
            this.ParamValue4 = new FEBuilderGBA.TextBoxEx();
            this.ParamValue2 = new FEBuilderGBA.TextBoxEx();
            this.ParamSrc4 = new System.Windows.Forms.NumericUpDown();
            this.ParamLabel4 = new System.Windows.Forms.Label();
            this.ParamSrc2 = new System.Windows.Forms.NumericUpDown();
            this.ParamLabel2 = new System.Windows.Forms.Label();
            this.ParamValue5 = new FEBuilderGBA.TextBoxEx();
            this.ParamValue3 = new FEBuilderGBA.TextBoxEx();
            this.ParamValue1 = new FEBuilderGBA.TextBoxEx();
            this.ParamSrc5 = new System.Windows.Forms.NumericUpDown();
            this.ParamLabel5 = new System.Windows.Forms.Label();
            this.ParamSrc3 = new System.Windows.Forms.NumericUpDown();
            this.ParamLabel3 = new System.Windows.Forms.Label();
            this.ParamSrc1 = new System.Windows.Forms.NumericUpDown();
            this.ParamLabel1 = new System.Windows.Forms.Label();
            this.Script = new FEBuilderGBA.ListBoxEx();
            this.label8 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.N_ReloadListButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.N_ReadCount = new System.Windows.Forms.NumericUpDown();
            this.AllWriteButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton_255 = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.ListBoxPanel.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.ControlPanelCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(1053, -2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 33);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // ListBoxPanel
            // 
            this.ListBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ListBoxPanel.Controls.Add(this.EventToFileButton);
            this.ListBoxPanel.Controls.Add(this.FileToEventButton);
            this.ListBoxPanel.Controls.Add(this.ControlPanel);
            this.ListBoxPanel.Controls.Add(this.Script);
            this.ListBoxPanel.Location = new System.Drawing.Point(610, 73);
            this.ListBoxPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ListBoxPanel.Name = "ListBoxPanel";
            this.ListBoxPanel.Size = new System.Drawing.Size(956, 717);
            this.ListBoxPanel.TabIndex = 149;
            // 
            // EventToFileButton
            // 
            this.EventToFileButton.Location = new System.Drawing.Point(733, 683);
            this.EventToFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.EventToFileButton.Name = "EventToFileButton";
            this.EventToFileButton.Size = new System.Drawing.Size(219, 30);
            this.EventToFileButton.TabIndex = 17;
            this.EventToFileButton.Text = "ファイルへエクスポート";
            this.EventToFileButton.UseVisualStyleBackColor = true;
            this.EventToFileButton.Click += new System.EventHandler(this.EventToFileButton_Click);
            // 
            // FileToEventButton
            // 
            this.FileToEventButton.Location = new System.Drawing.Point(510, 683);
            this.FileToEventButton.Margin = new System.Windows.Forms.Padding(2);
            this.FileToEventButton.Name = "FileToEventButton";
            this.FileToEventButton.Size = new System.Drawing.Size(219, 30);
            this.FileToEventButton.TabIndex = 18;
            this.FileToEventButton.Text = "ファイルからインポート";
            this.FileToEventButton.UseVisualStyleBackColor = true;
            this.FileToEventButton.Click += new System.EventHandler(this.FileToEventButton_Click);
            // 
            // ControlPanel
            // 
            this.ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ControlPanel.Controls.Add(this.ControlPanelCommand);
            this.ControlPanel.Controls.Add(this.CloseButton);
            this.ControlPanel.Controls.Add(this.ParamValue4);
            this.ControlPanel.Controls.Add(this.ParamValue2);
            this.ControlPanel.Controls.Add(this.ParamSrc4);
            this.ControlPanel.Controls.Add(this.ParamLabel4);
            this.ControlPanel.Controls.Add(this.ParamSrc2);
            this.ControlPanel.Controls.Add(this.ParamLabel2);
            this.ControlPanel.Controls.Add(this.ParamValue5);
            this.ControlPanel.Controls.Add(this.ParamValue3);
            this.ControlPanel.Controls.Add(this.ParamValue1);
            this.ControlPanel.Controls.Add(this.ParamSrc5);
            this.ControlPanel.Controls.Add(this.ParamLabel5);
            this.ControlPanel.Controls.Add(this.ParamSrc3);
            this.ControlPanel.Controls.Add(this.ParamLabel3);
            this.ControlPanel.Controls.Add(this.ParamSrc1);
            this.ControlPanel.Controls.Add(this.ParamLabel1);
            this.ControlPanel.Location = new System.Drawing.Point(-1, 321);
            this.ControlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(953, 249);
            this.ControlPanel.TabIndex = 2;
            this.ControlPanel.Visible = false;
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
            this.ControlPanelCommand.Location = new System.Drawing.Point(0, 100);
            this.ControlPanelCommand.Margin = new System.Windows.Forms.Padding(4);
            this.ControlPanelCommand.Name = "ControlPanelCommand";
            this.ControlPanelCommand.Size = new System.Drawing.Size(952, 143);
            this.ControlPanelCommand.TabIndex = 202;
            // 
            // AddressLabel
            // 
            this.AddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressLabel.Location = new System.Drawing.Point(549, 64);
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
            this.AddressTextBox.Location = new System.Drawing.Point(685, 63);
            this.AddressTextBox.Name = "AddressTextBox";
            this.AddressTextBox.ReadOnly = true;
            this.AddressTextBox.Size = new System.Drawing.Size(135, 25);
            this.AddressTextBox.TabIndex = 203;
            this.AddressTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AddressTextBox_MouseDoubleClick);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.ErrorMessage = "";
            this.CommentTextBox.Location = new System.Drawing.Point(143, 8);
            this.CommentTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Placeholder = "";
            this.CommentTextBox.Size = new System.Drawing.Size(679, 25);
            this.CommentTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(9, 8);
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
            this.ASMTextBox.Location = new System.Drawing.Point(143, 63);
            this.ASMTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ASMTextBox.Name = "ASMTextBox";
            this.ASMTextBox.Placeholder = "";
            this.ASMTextBox.Size = new System.Drawing.Size(400, 25);
            this.ASMTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(9, 36);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 58;
            this.label3.Text = "説明";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScriptChangeButton
            // 
            this.ScriptChangeButton.Location = new System.Drawing.Point(826, 8);
            this.ScriptChangeButton.Margin = new System.Windows.Forms.Padding(2);
            this.ScriptChangeButton.Name = "ScriptChangeButton";
            this.ScriptChangeButton.Size = new System.Drawing.Size(122, 56);
            this.ScriptChangeButton.TabIndex = 2;
            this.ScriptChangeButton.Text = "命令変更";
            this.ScriptChangeButton.UseVisualStyleBackColor = true;
            this.ScriptChangeButton.Click += new System.EventHandler(this.ScriptChangeButton_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(9, 63);
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
            this.ScriptCodeName.Location = new System.Drawing.Point(143, 36);
            this.ScriptCodeName.Margin = new System.Windows.Forms.Padding(2);
            this.ScriptCodeName.Name = "ScriptCodeName";
            this.ScriptCodeName.Placeholder = "";
            this.ScriptCodeName.ReadOnly = true;
            this.ScriptCodeName.Size = new System.Drawing.Size(677, 25);
            this.ScriptCodeName.TabIndex = 1;
            this.ScriptCodeName.MouseEnter += new System.EventHandler(this.ScriptCodeName_MouseEnter);
            this.ScriptCodeName.MouseLeave += new System.EventHandler(this.ScriptCodeName_MouseLeave);
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(14, 91);
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
            this.NewButton.Location = new System.Drawing.Point(654, 92);
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
            this.RemoveButton.Location = new System.Drawing.Point(538, 92);
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
            this.CloseButton.Location = new System.Drawing.Point(867, 0);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(2);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(82, 32);
            this.CloseButton.TabIndex = 201;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ParamValue4
            // 
            this.ParamValue4.ErrorMessage = "";
            this.ParamValue4.Location = new System.Drawing.Point(257, 67);
            this.ParamValue4.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue4.Name = "ParamValue4";
            this.ParamValue4.Placeholder = "";
            this.ParamValue4.ReadOnly = true;
            this.ParamValue4.Size = new System.Drawing.Size(216, 25);
            this.ParamValue4.TabIndex = 54;
            // 
            // ParamValue2
            // 
            this.ParamValue2.ErrorMessage = "";
            this.ParamValue2.Location = new System.Drawing.Point(257, 33);
            this.ParamValue2.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue2.Name = "ParamValue2";
            this.ParamValue2.Placeholder = "";
            this.ParamValue2.ReadOnly = true;
            this.ParamValue2.Size = new System.Drawing.Size(218, 25);
            this.ParamValue2.TabIndex = 53;
            // 
            // ParamSrc4
            // 
            this.ParamSrc4.Hexadecimal = true;
            this.ParamSrc4.Location = new System.Drawing.Point(139, 67);
            this.ParamSrc4.Margin = new System.Windows.Forms.Padding(2);
            this.ParamSrc4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ParamSrc4.Name = "ParamSrc4";
            this.ParamSrc4.Size = new System.Drawing.Size(117, 25);
            this.ParamSrc4.TabIndex = 3;
            // 
            // ParamLabel4
            // 
            this.ParamLabel4.Location = new System.Drawing.Point(5, 70);
            this.ParamLabel4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ParamLabel4.Name = "ParamLabel4";
            this.ParamLabel4.Size = new System.Drawing.Size(127, 18);
            this.ParamLabel4.TabIndex = 45;
            this.ParamLabel4.Text = "パラメータ4";
            this.ParamLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ParamSrc2
            // 
            this.ParamSrc2.Hexadecimal = true;
            this.ParamSrc2.Location = new System.Drawing.Point(139, 34);
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
            this.ParamLabel2.Location = new System.Drawing.Point(5, 38);
            this.ParamLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ParamLabel2.Name = "ParamLabel2";
            this.ParamLabel2.Size = new System.Drawing.Size(127, 18);
            this.ParamLabel2.TabIndex = 43;
            this.ParamLabel2.Text = "パラメータ2";
            this.ParamLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ParamValue5
            // 
            this.ParamValue5.ErrorMessage = "";
            this.ParamValue5.Location = new System.Drawing.Point(731, 67);
            this.ParamValue5.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue5.Name = "ParamValue5";
            this.ParamValue5.Placeholder = "";
            this.ParamValue5.ReadOnly = true;
            this.ParamValue5.Size = new System.Drawing.Size(218, 25);
            this.ParamValue5.TabIndex = 40;
            // 
            // ParamValue3
            // 
            this.ParamValue3.ErrorMessage = "";
            this.ParamValue3.Location = new System.Drawing.Point(731, 33);
            this.ParamValue3.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue3.Name = "ParamValue3";
            this.ParamValue3.Placeholder = "";
            this.ParamValue3.ReadOnly = true;
            this.ParamValue3.Size = new System.Drawing.Size(218, 25);
            this.ParamValue3.TabIndex = 39;
            // 
            // ParamValue1
            // 
            this.ParamValue1.ErrorMessage = "";
            this.ParamValue1.Location = new System.Drawing.Point(257, 4);
            this.ParamValue1.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue1.Name = "ParamValue1";
            this.ParamValue1.Placeholder = "";
            this.ParamValue1.ReadOnly = true;
            this.ParamValue1.Size = new System.Drawing.Size(218, 25);
            this.ParamValue1.TabIndex = 38;
            // 
            // ParamSrc5
            // 
            this.ParamSrc5.Hexadecimal = true;
            this.ParamSrc5.Location = new System.Drawing.Point(612, 65);
            this.ParamSrc5.Margin = new System.Windows.Forms.Padding(2);
            this.ParamSrc5.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ParamSrc5.Name = "ParamSrc5";
            this.ParamSrc5.Size = new System.Drawing.Size(117, 25);
            this.ParamSrc5.TabIndex = 4;
            // 
            // ParamLabel5
            // 
            this.ParamLabel5.Location = new System.Drawing.Point(479, 67);
            this.ParamLabel5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ParamLabel5.Name = "ParamLabel5";
            this.ParamLabel5.Size = new System.Drawing.Size(127, 18);
            this.ParamLabel5.TabIndex = 31;
            this.ParamLabel5.Text = "パラメータ5";
            this.ParamLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ParamSrc3
            // 
            this.ParamSrc3.Hexadecimal = true;
            this.ParamSrc3.Location = new System.Drawing.Point(612, 34);
            this.ParamSrc3.Margin = new System.Windows.Forms.Padding(2);
            this.ParamSrc3.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ParamSrc3.Name = "ParamSrc3";
            this.ParamSrc3.Size = new System.Drawing.Size(117, 25);
            this.ParamSrc3.TabIndex = 2;
            // 
            // ParamLabel3
            // 
            this.ParamLabel3.Location = new System.Drawing.Point(479, 40);
            this.ParamLabel3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ParamLabel3.Name = "ParamLabel3";
            this.ParamLabel3.Size = new System.Drawing.Size(127, 18);
            this.ParamLabel3.TabIndex = 29;
            this.ParamLabel3.Text = "パラメータ3";
            this.ParamLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ParamSrc1
            // 
            this.ParamSrc1.Hexadecimal = true;
            this.ParamSrc1.Location = new System.Drawing.Point(139, 4);
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
            this.ParamLabel1.Location = new System.Drawing.Point(5, 7);
            this.ParamLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ParamLabel1.Name = "ParamLabel1";
            this.ParamLabel1.Size = new System.Drawing.Size(127, 18);
            this.ParamLabel1.TabIndex = 1;
            this.ParamLabel1.Text = "パラメータ1";
            this.ParamLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Script
            // 
            this.Script.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Script.FormattingEnabled = true;
            this.Script.IntegralHeight = false;
            this.Script.ItemHeight = 18;
            this.Script.Location = new System.Drawing.Point(2, -1);
            this.Script.Margin = new System.Windows.Forms.Padding(4);
            this.Script.Name = "Script";
            this.Script.Size = new System.Drawing.Size(953, 678);
            this.Script.TabIndex = 0;
            this.Script.SelectedIndexChanged += new System.EventHandler(this.Script_SelectedIndexChanged);
            this.Script.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Script_KeyDown);
            this.Script.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Script_MouseDoubleClick);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(598, -1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 33);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panel5.Location = new System.Drawing.Point(610, 45);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(956, 29);
            this.panel5.TabIndex = 147;
            // 
            // N_ReloadListButton
            // 
            this.N_ReloadListButton.Location = new System.Drawing.Point(551, -2);
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
            this.label2.Location = new System.Drawing.Point(320, -3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 32);
            this.label2.TabIndex = 61;
            this.label2.Text = "読込バイト数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_ReadCount
            // 
            this.N_ReadCount.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.N_ReadCount.Location = new System.Drawing.Point(456, 0);
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
            this.AllWriteButton.Location = new System.Drawing.Point(787, -2);
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
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressListExpandsButton_255);
            this.panel6.Controls.Add(this.label30);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(12, 45);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(599, 745);
            this.panel6.TabIndex = 150;
            // 
            // AddressListExpandsButton_255
            // 
            this.AddressListExpandsButton_255.Location = new System.Drawing.Point(-2, 714);
            this.AddressListExpandsButton_255.Name = "AddressListExpandsButton_255";
            this.AddressListExpandsButton_255.Size = new System.Drawing.Size(597, 30);
            this.AddressListExpandsButton_255.TabIndex = 115;
            this.AddressListExpandsButton_255.Text = "リストの拡張";
            this.AddressListExpandsButton_255.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Location = new System.Drawing.Point(-1, -1);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(599, 27);
            this.label30.TabIndex = 106;
            this.label30.Text = "名前";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 25);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(599, 679);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(969, 4);
            this.ReadCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.FilterComboBox);
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1554, 30);
            this.panel3.TabIndex = 148;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(-1, -2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 33);
            this.label5.TabIndex = 31;
            this.label5.Text = "切替";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Location = new System.Drawing.Point(148, 2);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(447, 26);
            this.FilterComboBox.TabIndex = 30;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(879, -1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 32);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(743, 4);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // AIScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1567, 793);
            this.Controls.Add(this.ListBoxPanel);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Name = "AIScriptForm";
            this.Text = "AIScript";
            this.Load += new System.EventHandler(this.AIScriptForm_Load);
            this.ListBoxPanel.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.ControlPanelCommand.ResumeLayout(false);
            this.ControlPanelCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamSrc1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.N_ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Panel ListBoxPanel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label30;
        private ListBoxEx AddressList;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Button AllWriteButton;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.Button AddressListExpandsButton_255;
        private ListBoxEx Script;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.Panel ControlPanelCommand;
        private FEBuilderGBA.TextBoxEx ASMTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ScriptChangeButton;
        private System.Windows.Forms.Label label4;
        private FEBuilderGBA.TextBoxEx ScriptCodeName;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button CloseButton;
        private TextBoxEx ParamValue4;
        private TextBoxEx ParamValue2;
        private System.Windows.Forms.NumericUpDown ParamSrc4;
        private System.Windows.Forms.Label ParamLabel4;
        private System.Windows.Forms.NumericUpDown ParamSrc2;
        private System.Windows.Forms.Label ParamLabel2;
        private TextBoxEx ParamValue5;
        private TextBoxEx ParamValue3;
        private TextBoxEx ParamValue1;
        private System.Windows.Forms.NumericUpDown ParamSrc5;
        private System.Windows.Forms.Label ParamLabel5;
        private System.Windows.Forms.NumericUpDown ParamSrc3;
        private System.Windows.Forms.Label ParamLabel3;
        private System.Windows.Forms.NumericUpDown ParamSrc1;
        private System.Windows.Forms.Label ParamLabel1;
        private System.Windows.Forms.Button N_ReloadListButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown N_ReadCount;
        private TextBoxEx CommentTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button EventToFileButton;
        private System.Windows.Forms.Button FileToEventButton;
    }
}