namespace FEBuilderGBA
{
    partial class PaletteChangeColorsForm
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
            this.NewColor = new System.Windows.Forms.Button();
            this.OldColor = new System.Windows.Forms.Panel();
            this.NewColorR = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NewColorG = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.NewColorB = new System.Windows.Forms.NumericUpDown();
            this.Ref01Color = new System.Windows.Forms.Panel();
            this.Ref01 = new System.Windows.Forms.CheckBox();
            this.Ref02 = new System.Windows.Forms.CheckBox();
            this.Ref02Color = new System.Windows.Forms.Panel();
            this.Ref04 = new System.Windows.Forms.CheckBox();
            this.Ref04Color = new System.Windows.Forms.Panel();
            this.Ref03 = new System.Windows.Forms.CheckBox();
            this.Ref03Color = new System.Windows.Forms.Panel();
            this.Ref08 = new System.Windows.Forms.CheckBox();
            this.Ref08Color = new System.Windows.Forms.Panel();
            this.Ref07 = new System.Windows.Forms.CheckBox();
            this.Ref07Color = new System.Windows.Forms.Panel();
            this.Ref06 = new System.Windows.Forms.CheckBox();
            this.Ref06Color = new System.Windows.Forms.Panel();
            this.Ref05 = new System.Windows.Forms.CheckBox();
            this.Ref05Color = new System.Windows.Forms.Panel();
            this.Ref16 = new System.Windows.Forms.CheckBox();
            this.Ref16Color = new System.Windows.Forms.Panel();
            this.Ref15 = new System.Windows.Forms.CheckBox();
            this.Ref15Color = new System.Windows.Forms.Panel();
            this.Ref14 = new System.Windows.Forms.CheckBox();
            this.Ref14Color = new System.Windows.Forms.Panel();
            this.Ref13 = new System.Windows.Forms.CheckBox();
            this.Ref13Color = new System.Windows.Forms.Panel();
            this.Ref12 = new System.Windows.Forms.CheckBox();
            this.Ref12Color = new System.Windows.Forms.Panel();
            this.Ref11 = new System.Windows.Forms.CheckBox();
            this.Ref11Color = new System.Windows.Forms.Panel();
            this.Ref10 = new System.Windows.Forms.CheckBox();
            this.Ref10Color = new System.Windows.Forms.Panel();
            this.Ref09 = new System.Windows.Forms.CheckBox();
            this.Ref09Color = new System.Windows.Forms.Panel();
            this.OKButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.Preview = new FEBuilderGBA.InterpolatedPictureBox();
            this.labelEx4 = new FEBuilderGBA.LabelEx();
            this.labelEx3 = new FEBuilderGBA.LabelEx();
            this.OldColorInfo = new FEBuilderGBA.TextBoxEx();
            this.labelEx2 = new FEBuilderGBA.LabelEx();
            this.labelEx1 = new FEBuilderGBA.LabelEx();
            ((System.ComponentModel.ISupportInitialize)(this.NewColorR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewColorG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewColorB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // NewColor
            // 
            this.NewColor.Location = new System.Drawing.Point(179, 41);
            this.NewColor.Name = "NewColor";
            this.NewColor.Size = new System.Drawing.Size(133, 36);
            this.NewColor.TabIndex = 2;
            this.NewColor.UseVisualStyleBackColor = true;
            this.NewColor.Click += new System.EventHandler(this.NewColor_Click);
            // 
            // OldColor
            // 
            this.OldColor.Location = new System.Drawing.Point(179, 6);
            this.OldColor.Name = "OldColor";
            this.OldColor.Size = new System.Drawing.Size(133, 34);
            this.OldColor.TabIndex = 3;
            // 
            // NewColorR
            // 
            this.NewColorR.Location = new System.Drawing.Point(364, 48);
            this.NewColorR.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NewColorR.Name = "NewColorR";
            this.NewColorR.Size = new System.Drawing.Size(78, 25);
            this.NewColorR.TabIndex = 4;
            this.NewColorR.ValueChanged += new System.EventHandler(this.NewColorR_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(324, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "R:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(447, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "G:";
            // 
            // NewColorG
            // 
            this.NewColorG.Location = new System.Drawing.Point(481, 48);
            this.NewColorG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NewColorG.Name = "NewColorG";
            this.NewColorG.Size = new System.Drawing.Size(78, 25);
            this.NewColorG.TabIndex = 6;
            this.NewColorG.ValueChanged += new System.EventHandler(this.NewColorR_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(570, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "B:";
            // 
            // NewColorB
            // 
            this.NewColorB.Location = new System.Drawing.Point(599, 48);
            this.NewColorB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NewColorB.Name = "NewColorB";
            this.NewColorB.Size = new System.Drawing.Size(78, 25);
            this.NewColorB.TabIndex = 8;
            this.NewColorB.ValueChanged += new System.EventHandler(this.NewColorR_ValueChanged);
            // 
            // Ref01Color
            // 
            this.Ref01Color.Location = new System.Drawing.Point(106, 122);
            this.Ref01Color.Name = "Ref01Color";
            this.Ref01Color.Size = new System.Drawing.Size(122, 29);
            this.Ref01Color.TabIndex = 92;
            // 
            // Ref01
            // 
            this.Ref01.AutoSize = true;
            this.Ref01.Location = new System.Drawing.Point(17, 127);
            this.Ref01.Name = "Ref01";
            this.Ref01.Size = new System.Drawing.Size(68, 22);
            this.Ref01.TabIndex = 93;
            this.Ref01.Text = "No.1";
            this.Ref01.UseVisualStyleBackColor = true;
            this.Ref01.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref02
            // 
            this.Ref02.AutoSize = true;
            this.Ref02.Location = new System.Drawing.Point(17, 159);
            this.Ref02.Name = "Ref02";
            this.Ref02.Size = new System.Drawing.Size(68, 22);
            this.Ref02.TabIndex = 95;
            this.Ref02.Text = "No.2";
            this.Ref02.UseVisualStyleBackColor = true;
            this.Ref02.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref02Color
            // 
            this.Ref02Color.Location = new System.Drawing.Point(106, 154);
            this.Ref02Color.Name = "Ref02Color";
            this.Ref02Color.Size = new System.Drawing.Size(122, 29);
            this.Ref02Color.TabIndex = 94;
            // 
            // Ref04
            // 
            this.Ref04.AutoSize = true;
            this.Ref04.Location = new System.Drawing.Point(17, 225);
            this.Ref04.Name = "Ref04";
            this.Ref04.Size = new System.Drawing.Size(68, 22);
            this.Ref04.TabIndex = 99;
            this.Ref04.Text = "No.4";
            this.Ref04.UseVisualStyleBackColor = true;
            this.Ref04.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref04Color
            // 
            this.Ref04Color.Location = new System.Drawing.Point(106, 220);
            this.Ref04Color.Name = "Ref04Color";
            this.Ref04Color.Size = new System.Drawing.Size(122, 29);
            this.Ref04Color.TabIndex = 98;
            // 
            // Ref03
            // 
            this.Ref03.AutoSize = true;
            this.Ref03.Location = new System.Drawing.Point(17, 193);
            this.Ref03.Name = "Ref03";
            this.Ref03.Size = new System.Drawing.Size(68, 22);
            this.Ref03.TabIndex = 97;
            this.Ref03.Text = "No.3";
            this.Ref03.UseVisualStyleBackColor = true;
            this.Ref03.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref03Color
            // 
            this.Ref03Color.Location = new System.Drawing.Point(106, 188);
            this.Ref03Color.Name = "Ref03Color";
            this.Ref03Color.Size = new System.Drawing.Size(122, 29);
            this.Ref03Color.TabIndex = 96;
            // 
            // Ref08
            // 
            this.Ref08.AutoSize = true;
            this.Ref08.Location = new System.Drawing.Point(17, 357);
            this.Ref08.Name = "Ref08";
            this.Ref08.Size = new System.Drawing.Size(68, 22);
            this.Ref08.TabIndex = 107;
            this.Ref08.Text = "No.8";
            this.Ref08.UseVisualStyleBackColor = true;
            this.Ref08.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref08Color
            // 
            this.Ref08Color.Location = new System.Drawing.Point(106, 352);
            this.Ref08Color.Name = "Ref08Color";
            this.Ref08Color.Size = new System.Drawing.Size(122, 29);
            this.Ref08Color.TabIndex = 106;
            // 
            // Ref07
            // 
            this.Ref07.AutoSize = true;
            this.Ref07.Location = new System.Drawing.Point(17, 325);
            this.Ref07.Name = "Ref07";
            this.Ref07.Size = new System.Drawing.Size(68, 22);
            this.Ref07.TabIndex = 105;
            this.Ref07.Text = "No.7";
            this.Ref07.UseVisualStyleBackColor = true;
            this.Ref07.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref07Color
            // 
            this.Ref07Color.Location = new System.Drawing.Point(106, 320);
            this.Ref07Color.Name = "Ref07Color";
            this.Ref07Color.Size = new System.Drawing.Size(122, 29);
            this.Ref07Color.TabIndex = 104;
            // 
            // Ref06
            // 
            this.Ref06.AutoSize = true;
            this.Ref06.Location = new System.Drawing.Point(17, 291);
            this.Ref06.Name = "Ref06";
            this.Ref06.Size = new System.Drawing.Size(68, 22);
            this.Ref06.TabIndex = 103;
            this.Ref06.Text = "No.6";
            this.Ref06.UseVisualStyleBackColor = true;
            this.Ref06.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref06Color
            // 
            this.Ref06Color.Location = new System.Drawing.Point(106, 286);
            this.Ref06Color.Name = "Ref06Color";
            this.Ref06Color.Size = new System.Drawing.Size(122, 29);
            this.Ref06Color.TabIndex = 102;
            // 
            // Ref05
            // 
            this.Ref05.AutoSize = true;
            this.Ref05.Location = new System.Drawing.Point(17, 259);
            this.Ref05.Name = "Ref05";
            this.Ref05.Size = new System.Drawing.Size(68, 22);
            this.Ref05.TabIndex = 101;
            this.Ref05.Text = "No.5";
            this.Ref05.UseVisualStyleBackColor = true;
            this.Ref05.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref05Color
            // 
            this.Ref05Color.Location = new System.Drawing.Point(106, 254);
            this.Ref05Color.Name = "Ref05Color";
            this.Ref05Color.Size = new System.Drawing.Size(122, 29);
            this.Ref05Color.TabIndex = 100;
            // 
            // Ref16
            // 
            this.Ref16.AutoSize = true;
            this.Ref16.Location = new System.Drawing.Point(17, 619);
            this.Ref16.Name = "Ref16";
            this.Ref16.Size = new System.Drawing.Size(77, 22);
            this.Ref16.TabIndex = 123;
            this.Ref16.Text = "No.16";
            this.Ref16.UseVisualStyleBackColor = true;
            this.Ref16.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref16Color
            // 
            this.Ref16Color.Location = new System.Drawing.Point(106, 614);
            this.Ref16Color.Name = "Ref16Color";
            this.Ref16Color.Size = new System.Drawing.Size(122, 29);
            this.Ref16Color.TabIndex = 122;
            // 
            // Ref15
            // 
            this.Ref15.AutoSize = true;
            this.Ref15.Location = new System.Drawing.Point(17, 587);
            this.Ref15.Name = "Ref15";
            this.Ref15.Size = new System.Drawing.Size(77, 22);
            this.Ref15.TabIndex = 121;
            this.Ref15.Text = "No.15";
            this.Ref15.UseVisualStyleBackColor = true;
            this.Ref15.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref15Color
            // 
            this.Ref15Color.Location = new System.Drawing.Point(106, 582);
            this.Ref15Color.Name = "Ref15Color";
            this.Ref15Color.Size = new System.Drawing.Size(122, 29);
            this.Ref15Color.TabIndex = 120;
            // 
            // Ref14
            // 
            this.Ref14.AutoSize = true;
            this.Ref14.Location = new System.Drawing.Point(17, 553);
            this.Ref14.Name = "Ref14";
            this.Ref14.Size = new System.Drawing.Size(77, 22);
            this.Ref14.TabIndex = 119;
            this.Ref14.Text = "No.14";
            this.Ref14.UseVisualStyleBackColor = true;
            this.Ref14.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref14Color
            // 
            this.Ref14Color.Location = new System.Drawing.Point(106, 548);
            this.Ref14Color.Name = "Ref14Color";
            this.Ref14Color.Size = new System.Drawing.Size(122, 29);
            this.Ref14Color.TabIndex = 118;
            // 
            // Ref13
            // 
            this.Ref13.AutoSize = true;
            this.Ref13.Location = new System.Drawing.Point(17, 521);
            this.Ref13.Name = "Ref13";
            this.Ref13.Size = new System.Drawing.Size(77, 22);
            this.Ref13.TabIndex = 117;
            this.Ref13.Text = "No.13";
            this.Ref13.UseVisualStyleBackColor = true;
            this.Ref13.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref13Color
            // 
            this.Ref13Color.Location = new System.Drawing.Point(106, 516);
            this.Ref13Color.Name = "Ref13Color";
            this.Ref13Color.Size = new System.Drawing.Size(122, 29);
            this.Ref13Color.TabIndex = 116;
            // 
            // Ref12
            // 
            this.Ref12.AutoSize = true;
            this.Ref12.Location = new System.Drawing.Point(17, 487);
            this.Ref12.Name = "Ref12";
            this.Ref12.Size = new System.Drawing.Size(77, 22);
            this.Ref12.TabIndex = 115;
            this.Ref12.Text = "No.12";
            this.Ref12.UseVisualStyleBackColor = true;
            this.Ref12.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref12Color
            // 
            this.Ref12Color.Location = new System.Drawing.Point(106, 482);
            this.Ref12Color.Name = "Ref12Color";
            this.Ref12Color.Size = new System.Drawing.Size(122, 29);
            this.Ref12Color.TabIndex = 114;
            // 
            // Ref11
            // 
            this.Ref11.AutoSize = true;
            this.Ref11.Location = new System.Drawing.Point(17, 455);
            this.Ref11.Name = "Ref11";
            this.Ref11.Size = new System.Drawing.Size(77, 22);
            this.Ref11.TabIndex = 113;
            this.Ref11.Text = "No.11";
            this.Ref11.UseVisualStyleBackColor = true;
            this.Ref11.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref11Color
            // 
            this.Ref11Color.Location = new System.Drawing.Point(106, 450);
            this.Ref11Color.Name = "Ref11Color";
            this.Ref11Color.Size = new System.Drawing.Size(122, 29);
            this.Ref11Color.TabIndex = 112;
            // 
            // Ref10
            // 
            this.Ref10.AutoSize = true;
            this.Ref10.Location = new System.Drawing.Point(17, 421);
            this.Ref10.Name = "Ref10";
            this.Ref10.Size = new System.Drawing.Size(77, 22);
            this.Ref10.TabIndex = 111;
            this.Ref10.Text = "No.10";
            this.Ref10.UseVisualStyleBackColor = true;
            this.Ref10.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref10Color
            // 
            this.Ref10Color.Location = new System.Drawing.Point(106, 416);
            this.Ref10Color.Name = "Ref10Color";
            this.Ref10Color.Size = new System.Drawing.Size(122, 29);
            this.Ref10Color.TabIndex = 110;
            // 
            // Ref09
            // 
            this.Ref09.AutoSize = true;
            this.Ref09.Location = new System.Drawing.Point(17, 389);
            this.Ref09.Name = "Ref09";
            this.Ref09.Size = new System.Drawing.Size(68, 22);
            this.Ref09.TabIndex = 109;
            this.Ref09.Text = "No.9";
            this.Ref09.UseVisualStyleBackColor = true;
            this.Ref09.CheckedChanged += new System.EventHandler(this.Ref01_CheckedChanged);
            // 
            // Ref09Color
            // 
            this.Ref09Color.Location = new System.Drawing.Point(106, 384);
            this.Ref09Color.Name = "Ref09Color";
            this.Ref09Color.Size = new System.Drawing.Size(122, 29);
            this.Ref09Color.TabIndex = 108;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(601, 606);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(228, 36);
            this.OKButton.TabIndex = 127;
            this.OKButton.Text = "変更";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(700, 41);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(129, 36);
            this.ResetButton.TabIndex = 129;
            this.ResetButton.Text = "リセット";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // Preview
            // 
            this.Preview.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.Preview.Location = new System.Drawing.Point(254, 127);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(572, 382);
            this.Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Preview.TabIndex = 128;
            this.Preview.TabStop = false;
            // 
            // labelEx4
            // 
            this.labelEx4.AutoSize = true;
            this.labelEx4.ErrorMessage = "";
            this.labelEx4.Location = new System.Drawing.Point(260, 98);
            this.labelEx4.Name = "labelEx4";
            this.labelEx4.Size = new System.Drawing.Size(76, 18);
            this.labelEx4.TabIndex = 126;
            this.labelEx4.Text = "プレビュー";
            // 
            // labelEx3
            // 
            this.labelEx3.AutoSize = true;
            this.labelEx3.ErrorMessage = "";
            this.labelEx3.Location = new System.Drawing.Point(14, 98);
            this.labelEx3.Name = "labelEx3";
            this.labelEx3.Size = new System.Drawing.Size(154, 18);
            this.labelEx3.TabIndex = 125;
            this.labelEx3.Text = "連動して変更する色";
            // 
            // OldColorInfo
            // 
            this.OldColorInfo.ErrorMessage = "";
            this.OldColorInfo.Location = new System.Drawing.Point(328, 10);
            this.OldColorInfo.Name = "OldColorInfo";
            this.OldColorInfo.Placeholder = "";
            this.OldColorInfo.Size = new System.Drawing.Size(349, 25);
            this.OldColorInfo.TabIndex = 10;
            // 
            // labelEx2
            // 
            this.labelEx2.AutoSize = true;
            this.labelEx2.ErrorMessage = "";
            this.labelEx2.Location = new System.Drawing.Point(13, 50);
            this.labelEx2.Name = "labelEx2";
            this.labelEx2.Size = new System.Drawing.Size(72, 18);
            this.labelEx2.TabIndex = 1;
            this.labelEx2.Text = "新しい色";
            // 
            // labelEx1
            // 
            this.labelEx1.AutoSize = true;
            this.labelEx1.ErrorMessage = "";
            this.labelEx1.Location = new System.Drawing.Point(13, 13);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(91, 18);
            this.labelEx1.TabIndex = 0;
            this.labelEx1.Text = "変更する色";
            // 
            // PaletteChangeColorsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 654);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.labelEx4);
            this.Controls.Add(this.labelEx3);
            this.Controls.Add(this.Ref16);
            this.Controls.Add(this.Ref16Color);
            this.Controls.Add(this.Ref15);
            this.Controls.Add(this.Ref15Color);
            this.Controls.Add(this.Ref14);
            this.Controls.Add(this.Ref14Color);
            this.Controls.Add(this.Ref13);
            this.Controls.Add(this.Ref13Color);
            this.Controls.Add(this.Ref12);
            this.Controls.Add(this.Ref12Color);
            this.Controls.Add(this.Ref11);
            this.Controls.Add(this.Ref11Color);
            this.Controls.Add(this.Ref10);
            this.Controls.Add(this.Ref10Color);
            this.Controls.Add(this.Ref09);
            this.Controls.Add(this.Ref09Color);
            this.Controls.Add(this.Ref08);
            this.Controls.Add(this.Ref08Color);
            this.Controls.Add(this.Ref07);
            this.Controls.Add(this.Ref07Color);
            this.Controls.Add(this.Ref06);
            this.Controls.Add(this.Ref06Color);
            this.Controls.Add(this.Ref05);
            this.Controls.Add(this.Ref05Color);
            this.Controls.Add(this.Ref04);
            this.Controls.Add(this.Ref04Color);
            this.Controls.Add(this.Ref03);
            this.Controls.Add(this.Ref03Color);
            this.Controls.Add(this.Ref02);
            this.Controls.Add(this.Ref02Color);
            this.Controls.Add(this.Ref01);
            this.Controls.Add(this.Ref01Color);
            this.Controls.Add(this.OldColorInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NewColorB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NewColorG);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NewColorR);
            this.Controls.Add(this.OldColor);
            this.Controls.Add(this.NewColor);
            this.Controls.Add(this.labelEx2);
            this.Controls.Add(this.labelEx1);
            this.Name = "PaletteChangeColorsForm";
            this.Text = "似た色を連動して置き換える";
            this.Load += new System.EventHandler(this.PaletteChangeColorsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NewColorR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewColorG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewColorB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LabelEx labelEx1;
        private LabelEx labelEx2;
        private System.Windows.Forms.Button NewColor;
        private System.Windows.Forms.Panel OldColor;
        private System.Windows.Forms.NumericUpDown NewColorR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NewColorG;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NewColorB;
        private TextBoxEx OldColorInfo;
        private System.Windows.Forms.Panel Ref01Color;
        private System.Windows.Forms.CheckBox Ref01;
        private System.Windows.Forms.CheckBox Ref02;
        private System.Windows.Forms.Panel Ref02Color;
        private System.Windows.Forms.CheckBox Ref04;
        private System.Windows.Forms.Panel Ref04Color;
        private System.Windows.Forms.CheckBox Ref03;
        private System.Windows.Forms.Panel Ref03Color;
        private System.Windows.Forms.CheckBox Ref08;
        private System.Windows.Forms.Panel Ref08Color;
        private System.Windows.Forms.CheckBox Ref07;
        private System.Windows.Forms.Panel Ref07Color;
        private System.Windows.Forms.CheckBox Ref06;
        private System.Windows.Forms.Panel Ref06Color;
        private System.Windows.Forms.CheckBox Ref05;
        private System.Windows.Forms.Panel Ref05Color;
        private System.Windows.Forms.CheckBox Ref16;
        private System.Windows.Forms.Panel Ref16Color;
        private System.Windows.Forms.CheckBox Ref15;
        private System.Windows.Forms.Panel Ref15Color;
        private System.Windows.Forms.CheckBox Ref14;
        private System.Windows.Forms.Panel Ref14Color;
        private System.Windows.Forms.CheckBox Ref13;
        private System.Windows.Forms.Panel Ref13Color;
        private System.Windows.Forms.CheckBox Ref12;
        private System.Windows.Forms.Panel Ref12Color;
        private System.Windows.Forms.CheckBox Ref11;
        private System.Windows.Forms.Panel Ref11Color;
        private System.Windows.Forms.CheckBox Ref10;
        private System.Windows.Forms.Panel Ref10Color;
        private System.Windows.Forms.CheckBox Ref09;
        private System.Windows.Forms.Panel Ref09Color;
        private LabelEx labelEx3;
        private LabelEx labelEx4;
        private System.Windows.Forms.Button OKButton;
        private InterpolatedPictureBox Preview;
        private System.Windows.Forms.Button ResetButton;
    }
}