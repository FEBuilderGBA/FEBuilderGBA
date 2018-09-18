namespace FEBuilderGBA
{
    partial class ToolPortraitMakerForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.BustshotFilename = new FEBuilderGBA.TextBoxEx();
            this.BustshotSelectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FaceFilename = new FEBuilderGBA.TextBoxEx();
            this.FaceSelectButton = new System.Windows.Forms.Button();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.ClipX = new System.Windows.Forms.NumericUpDown();
            this.ClipY = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MakeButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.FaceY = new System.Windows.Forms.NumericUpDown();
            this.FaceX = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.EyeY = new System.Windows.Forms.NumericUpDown();
            this.EyeX = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.MouthY = new System.Windows.Forms.NumericUpDown();
            this.MouthX = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.FaceW = new System.Windows.Forms.NumericUpDown();
            this.RevLeftRightCheckBox = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ClipScale = new System.Windows.Forms.NumericUpDown();
            this.label123 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.EyeH = new System.Windows.Forms.NumericUpDown();
            this.EyeW = new System.Windows.Forms.NumericUpDown();
            this.label124 = new System.Windows.Forms.Label();
            this.label122 = new System.Windows.Forms.Label();
            this.MouthH = new System.Windows.Forms.NumericUpDown();
            this.MouthW = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.FaceTestType = new System.Windows.Forms.ComboBox();
            this.DecreaseColorCheckBox = new System.Windows.Forms.CheckBox();
            this.RotateYComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClipX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClipY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaceY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaceX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EyeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EyeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaceW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClipScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EyeH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EyeW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthW)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(16, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 31);
            this.label2.TabIndex = 91;
            this.label2.Text = "バストショット画像";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BustshotFilename
            // 
            this.BustshotFilename.ErrorMessage = "";
            this.BustshotFilename.Location = new System.Drawing.Point(369, 49);
            this.BustshotFilename.Margin = new System.Windows.Forms.Padding(4);
            this.BustshotFilename.Name = "BustshotFilename";
            this.BustshotFilename.Placeholder = "";
            this.BustshotFilename.Size = new System.Drawing.Size(813, 25);
            this.BustshotFilename.TabIndex = 1;
            this.BustshotFilename.DoubleClick += new System.EventHandler(this.BustupFilename_DoubleClick);
            // 
            // BustshotSelectButton
            // 
            this.BustshotSelectButton.Location = new System.Drawing.Point(231, 46);
            this.BustshotSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.BustshotSelectButton.Name = "BustshotSelectButton";
            this.BustshotSelectButton.Size = new System.Drawing.Size(130, 31);
            this.BustshotSelectButton.TabIndex = 0;
            this.BustshotSelectButton.Text = "別ファイル選択";
            this.BustshotSelectButton.UseVisualStyleBackColor = true;
            this.BustshotSelectButton.Click += new System.EventHandler(this.BustupSelectButton_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(16, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 31);
            this.label1.TabIndex = 94;
            this.label1.Text = "表情画像";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FaceFilename
            // 
            this.FaceFilename.ErrorMessage = "";
            this.FaceFilename.Location = new System.Drawing.Point(369, 87);
            this.FaceFilename.Margin = new System.Windows.Forms.Padding(4);
            this.FaceFilename.Name = "FaceFilename";
            this.FaceFilename.Placeholder = "";
            this.FaceFilename.Size = new System.Drawing.Size(813, 25);
            this.FaceFilename.TabIndex = 3;
            this.FaceFilename.DoubleClick += new System.EventHandler(this.FaceFilename_DoubleClick);
            // 
            // FaceSelectButton
            // 
            this.FaceSelectButton.Location = new System.Drawing.Point(231, 84);
            this.FaceSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.FaceSelectButton.Name = "FaceSelectButton";
            this.FaceSelectButton.Size = new System.Drawing.Size(130, 31);
            this.FaceSelectButton.TabIndex = 2;
            this.FaceSelectButton.Text = "別ファイル選択";
            this.FaceSelectButton.UseVisualStyleBackColor = true;
            this.FaceSelectButton.Click += new System.EventHandler(this.FaceSelectButton_Click);
            // 
            // Canvas
            // 
            this.Canvas.Location = new System.Drawing.Point(303, 124);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(880, 626);
            this.Canvas.TabIndex = 97;
            this.Canvas.TabStop = false;
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            // 
            // ClipX
            // 
            this.ClipX.Location = new System.Drawing.Point(231, 312);
            this.ClipX.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.ClipX.Name = "ClipX";
            this.ClipX.Size = new System.Drawing.Size(66, 25);
            this.ClipX.TabIndex = 10;
            this.ClipX.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // ClipY
            // 
            this.ClipY.Location = new System.Drawing.Point(231, 345);
            this.ClipY.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.ClipY.Name = "ClipY";
            this.ClipY.Size = new System.Drawing.Size(66, 25);
            this.ClipY.TabIndex = 11;
            this.ClipY.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(16, 309);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 31);
            this.label3.TabIndex = 100;
            this.label3.Text = "切り抜きX";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(16, 344);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(209, 31);
            this.label4.TabIndex = 101;
            this.label4.Text = "切り抜きY";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MakeButton
            // 
            this.MakeButton.Location = new System.Drawing.Point(16, 770);
            this.MakeButton.Margin = new System.Windows.Forms.Padding(4);
            this.MakeButton.Name = "MakeButton";
            this.MakeButton.Size = new System.Drawing.Size(257, 31);
            this.MakeButton.TabIndex = 23;
            this.MakeButton.Text = "作成";
            this.MakeButton.UseVisualStyleBackColor = true;
            this.MakeButton.Click += new System.EventHandler(this.MakeButton_Click);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(16, 159);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 31);
            this.label5.TabIndex = 106;
            this.label5.Text = "表情Y";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(16, 124);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(209, 31);
            this.label6.TabIndex = 105;
            this.label6.Text = "表情X";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FaceY
            // 
            this.FaceY.Location = new System.Drawing.Point(231, 160);
            this.FaceY.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.FaceY.Name = "FaceY";
            this.FaceY.Size = new System.Drawing.Size(66, 25);
            this.FaceY.TabIndex = 6;
            this.FaceY.ValueChanged += new System.EventHandler(this.ReDrawNoCache);
            // 
            // FaceX
            // 
            this.FaceX.Location = new System.Drawing.Point(231, 127);
            this.FaceX.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.FaceX.Name = "FaceX";
            this.FaceX.Size = new System.Drawing.Size(66, 25);
            this.FaceX.TabIndex = 5;
            this.FaceX.ValueChanged += new System.EventHandler(this.ReDrawNoCache);
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(16, 471);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(209, 31);
            this.label7.TabIndex = 110;
            this.label7.Text = "眼の位置Y";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(16, 436);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(209, 31);
            this.label8.TabIndex = 109;
            this.label8.Text = "眼の位置X";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EyeY
            // 
            this.EyeY.Location = new System.Drawing.Point(231, 472);
            this.EyeY.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.EyeY.Name = "EyeY";
            this.EyeY.Size = new System.Drawing.Size(66, 25);
            this.EyeY.TabIndex = 14;
            this.EyeY.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // EyeX
            // 
            this.EyeX.Location = new System.Drawing.Point(231, 439);
            this.EyeX.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.EyeX.Name = "EyeX";
            this.EyeX.Size = new System.Drawing.Size(66, 25);
            this.EyeX.TabIndex = 13;
            this.EyeX.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(16, 630);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(209, 31);
            this.label9.TabIndex = 114;
            this.label9.Text = "口の位置Y";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(16, 595);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(209, 31);
            this.label10.TabIndex = 113;
            this.label10.Text = "口の位置X";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MouthY
            // 
            this.MouthY.Location = new System.Drawing.Point(231, 631);
            this.MouthY.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.MouthY.Name = "MouthY";
            this.MouthY.Size = new System.Drawing.Size(66, 25);
            this.MouthY.TabIndex = 18;
            this.MouthY.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // MouthX
            // 
            this.MouthX.Location = new System.Drawing.Point(231, 598);
            this.MouthX.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.MouthX.Name = "MouthX";
            this.MouthX.Size = new System.Drawing.Size(66, 25);
            this.MouthX.TabIndex = 17;
            this.MouthX.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Location = new System.Drawing.Point(16, 195);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(209, 31);
            this.label12.TabIndex = 117;
            this.label12.Text = "表情の幅高さ";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FaceW
            // 
            this.FaceW.Location = new System.Drawing.Point(231, 199);
            this.FaceW.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.FaceW.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.FaceW.Name = "FaceW";
            this.FaceW.Size = new System.Drawing.Size(66, 25);
            this.FaceW.TabIndex = 7;
            this.FaceW.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.FaceW.ValueChanged += new System.EventHandler(this.ReDrawNoCache);
            // 
            // RevLeftRightCheckBox
            // 
            this.RevLeftRightCheckBox.AutoSize = true;
            this.RevLeftRightCheckBox.Location = new System.Drawing.Point(16, 237);
            this.RevLeftRightCheckBox.Name = "RevLeftRightCheckBox";
            this.RevLeftRightCheckBox.Size = new System.Drawing.Size(135, 22);
            this.RevLeftRightCheckBox.TabIndex = 9;
            this.RevLeftRightCheckBox.Text = "左右反転する";
            this.RevLeftRightCheckBox.UseVisualStyleBackColor = true;
            this.RevLeftRightCheckBox.CheckedChanged += new System.EventHandler(this.ReDrawNoCache);
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(16, 379);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(209, 31);
            this.label11.TabIndex = 120;
            this.label11.Text = "切り抜き倍率%";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ClipScale
            // 
            this.ClipScale.Location = new System.Drawing.Point(231, 380);
            this.ClipScale.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.ClipScale.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ClipScale.Name = "ClipScale";
            this.ClipScale.Size = new System.Drawing.Size(66, 25);
            this.ClipScale.TabIndex = 12;
            this.ClipScale.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ClipScale.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // label123
            // 
            this.label123.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label123.Location = new System.Drawing.Point(16, 540);
            this.label123.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label123.Name = "label123";
            this.label123.Size = new System.Drawing.Size(209, 31);
            this.label123.TabIndex = 124;
            this.label123.Text = "眼の位置H";
            this.label123.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(16, 505);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(209, 31);
            this.label14.TabIndex = 123;
            this.label14.Text = "眼の位置W";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EyeH
            // 
            this.EyeH.Location = new System.Drawing.Point(231, 541);
            this.EyeH.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.EyeH.Name = "EyeH";
            this.EyeH.Size = new System.Drawing.Size(66, 25);
            this.EyeH.TabIndex = 16;
            this.EyeH.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // EyeW
            // 
            this.EyeW.Location = new System.Drawing.Point(231, 508);
            this.EyeW.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.EyeW.Name = "EyeW";
            this.EyeW.Size = new System.Drawing.Size(66, 25);
            this.EyeW.TabIndex = 15;
            this.EyeW.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // label124
            // 
            this.label124.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label124.Location = new System.Drawing.Point(16, 699);
            this.label124.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label124.Name = "label124";
            this.label124.Size = new System.Drawing.Size(209, 31);
            this.label124.TabIndex = 128;
            this.label124.Text = "口の位置H";
            this.label124.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label122
            // 
            this.label122.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label122.Location = new System.Drawing.Point(16, 664);
            this.label122.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label122.Name = "label122";
            this.label122.Size = new System.Drawing.Size(209, 31);
            this.label122.TabIndex = 127;
            this.label122.Text = "口の位置W";
            this.label122.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MouthH
            // 
            this.MouthH.Location = new System.Drawing.Point(231, 700);
            this.MouthH.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.MouthH.Name = "MouthH";
            this.MouthH.Size = new System.Drawing.Size(66, 25);
            this.MouthH.TabIndex = 22;
            this.MouthH.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // MouthW
            // 
            this.MouthW.Location = new System.Drawing.Point(231, 667);
            this.MouthW.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.MouthW.Name = "MouthW";
            this.MouthW.Size = new System.Drawing.Size(66, 25);
            this.MouthW.TabIndex = 19;
            this.MouthW.ValueChanged += new System.EventHandler(this.ReDraw);
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(16, 9);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(1166, 31);
            this.label13.TabIndex = 129;
            this.label13.Text = "RPGツクールの顔画像を変換します(テスト)";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Location = new System.Drawing.Point(303, 770);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(209, 31);
            this.label15.TabIndex = 130;
            this.label15.Text = "表情テスト";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FaceTestType
            // 
            this.FaceTestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FaceTestType.FormattingEnabled = true;
            this.FaceTestType.Items.AddRange(new object[] {
            "0=普通",
            "1=怒り",
            "2=苦笑い",
            "3=半分目を閉じる",
            "4=会釈",
            "5=不安",
            "6=驚き",
            "7=目を閉じる"});
            this.FaceTestType.Location = new System.Drawing.Point(520, 772);
            this.FaceTestType.Name = "FaceTestType";
            this.FaceTestType.Size = new System.Drawing.Size(243, 26);
            this.FaceTestType.TabIndex = 131;
            this.FaceTestType.SelectedIndexChanged += new System.EventHandler(this.ReDrawNoCache);
            // 
            // DecreaseColorCheckBox
            // 
            this.DecreaseColorCheckBox.AutoSize = true;
            this.DecreaseColorCheckBox.Location = new System.Drawing.Point(16, 741);
            this.DecreaseColorCheckBox.Name = "DecreaseColorCheckBox";
            this.DecreaseColorCheckBox.Size = new System.Drawing.Size(106, 22);
            this.DecreaseColorCheckBox.TabIndex = 132;
            this.DecreaseColorCheckBox.Text = "16色減色";
            this.DecreaseColorCheckBox.UseVisualStyleBackColor = true;
            this.DecreaseColorCheckBox.CheckedChanged += new System.EventHandler(this.ReDraw);
            // 
            // RotateYComboBox
            // 
            this.RotateYComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RotateYComboBox.FormattingEnabled = true;
            this.RotateYComboBox.Items.AddRange(new object[] {
            "0=傾けない",
            "1=傾ける1",
            "2=傾ける2",
            "3=傾ける3",
            "4=傾ける4",
            "5=傾ける5"});
            this.RotateYComboBox.Location = new System.Drawing.Point(16, 266);
            this.RotateYComboBox.Name = "RotateYComboBox";
            this.RotateYComboBox.Size = new System.Drawing.Size(281, 26);
            this.RotateYComboBox.TabIndex = 133;
            this.RotateYComboBox.SelectedIndexChanged += new System.EventHandler(this.ReDrawNoCache);
            // 
            // ToolPortraitMakerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1195, 811);
            this.Controls.Add(this.RotateYComboBox);
            this.Controls.Add(this.DecreaseColorCheckBox);
            this.Controls.Add(this.FaceTestType);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label124);
            this.Controls.Add(this.label122);
            this.Controls.Add(this.MouthH);
            this.Controls.Add(this.MouthW);
            this.Controls.Add(this.label123);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.EyeH);
            this.Controls.Add(this.EyeW);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ClipScale);
            this.Controls.Add(this.RevLeftRightCheckBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.FaceW);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.MouthY);
            this.Controls.Add(this.MouthX);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.EyeY);
            this.Controls.Add(this.EyeX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.FaceY);
            this.Controls.Add(this.FaceX);
            this.Controls.Add(this.MakeButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BustshotSelectButton);
            this.Controls.Add(this.ClipY);
            this.Controls.Add(this.BustshotFilename);
            this.Controls.Add(this.ClipX);
            this.Controls.Add(this.FaceSelectButton);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.FaceFilename);
            this.Controls.Add(this.label1);
            this.Name = "ToolPortraitMakerForm";
            this.Text = "顔画像生成ツール";
            this.Load += new System.EventHandler(this.ToolPortraitMakerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClipX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClipY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaceY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaceX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EyeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EyeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaceW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClipScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EyeH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EyeW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouthW)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private FEBuilderGBA.TextBoxEx FaceFilename;
        private System.Windows.Forms.Button FaceSelectButton;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx BustshotFilename;
        private System.Windows.Forms.Button BustshotSelectButton;
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.Button MakeButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ClipY;
        private System.Windows.Forms.NumericUpDown ClipX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown FaceY;
        private System.Windows.Forms.NumericUpDown FaceX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown EyeY;
        private System.Windows.Forms.NumericUpDown EyeX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown MouthY;
        private System.Windows.Forms.NumericUpDown MouthX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown FaceW;
        private System.Windows.Forms.CheckBox RevLeftRightCheckBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown ClipScale;
        private System.Windows.Forms.Label label123;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown EyeH;
        private System.Windows.Forms.NumericUpDown EyeW;
        private System.Windows.Forms.Label label124;
        private System.Windows.Forms.Label label122;
        private System.Windows.Forms.NumericUpDown MouthH;
        private System.Windows.Forms.NumericUpDown MouthW;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox FaceTestType;
        private System.Windows.Forms.CheckBox DecreaseColorCheckBox;
        private System.Windows.Forms.ComboBox RotateYComboBox;
    }
}