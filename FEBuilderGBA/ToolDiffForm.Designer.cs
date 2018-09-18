namespace FEBuilderGBA
{
    partial class ToolDiffForm
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
            this.OtherFilename = new FEBuilderGBA.TextBoxEx();
            this.OtherSelectButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.RecoverMissMatchNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.MakeBinPatchButton = new System.Windows.Forms.Button();
            this.groupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.groupBox2 = new FEBuilderGBA.CustomColorGroupBox();
            this.RecoverMissMatchDiff3NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.Diff3Method = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.MakeBinPatch3Button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BFilename = new FEBuilderGBA.TextBoxEx();
            this.BFileSelectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.AFilename = new FEBuilderGBA.TextBoxEx();
            this.AFileSelectButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RecoverMissMatchNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RecoverMissMatchDiff3NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OtherFilename
            // 
            this.OtherFilename.Location = new System.Drawing.Point(450, 30);
            this.OtherFilename.Margin = new System.Windows.Forms.Padding(4);
            this.OtherFilename.Name = "OtherFilename";
            this.OtherFilename.Size = new System.Drawing.Size(346, 25);
            this.OtherFilename.TabIndex = 90;
            this.OtherFilename.TextChanged += new System.EventHandler(this.OtherFilename_TextChanged);
            this.OtherFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherFilename_MouseDoubleClick);
            // 
            // OtherSelectButton
            // 
            this.OtherSelectButton.Location = new System.Drawing.Point(312, 27);
            this.OtherSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.OtherSelectButton.Name = "OtherSelectButton";
            this.OtherSelectButton.Size = new System.Drawing.Size(130, 31);
            this.OtherSelectButton.TabIndex = 89;
            this.OtherSelectButton.Text = "別ファイル選択";
            this.OtherSelectButton.UseVisualStyleBackColor = true;
            this.OtherSelectButton.Click += new System.EventHandler(this.OtherSelectButton_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(7, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 31);
            this.label2.TabIndex = 88;
            this.label2.Text = "比較ファイル";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // RecoverMissMatchNumericUpDown
            // 

            this.RecoverMissMatchNumericUpDown.Location = new System.Drawing.Point(312, 66);
            this.RecoverMissMatchNumericUpDown.Name = "RecoverMissMatchNumericUpDown";
            this.RecoverMissMatchNumericUpDown.Size = new System.Drawing.Size(120, 25);
            this.RecoverMissMatchNumericUpDown.TabIndex = 93;
            this.RecoverMissMatchNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(7, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 31);
            this.label1.TabIndex = 92;
            this.label1.Text = "容認差異";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MakeBinPatchButton
            // 
            this.MakeBinPatchButton.Location = new System.Drawing.Point(305, 135);
            this.MakeBinPatchButton.Name = "MakeBinPatchButton";
            this.MakeBinPatchButton.Size = new System.Drawing.Size(491, 32);
            this.MakeBinPatchButton.TabIndex = 91;
            this.MakeBinPatchButton.Text = "差分をBINパッチとして作成する";
            this.MakeBinPatchButton.UseVisualStyleBackColor = true;
            this.MakeBinPatchButton.Click += new System.EventHandler(this.MakeBinPatchButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BorderColor = System.Drawing.Color.Empty;
            this.groupBox1.Controls.Add(this.RecoverMissMatchNumericUpDown);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.OtherFilename);
            this.groupBox1.Controls.Add(this.MakeBinPatchButton);
            this.groupBox1.Controls.Add(this.OtherSelectButton);
            this.groupBox1.Location = new System.Drawing.Point(16, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(826, 170);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "バイナリ差分";
            // 
            // groupBox2
            // 
            this.groupBox2.BorderColor = System.Drawing.Color.Empty;
            this.groupBox2.Controls.Add(this.RecoverMissMatchDiff3NumericUpDown);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.Diff3Method);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.MakeBinPatch3Button);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.BFilename);
            this.groupBox2.Controls.Add(this.BFileSelectButton);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.AFilename);
            this.groupBox2.Controls.Add(this.AFileSelectButton);
            this.groupBox2.Location = new System.Drawing.Point(16, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(826, 251);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "3者差分";
            // 
            // RecoverMissMatchDiff3NumericUpDown
            // 

            this.RecoverMissMatchDiff3NumericUpDown.Location = new System.Drawing.Point(312, 109);
            this.RecoverMissMatchDiff3NumericUpDown.Name = "RecoverMissMatchDiff3NumericUpDown";
            this.RecoverMissMatchDiff3NumericUpDown.Size = new System.Drawing.Size(120, 25);
            this.RecoverMissMatchDiff3NumericUpDown.TabIndex = 101;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(7, 103);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(296, 31);
            this.label6.TabIndex = 100;
            this.label6.Text = "容認差異";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Diff3Method
            // 
            this.Diff3Method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Diff3Method.FormattingEnabled = true;
            this.Diff3Method.Items.AddRange(new object[] {
            "AとBにあって、自分にだけないもの"});
            this.Diff3Method.Location = new System.Drawing.Point(312, 142);
            this.Diff3Method.Name = "Diff3Method";
            this.Diff3Method.Size = new System.Drawing.Size(484, 26);
            this.Diff3Method.TabIndex = 99;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(7, 139);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(296, 31);
            this.label5.TabIndex = 98;
            this.label5.Text = "比較方法";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MakeBinPatch3Button
            // 
            this.MakeBinPatch3Button.Location = new System.Drawing.Point(305, 198);
            this.MakeBinPatch3Button.Name = "MakeBinPatch3Button";
            this.MakeBinPatch3Button.Size = new System.Drawing.Size(491, 32);
            this.MakeBinPatch3Button.TabIndex = 97;
            this.MakeBinPatch3Button.Text = "差分をBINパッチとして作成する";
            this.MakeBinPatch3Button.UseVisualStyleBackColor = true;
            this.MakeBinPatch3Button.Click += new System.EventHandler(this.MakeBinPatch3Button_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(7, 68);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(296, 31);
            this.label4.TabIndex = 94;
            this.label4.Text = "比較ファイルB";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BFilename
            // 
            this.BFilename.Location = new System.Drawing.Point(450, 70);
            this.BFilename.Margin = new System.Windows.Forms.Padding(4);
            this.BFilename.Name = "BFilename";
            this.BFilename.Size = new System.Drawing.Size(346, 25);
            this.BFilename.TabIndex = 96;
            this.BFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BFilename_MouseDoubleClick);
            // 
            // BFileSelectButton
            // 
            this.BFileSelectButton.Location = new System.Drawing.Point(312, 67);
            this.BFileSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.BFileSelectButton.Name = "BFileSelectButton";
            this.BFileSelectButton.Size = new System.Drawing.Size(130, 31);
            this.BFileSelectButton.TabIndex = 95;
            this.BFileSelectButton.Text = "別ファイル選択";
            this.BFileSelectButton.UseVisualStyleBackColor = true;
            this.BFileSelectButton.Click += new System.EventHandler(this.BFileSelectButton_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(7, 34);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(296, 31);
            this.label3.TabIndex = 91;
            this.label3.Text = "比較ファイルA";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AFilename
            // 
            this.AFilename.Location = new System.Drawing.Point(450, 36);
            this.AFilename.Margin = new System.Windows.Forms.Padding(4);
            this.AFilename.Name = "AFilename";
            this.AFilename.Size = new System.Drawing.Size(346, 25);
            this.AFilename.TabIndex = 93;
            this.AFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AFilename_MouseDoubleClick);
            // 
            // AFileSelectButton
            // 
            this.AFileSelectButton.Location = new System.Drawing.Point(312, 33);
            this.AFileSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.AFileSelectButton.Name = "AFileSelectButton";
            this.AFileSelectButton.Size = new System.Drawing.Size(130, 31);
            this.AFileSelectButton.TabIndex = 92;
            this.AFileSelectButton.Text = "別ファイル選択";
            this.AFileSelectButton.UseVisualStyleBackColor = true;
            this.AFileSelectButton.Click += new System.EventHandler(this.AFileSelectButton_Click);
            // 
            // DiffToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(854, 533);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DiffToolForm";
            this.Text = "DIFFツール";
            this.Load += new System.EventHandler(this.DiffToolForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RecoverMissMatchNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RecoverMissMatchDiff3NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FEBuilderGBA.TextBoxEx OtherFilename;
        private System.Windows.Forms.Button OtherSelectButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button MakeBinPatchButton;
        private System.Windows.Forms.NumericUpDown RecoverMissMatchNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button MakeBinPatch3Button;
        private System.Windows.Forms.Label label4;
        private FEBuilderGBA.TextBoxEx BFilename;
        private System.Windows.Forms.Button BFileSelectButton;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx AFilename;
        private System.Windows.Forms.Button AFileSelectButton;
        private System.Windows.Forms.ComboBox Diff3Method;
        private System.Windows.Forms.NumericUpDown RecoverMissMatchDiff3NumericUpDown;
        private System.Windows.Forms.Label label6;
        private CustomColorGroupBox groupBox1;
        private CustomColorGroupBox groupBox2;
    }
}