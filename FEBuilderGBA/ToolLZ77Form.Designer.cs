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
            this.groupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.DeCompressDESTFilename = new FEBuilderGBA.TextBoxEx();
            this.DeCompressDESTSelectButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DeCompressFireButton = new System.Windows.Forms.Button();
            this.DeCompressAddress = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.DeCompressSRCFilename = new FEBuilderGBA.TextBoxEx();
            this.DeCompressSRCSelectButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new FEBuilderGBA.CustomColorGroupBox();
            this.CompressDESTFilename = new FEBuilderGBA.TextBoxEx();
            this.CompressDESTSelectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.CompressFireButton = new System.Windows.Forms.Button();
            this.CompressSRCFilename = new FEBuilderGBA.TextBoxEx();
            this.CompressSRCSelectButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeCompressAddress)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BorderColor = System.Drawing.Color.Empty;
            this.groupBox1.Controls.Add(this.DeCompressDESTFilename);
            this.groupBox1.Controls.Add(this.DeCompressDESTSelectButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.DeCompressFireButton);
            this.groupBox1.Controls.Add(this.DeCompressAddress);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.DeCompressSRCFilename);
            this.groupBox1.Controls.Add(this.DeCompressSRCSelectButton);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(16, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(670, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "解凍する";
            // 
            // DeCompressDESTFilename
            // 
            this.DeCompressDESTFilename.Location = new System.Drawing.Point(310, 88);
            this.DeCompressDESTFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DeCompressDESTFilename.Name = "DeCompressDESTFilename";
            this.DeCompressDESTFilename.Size = new System.Drawing.Size(346, 25);
            this.DeCompressDESTFilename.TabIndex = 97;
            this.DeCompressDESTFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DeCompressDESTFilename_MouseDoubleClick);
            // 
            // DeCompressDESTSelectButton
            // 
            this.DeCompressDESTSelectButton.Location = new System.Drawing.Point(172, 85);
            this.DeCompressDESTSelectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.label4.Location = new System.Drawing.Point(7, 83);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 31);
            this.label4.TabIndex = 95;
            this.label4.Text = "DEST";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DeCompressFireButton
            // 
            this.DeCompressFireButton.Location = new System.Drawing.Point(172, 132);
            this.DeCompressFireButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.DeCompressAddress.Location = new System.Drawing.Point(172, 58);
            this.DeCompressAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.label1.Location = new System.Drawing.Point(7, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 31);
            this.label1.TabIndex = 88;
            this.label1.Text = "SRC開始アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DeCompressSRCFilename
            // 
            this.DeCompressSRCFilename.Location = new System.Drawing.Point(310, 24);
            this.DeCompressSRCFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DeCompressSRCFilename.Name = "DeCompressSRCFilename";
            this.DeCompressSRCFilename.Size = new System.Drawing.Size(346, 25);
            this.DeCompressSRCFilename.TabIndex = 87;
            this.DeCompressSRCFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DeCompressSRCFilename_MouseDoubleClick);
            // 
            // DeCompressSRCSelectButton
            // 
            this.DeCompressSRCSelectButton.Location = new System.Drawing.Point(172, 20);
            this.DeCompressSRCSelectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.label9.Location = new System.Drawing.Point(7, 23);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 31);
            this.label9.TabIndex = 85;
            this.label9.Text = "SRC";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.BorderColor = System.Drawing.Color.Empty;
            this.groupBox2.Controls.Add(this.CompressDESTFilename);
            this.groupBox2.Controls.Add(this.CompressDESTSelectButton);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.CompressFireButton);
            this.groupBox2.Controls.Add(this.CompressSRCFilename);
            this.groupBox2.Controls.Add(this.CompressSRCSelectButton);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(14, 196);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(670, 156);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "圧縮する";
            // 
            // CompressDESTFilename
            // 
            this.CompressDESTFilename.Location = new System.Drawing.Point(310, 55);
            this.CompressDESTFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CompressDESTFilename.Name = "CompressDESTFilename";
            this.CompressDESTFilename.Size = new System.Drawing.Size(346, 25);
            this.CompressDESTFilename.TabIndex = 94;
            this.CompressDESTFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CompressDESTFilename_MouseDoubleClick);
            // 
            // CompressDESTSelectButton
            // 
            this.CompressDESTSelectButton.Location = new System.Drawing.Point(172, 52);
            this.CompressDESTSelectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.label3.Location = new System.Drawing.Point(7, 54);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 31);
            this.label3.TabIndex = 92;
            this.label3.Text = "DEST";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressFireButton
            // 
            this.CompressFireButton.Location = new System.Drawing.Point(172, 96);
            this.CompressFireButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CompressFireButton.Name = "CompressFireButton";
            this.CompressFireButton.Size = new System.Drawing.Size(462, 31);
            this.CompressFireButton.TabIndex = 91;
            this.CompressFireButton.Text = "圧縮する";
            this.CompressFireButton.UseVisualStyleBackColor = true;
            this.CompressFireButton.Click += new System.EventHandler(this.CompressFireButton_Click);
            // 
            // CompressSRCFilename
            // 
            this.CompressSRCFilename.Location = new System.Drawing.Point(310, 25);
            this.CompressSRCFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CompressSRCFilename.Name = "CompressSRCFilename";
            this.CompressSRCFilename.Size = new System.Drawing.Size(346, 25);
            this.CompressSRCFilename.TabIndex = 90;
            this.CompressSRCFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CompressSRCFilename_MouseDoubleClick);
            // 
            // CompressSRCSelectButton
            // 
            this.CompressSRCSelectButton.Location = new System.Drawing.Point(172, 32);
            this.CompressSRCSelectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.label2.Location = new System.Drawing.Point(7, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 31);
            this.label2.TabIndex = 88;
            this.label2.Text = "SRC";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LZ77ToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(698, 366);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LZ77ToolForm";
            this.Text = "LZ77ツール";
            this.Load += new System.EventHandler(this.LZ77ToolForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeCompressAddress)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private CustomColorGroupBox groupBox1;
        private CustomColorGroupBox groupBox2;

    }
}