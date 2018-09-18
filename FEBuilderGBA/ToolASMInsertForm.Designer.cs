namespace FEBuilderGBA
{
    partial class ToolASMInsertForm
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
            this.FREEAREA = new System.Windows.Forms.NumericUpDown();
            this.FREEAREALabel = new System.Windows.Forms.Label();
            this.PatchMakerButton = new System.Windows.Forms.Button();
            this.WriteButton = new System.Windows.Forms.Button();
            this.HookRegisterLabel = new System.Windows.Forms.Label();
            this.HookRegister = new System.Windows.Forms.ComboBox();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Method = new System.Windows.Forms.ComboBox();
            this.SRCFilename = new FEBuilderGBA.TextBoxEx();
            this.SRCSelectButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.UndoButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FREEAREA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.UndoButton);
            this.panel1.Controls.Add(this.FREEAREA);
            this.panel1.Controls.Add(this.FREEAREALabel);
            this.panel1.Controls.Add(this.PatchMakerButton);
            this.panel1.Controls.Add(this.WriteButton);
            this.panel1.Controls.Add(this.HookRegisterLabel);
            this.panel1.Controls.Add(this.HookRegister);
            this.panel1.Controls.Add(this.Address);
            this.panel1.Controls.Add(this.AddressLabel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Method);
            this.panel1.Controls.Add(this.SRCFilename);
            this.panel1.Controls.Add(this.SRCSelectButton);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 406);
            this.panel1.TabIndex = 0;
            // 
            // FREEAREA
            // 
            this.FREEAREA.Hexadecimal = true;
            this.FREEAREA.Location = new System.Drawing.Point(305, 239);
            this.FREEAREA.Margin = new System.Windows.Forms.Padding(2);
            this.FREEAREA.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.FREEAREA.Name = "FREEAREA";
            this.FREEAREA.Size = new System.Drawing.Size(130, 25);
            this.FREEAREA.TabIndex = 101;
            // 
            // FREEAREALabel
            // 
            this.FREEAREALabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FREEAREALabel.Location = new System.Drawing.Point(6, 235);
            this.FREEAREALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FREEAREALabel.Name = "FREEAREALabel";
            this.FREEAREALabel.Size = new System.Drawing.Size(291, 31);
            this.FREEAREALabel.TabIndex = 100;
            this.FREEAREALabel.Text = "フリーエリアの定義";
            this.FREEAREALabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PatchMakerButton
            // 
            this.PatchMakerButton.Location = new System.Drawing.Point(6, 355);
            this.PatchMakerButton.Name = "PatchMakerButton";
            this.PatchMakerButton.Size = new System.Drawing.Size(153, 48);
            this.PatchMakerButton.TabIndex = 98;
            this.PatchMakerButton.Text = "Patch Maker";
            this.PatchMakerButton.UseVisualStyleBackColor = true;
            this.PatchMakerButton.Click += new System.EventHandler(this.PatchMakerButton_Click);
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(362, 355);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(4);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(291, 45);
            this.WriteButton.TabIndex = 97;
            this.WriteButton.Text = "実行";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // HookRegisterLabel
            // 
            this.HookRegisterLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HookRegisterLabel.Location = new System.Drawing.Point(6, 205);
            this.HookRegisterLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HookRegisterLabel.Name = "HookRegisterLabel";
            this.HookRegisterLabel.Size = new System.Drawing.Size(291, 31);
            this.HookRegisterLabel.TabIndex = 96;
            this.HookRegisterLabel.Text = "フックに利用するレジスタ";
            this.HookRegisterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HookRegister
            // 
            this.HookRegister.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HookRegister.FormattingEnabled = true;
            this.HookRegister.Items.AddRange(new object[] {
            "r0",
            "r1",
            "r2",
            "r3",
            "r4",
            "r5",
            "r6",
            "r7",
            "r8"});
            this.HookRegister.Location = new System.Drawing.Point(305, 207);
            this.HookRegister.Name = "HookRegister";
            this.HookRegister.Size = new System.Drawing.Size(345, 26);
            this.HookRegister.TabIndex = 95;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(305, 176);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 94;
            // 
            // AddressLabel
            // 
            this.AddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressLabel.Location = new System.Drawing.Point(6, 175);
            this.AddressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(291, 31);
            this.AddressLabel.TabIndex = 93;
            this.AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(463, 18);
            this.label1.TabIndex = 92;
            this.label1.Text = "アセンブリ(コンパイル)に成功した場合は、ROMに埋め込みますか?";
            // 
            // Method
            // 
            this.Method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Method.FormattingEnabled = true;
            this.Method.Items.AddRange(new object[] {
            "いいえ、ROMには干渉しません",
            "所定のアドレスに埋め込みます",
            "フリー領域に配置してジャンプコードを埋め込みます"});
            this.Method.Location = new System.Drawing.Point(4, 120);
            this.Method.Name = "Method";
            this.Method.Size = new System.Drawing.Size(647, 26);
            this.Method.TabIndex = 91;
            this.Method.SelectedIndexChanged += new System.EventHandler(this.Method_SelectedIndexChanged);
            // 
            // SRCFilename
            // 
            this.SRCFilename.ErrorMessage = "";
            this.SRCFilename.Location = new System.Drawing.Point(305, 16);
            this.SRCFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SRCFilename.Name = "SRCFilename";
            this.SRCFilename.Placeholder = "";
            this.SRCFilename.Size = new System.Drawing.Size(346, 25);
            this.SRCFilename.TabIndex = 90;
            this.SRCFilename.DoubleClick += new System.EventHandler(this.SRCFilename_DoubleClick);
            // 
            // SRCSelectButton
            // 
            this.SRCSelectButton.Location = new System.Drawing.Point(167, 12);
            this.SRCSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.SRCSelectButton.Name = "SRCSelectButton";
            this.SRCSelectButton.Size = new System.Drawing.Size(130, 31);
            this.SRCSelectButton.TabIndex = 89;
            this.SRCSelectButton.Text = "別ファイル選択";
            this.SRCSelectButton.UseVisualStyleBackColor = true;
            this.SRCSelectButton.Click += new System.EventHandler(this.SRCSelectButton_Click);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(2, 15);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 31);
            this.label9.TabIndex = 88;
            this.label9.Text = "SRC";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UndoButton
            // 
            this.UndoButton.Location = new System.Drawing.Point(167, 355);
            this.UndoButton.Margin = new System.Windows.Forms.Padding(4);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(153, 45);
            this.UndoButton.TabIndex = 102;
            this.UndoButton.Text = "Undo";
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Visible = false;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // ToolASMInsertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(690, 430);
            this.Controls.Add(this.panel1);
            this.Name = "ToolASMInsertForm";
            this.Text = "ASMで追加";
            this.Load += new System.EventHandler(this.ToolASMInsertForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FREEAREA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private TextBoxEx SRCFilename;
        private System.Windows.Forms.Button SRCSelectButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Method;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label HookRegisterLabel;
        private System.Windows.Forms.ComboBox HookRegister;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Button PatchMakerButton;
        private System.Windows.Forms.NumericUpDown FREEAREA;
        private System.Windows.Forms.Label FREEAREALabel;
        private System.Windows.Forms.Button UndoButton;
    }
}