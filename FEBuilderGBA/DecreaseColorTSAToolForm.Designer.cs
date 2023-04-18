namespace FEBuilderGBA
{
    partial class DecreaseColorTSAToolForm
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
            this.IgnoreTSA = new System.Windows.Forms.CheckBox();
            this.ConvertSizeMethod = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ConvertReserveColor = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ConvertYohaku = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.ConvertPaletteNo = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ConvertHeight = new System.Windows.Forms.NumericUpDown();
            this.AFileSelectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.AFilename = new FEBuilderGBA.TextBoxEx();
            this.ConvertWidth = new System.Windows.Forms.NumericUpDown();
            this.BFileSelectButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.BFilename = new FEBuilderGBA.TextBoxEx();
            this.Method = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.MakeButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertYohaku)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertPaletteNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.IgnoreTSA);
            this.panel1.Controls.Add(this.ConvertSizeMethod);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.ConvertReserveColor);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.ConvertYohaku);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.ConvertPaletteNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ConvertHeight);
            this.panel1.Controls.Add(this.AFileSelectButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.AFilename);
            this.panel1.Controls.Add(this.ConvertWidth);
            this.panel1.Controls.Add(this.BFileSelectButton);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.BFilename);
            this.panel1.Controls.Add(this.Method);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.MakeButton);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(960, 376);
            this.panel1.TabIndex = 0;
            // 
            // IgnoreTSA
            // 
            this.IgnoreTSA.AutoSize = true;
            this.IgnoreTSA.Location = new System.Drawing.Point(533, 242);
            this.IgnoreTSA.Name = "IgnoreTSA";
            this.IgnoreTSA.Size = new System.Drawing.Size(117, 22);
            this.IgnoreTSA.TabIndex = 112;
            this.IgnoreTSA.Text = "TSAを無視";
            this.IgnoreTSA.UseVisualStyleBackColor = true;
            this.IgnoreTSA.Visible = false;
            // 
            // ConvertSizeMethod
            // 
            this.ConvertSizeMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConvertSizeMethod.FormattingEnabled = true;
            this.ConvertSizeMethod.Items.AddRange(new object[] {
            "00=切り抜き",
            "01=拡縮"});
            this.ConvertSizeMethod.Location = new System.Drawing.Point(316, 197);
            this.ConvertSizeMethod.Name = "ConvertSizeMethod";
            this.ConvertSizeMethod.Size = new System.Drawing.Size(638, 26);
            this.ConvertSizeMethod.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(13, 194);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(296, 31);
            this.label10.TabIndex = 111;
            this.label10.Text = "サイズ補正方法";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConvertReserveColor
            // 
            this.ConvertReserveColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConvertReserveColor.FormattingEnabled = true;
            this.ConvertReserveColor.Items.AddRange(new object[] {
            "00=16色すべて利用する",
            "01=15色 0番目は背景色として予約"});
            this.ConvertReserveColor.Location = new System.Drawing.Point(316, 273);
            this.ConvertReserveColor.Name = "ConvertReserveColor";
            this.ConvertReserveColor.Size = new System.Drawing.Size(638, 26);
            this.ConvertReserveColor.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(13, 270);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(296, 31);
            this.label9.TabIndex = 109;
            this.label9.Text = "透過色";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConvertYohaku
            // 
            this.ConvertYohaku.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.ConvertYohaku.Location = new System.Drawing.Point(838, 126);
            this.ConvertYohaku.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.ConvertYohaku.Name = "ConvertYohaku";
            this.ConvertYohaku.Size = new System.Drawing.Size(120, 25);
            this.ConvertYohaku.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(533, 120);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(296, 31);
            this.label8.TabIndex = 107;
            this.label8.Text = "+余白";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConvertPaletteNo
            // 
            this.ConvertPaletteNo.Location = new System.Drawing.Point(318, 240);
            this.ConvertPaletteNo.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.ConvertPaletteNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ConvertPaletteNo.Name = "ConvertPaletteNo";
            this.ConvertPaletteNo.Size = new System.Drawing.Size(120, 25);
            this.ConvertPaletteNo.TabIndex = 9;
            this.ConvertPaletteNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ConvertPaletteNo.ValueChanged += new System.EventHandler(this.ConvertPaletteNo_ValueChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(13, 234);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 31);
            this.label2.TabIndex = 104;
            this.label2.Text = "パレット数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(13, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(296, 31);
            this.label3.TabIndex = 91;
            this.label3.Text = "元ファイル";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConvertHeight
            // 
            this.ConvertHeight.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.ConvertHeight.Location = new System.Drawing.Point(318, 162);
            this.ConvertHeight.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ConvertHeight.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.ConvertHeight.Name = "ConvertHeight";
            this.ConvertHeight.Size = new System.Drawing.Size(120, 25);
            this.ConvertHeight.TabIndex = 7;
            this.ConvertHeight.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // AFileSelectButton
            // 
            this.AFileSelectButton.Location = new System.Drawing.Point(318, 12);
            this.AFileSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.AFileSelectButton.Name = "AFileSelectButton";
            this.AFileSelectButton.Size = new System.Drawing.Size(130, 31);
            this.AFileSelectButton.TabIndex = 0;
            this.AFileSelectButton.Text = "ファイル選択";
            this.AFileSelectButton.UseVisualStyleBackColor = true;
            this.AFileSelectButton.Click += new System.EventHandler(this.AFileSelectButton_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(13, 156);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 31);
            this.label1.TabIndex = 102;
            this.label1.Text = "高さ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AFilename
            // 
            this.AFilename.ErrorMessage = "";
            this.AFilename.Location = new System.Drawing.Point(456, 15);
            this.AFilename.Margin = new System.Windows.Forms.Padding(4);
            this.AFilename.Name = "AFilename";
            this.AFilename.Placeholder = "";
            this.AFilename.Size = new System.Drawing.Size(500, 25);
            this.AFilename.TabIndex = 1;
            this.AFilename.DoubleClick += new System.EventHandler(this.AFilename_DoubleClick);
            // 
            // ConvertWidth
            // 
            this.ConvertWidth.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.ConvertWidth.Location = new System.Drawing.Point(318, 124);
            this.ConvertWidth.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ConvertWidth.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.ConvertWidth.Name = "ConvertWidth";
            this.ConvertWidth.Size = new System.Drawing.Size(120, 25);
            this.ConvertWidth.TabIndex = 5;
            this.ConvertWidth.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // BFileSelectButton
            // 
            this.BFileSelectButton.Location = new System.Drawing.Point(318, 46);
            this.BFileSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.BFileSelectButton.Name = "BFileSelectButton";
            this.BFileSelectButton.Size = new System.Drawing.Size(130, 31);
            this.BFileSelectButton.TabIndex = 2;
            this.BFileSelectButton.Text = "ファイル選択";
            this.BFileSelectButton.UseVisualStyleBackColor = true;
            this.BFileSelectButton.Click += new System.EventHandler(this.BFileSelectButton_Click);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(13, 82);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(296, 31);
            this.label6.TabIndex = 100;
            this.label6.Text = "種類";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BFilename
            // 
            this.BFilename.ErrorMessage = "";
            this.BFilename.Location = new System.Drawing.Point(456, 49);
            this.BFilename.Margin = new System.Windows.Forms.Padding(4);
            this.BFilename.Name = "BFilename";
            this.BFilename.Placeholder = "";
            this.BFilename.Size = new System.Drawing.Size(500, 25);
            this.BFilename.TabIndex = 3;
            this.BFilename.DoubleClick += new System.EventHandler(this.BFilename_DoubleClick);
            // 
            // Method
            // 
            this.Method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Method.FormattingEnabled = true;
            this.Method.Items.AddRange(new object[] {
            "00=自分で決める",
            "01=背景(BG,CG)",
            "02=戦闘背景",
            "03=ワールドマップ",
            "04=ワールドマップ(イベント用)",
            "05=TSAを利用しない256色",
            "06=ステータス画面の背景(FE8)",
            "07=一枚絵マップチップ(5パレット)",
            "08=一枚絵マップチップ(10パレット)",
            "09=TSAを利用しないBG256色(カットシーン)",
            "0A=TSAを利用しないBG224色(会話シーン用)"});
            this.Method.Location = new System.Drawing.Point(316, 87);
            this.Method.Name = "Method";
            this.Method.Size = new System.Drawing.Size(638, 26);
            this.Method.TabIndex = 4;
            this.Method.SelectedIndexChanged += new System.EventHandler(this.Method_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(13, 47);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(296, 31);
            this.label4.TabIndex = 94;
            this.label4.Text = "出力ファイル";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(13, 118);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(296, 31);
            this.label5.TabIndex = 98;
            this.label5.Text = "幅";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MakeButton
            // 
            this.MakeButton.Location = new System.Drawing.Point(316, 332);
            this.MakeButton.Name = "MakeButton";
            this.MakeButton.Size = new System.Drawing.Size(491, 32);
            this.MakeButton.TabIndex = 11;
            this.MakeButton.Text = "開始";
            this.MakeButton.UseVisualStyleBackColor = true;
            this.MakeButton.Click += new System.EventHandler(this.MakeButton_Click);
            // 
            // DecreaseColorTSAToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(984, 401);
            this.Controls.Add(this.panel1);
            this.Name = "DecreaseColorTSAToolForm";
            this.Text = "減色ツール";
            this.Load += new System.EventHandler(this.DecreaseColorTSAToolForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertYohaku)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertPaletteNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConvertWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown ConvertWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox Method;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button MakeButton;
        private System.Windows.Forms.Label label4;
        private FEBuilderGBA.TextBoxEx BFilename;
        private System.Windows.Forms.Button BFileSelectButton;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx AFilename;
        private System.Windows.Forms.Button AFileSelectButton;
        private System.Windows.Forms.NumericUpDown ConvertPaletteNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ConvertHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ConvertYohaku;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ConvertReserveColor;
        private System.Windows.Forms.ComboBox ConvertSizeMethod;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox IgnoreTSA;
    }
}