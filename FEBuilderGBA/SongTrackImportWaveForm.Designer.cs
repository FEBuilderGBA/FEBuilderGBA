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
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.LoopComboBox = new System.Windows.Forms.ComboBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.J_4_INSTRUMENT_ADDR = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.MyCancelButton);
            this.panel1.Controls.Add(this.LoopComboBox);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.J_4_INSTRUMENT_ADDR);
            this.panel1.Location = new System.Drawing.Point(10, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(672, 270);
            this.panel1.TabIndex = 0;
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.Location = new System.Drawing.Point(17, 223);
            this.MyCancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(242, 28);
            this.MyCancelButton.TabIndex = 181;
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
            this.LoopComboBox.Location = new System.Drawing.Point(297, 172);
            this.LoopComboBox.Name = "LoopComboBox";
            this.LoopComboBox.Size = new System.Drawing.Size(363, 26);
            this.LoopComboBox.TabIndex = 180;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(377, 223);
            this.OKButton.Margin = new System.Windows.Forms.Padding(4);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(283, 28);
            this.OKButton.TabIndex = 179;
            this.OKButton.Text = "インポート";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // J_4_INSTRUMENT_ADDR
            // 
            this.J_4_INSTRUMENT_ADDR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_INSTRUMENT_ADDR.Location = new System.Drawing.Point(17, 172);
            this.J_4_INSTRUMENT_ADDR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_INSTRUMENT_ADDR.Name = "J_4_INSTRUMENT_ADDR";
            this.J_4_INSTRUMENT_ADDR.Size = new System.Drawing.Size(260, 26);
            this.J_4_INSTRUMENT_ADDR.TabIndex = 178;
            this.J_4_INSTRUMENT_ADDR.Text = "ループ";
            this.J_4_INSTRUMENT_ADDR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(646, 98);
            this.label1.TabIndex = 182;
            this.label1.Text = "Waveファイルは効果音に使うことを想定しています。\r\nそれを、音楽に利用すると、大量に容量を消費しますが、インポートしてもよろしいですか？";
            // 
            // SongTrackImportWaveForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(696, 284);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SongTrackImportWaveForm";
            this.Text = "Waveをインポート";
            this.Load += new System.EventHandler(this.SongTrackImportWaveForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.ComboBox LoopComboBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label J_4_INSTRUMENT_ADDR;
        private System.Windows.Forms.Label label1;
    }
}