namespace FEBuilderGBA
{
    partial class SongTrackImportWaveForm
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
            this.Volume = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Chunnel = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PreviewResult = new System.Windows.Forms.TextBox();
            this.Preview_button = new System.Windows.Forms.Button();
            this.Strip = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.HZ = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.LoopComboBox = new System.Windows.Forms.ComboBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.J_4_INSTRUMENT_ADDR = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Volume);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.Chunnel);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.PreviewResult);
            this.panel1.Controls.Add(this.Preview_button);
            this.panel1.Controls.Add(this.Strip);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.HZ);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.MyCancelButton);
            this.panel1.Controls.Add(this.LoopComboBox);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.J_4_INSTRUMENT_ADDR);
            this.panel1.Location = new System.Drawing.Point(10, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(672, 611);
            this.panel1.TabIndex = 0;
            // 
            // Volume
            // 
            this.Volume.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Volume.FormattingEnabled = true;
            this.Volume.Items.AddRange(new object[] {
            "0=変更しない",
            "120=120%",
            "130=130%",
            "150=150%",
            "170=170%",
            "200=200%",
            "230=230%",
            "250=250%",
            "270=270%",
            "300=300%",
            "320=320%",
            "340=340%",
            "360=360%",
            "380=380%",
            "400=400%",
            "420=420%",
            "440=440%",
            "460=460%",
            "480=480%",
            "500=500%"});
            this.Volume.Location = new System.Drawing.Point(211, 222);
            this.Volume.Name = "Volume";
            this.Volume.Size = new System.Drawing.Size(449, 26);
            this.Volume.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 230);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 18);
            this.label7.TabIndex = 199;
            this.label7.Text = "音量";
            // 
            // Chunnel
            // 
            this.Chunnel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Chunnel.FormattingEnabled = true;
            this.Chunnel.Items.AddRange(new object[] {
            "0=変更しない",
            "1=8bit mono"});
            this.Chunnel.Location = new System.Drawing.Point(211, 186);
            this.Chunnel.Name = "Chunnel";
            this.Chunnel.Size = new System.Drawing.Size(449, 26);
            this.Chunnel.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 194);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 18);
            this.label6.TabIndex = 197;
            this.label6.Text = "チャンネル";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(19, 414);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(641, 76);
            this.textBox2.TabIndex = 196;
            this.textBox2.Text = "歌などを入れる場合は、ループが必要です。\r\n楽器データの場合は不要です。\r\nインポート後に、楽譜データから変更することもできます。";
            // 
            // label5
            // 
            this.label5.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 384);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 18);
            this.label5.TabIndex = 190;
            this.label5.Text = "楽譜のループ";
            // 
            // PreviewResult
            // 
            this.PreviewResult.Location = new System.Drawing.Point(19, 275);
            this.PreviewResult.Name = "PreviewResult";
            this.PreviewResult.ReadOnly = true;
            this.PreviewResult.Size = new System.Drawing.Size(497, 25);
            this.PreviewResult.TabIndex = 189;
            this.PreviewResult.DoubleClick += new System.EventHandler(this.PreviewResult_DoubleClick);
            // 
            // Preview_button
            // 
            this.Preview_button.Location = new System.Drawing.Point(532, 271);
            this.Preview_button.Name = "Preview_button";
            this.Preview_button.Size = new System.Drawing.Size(128, 32);
            this.Preview_button.TabIndex = 4;
            this.Preview_button.Text = "Preview";
            this.Preview_button.UseVisualStyleBackColor = true;
            this.Preview_button.Click += new System.EventHandler(this.Preview_button_Click);
            // 
            // Strip
            // 
            this.Strip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Strip.FormattingEnabled = true;
            this.Strip.Items.AddRange(new object[] {
            "0=除去しない",
            "1=無音を除去する",
            "2=もっと強力に無音を除去する"});
            this.Strip.Location = new System.Drawing.Point(211, 147);
            this.Strip.Name = "Strip";
            this.Strip.Size = new System.Drawing.Size(449, 26);
            this.Strip.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 18);
            this.label4.TabIndex = 186;
            this.label4.Text = "前後の無音除去";
            // 
            // HZ
            // 
            this.HZ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HZ.FormattingEnabled = true;
            this.HZ.Items.AddRange(new object[] {
            "0=変更しない",
            "8000=8000Hz",
            "11025=11025Hz",
            "12000=12000Hz",
            "16000=16000Hz",
            "22050=22050Hz",
            "24000=24000Hz"});
            this.HZ.Location = new System.Drawing.Point(211, 111);
            this.HZ.Name = "HZ";
            this.HZ.Size = new System.Drawing.Size(449, 26);
            this.HZ.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 18);
            this.label3.TabIndex = 184;
            this.label3.Text = "音質を下げる";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(17, 77);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(374, 26);
            this.label2.TabIndex = 183;
            this.label2.Text = "最適化";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(646, 43);
            this.label1.TabIndex = 182;
            this.label1.Text = "Waveファイルは効果音に使うことを想定しています。\r\nそれを、音楽に利用すると、大量に容量を消費しますが、インポートしてもよろしいですか？";
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(19, 520);
            this.MyCancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(242, 28);
            this.MyCancelButton.TabIndex = 6;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            // 
            // LoopComboBox
            // 
            this.LoopComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LoopComboBox.FormattingEnabled = true;
            this.LoopComboBox.Items.AddRange(new object[] {
            "ループしない",
            "ループする"});
            this.LoopComboBox.Location = new System.Drawing.Point(211, 376);
            this.LoopComboBox.Name = "LoopComboBox";
            this.LoopComboBox.Size = new System.Drawing.Size(449, 26);
            this.LoopComboBox.TabIndex = 5;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(379, 520);
            this.OKButton.Margin = new System.Windows.Forms.Padding(4);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(283, 28);
            this.OKButton.TabIndex = 7;
            this.OKButton.Text = "インポート";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // J_4_INSTRUMENT_ADDR
            // 
            this.J_4_INSTRUMENT_ADDR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_INSTRUMENT_ADDR.Location = new System.Drawing.Point(17, 340);
            this.J_4_INSTRUMENT_ADDR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_INSTRUMENT_ADDR.Name = "J_4_INSTRUMENT_ADDR";
            this.J_4_INSTRUMENT_ADDR.Size = new System.Drawing.Size(374, 26);
            this.J_4_INSTRUMENT_ADDR.TabIndex = 178;
            this.J_4_INSTRUMENT_ADDR.Text = "楽譜";
            this.J_4_INSTRUMENT_ADDR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SongTrackImportWaveForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(696, 628);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SongTrackImportWaveForm";
            this.Text = "Waveのインポート";
            this.Load += new System.EventHandler(this.SongTrackImportWaveForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.ComboBox LoopComboBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label J_4_INSTRUMENT_ADDR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox HZ;
        private System.Windows.Forms.ComboBox Strip;
        private System.Windows.Forms.Button Preview_button;
        private System.Windows.Forms.TextBox PreviewResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox Chunnel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox Volume;
        private System.Windows.Forms.Label label7;
    }
}