namespace FEBuilderGBA
{
    partial class SongTrackImportSelectInstrumentForm
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
            this.NotChangeButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.InstrumentSelectComboBox = new System.Windows.Forms.ComboBox();
            this.Instrument = new System.Windows.Forms.NumericUpDown();
            this.J_4_INSTRUMENT_ADDR = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Instrument)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.NotChangeButton);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.InstrumentSelectComboBox);
            this.panel1.Controls.Add(this.Instrument);
            this.panel1.Controls.Add(this.J_4_INSTRUMENT_ADDR);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(10, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(728, 270);
            this.panel1.TabIndex = 0;
            // 
            // NotChangeButton
            // 
            this.NotChangeButton.Location = new System.Drawing.Point(8, 230);
            this.NotChangeButton.Margin = new System.Windows.Forms.Padding(4);
            this.NotChangeButton.Name = "NotChangeButton";
            this.NotChangeButton.Size = new System.Drawing.Size(340, 28);
            this.NotChangeButton.TabIndex = 1;
            this.NotChangeButton.Text = "ディフォルト値から変更しない";
            this.NotChangeButton.UseVisualStyleBackColor = true;
            this.NotChangeButton.Click += new System.EventHandler(this.NotChangeButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(377, 230);
            this.OKButton.Margin = new System.Windows.Forms.Padding(4);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(347, 28);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "選択する";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // InstrumentSelectComboBox
            // 
            this.InstrumentSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InstrumentSelectComboBox.FormattingEnabled = true;
            this.InstrumentSelectComboBox.Location = new System.Drawing.Point(354, 177);
            this.InstrumentSelectComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.InstrumentSelectComboBox.Name = "InstrumentSelectComboBox";
            this.InstrumentSelectComboBox.Size = new System.Drawing.Size(370, 26);
            this.InstrumentSelectComboBox.TabIndex = 2;
            this.InstrumentSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.InstrumentSelectComboBox_SelectedIndexChanged);
            // 
            // Instrument
            // 
            this.Instrument.Hexadecimal = true;
            this.Instrument.Location = new System.Drawing.Point(214, 179);
            this.Instrument.Margin = new System.Windows.Forms.Padding(2);
            this.Instrument.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Instrument.Name = "Instrument";
            this.Instrument.Size = new System.Drawing.Size(134, 25);
            this.Instrument.TabIndex = 3;
            // 
            // J_4_INSTRUMENT_ADDR
            // 
            this.J_4_INSTRUMENT_ADDR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4_INSTRUMENT_ADDR.Location = new System.Drawing.Point(8, 179);
            this.J_4_INSTRUMENT_ADDR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4_INSTRUMENT_ADDR.Name = "J_4_INSTRUMENT_ADDR";
            this.J_4_INSTRUMENT_ADDR.Size = new System.Drawing.Size(202, 26);
            this.J_4_INSTRUMENT_ADDR.TabIndex = 172;
            this.J_4_INSTRUMENT_ADDR.Text = "楽器セット";
            this.J_4_INSTRUMENT_ADDR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(445, 108);
            this.label1.TabIndex = 0;
            this.label1.Text = "曲を演奏するときに利用する楽器を選択してください。\r\nディフォルトは現在の曲の楽器アドレスになっています。\r\n\r\nmidi楽器とFEの楽器は、並び順が違うので、注" +
    "意してください。\r\n自分で楽器の順番を調整するか、\r\nNIMAP(Native Instrument Map)パッチを利用してください。";
            // 
            // SongTrackImportSelectInstrumentForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(751, 284);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SongTrackImportSelectInstrumentForm";
            this.Text = "楽器を選択してください";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Instrument)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ComboBox InstrumentSelectComboBox;
        private System.Windows.Forms.NumericUpDown Instrument;
        private System.Windows.Forms.Label J_4_INSTRUMENT_ADDR;
        private System.Windows.Forms.Button NotChangeButton;
    }
}