namespace FEBuilderGBA
{
    partial class GraphicsToolForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.PALETTENO = new System.Windows.Forms.NumericUpDown();
            this.ImageOption = new System.Windows.Forms.ComboBox();
            this.TSAOption = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Image = new System.Windows.Forms.NumericUpDown();
            this.TSA = new System.Windows.Forms.NumericUpDown();
            this.PALETTE = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.PALETTE_Recomend = new System.Windows.Forms.ComboBox();
            this.TSA_Recomend = new System.Windows.Forms.ComboBox();
            this.ZImageNo = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.USE_PALETTE_NUMBER = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ZoomComboBox = new System.Windows.Forms.ComboBox();
            this.PicHeight = new System.Windows.Forms.NumericUpDown();
            this.PicWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PaletteEditorButton = new System.Windows.Forms.Button();
            this.TSAEditorButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.PatchMakerButton = new System.Windows.Forms.Button();
            this.YYCHARMODEPAgeUp = new System.Windows.Forms.Button();
            this.YYCHARMODEPAgeDown = new System.Windows.Forms.Button();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.SecondImagePanel = new System.Windows.Forms.Panel();
            this.Image2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.KeepTSAComboBox = new System.Windows.Forms.ComboBox();
            this.X_BG_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PALETTENO)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TSA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PALETTE)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZImageNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicWidth)).BeginInit();
            this.panel2.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.SecondImagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_BG_PIC)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 248);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 31);
            this.label1.TabIndex = 75;
            this.label1.Text = "パレット番号";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PALETTENO
            // 
            this.PALETTENO.Location = new System.Drawing.Point(152, 254);
            this.PALETTENO.Margin = new System.Windows.Forms.Padding(2);
            this.PALETTENO.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.PALETTENO.Name = "PALETTENO";
            this.PALETTENO.Size = new System.Drawing.Size(120, 25);
            this.PALETTENO.TabIndex = 11;
            this.PALETTENO.ValueChanged += new System.EventHandler(this.ImageOption_SelectedIndexChanged);
            // 
            // ImageOption
            // 
            this.ImageOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ImageOption.FormattingEnabled = true;
            this.ImageOption.Items.AddRange(new object[] {
            "圧縮あり画像",
            "無圧縮画像",
            "2分割",
            "圧縮256色"});
            this.ImageOption.Location = new System.Drawing.Point(0, 100);
            this.ImageOption.Margin = new System.Windows.Forms.Padding(2);
            this.ImageOption.Name = "ImageOption";
            this.ImageOption.Size = new System.Drawing.Size(276, 26);
            this.ImageOption.TabIndex = 6;
            this.ImageOption.SelectedIndexChanged += new System.EventHandler(this.ImageOption_SelectedIndexChanged);
            // 
            // TSAOption
            // 
            this.TSAOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TSAOption.FormattingEnabled = true;
            this.TSAOption.Items.AddRange(new object[] {
            "TSAを利用しない",
            "圧縮TSAを利用する",
            "圧縮ヘッダ付きTSAを利用する",
            "無圧縮ヘッダ付きTSAを利用する",
            "無圧縮TSAを利用する"});
            this.TSAOption.Location = new System.Drawing.Point(0, 172);
            this.TSAOption.Margin = new System.Windows.Forms.Padding(2);
            this.TSAOption.Name = "TSAOption";
            this.TSAOption.Size = new System.Drawing.Size(274, 26);
            this.TSAOption.TabIndex = 8;
            this.TSAOption.SelectedIndexChanged += new System.EventHandler(this.TSA_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.X_BG_PIC);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(808, 491);
            this.panel1.TabIndex = 71;
            // 
            // Image
            // 
            this.Image.Hexadecimal = true;
            this.Image.Location = new System.Drawing.Point(152, 68);
            this.Image.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Image.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Image.Name = "Image";
            this.Image.Size = new System.Drawing.Size(120, 25);
            this.Image.TabIndex = 5;
            this.Image.ValueChanged += new System.EventHandler(this.Image_ValueChanged);
            // 
            // TSA
            // 
            this.TSA.Hexadecimal = true;
            this.TSA.Location = new System.Drawing.Point(152, 139);
            this.TSA.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.TSA.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.TSA.Name = "TSA";
            this.TSA.Size = new System.Drawing.Size(120, 25);
            this.TSA.TabIndex = 7;
            this.TSA.ValueChanged += new System.EventHandler(this.TSA_ValueChanged);
            // 
            // PALETTE
            // 
            this.PALETTE.Hexadecimal = true;
            this.PALETTE.Location = new System.Drawing.Point(152, 216);
            this.PALETTE.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.PALETTE.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PALETTE.Name = "PALETTE";
            this.PALETTE.Size = new System.Drawing.Size(120, 25);
            this.PALETTE.TabIndex = 10;
            this.PALETTE.ValueChanged += new System.EventHandler(this.ImageOption_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(0, 216);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 31);
            this.label8.TabIndex = 70;
            this.label8.Text = "パレット";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(0, 139);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 31);
            this.label6.TabIndex = 69;
            this.label6.Text = "TSA";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(0, 67);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 31);
            this.label7.TabIndex = 68;
            this.label7.Text = "画像アドレス";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(0, 293);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 31);
            this.label3.TabIndex = 77;
            this.label3.Text = "幅/8";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Location = new System.Drawing.Point(6, 7);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1304, 495);
            this.panel3.TabIndex = 78;
            // 
            // PALETTE_Recomend
            // 
            this.PALETTE_Recomend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PALETTE_Recomend.FormattingEnabled = true;
            this.PALETTE_Recomend.Location = new System.Drawing.Point(280, 215);
            this.PALETTE_Recomend.Margin = new System.Windows.Forms.Padding(4);
            this.PALETTE_Recomend.Name = "PALETTE_Recomend";
            this.PALETTE_Recomend.Size = new System.Drawing.Size(208, 26);
            this.PALETTE_Recomend.TabIndex = 12;
            this.PALETTE_Recomend.SelectedIndexChanged += new System.EventHandler(this.PALETTE_Recomend_SelectedIndexChanged);
            // 
            // TSA_Recomend
            // 
            this.TSA_Recomend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TSA_Recomend.FormattingEnabled = true;
            this.TSA_Recomend.Location = new System.Drawing.Point(280, 138);
            this.TSA_Recomend.Margin = new System.Windows.Forms.Padding(4);
            this.TSA_Recomend.Name = "TSA_Recomend";
            this.TSA_Recomend.Size = new System.Drawing.Size(208, 26);
            this.TSA_Recomend.TabIndex = 9;
            this.TSA_Recomend.SelectedIndexChanged += new System.EventHandler(this.TSA_Recomend_SelectedIndexChanged);
            // 
            // ZImageNo
            // 
            this.ZImageNo.Hexadecimal = true;
            this.ZImageNo.Location = new System.Drawing.Point(152, 8);
            this.ZImageNo.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ZImageNo.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ZImageNo.Name = "ZImageNo";
            this.ZImageNo.Size = new System.Drawing.Size(120, 25);
            this.ZImageNo.TabIndex = 4;
            this.ZImageNo.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ZImageNo.ValueChanged += new System.EventHandler(this.ZImageNo_ValueChanged);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(0, 4);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 31);
            this.label9.TabIndex = 83;
            this.label9.Text = "No";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // USE_PALETTE_NUMBER
            // 
            this.USE_PALETTE_NUMBER.AutoSize = true;
            this.USE_PALETTE_NUMBER.Location = new System.Drawing.Point(323, 411);
            this.USE_PALETTE_NUMBER.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.USE_PALETTE_NUMBER.Name = "USE_PALETTE_NUMBER";
            this.USE_PALETTE_NUMBER.Size = new System.Drawing.Size(0, 18);
            this.USE_PALETTE_NUMBER.TabIndex = 82;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(2, 404);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(307, 31);
            this.label5.TabIndex = 81;
            this.label5.Text = "画像が利用している16色パレットの個数";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ZoomComboBox
            // 
            this.ZoomComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ZoomComboBox.FormattingEnabled = true;
            this.ZoomComboBox.Items.AddRange(new object[] {
            "拡大しない",
            "2倍拡大",
            "3倍拡大",
            "4倍拡大"});
            this.ZoomComboBox.Location = new System.Drawing.Point(0, 362);
            this.ZoomComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.ZoomComboBox.Name = "ZoomComboBox";
            this.ZoomComboBox.Size = new System.Drawing.Size(268, 26);
            this.ZoomComboBox.TabIndex = 15;
            this.ZoomComboBox.SelectedIndexChanged += new System.EventHandler(this.ZoomComboBox_SelectedIndexChanged);
            // 
            // PicHeight
            // 
            this.PicHeight.Location = new System.Drawing.Point(152, 331);
            this.PicHeight.Margin = new System.Windows.Forms.Padding(2);
            this.PicHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PicHeight.Name = "PicHeight";
            this.PicHeight.Size = new System.Drawing.Size(120, 25);
            this.PicHeight.TabIndex = 14;
            this.PicHeight.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.PicHeight.ValueChanged += new System.EventHandler(this.ImageOption_SelectedIndexChanged);
            // 
            // PicWidth
            // 
            this.PicWidth.Location = new System.Drawing.Point(152, 296);
            this.PicWidth.Margin = new System.Windows.Forms.Padding(2);
            this.PicWidth.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.PicWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PicWidth.Name = "PicWidth";
            this.PicWidth.Size = new System.Drawing.Size(120, 25);
            this.PicWidth.TabIndex = 13;
            this.PicWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.PicWidth.ValueChanged += new System.EventHandler(this.ImageOption_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(0, 325);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 31);
            this.label4.TabIndex = 78;
            this.label4.Text = "高さ/8";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.PaletteEditorButton);
            this.panel2.Controls.Add(this.TSAEditorButton);
            this.panel2.Controls.Add(this.ImportButton);
            this.panel2.Controls.Add(this.ExportButton);
            this.panel2.Controls.Add(this.PatchMakerButton);
            this.panel2.Location = new System.Drawing.Point(6, 506);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1311, 49);
            this.panel2.TabIndex = 79;
            // 
            // PaletteEditorButton
            // 
            this.PaletteEditorButton.Location = new System.Drawing.Point(504, 3);
            this.PaletteEditorButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.PaletteEditorButton.Name = "PaletteEditorButton";
            this.PaletteEditorButton.Size = new System.Drawing.Size(160, 41);
            this.PaletteEditorButton.TabIndex = 1;
            this.PaletteEditorButton.Text = "パレットエディタ";
            this.PaletteEditorButton.UseVisualStyleBackColor = true;
            this.PaletteEditorButton.Click += new System.EventHandler(this.PaletteEditorButton_Click);
            // 
            // TSAEditorButton
            // 
            this.TSAEditorButton.Location = new System.Drawing.Point(293, 3);
            this.TSAEditorButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.TSAEditorButton.Name = "TSAEditorButton";
            this.TSAEditorButton.Size = new System.Drawing.Size(160, 41);
            this.TSAEditorButton.TabIndex = 0;
            this.TSAEditorButton.Text = "TSA Editor";
            this.TSAEditorButton.UseVisualStyleBackColor = true;
            this.TSAEditorButton.Click += new System.EventHandler(this.TSAEditorButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(702, 4);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(131, 40);
            this.ImportButton.TabIndex = 2;
            this.ImportButton.Text = "画像読込";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(890, 4);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(131, 40);
            this.ExportButton.TabIndex = 3;
            this.ExportButton.Text = "画像取出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // PatchMakerButton
            // 
            this.PatchMakerButton.Location = new System.Drawing.Point(1094, 4);
            this.PatchMakerButton.Margin = new System.Windows.Forms.Padding(4);
            this.PatchMakerButton.Name = "PatchMakerButton";
            this.PatchMakerButton.Size = new System.Drawing.Size(131, 40);
            this.PatchMakerButton.TabIndex = 4;
            this.PatchMakerButton.Text = "PatchMaker";
            this.PatchMakerButton.UseVisualStyleBackColor = true;
            this.PatchMakerButton.Click += new System.EventHandler(this.PatchMakerButton_Click);
            // 
            // YYCHARMODEPAgeUp
            // 
            this.YYCHARMODEPAgeUp.Location = new System.Drawing.Point(280, 67);
            this.YYCHARMODEPAgeUp.Margin = new System.Windows.Forms.Padding(4);
            this.YYCHARMODEPAgeUp.Name = "YYCHARMODEPAgeUp";
            this.YYCHARMODEPAgeUp.Size = new System.Drawing.Size(91, 28);
            this.YYCHARMODEPAgeUp.TabIndex = 86;
            this.YYCHARMODEPAgeUp.Text = "PageUp";
            this.YYCHARMODEPAgeUp.UseVisualStyleBackColor = true;
            this.YYCHARMODEPAgeUp.Visible = false;
            this.YYCHARMODEPAgeUp.Click += new System.EventHandler(this.YYCHARMODEPAgeUp_Click);
            // 
            // YYCHARMODEPAgeDown
            // 
            this.YYCHARMODEPAgeDown.Location = new System.Drawing.Point(378, 67);
            this.YYCHARMODEPAgeDown.Margin = new System.Windows.Forms.Padding(4);
            this.YYCHARMODEPAgeDown.Name = "YYCHARMODEPAgeDown";
            this.YYCHARMODEPAgeDown.Size = new System.Drawing.Size(103, 28);
            this.YYCHARMODEPAgeDown.TabIndex = 87;
            this.YYCHARMODEPAgeDown.Text = "PageDown";
            this.YYCHARMODEPAgeDown.UseVisualStyleBackColor = true;
            this.YYCHARMODEPAgeDown.Visible = false;
            this.YYCHARMODEPAgeDown.Click += new System.EventHandler(this.YYCHARMODEPAgeDown_Click);
            // 
            // ControlPanel
            // 
            this.ControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlPanel.Controls.Add(this.SecondImagePanel);
            this.ControlPanel.Controls.Add(this.KeepTSAComboBox);
            this.ControlPanel.Controls.Add(this.YYCHARMODEPAgeDown);
            this.ControlPanel.Controls.Add(this.label7);
            this.ControlPanel.Controls.Add(this.TSAOption);
            this.ControlPanel.Controls.Add(this.YYCHARMODEPAgeUp);
            this.ControlPanel.Controls.Add(this.Image);
            this.ControlPanel.Controls.Add(this.PALETTE_Recomend);
            this.ControlPanel.Controls.Add(this.ImageOption);
            this.ControlPanel.Controls.Add(this.TSA_Recomend);
            this.ControlPanel.Controls.Add(this.TSA);
            this.ControlPanel.Controls.Add(this.ZImageNo);
            this.ControlPanel.Controls.Add(this.label3);
            this.ControlPanel.Controls.Add(this.label9);
            this.ControlPanel.Controls.Add(this.PALETTE);
            this.ControlPanel.Controls.Add(this.USE_PALETTE_NUMBER);
            this.ControlPanel.Controls.Add(this.PALETTENO);
            this.ControlPanel.Controls.Add(this.label5);
            this.ControlPanel.Controls.Add(this.label8);
            this.ControlPanel.Controls.Add(this.ZoomComboBox);
            this.ControlPanel.Controls.Add(this.label4);
            this.ControlPanel.Controls.Add(this.PicHeight);
            this.ControlPanel.Controls.Add(this.label6);
            this.ControlPanel.Controls.Add(this.label1);
            this.ControlPanel.Controls.Add(this.PicWidth);
            this.ControlPanel.Location = new System.Drawing.Point(811, 7);
            this.ControlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(499, 491);
            this.ControlPanel.TabIndex = 88;
            // 
            // SecondImagePanel
            // 
            this.SecondImagePanel.Controls.Add(this.Image2);
            this.SecondImagePanel.Controls.Add(this.label2);
            this.SecondImagePanel.Location = new System.Drawing.Point(280, 102);
            this.SecondImagePanel.Name = "SecondImagePanel";
            this.SecondImagePanel.Size = new System.Drawing.Size(218, 29);
            this.SecondImagePanel.TabIndex = 90;
            this.SecondImagePanel.Visible = false;
            // 
            // Image2
            // 
            this.Image2.Hexadecimal = true;
            this.Image2.Location = new System.Drawing.Point(97, 3);
            this.Image2.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Image2.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Image2.Name = "Image2";
            this.Image2.Size = new System.Drawing.Size(104, 25);
            this.Image2.TabIndex = 88;
            this.Image2.ValueChanged += new System.EventHandler(this.ImageOption_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 18);
            this.label2.TabIndex = 89;
            this.label2.Text = "第2画像";
            // 
            // KeepTSAComboBox
            // 
            this.KeepTSAComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeepTSAComboBox.FormattingEnabled = true;
            this.KeepTSAComboBox.Items.AddRange(new object[] {
            "編集時にTSAを再構築する",
            "編集時にでもTSAを維持して最小限の変更をする"});
            this.KeepTSAComboBox.Location = new System.Drawing.Point(2, 450);
            this.KeepTSAComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.KeepTSAComboBox.Name = "KeepTSAComboBox";
            this.KeepTSAComboBox.Size = new System.Drawing.Size(495, 26);
            this.KeepTSAComboBox.TabIndex = 10;
            this.KeepTSAComboBox.SelectedIndexChanged += new System.EventHandler(this.KeepTSAComboBox_SelectedIndexChanged);
            // 
            // X_BG_PIC
            // 
            this.X_BG_PIC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.X_BG_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_BG_PIC.Location = new System.Drawing.Point(0, 2);
            this.X_BG_PIC.Margin = new System.Windows.Forms.Padding(5);
            this.X_BG_PIC.Name = "X_BG_PIC";
            this.X_BG_PIC.Size = new System.Drawing.Size(388, 242);
            this.X_BG_PIC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.X_BG_PIC.TabIndex = 67;
            this.X_BG_PIC.TabStop = false;
            // 
            // GraphicsToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1318, 562);
            this.Controls.Add(this.ControlPanel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GraphicsToolForm";
            this.Text = "グラフィックツール";
            this.Load += new System.EventHandler(this.GraphicsToolForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PALETTENO)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TSA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PALETTE)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ZImageNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicWidth)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.SecondImagePanel.ResumeLayout(false);
            this.SecondImagePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_BG_PIC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown Image;
        private System.Windows.Forms.NumericUpDown TSA;
        private System.Windows.Forms.NumericUpDown PALETTE;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown PALETTENO;
        private System.Windows.Forms.ComboBox ImageOption;
        private System.Windows.Forms.ComboBox TSAOption;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown PicHeight;
        private System.Windows.Forms.NumericUpDown PicWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ZoomComboBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button PatchMakerButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label USE_PALETTE_NUMBER;
        private System.Windows.Forms.NumericUpDown ZImageNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox PALETTE_Recomend;
        private System.Windows.Forms.ComboBox TSA_Recomend;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button YYCHARMODEPAgeDown;
        private System.Windows.Forms.Button YYCHARMODEPAgeUp;
        private System.Windows.Forms.Panel ControlPanel;
        private InterpolatedPictureBox X_BG_PIC;
        private System.Windows.Forms.ComboBox KeepTSAComboBox;
        private System.Windows.Forms.Button TSAEditorButton;
        private System.Windows.Forms.Button PaletteEditorButton;
        private System.Windows.Forms.NumericUpDown Image2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel SecondImagePanel;

    }
}