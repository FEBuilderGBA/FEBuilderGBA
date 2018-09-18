namespace FEBuilderGBA
{
    partial class ToolMagicEffectMakerForm
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
            this.label13 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OrignalFilenameSelectButton = new System.Windows.Forms.Button();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ConvertType = new System.Windows.Forms.ComboBox();
            this.PartsX = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.PartsY = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.ResizeH = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.ResizeW = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.ResizeY = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.ResizeX = new System.Windows.Forms.NumericUpDown();
            this.RunButton = new System.Windows.Forms.Button();
            this.SoundFirePlaySoundButton = new System.Windows.Forms.Button();
            this.SoundFire = new System.Windows.Forms.NumericUpDown();
            this.SoundFireInfo = new FEBuilderGBA.TextBoxEx();
            this.label14 = new System.Windows.Forms.Label();
            this.FrameWait = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.SoundHitInfo = new FEBuilderGBA.TextBoxEx();
            this.SoundHitPlaySoundButton = new System.Windows.Forms.Button();
            this.SoundHit = new System.Windows.Forms.NumericUpDown();
            this.SoundHitLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PartsX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartsY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SoundFire)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SoundHit)).BeginInit();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(11, 9);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(1035, 31);
            this.label13.TabIndex = 133;
            this.label13.Text = "RPGツクールのエフェクトを変換します";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(11, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(254, 31);
            this.label2.TabIndex = 132;
            this.label2.Text = "画像";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OrignalFilenameSelectButton
            // 
            this.OrignalFilenameSelectButton.Location = new System.Drawing.Point(271, 46);
            this.OrignalFilenameSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalFilenameSelectButton.Name = "OrignalFilenameSelectButton";
            this.OrignalFilenameSelectButton.Size = new System.Drawing.Size(130, 31);
            this.OrignalFilenameSelectButton.TabIndex = 130;
            this.OrignalFilenameSelectButton.Text = "別ファイル選択";
            this.OrignalFilenameSelectButton.UseVisualStyleBackColor = true;
            this.OrignalFilenameSelectButton.Click += new System.EventHandler(this.OrignalFilenameSelectButton_Click);
            // 
            // OrignalFilename
            // 
            this.OrignalFilename.ErrorMessage = "";
            this.OrignalFilename.Location = new System.Drawing.Point(407, 49);
            this.OrignalFilename.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalFilename.Name = "OrignalFilename";
            this.OrignalFilename.Placeholder = "";
            this.OrignalFilename.Size = new System.Drawing.Size(641, 25);
            this.OrignalFilename.TabIndex = 131;
            this.OrignalFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OrignalFilename_MouseDoubleClick);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(11, 89);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(254, 31);
            this.label6.TabIndex = 134;
            this.label6.Text = "パーツの大きさ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(11, 129);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 31);
            this.label1.TabIndex = 135;
            this.label1.Text = "変換先";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(11, 169);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(254, 31);
            this.label3.TabIndex = 136;
            this.label3.Text = "発動時効果音";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(11, 285);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(254, 31);
            this.label4.TabIndex = 137;
            this.label4.Text = "リサイズ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(11, 325);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(254, 31);
            this.label5.TabIndex = 138;
            this.label5.Text = "開始位置";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConvertType
            // 
            this.ConvertType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConvertType.FormattingEnabled = true;
            this.ConvertType.Items.AddRange(new object[] {
            "クラススキル",
            "クラススキル(FE8U:防衛スキル)",
            "魔法(FEditor) (画面中央)",
            "魔法(CSA) (画面中央)",
            "魔法(FEditor) (ユニットの位置)",
            "魔法(CSA) (ユニットの位置)"});
            this.ConvertType.Location = new System.Drawing.Point(271, 132);
            this.ConvertType.Name = "ConvertType";
            this.ConvertType.Size = new System.Drawing.Size(779, 26);
            this.ConvertType.TabIndex = 139;
            this.ConvertType.SelectedIndexChanged += new System.EventHandler(this.ConvertType_SelectedIndexChanged);
            // 
            // PartsX
            // 
            this.PartsX.Location = new System.Drawing.Point(315, 92);
            this.PartsX.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.PartsX.Name = "PartsX";
            this.PartsX.Size = new System.Drawing.Size(84, 25);
            this.PartsX.TabIndex = 140;
            this.PartsX.Value = new decimal(new int[] {
            192,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(271, 86);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 31);
            this.label7.TabIndex = 141;
            this.label7.Text = "X";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(407, 86);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 31);
            this.label8.TabIndex = 143;
            this.label8.Text = "Y";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PartsY
            // 
            this.PartsY.Location = new System.Drawing.Point(458, 92);
            this.PartsY.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.PartsY.Name = "PartsY";
            this.PartsY.Size = new System.Drawing.Size(84, 25);
            this.PartsY.TabIndex = 142;
            this.PartsY.Value = new decimal(new int[] {
            192,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(407, 282);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 31);
            this.label9.TabIndex = 147;
            this.label9.Text = "H";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResizeH
            // 
            this.ResizeH.Location = new System.Drawing.Point(457, 288);
            this.ResizeH.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.ResizeH.Name = "ResizeH";
            this.ResizeH.Size = new System.Drawing.Size(84, 25);
            this.ResizeH.TabIndex = 146;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(271, 282);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 31);
            this.label10.TabIndex = 145;
            this.label10.Text = "W";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResizeW
            // 
            this.ResizeW.Location = new System.Drawing.Point(315, 288);
            this.ResizeW.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.ResizeW.Name = "ResizeW";
            this.ResizeW.Size = new System.Drawing.Size(84, 25);
            this.ResizeW.TabIndex = 144;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(407, 325);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 31);
            this.label11.TabIndex = 151;
            this.label11.Text = "Y";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResizeY
            // 
            this.ResizeY.Location = new System.Drawing.Point(457, 331);
            this.ResizeY.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.ResizeY.Name = "ResizeY";
            this.ResizeY.Size = new System.Drawing.Size(84, 25);
            this.ResizeY.TabIndex = 150;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Location = new System.Drawing.Point(271, 325);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 31);
            this.label12.TabIndex = 149;
            this.label12.Text = "X";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResizeX
            // 
            this.ResizeX.Location = new System.Drawing.Point(315, 331);
            this.ResizeX.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.ResizeX.Name = "ResizeX";
            this.ResizeX.Size = new System.Drawing.Size(84, 25);
            this.ResizeX.TabIndex = 148;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(11, 371);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(531, 37);
            this.RunButton.TabIndex = 152;
            this.RunButton.Text = "生成開始";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // SoundFirePlaySoundButton
            // 
            this.SoundFirePlaySoundButton.Location = new System.Drawing.Point(511, 170);
            this.SoundFirePlaySoundButton.Margin = new System.Windows.Forms.Padding(2);
            this.SoundFirePlaySoundButton.Name = "SoundFirePlaySoundButton";
            this.SoundFirePlaySoundButton.Size = new System.Drawing.Size(31, 28);
            this.SoundFirePlaySoundButton.TabIndex = 156;
            this.SoundFirePlaySoundButton.Text = "♪";
            this.SoundFirePlaySoundButton.UseVisualStyleBackColor = true;
            // 
            // SoundFire
            // 
            this.SoundFire.Hexadecimal = true;
            this.SoundFire.Location = new System.Drawing.Point(271, 173);
            this.SoundFire.Margin = new System.Windows.Forms.Padding(2);
            this.SoundFire.Maximum = new decimal(new int[] {
            16777216,
            0,
            0,
            0});
            this.SoundFire.Name = "SoundFire";
            this.SoundFire.Size = new System.Drawing.Size(69, 25);
            this.SoundFire.TabIndex = 155;
            // 
            // SoundFireInfo
            // 
            this.SoundFireInfo.ErrorMessage = "";
            this.SoundFireInfo.Location = new System.Drawing.Point(346, 172);
            this.SoundFireInfo.Margin = new System.Windows.Forms.Padding(4);
            this.SoundFireInfo.Name = "SoundFireInfo";
            this.SoundFireInfo.Placeholder = "";
            this.SoundFireInfo.Size = new System.Drawing.Size(159, 25);
            this.SoundFireInfo.TabIndex = 157;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(11, 246);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(254, 31);
            this.label14.TabIndex = 158;
            this.label14.Text = "フレーム間隔";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrameWait
            // 
            this.FrameWait.Location = new System.Drawing.Point(271, 250);
            this.FrameWait.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.FrameWait.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FrameWait.Name = "FrameWait";
            this.FrameWait.Size = new System.Drawing.Size(84, 25);
            this.FrameWait.TabIndex = 159;
            this.FrameWait.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(372, 256);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 18);
            this.label15.TabIndex = 160;
            this.label15.Text = "1/60秒";
            // 
            // SoundHitInfo
            // 
            this.SoundHitInfo.ErrorMessage = "";
            this.SoundHitInfo.Location = new System.Drawing.Point(346, 209);
            this.SoundHitInfo.Margin = new System.Windows.Forms.Padding(4);
            this.SoundHitInfo.Name = "SoundHitInfo";
            this.SoundHitInfo.Placeholder = "";
            this.SoundHitInfo.Size = new System.Drawing.Size(159, 25);
            this.SoundHitInfo.TabIndex = 164;
            // 
            // SoundHitPlaySoundButton
            // 
            this.SoundHitPlaySoundButton.Location = new System.Drawing.Point(511, 207);
            this.SoundHitPlaySoundButton.Margin = new System.Windows.Forms.Padding(2);
            this.SoundHitPlaySoundButton.Name = "SoundHitPlaySoundButton";
            this.SoundHitPlaySoundButton.Size = new System.Drawing.Size(31, 28);
            this.SoundHitPlaySoundButton.TabIndex = 163;
            this.SoundHitPlaySoundButton.Text = "♪";
            this.SoundHitPlaySoundButton.UseVisualStyleBackColor = true;
            // 
            // SoundHit
            // 
            this.SoundHit.Hexadecimal = true;
            this.SoundHit.Location = new System.Drawing.Point(271, 210);
            this.SoundHit.Margin = new System.Windows.Forms.Padding(2);
            this.SoundHit.Maximum = new decimal(new int[] {
            16777216,
            0,
            0,
            0});
            this.SoundHit.Name = "SoundHit";
            this.SoundHit.Size = new System.Drawing.Size(69, 25);
            this.SoundHit.TabIndex = 162;
            // 
            // SoundHitLabel
            // 
            this.SoundHitLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SoundHitLabel.Location = new System.Drawing.Point(11, 206);
            this.SoundHitLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SoundHitLabel.Name = "SoundHitLabel";
            this.SoundHitLabel.Size = new System.Drawing.Size(254, 31);
            this.SoundHitLabel.TabIndex = 161;
            this.SoundHitLabel.Text = "魔法の命中音";
            this.SoundHitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToolMagicEffectMakerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1062, 451);
            this.Controls.Add(this.SoundHitInfo);
            this.Controls.Add(this.SoundHitPlaySoundButton);
            this.Controls.Add(this.SoundHit);
            this.Controls.Add(this.SoundHitLabel);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.FrameWait);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.SoundFireInfo);
            this.Controls.Add(this.SoundFirePlaySoundButton);
            this.Controls.Add(this.SoundFire);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ResizeY);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ResizeX);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ResizeH);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ResizeW);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.PartsY);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.PartsX);
            this.Controls.Add(this.ConvertType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OrignalFilenameSelectButton);
            this.Controls.Add(this.OrignalFilename);
            this.Name = "ToolMagicEffectMakerForm";
            this.Text = "魔法エフェクトの変換ツール";
            this.Load += new System.EventHandler(this.ToolMagicEffectMakerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PartsX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PartsY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SoundFire)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SoundHit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OrignalFilenameSelectButton;
        private TextBoxEx OrignalFilename;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ConvertType;
        private System.Windows.Forms.NumericUpDown PartsX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown PartsY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ResizeH;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown ResizeW;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown ResizeY;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown ResizeX;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button SoundFirePlaySoundButton;
        private System.Windows.Forms.NumericUpDown SoundFire;
        private TextBoxEx SoundFireInfo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown FrameWait;
        private System.Windows.Forms.Label label15;
        private TextBoxEx SoundHitInfo;
        private System.Windows.Forms.Button SoundHitPlaySoundButton;
        private System.Windows.Forms.NumericUpDown SoundHit;
        private System.Windows.Forms.Label SoundHitLabel;

    }
}