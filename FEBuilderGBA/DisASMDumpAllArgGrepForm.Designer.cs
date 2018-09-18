namespace FEBuilderGBA
{
    partial class DisASMDumpAllArgGrepForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.GrepButton = new System.Windows.Forms.Button();
            this.customColorGroupBox4 = new FEBuilderGBA.CustomColorGroupBox();
            this.ResultOptionHideUnknowArgCheckBox = new System.Windows.Forms.CheckBox();
            this.ResultOptionHideFunctionCallCheckBox = new System.Windows.Forms.CheckBox();
            this.customColorGroupBox3 = new FEBuilderGBA.CustomColorGroupBox();
            this.TargtFunctionTextBox = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.customColorGroupBox2 = new FEBuilderGBA.CustomColorGroupBox();
            this.AllowRowsNumbe = new System.Windows.Forms.NumericUpDown();
            this.FREEAREALabel = new System.Windows.Forms.Label();
            this.HookRegisterLabel = new System.Windows.Forms.Label();
            this.SearhRegister = new System.Windows.Forms.ComboBox();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SRCFileSelectButton = new System.Windows.Forms.Button();
            this.SRCFileTextBox = new FEBuilderGBA.TextBoxEx();
            this.customColorGroupBox4.SuspendLayout();
            this.customColorGroupBox3.SuspendLayout();
            this.customColorGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AllowRowsNumbe)).BeginInit();
            this.customColorGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(841, 97);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "逆アセンブルしたソースコードを対象に引数の検索を行います。\r\n例えば、 m4aSongNumStart のr0引数を探せば効果音のリストが作れます。\r\nmov r" +
    "0,#0x6a\r\nbl m4aSongNumStart";
            // 
            // GrepButton
            // 
            this.GrepButton.Location = new System.Drawing.Point(637, 666);
            this.GrepButton.Name = "GrepButton";
            this.GrepButton.Size = new System.Drawing.Size(227, 41);
            this.GrepButton.TabIndex = 107;
            this.GrepButton.Text = "検索開始";
            this.GrepButton.UseVisualStyleBackColor = true;
            this.GrepButton.Click += new System.EventHandler(this.GrepButton_Click);
            // 
            // customColorGroupBox4
            // 
            this.customColorGroupBox4.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox4.Controls.Add(this.ResultOptionHideUnknowArgCheckBox);
            this.customColorGroupBox4.Controls.Add(this.ResultOptionHideFunctionCallCheckBox);
            this.customColorGroupBox4.Location = new System.Drawing.Point(13, 550);
            this.customColorGroupBox4.Name = "customColorGroupBox4";
            this.customColorGroupBox4.Size = new System.Drawing.Size(850, 110);
            this.customColorGroupBox4.TabIndex = 108;
            this.customColorGroupBox4.TabStop = false;
            this.customColorGroupBox4.Text = "結果の成型";
            // 
            // ResultOptionHideUnknowArgCheckBox
            // 
            this.ResultOptionHideUnknowArgCheckBox.AutoSize = true;
            this.ResultOptionHideUnknowArgCheckBox.Location = new System.Drawing.Point(9, 70);
            this.ResultOptionHideUnknowArgCheckBox.Name = "ResultOptionHideUnknowArgCheckBox";
            this.ResultOptionHideUnknowArgCheckBox.Size = new System.Drawing.Size(360, 22);
            this.ResultOptionHideUnknowArgCheckBox.TabIndex = 1;
            this.ResultOptionHideUnknowArgCheckBox.Text = "用途が判明していない関数呼び出しのみ表示";
            this.ResultOptionHideUnknowArgCheckBox.UseVisualStyleBackColor = true;
            // 
            // ResultOptionHideFunctionCallCheckBox
            // 
            this.ResultOptionHideFunctionCallCheckBox.AutoSize = true;
            this.ResultOptionHideFunctionCallCheckBox.Location = new System.Drawing.Point(9, 33);
            this.ResultOptionHideFunctionCallCheckBox.Name = "ResultOptionHideFunctionCallCheckBox";
            this.ResultOptionHideFunctionCallCheckBox.Size = new System.Drawing.Size(298, 22);
            this.ResultOptionHideFunctionCallCheckBox.TabIndex = 0;
            this.ResultOptionHideFunctionCallCheckBox.Text = "検索結果に関数呼び出しは含めない";
            this.ResultOptionHideFunctionCallCheckBox.UseVisualStyleBackColor = true;
            // 
            // customColorGroupBox3
            // 
            this.customColorGroupBox3.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox3.Controls.Add(this.TargtFunctionTextBox);
            this.customColorGroupBox3.Controls.Add(this.label3);
            this.customColorGroupBox3.Location = new System.Drawing.Point(12, 284);
            this.customColorGroupBox3.Name = "customColorGroupBox3";
            this.customColorGroupBox3.Size = new System.Drawing.Size(851, 106);
            this.customColorGroupBox3.TabIndex = 106;
            this.customColorGroupBox3.TabStop = false;
            this.customColorGroupBox3.Text = "調べる関数";
            // 
            // TargtFunctionTextBox
            // 
            this.TargtFunctionTextBox.ErrorMessage = "";
            this.TargtFunctionTextBox.Location = new System.Drawing.Point(363, 55);
            this.TargtFunctionTextBox.Name = "TargtFunctionTextBox";
            this.TargtFunctionTextBox.Placeholder = "m4aSongNumStart または、D01FC";
            this.TargtFunctionTextBox.Size = new System.Drawing.Size(479, 25);
            this.TargtFunctionTextBox.TabIndex = 104;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(10, 31);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(346, 72);
            this.label3.TabIndex = 103;
            this.label3.Text = "blまたはb呼び出しの関数名\r\nまたは、関数のアドレスを指定ください。";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customColorGroupBox2
            // 
            this.customColorGroupBox2.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox2.Controls.Add(this.AllowRowsNumbe);
            this.customColorGroupBox2.Controls.Add(this.FREEAREALabel);
            this.customColorGroupBox2.Controls.Add(this.HookRegisterLabel);
            this.customColorGroupBox2.Controls.Add(this.SearhRegister);
            this.customColorGroupBox2.Location = new System.Drawing.Point(13, 396);
            this.customColorGroupBox2.Name = "customColorGroupBox2";
            this.customColorGroupBox2.Size = new System.Drawing.Size(851, 139);
            this.customColorGroupBox2.TabIndex = 3;
            this.customColorGroupBox2.TabStop = false;
            this.customColorGroupBox2.Text = "探す引数";
            // 
            // AllowRowsNumbe
            // 
            this.AllowRowsNumbe.Hexadecimal = true;
            this.AllowRowsNumbe.Location = new System.Drawing.Point(361, 64);
            this.AllowRowsNumbe.Margin = new System.Windows.Forms.Padding(2);
            this.AllowRowsNumbe.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.AllowRowsNumbe.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AllowRowsNumbe.Name = "AllowRowsNumbe";
            this.AllowRowsNumbe.Size = new System.Drawing.Size(130, 25);
            this.AllowRowsNumbe.TabIndex = 105;
            this.AllowRowsNumbe.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // FREEAREALabel
            // 
            this.FREEAREALabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FREEAREALabel.Location = new System.Drawing.Point(9, 62);
            this.FREEAREALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FREEAREALabel.Name = "FREEAREALabel";
            this.FREEAREALabel.Size = new System.Drawing.Size(346, 31);
            this.FREEAREALabel.TabIndex = 104;
            this.FREEAREALabel.Text = "許容する行数";
            this.FREEAREALabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HookRegisterLabel
            // 
            this.HookRegisterLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HookRegisterLabel.Location = new System.Drawing.Point(9, 32);
            this.HookRegisterLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HookRegisterLabel.Name = "HookRegisterLabel";
            this.HookRegisterLabel.Size = new System.Drawing.Size(346, 31);
            this.HookRegisterLabel.TabIndex = 103;
            this.HookRegisterLabel.Text = "探すレジスタ";
            this.HookRegisterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SearhRegister
            // 
            this.SearhRegister.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SearhRegister.FormattingEnabled = true;
            this.SearhRegister.Items.AddRange(new object[] {
            "r0",
            "r1",
            "r2",
            "r3",
            "r4",
            "r5",
            "r6",
            "r7",
            "r8"});
            this.SearhRegister.Location = new System.Drawing.Point(361, 32);
            this.SearhRegister.Name = "SearhRegister";
            this.SearhRegister.Size = new System.Drawing.Size(345, 26);
            this.SearhRegister.TabIndex = 102;
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.label1);
            this.customColorGroupBox1.Controls.Add(this.SRCFileSelectButton);
            this.customColorGroupBox1.Controls.Add(this.SRCFileTextBox);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 128);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(851, 142);
            this.customColorGroupBox1.TabIndex = 2;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "Grepする逆アセンブラソース";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(528, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "事前に逆アセンブラによって、ソースコードをすべて逆アセンブルしてください。";
            // 
            // SRCFileSelectButton
            // 
            this.SRCFileSelectButton.Location = new System.Drawing.Point(7, 35);
            this.SRCFileSelectButton.Name = "SRCFileSelectButton";
            this.SRCFileSelectButton.Size = new System.Drawing.Size(126, 28);
            this.SRCFileSelectButton.TabIndex = 1;
            this.SRCFileSelectButton.Text = "参照";
            this.SRCFileSelectButton.UseVisualStyleBackColor = true;
            this.SRCFileSelectButton.Click += new System.EventHandler(this.SRCFileSelectButton_Click);
            // 
            // SRCFileTextBox
            // 
            this.SRCFileTextBox.ErrorMessage = "";
            this.SRCFileTextBox.Location = new System.Drawing.Point(139, 38);
            this.SRCFileTextBox.Name = "SRCFileTextBox";
            this.SRCFileTextBox.Placeholder = "";
            this.SRCFileTextBox.Size = new System.Drawing.Size(703, 25);
            this.SRCFileTextBox.TabIndex = 0;
            this.SRCFileTextBox.DoubleClick += new System.EventHandler(this.SRCFileTextBox_DoubleClick);
            // 
            // DisASMDumpAllArgGrepForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(875, 719);
            this.Controls.Add(this.customColorGroupBox4);
            this.Controls.Add(this.GrepButton);
            this.Controls.Add(this.customColorGroupBox3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.customColorGroupBox2);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "DisASMDumpAllArgGrepForm";
            this.Text = "ArgGrep";
            this.Load += new System.EventHandler(this.DisASMDumpAllArgGrepForm_Load);
            this.customColorGroupBox4.ResumeLayout(false);
            this.customColorGroupBox4.PerformLayout();
            this.customColorGroupBox3.ResumeLayout(false);
            this.customColorGroupBox3.PerformLayout();
            this.customColorGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AllowRowsNumbe)).EndInit();
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomColorGroupBox customColorGroupBox1;
        private CustomColorGroupBox customColorGroupBox2;
        private System.Windows.Forms.Button SRCFileSelectButton;
        private TextBoxEx SRCFileTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown AllowRowsNumbe;
        private System.Windows.Forms.Label FREEAREALabel;
        private System.Windows.Forms.Label HookRegisterLabel;
        private System.Windows.Forms.ComboBox SearhRegister;
        private System.Windows.Forms.TextBox textBox1;
        private CustomColorGroupBox customColorGroupBox3;
        private System.Windows.Forms.Label label3;
        private TextBoxEx TargtFunctionTextBox;
        private System.Windows.Forms.Button GrepButton;
        private CustomColorGroupBox customColorGroupBox4;
        private System.Windows.Forms.CheckBox ResultOptionHideUnknowArgCheckBox;
        private System.Windows.Forms.CheckBox ResultOptionHideFunctionCallCheckBox;
    }
}