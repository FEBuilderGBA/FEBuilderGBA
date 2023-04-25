namespace FEBuilderGBA
{
    partial class SongInstrumentImportWaveForm
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
            this.NO_DPCM_PATCH = new System.Windows.Forms.Label();
            this.DPCMLookahead = new System.Windows.Forms.ComboBox();
            this.LABEL_DPCM_LOOKAHEAD = new System.Windows.Forms.Label();
            this.UseDPCMCompress = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.HZ = new System.Windows.Forms.ComboBox();
            this.Volume = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.Strip = new System.Windows.Forms.ComboBox();
            this.PreviewResult = new System.Windows.Forms.TextBox();
            this.Chunnel = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Preview_button = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.NO_DPCM_PATCH);
            this.panel1.Controls.Add(this.DPCMLookahead);
            this.panel1.Controls.Add(this.LABEL_DPCM_LOOKAHEAD);
            this.panel1.Controls.Add(this.UseDPCMCompress);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.HZ);
            this.panel1.Controls.Add(this.Volume);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.MyCancelButton);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.Strip);
            this.panel1.Controls.Add(this.PreviewResult);
            this.panel1.Controls.Add(this.Chunnel);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.Preview_button);
            this.panel1.Location = new System.Drawing.Point(10, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(694, 426);
            this.panel1.TabIndex = 0;
            // 
            // NO_DPCM_PATCH
            // 
            this.NO_DPCM_PATCH.AccessibleDescription = "";
            this.NO_DPCM_PATCH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NO_DPCM_PATCH.Location = new System.Drawing.Point(214, 196);
            this.NO_DPCM_PATCH.Name = "NO_DPCM_PATCH";
            this.NO_DPCM_PATCH.Size = new System.Drawing.Size(427, 70);
            this.NO_DPCM_PATCH.TabIndex = 204;
            this.NO_DPCM_PATCH.Text = "m4a_hq_mixer Patchがインストールされていないので、DPCM圧縮は利用できません。";
            // 
            // DPCMLookahead
            // 
            this.DPCMLookahead.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DPCMLookahead.FormattingEnabled = true;
            this.DPCMLookahead.Items.AddRange(new object[] {
            "1=1(Poor Quality)",
            "2=2",
            "3=3(Default)",
            "4=4",
            "5=5(Very Slow)"});
            this.DPCMLookahead.Location = new System.Drawing.Point(214, 237);
            this.DPCMLookahead.Name = "DPCMLookahead";
            this.DPCMLookahead.Size = new System.Drawing.Size(449, 26);
            this.DPCMLookahead.TabIndex = 5;
            // 
            // LABEL_DPCM_LOOKAHEAD
            // 
            this.LABEL_DPCM_LOOKAHEAD.AccessibleDescription = "DPCM圧縮処理時の先読みの値です。\\r\\nディフォルトは3です。\\r\\n値を大きくすると、より先読みをします。\\r\\nただし、圧縮率は一切変わりません。";
            this.LABEL_DPCM_LOOKAHEAD.AutoSize = true;
            this.LABEL_DPCM_LOOKAHEAD.Location = new System.Drawing.Point(17, 245);
            this.LABEL_DPCM_LOOKAHEAD.Name = "LABEL_DPCM_LOOKAHEAD";
            this.LABEL_DPCM_LOOKAHEAD.Size = new System.Drawing.Size(136, 18);
            this.LABEL_DPCM_LOOKAHEAD.TabIndex = 202;
            this.LABEL_DPCM_LOOKAHEAD.Text = "DPCM lookahead";
            // 
            // UseDPCMCompress
            // 
            this.UseDPCMCompress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UseDPCMCompress.FormattingEnabled = true;
            this.UseDPCMCompress.Items.AddRange(new object[] {
            "0=圧縮しない",
            "1=圧縮する"});
            this.UseDPCMCompress.Location = new System.Drawing.Point(214, 197);
            this.UseDPCMCompress.Name = "UseDPCMCompress";
            this.UseDPCMCompress.Size = new System.Drawing.Size(449, 26);
            this.UseDPCMCompress.TabIndex = 4;
            this.UseDPCMCompress.SelectedIndexChanged += new System.EventHandler(this.UseDPCMCompress_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AccessibleDescription = "m4a_hq_mixer patchを利用して、DPCM圧縮を行えます。\\r\\n圧縮すると容量がおよそ半分になります。\\r\\nただ、品質が極わずかですが低下します" +
    "。\\r\\nmidi楽器として使いたいWAVならばoffにしてください。\\r\\nそれ以外のボイスデータなどであれば、可能であれば有効にすることをお勧めします。";
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 18);
            this.label2.TabIndex = 200;
            this.label2.Text = "DPCM圧縮";
            // 
            // label3
            // 
            this.label3.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 18);
            this.label3.TabIndex = 184;
            this.label3.Text = "音質を下げる";
            // 
            // HZ
            // 
            this.HZ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HZ.FormattingEnabled = true;
            this.HZ.Items.AddRange(new object[] {
            "0=変更しない",
            "5734=5734Hz(M4A最低音質)",
            "6000=6000Hz",
            "7000=7000Hz",
            "8000=8000Hz(一般的なツールでの最低音質)",
            "11025=11025Hz",
            "12000=12000Hz",
            "13379=13379Hz(楽器でよく使われる音質)",
            "16000=16000Hz",
            "22050=22050Hz",
            "24000=24000Hz",
            "42048=42048Hz(M4A最高音質)"});
            this.HZ.Location = new System.Drawing.Point(214, 47);
            this.HZ.Name = "HZ";
            this.HZ.Size = new System.Drawing.Size(449, 26);
            this.HZ.TabIndex = 0;
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
            this.Volume.Location = new System.Drawing.Point(214, 158);
            this.Volume.Name = "Volume";
            this.Volume.Size = new System.Drawing.Size(449, 26);
            this.Volume.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(646, 59);
            this.label1.TabIndex = 182;
            this.label1.Text = "Waveは容量をたくさん消費するため、低音質に変換してインポートすることをお勧めします。";
            // 
            // label4
            // 
            this.label4.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 18);
            this.label4.TabIndex = 186;
            this.label4.Text = "前後の無音除去";
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(16, 369);
            this.MyCancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(242, 38);
            this.MyCancelButton.TabIndex = 8;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 18);
            this.label7.TabIndex = 199;
            this.label7.Text = "音量";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(376, 369);
            this.OKButton.Margin = new System.Windows.Forms.Padding(4);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(283, 38);
            this.OKButton.TabIndex = 7;
            this.OKButton.Text = "インポート";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // Strip
            // 
            this.Strip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Strip.FormattingEnabled = true;
            this.Strip.Items.AddRange(new object[] {
            "0=除去しない",
            "1=無音を除去する",
            "2=もっと強力に無音を除去する"});
            this.Strip.Location = new System.Drawing.Point(214, 83);
            this.Strip.Name = "Strip";
            this.Strip.Size = new System.Drawing.Size(449, 26);
            this.Strip.TabIndex = 1;
            // 
            // PreviewResult
            // 
            this.PreviewResult.Location = new System.Drawing.Point(22, 279);
            this.PreviewResult.Multiline = true;
            this.PreviewResult.Name = "PreviewResult";
            this.PreviewResult.ReadOnly = true;
            this.PreviewResult.Size = new System.Drawing.Size(497, 80);
            this.PreviewResult.TabIndex = 189;
            this.PreviewResult.DoubleClick += new System.EventHandler(this.PreviewResult_DoubleClick);
            // 
            // Chunnel
            // 
            this.Chunnel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Chunnel.FormattingEnabled = true;
            this.Chunnel.Items.AddRange(new object[] {
            "0=変更しない",
            "1=8bit mono"});
            this.Chunnel.Location = new System.Drawing.Point(214, 122);
            this.Chunnel.Name = "Chunnel";
            this.Chunnel.Size = new System.Drawing.Size(449, 26);
            this.Chunnel.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AccessibleDescription = "Waveの音質を下げると、容量を小さくできます。";
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 18);
            this.label6.TabIndex = 197;
            this.label6.Text = "チャンネル";
            // 
            // Preview_button
            // 
            this.Preview_button.Location = new System.Drawing.Point(535, 275);
            this.Preview_button.Name = "Preview_button";
            this.Preview_button.Size = new System.Drawing.Size(128, 32);
            this.Preview_button.TabIndex = 6;
            this.Preview_button.Text = "Preview";
            this.Preview_button.UseVisualStyleBackColor = true;
            this.Preview_button.Click += new System.EventHandler(this.Preview_button_Click);
            // 
            // SongInstrumentImportWaveForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(717, 441);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SongInstrumentImportWaveForm";
            this.Text = "Waveのインポート";
            this.Load += new System.EventHandler(this.SongTrackImportWaveForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox HZ;
        private System.Windows.Forms.ComboBox Volume;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox Strip;
        private System.Windows.Forms.ComboBox Chunnel;
        private System.Windows.Forms.Button Preview_button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox PreviewResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox UseDPCMCompress;
        private System.Windows.Forms.ComboBox DPCMLookahead;
        private System.Windows.Forms.Label LABEL_DPCM_LOOKAHEAD;
        private System.Windows.Forms.Label NO_DPCM_PATCH;
    }
}