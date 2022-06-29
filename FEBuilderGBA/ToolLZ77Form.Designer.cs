namespace FEBuilderGBA
{
    partial class ToolLZ77Form
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
            this.DeCompressDESTFilename = new FEBuilderGBA.TextBoxEx();
            this.DeCompressDESTSelectButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DeCompressFireButton = new System.Windows.Forms.Button();
            this.DeCompressAddress = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.DeCompressSRCFilename = new FEBuilderGBA.TextBoxEx();
            this.DeCompressSRCSelectButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.CompressDESTFilename = new FEBuilderGBA.TextBoxEx();
            this.CompressDESTSelectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.CompressFireButton = new System.Windows.Forms.Button();
            this.CompressSRCFilename = new FEBuilderGBA.TextBoxEx();
            this.CompressSRCSelectButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ZeroClearTo = new System.Windows.Forms.NumericUpDown();
            this.ZeroClearFrom = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.ZeroClearButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.MoveLength = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.MoveToAddress = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.MoveFromAddress = new System.Windows.Forms.NumericUpDown();
            this.MoveButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxEx1 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx2 = new FEBuilderGBA.TextBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.DeCompressAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZeroClearTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZeroClearFrom)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MoveLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveToAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveFromAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // DeCompressDESTFilename
            // 
            this.DeCompressDESTFilename.ErrorMessage = "";
            this.DeCompressDESTFilename.Location = new System.Drawing.Point(310, 77);
            this.DeCompressDESTFilename.Margin = new System.Windows.Forms.Padding(4);
            this.DeCompressDESTFilename.Name = "DeCompressDESTFilename";
            this.DeCompressDESTFilename.Placeholder = "";
            this.DeCompressDESTFilename.Size = new System.Drawing.Size(346, 25);
            this.DeCompressDESTFilename.TabIndex = 97;
            this.DeCompressDESTFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DeCompressDESTFilename_MouseDoubleClick);
            // 
            // DeCompressDESTSelectButton
            // 
            this.DeCompressDESTSelectButton.Location = new System.Drawing.Point(172, 74);
            this.DeCompressDESTSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.DeCompressDESTSelectButton.Name = "DeCompressDESTSelectButton";
            this.DeCompressDESTSelectButton.Size = new System.Drawing.Size(130, 31);
            this.DeCompressDESTSelectButton.TabIndex = 96;
            this.DeCompressDESTSelectButton.Text = "別ファイル選択";
            this.DeCompressDESTSelectButton.UseVisualStyleBackColor = true;
            this.DeCompressDESTSelectButton.Click += new System.EventHandler(this.DeCompressDESTSelectButton_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(7, 72);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 31);
            this.label4.TabIndex = 95;
            this.label4.Text = "DEST";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DeCompressFireButton
            // 
            this.DeCompressFireButton.Location = new System.Drawing.Point(172, 121);
            this.DeCompressFireButton.Margin = new System.Windows.Forms.Padding(4);
            this.DeCompressFireButton.Name = "DeCompressFireButton";
            this.DeCompressFireButton.Size = new System.Drawing.Size(462, 31);
            this.DeCompressFireButton.TabIndex = 90;
            this.DeCompressFireButton.Text = "解凍する";
            this.DeCompressFireButton.UseVisualStyleBackColor = true;
            this.DeCompressFireButton.Click += new System.EventHandler(this.DeCompressFireButton_Click);
            // 
            // DeCompressAddress
            // 
            this.DeCompressAddress.Hexadecimal = true;
            this.DeCompressAddress.Location = new System.Drawing.Point(172, 47);
            this.DeCompressAddress.Margin = new System.Windows.Forms.Padding(4);
            this.DeCompressAddress.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.DeCompressAddress.Name = "DeCompressAddress";
            this.DeCompressAddress.Size = new System.Drawing.Size(144, 25);
            this.DeCompressAddress.TabIndex = 89;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(7, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 31);
            this.label1.TabIndex = 88;
            this.label1.Text = "SRC開始アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DeCompressSRCFilename
            // 
            this.DeCompressSRCFilename.ErrorMessage = "";
            this.DeCompressSRCFilename.Location = new System.Drawing.Point(310, 13);
            this.DeCompressSRCFilename.Margin = new System.Windows.Forms.Padding(4);
            this.DeCompressSRCFilename.Name = "DeCompressSRCFilename";
            this.DeCompressSRCFilename.Placeholder = "";
            this.DeCompressSRCFilename.Size = new System.Drawing.Size(346, 25);
            this.DeCompressSRCFilename.TabIndex = 87;
            this.DeCompressSRCFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DeCompressSRCFilename_MouseDoubleClick);
            // 
            // DeCompressSRCSelectButton
            // 
            this.DeCompressSRCSelectButton.Location = new System.Drawing.Point(172, 9);
            this.DeCompressSRCSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.DeCompressSRCSelectButton.Name = "DeCompressSRCSelectButton";
            this.DeCompressSRCSelectButton.Size = new System.Drawing.Size(130, 31);
            this.DeCompressSRCSelectButton.TabIndex = 86;
            this.DeCompressSRCSelectButton.Text = "別ファイル選択";
            this.DeCompressSRCSelectButton.UseVisualStyleBackColor = true;
            this.DeCompressSRCSelectButton.Click += new System.EventHandler(this.DeCompressSRCSelectButton_Click);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(7, 12);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 31);
            this.label9.TabIndex = 85;
            this.label9.Text = "SRC";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressDESTFilename
            // 
            this.CompressDESTFilename.ErrorMessage = "";
            this.CompressDESTFilename.Location = new System.Drawing.Point(310, 45);
            this.CompressDESTFilename.Margin = new System.Windows.Forms.Padding(4);
            this.CompressDESTFilename.Name = "CompressDESTFilename";
            this.CompressDESTFilename.Placeholder = "";
            this.CompressDESTFilename.Size = new System.Drawing.Size(346, 25);
            this.CompressDESTFilename.TabIndex = 94;
            this.CompressDESTFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CompressDESTFilename_MouseDoubleClick);
            // 
            // CompressDESTSelectButton
            // 
            this.CompressDESTSelectButton.Location = new System.Drawing.Point(172, 42);
            this.CompressDESTSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.CompressDESTSelectButton.Name = "CompressDESTSelectButton";
            this.CompressDESTSelectButton.Size = new System.Drawing.Size(130, 31);
            this.CompressDESTSelectButton.TabIndex = 93;
            this.CompressDESTSelectButton.Text = "別ファイル選択";
            this.CompressDESTSelectButton.UseVisualStyleBackColor = true;
            this.CompressDESTSelectButton.Click += new System.EventHandler(this.CompressDESTSelectButton_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(7, 44);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 31);
            this.label3.TabIndex = 92;
            this.label3.Text = "DEST";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressFireButton
            // 
            this.CompressFireButton.Location = new System.Drawing.Point(172, 86);
            this.CompressFireButton.Margin = new System.Windows.Forms.Padding(4);
            this.CompressFireButton.Name = "CompressFireButton";
            this.CompressFireButton.Size = new System.Drawing.Size(462, 31);
            this.CompressFireButton.TabIndex = 91;
            this.CompressFireButton.Text = "圧縮する";
            this.CompressFireButton.UseVisualStyleBackColor = true;
            this.CompressFireButton.Click += new System.EventHandler(this.CompressFireButton_Click);
            // 
            // CompressSRCFilename
            // 
            this.CompressSRCFilename.ErrorMessage = "";
            this.CompressSRCFilename.Location = new System.Drawing.Point(310, 15);
            this.CompressSRCFilename.Margin = new System.Windows.Forms.Padding(4);
            this.CompressSRCFilename.Name = "CompressSRCFilename";
            this.CompressSRCFilename.Placeholder = "";
            this.CompressSRCFilename.Size = new System.Drawing.Size(346, 25);
            this.CompressSRCFilename.TabIndex = 90;
            this.CompressSRCFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CompressSRCFilename_MouseDoubleClick);
            // 
            // CompressSRCSelectButton
            // 
            this.CompressSRCSelectButton.Location = new System.Drawing.Point(172, 13);
            this.CompressSRCSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.CompressSRCSelectButton.Name = "CompressSRCSelectButton";
            this.CompressSRCSelectButton.Size = new System.Drawing.Size(130, 31);
            this.CompressSRCSelectButton.TabIndex = 89;
            this.CompressSRCSelectButton.Text = "別ファイル選択";
            this.CompressSRCSelectButton.UseVisualStyleBackColor = true;
            this.CompressSRCSelectButton.Click += new System.EventHandler(this.CompressSRCSelectButton_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(7, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 31);
            this.label2.TabIndex = 88;
            this.label2.Text = "SRC";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ZeroClearTo
            // 
            this.ZeroClearTo.Hexadecimal = true;
            this.ZeroClearTo.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ZeroClearTo.Location = new System.Drawing.Point(174, 47);
            this.ZeroClearTo.Margin = new System.Windows.Forms.Padding(4);
            this.ZeroClearTo.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.ZeroClearTo.Name = "ZeroClearTo";
            this.ZeroClearTo.Size = new System.Drawing.Size(144, 25);
            this.ZeroClearTo.TabIndex = 96;
            // 
            // ZeroClearFrom
            // 
            this.ZeroClearFrom.Hexadecimal = true;
            this.ZeroClearFrom.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ZeroClearFrom.Location = new System.Drawing.Point(174, 14);
            this.ZeroClearFrom.Margin = new System.Windows.Forms.Padding(4);
            this.ZeroClearFrom.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.ZeroClearFrom.Name = "ZeroClearFrom";
            this.ZeroClearFrom.Size = new System.Drawing.Size(144, 25);
            this.ZeroClearFrom.TabIndex = 95;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(7, 40);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 31);
            this.label5.TabIndex = 92;
            this.label5.Text = "TO";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ZeroClearButton
            // 
            this.ZeroClearButton.Location = new System.Drawing.Point(172, 78);
            this.ZeroClearButton.Margin = new System.Windows.Forms.Padding(4);
            this.ZeroClearButton.Name = "ZeroClearButton";
            this.ZeroClearButton.Size = new System.Drawing.Size(462, 31);
            this.ZeroClearButton.TabIndex = 91;
            this.ZeroClearButton.Text = "この領域をゼロクリア";
            this.ZeroClearButton.UseVisualStyleBackColor = true;
            this.ZeroClearButton.Click += new System.EventHandler(this.ZeroClearButton_Click);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(7, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 31);
            this.label6.TabIndex = 88;
            this.label6.Text = "FROM";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(693, 334);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.DeCompressDESTFilename);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.DeCompressDESTSelectButton);
            this.tabPage1.Controls.Add(this.DeCompressSRCSelectButton);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.DeCompressSRCFilename);
            this.tabPage1.Controls.Add(this.DeCompressFireButton);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.DeCompressAddress);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(685, 302);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "解凍する";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 208);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(649, 88);
            this.textBox1.TabIndex = 98;
            this.textBox1.Text = "ROMのLZ77データを取り出して、解凍しファイルに保存します。\r\nデバッグ用の機能です。";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Controls.Add(this.CompressDESTFilename);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.CompressDESTSelectButton);
            this.tabPage2.Controls.Add(this.CompressSRCSelectButton);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.CompressSRCFilename);
            this.tabPage2.Controls.Add(this.CompressFireButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(685, 302);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "圧縮する";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(7, 208);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(649, 88);
            this.textBox2.TabIndex = 99;
            this.textBox2.Text = "特定のデータをlz77で圧縮して、ファイルに保存します。\r\nデバッグ用の機能です。";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.textBox3);
            this.tabPage3.Controls.Add(this.ZeroClearTo);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.ZeroClearFrom);
            this.tabPage3.Controls.Add(this.ZeroClearButton);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(685, 302);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "消去";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(7, 211);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(649, 88);
            this.textBox3.TabIndex = 100;
            this.textBox3.Text = "ROMの指定範囲を0クリアします。\r\n危険なので、通常は利用しないでください。";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.MoveLength);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.MoveToAddress);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.MoveFromAddress);
            this.tabPage4.Controls.Add(this.MoveButton);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Location = new System.Drawing.Point(4, 28);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(685, 302);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "移動";
            // 
            // MoveLength
            // 
            this.MoveLength.Location = new System.Drawing.Point(174, 53);
            this.MoveLength.Margin = new System.Windows.Forms.Padding(4);
            this.MoveLength.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.MoveLength.Name = "MoveLength";
            this.MoveLength.Size = new System.Drawing.Size(144, 25);
            this.MoveLength.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(7, 49);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(157, 31);
            this.label10.TabIndex = 102;
            this.label10.Text = "LENGTH";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MoveToAddress
            // 
            this.MoveToAddress.Hexadecimal = true;
            this.MoveToAddress.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.MoveToAddress.Location = new System.Drawing.Point(174, 108);
            this.MoveToAddress.Margin = new System.Windows.Forms.Padding(4);
            this.MoveToAddress.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.MoveToAddress.Name = "MoveToAddress";
            this.MoveToAddress.Size = new System.Drawing.Size(144, 25);
            this.MoveToAddress.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(7, 19);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(157, 31);
            this.label7.TabIndex = 97;
            this.label7.Text = "FROM";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MoveFromAddress
            // 
            this.MoveFromAddress.Hexadecimal = true;
            this.MoveFromAddress.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.MoveFromAddress.Location = new System.Drawing.Point(174, 23);
            this.MoveFromAddress.Margin = new System.Windows.Forms.Padding(4);
            this.MoveFromAddress.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.MoveFromAddress.Name = "MoveFromAddress";
            this.MoveFromAddress.Size = new System.Drawing.Size(144, 25);
            this.MoveFromAddress.TabIndex = 0;
            // 
            // MoveButton
            // 
            this.MoveButton.Location = new System.Drawing.Point(172, 140);
            this.MoveButton.Margin = new System.Windows.Forms.Padding(4);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(462, 31);
            this.MoveButton.TabIndex = 4;
            this.MoveButton.Text = "この領域を移動する";
            this.MoveButton.UseVisualStyleBackColor = true;
            this.MoveButton.Click += new System.EventHandler(this.MoveButton_Click);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(7, 102);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(157, 31);
            this.label8.TabIndex = 99;
            this.label8.Text = "TO";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.ErrorMessage = "";
            this.textBoxEx1.Location = new System.Drawing.Point(310, 72);
            this.textBoxEx1.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.Placeholder = "";
            this.textBoxEx1.Size = new System.Drawing.Size(346, 25);
            this.textBoxEx1.TabIndex = 103;
            // 
            // textBoxEx2
            // 
            this.textBoxEx2.ErrorMessage = "";
            this.textBoxEx2.Location = new System.Drawing.Point(310, 74);
            this.textBoxEx2.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEx2.Name = "textBoxEx2";
            this.textBoxEx2.Placeholder = "";
            this.textBoxEx2.Size = new System.Drawing.Size(346, 25);
            this.textBoxEx2.TabIndex = 105;
            // 
            // ToolLZ77Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(722, 359);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ToolLZ77Form";
            this.Text = "LZ77ツール";
            this.Load += new System.EventHandler(this.LZ77ToolForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DeCompressAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZeroClearTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZeroClearFrom)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MoveLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveToAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveFromAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button DeCompressSRCSelectButton;
        private FEBuilderGBA.TextBoxEx DeCompressSRCFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown DeCompressAddress;
        private System.Windows.Forms.Button DeCompressFireButton;
        private FEBuilderGBA.TextBoxEx CompressSRCFilename;
        private System.Windows.Forms.Button CompressSRCSelectButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CompressFireButton;
        private FEBuilderGBA.TextBoxEx CompressDESTFilename;
        private System.Windows.Forms.Button CompressDESTSelectButton;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx DeCompressDESTFilename;
        private System.Windows.Forms.Button DeCompressDESTSelectButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ZeroClearButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown ZeroClearTo;
        private System.Windows.Forms.NumericUpDown ZeroClearFrom;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox3;
        private TextBoxEx textBoxEx1;
        private TextBoxEx textBoxEx2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.NumericUpDown MoveToAddress;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown MoveFromAddress;
        private System.Windows.Forms.Button MoveButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown MoveLength;
        private System.Windows.Forms.Label label10;

    }
}