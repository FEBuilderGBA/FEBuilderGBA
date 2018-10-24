namespace FEBuilderGBA
{
    partial class TextToSpeechForm
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
            this.StartButton = new System.Windows.Forms.Button();
            this.EndButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Rate = new System.Windows.Forms.NumericUpDown();
            this.ShortNumLabel = new System.Windows.Forms.Label();
            this.ShortNum = new System.Windows.Forms.NumericUpDown();
            this.VoiceComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IconPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Rate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(396, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "合成音声エンジンで自動的にテキストを読み上げます。\r\nタイプミスを見つけるには音読するのが一番です。";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(16, 128);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(338, 57);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "読み上げ開始";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // EndButton
            // 
            this.EndButton.Location = new System.Drawing.Point(413, 128);
            this.EndButton.Name = "EndButton";
            this.EndButton.Size = new System.Drawing.Size(302, 57);
            this.EndButton.TabIndex = 2;
            this.EndButton.Text = "読み上げ停止";
            this.EndButton.UseVisualStyleBackColor = true;
            this.EndButton.Click += new System.EventHandler(this.EndButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 286);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "読み上げ速度";
            // 
            // Rate
            // 
            this.Rate.Location = new System.Drawing.Point(199, 284);
            this.Rate.Name = "Rate";
            this.Rate.Size = new System.Drawing.Size(120, 25);
            this.Rate.TabIndex = 5;
            this.Rate.ValueChanged += new System.EventHandler(this.Rate_ValueChanged);
            // 
            // ShortNumLabel
            // 
            this.ShortNumLabel.AutoSize = true;
            this.ShortNumLabel.Location = new System.Drawing.Point(350, 287);
            this.ShortNumLabel.Name = "ShortNumLabel";
            this.ShortNumLabel.Size = new System.Drawing.Size(278, 18);
            this.ShortNumLabel.TabIndex = 7;
            this.ShortNumLabel.Text = "このサイズ以上の文字列を読み上げる";
            // 
            // ShortNum
            // 
            this.ShortNum.Location = new System.Drawing.Point(705, 280);
            this.ShortNum.Name = "ShortNum";
            this.ShortNum.Size = new System.Drawing.Size(120, 25);
            this.ShortNum.TabIndex = 8;
            this.ShortNum.ValueChanged += new System.EventHandler(this.ShortNum_ValueChanged);
            // 
            // VoiceComboBox
            // 
            this.VoiceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VoiceComboBox.FormattingEnabled = true;
            this.VoiceComboBox.Location = new System.Drawing.Point(17, 241);
            this.VoiceComboBox.Name = "VoiceComboBox";
            this.VoiceComboBox.Size = new System.Drawing.Size(808, 26);
            this.VoiceComboBox.TabIndex = 10;
            this.VoiceComboBox.SelectedIndexChanged += new System.EventHandler(this.VoiceComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 18);
            this.label2.TabIndex = 11;
            this.label2.Text = "読み上げエンジン";
            // 
            // IconPictureBox
            // 
            this.IconPictureBox.Location = new System.Drawing.Point(25, 12);
            this.IconPictureBox.Name = "IconPictureBox";
            this.IconPictureBox.Size = new System.Drawing.Size(100, 100);
            this.IconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.IconPictureBox.TabIndex = 12;
            this.IconPictureBox.TabStop = false;
            // 
            // TextToSpeechForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 362);
            this.Controls.Add(this.IconPictureBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.VoiceComboBox);
            this.Controls.Add(this.ShortNum);
            this.Controls.Add(this.ShortNumLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Rate);
            this.Controls.Add(this.EndButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label1);
            this.Name = "TextToSpeechForm";
            this.Text = "読み上げ設定";
            this.Load += new System.EventHandler(this.TextToSpeechForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Rate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShortNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button EndButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Rate;
        private System.Windows.Forms.Label ShortNumLabel;
        private System.Windows.Forms.NumericUpDown ShortNum;
        private System.Windows.Forms.ComboBox VoiceComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox IconPictureBox;
    }
}