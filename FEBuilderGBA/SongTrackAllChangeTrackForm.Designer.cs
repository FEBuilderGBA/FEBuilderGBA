namespace FEBuilderGBA
{
    partial class SongTrackAllChangeTrackForm
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
            this.ChangeButton = new System.Windows.Forms.Button();
            this.customColorGroupBox3 = new FEBuilderGBA.CustomColorGroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PanNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.customColorGroupBox2 = new FEBuilderGBA.CustomColorGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.VolNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.VoiceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.VoiceListbox = new FEBuilderGBA.ListBoxEx();
            this.customColorGroupBox4 = new FEBuilderGBA.CustomColorGroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TempoUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.panel1.SuspendLayout();
            this.customColorGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanNumericUpDown)).BeginInit();
            this.customColorGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolNumericUpDown)).BeginInit();
            this.customColorGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VoiceNumericUpDown)).BeginInit();
            this.customColorGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TempoUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ChangeButton);
            this.panel1.Location = new System.Drawing.Point(12, 555);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(717, 46);
            this.panel1.TabIndex = 3;
            // 
            // ChangeButton
            // 
            this.ChangeButton.Location = new System.Drawing.Point(545, 2);
            this.ChangeButton.Name = "ChangeButton";
            this.ChangeButton.Size = new System.Drawing.Size(168, 42);
            this.ChangeButton.TabIndex = 0;
            this.ChangeButton.Text = "変更する";
            this.ChangeButton.UseVisualStyleBackColor = true;
            this.ChangeButton.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // customColorGroupBox3
            // 
            this.customColorGroupBox3.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox3.Controls.Add(this.label3);
            this.customColorGroupBox3.Controls.Add(this.PanNumericUpDown);
            this.customColorGroupBox3.Location = new System.Drawing.Point(12, 375);
            this.customColorGroupBox3.Name = "customColorGroupBox3";
            this.customColorGroupBox3.Size = new System.Drawing.Size(717, 83);
            this.customColorGroupBox3.TabIndex = 2;
            this.customColorGroupBox3.TabStop = false;
            this.customColorGroupBox3.Text = "PAN補正";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(194, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(330, 36);
            this.label3.TabIndex = 2;
            this.label3.Text = "全トラックのPANに、指定された値を足します。\r\nマイナスは左側、プラスは右側です。";
            // 
            // PanNumericUpDown
            // 

            this.PanNumericUpDown.Location = new System.Drawing.Point(11, 31);
            this.PanNumericUpDown.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.PanNumericUpDown.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.PanNumericUpDown.Name = "PanNumericUpDown";
            this.PanNumericUpDown.Size = new System.Drawing.Size(120, 25);
            this.PanNumericUpDown.TabIndex = 0;
            // 
            // customColorGroupBox2
            // 
            this.customColorGroupBox2.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox2.Controls.Add(this.label1);
            this.customColorGroupBox2.Controls.Add(this.VolNumericUpDown);
            this.customColorGroupBox2.Location = new System.Drawing.Point(12, 286);
            this.customColorGroupBox2.Name = "customColorGroupBox2";
            this.customColorGroupBox2.Size = new System.Drawing.Size(717, 83);
            this.customColorGroupBox2.TabIndex = 1;
            this.customColorGroupBox2.TabStop = false;
            this.customColorGroupBox2.Text = "音量補正";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 36);
            this.label1.TabIndex = 1;
            this.label1.Text = "全トラックの音量に、指定された値を足しこみます。\r\n大きくするほど、大きな音になります。";
            // 
            // VolNumericUpDown
            // 

            this.VolNumericUpDown.Location = new System.Drawing.Point(11, 32);
            this.VolNumericUpDown.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.VolNumericUpDown.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.VolNumericUpDown.Name = "VolNumericUpDown";
            this.VolNumericUpDown.Size = new System.Drawing.Size(120, 25);
            this.VolNumericUpDown.TabIndex = 0;
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.VoiceNumericUpDown);
            this.customColorGroupBox1.Controls.Add(this.label2);
            this.customColorGroupBox1.Controls.Add(this.VoiceListbox);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(717, 268);
            this.customColorGroupBox1.TabIndex = 0;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "楽器変更";
            // 
            // VoiceNumericUpDown
            // 

            this.VoiceNumericUpDown.Location = new System.Drawing.Point(624, 49);
            this.VoiceNumericUpDown.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.VoiceNumericUpDown.Name = "VoiceNumericUpDown";
            this.VoiceNumericUpDown.Size = new System.Drawing.Size(83, 25);
            this.VoiceNumericUpDown.TabIndex = 7;
            this.VoiceNumericUpDown.ValueChanged += new System.EventHandler(this.VoiceNumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(624, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "変更先";
            // 
            // VoiceListbox
            // 
            this.VoiceListbox.FormattingEnabled = true;
            this.VoiceListbox.ItemHeight = 18;
            this.VoiceListbox.Location = new System.Drawing.Point(6, 24);
            this.VoiceListbox.Name = "VoiceListbox";
            this.VoiceListbox.Size = new System.Drawing.Size(612, 238);
            this.VoiceListbox.TabIndex = 0;
            this.VoiceListbox.SelectedIndexChanged += new System.EventHandler(this.VoiceListbox_SelectedIndexChanged);
            // 
            // customColorGroupBox4
            // 
            this.customColorGroupBox4.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox4.Controls.Add(this.label4);
            this.customColorGroupBox4.Controls.Add(this.TempoUpDown1);
            this.customColorGroupBox4.Location = new System.Drawing.Point(12, 464);
            this.customColorGroupBox4.Name = "customColorGroupBox4";
            this.customColorGroupBox4.Size = new System.Drawing.Size(717, 83);
            this.customColorGroupBox4.TabIndex = 4;
            this.customColorGroupBox4.TabStop = false;
            this.customColorGroupBox4.Text = "テンポ補正";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(194, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(338, 36);
            this.label4.TabIndex = 2;
            this.label4.Text = "全トラックのテンポに、指定された値を足します。\r\n大きくするほど早くなります。";
            // 
            // TempoUpDown1
            // 

            this.TempoUpDown1.Location = new System.Drawing.Point(11, 31);
            this.TempoUpDown1.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.TempoUpDown1.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.TempoUpDown1.Name = "TempoUpDown1";
            this.TempoUpDown1.Size = new System.Drawing.Size(120, 25);
            this.TempoUpDown1.TabIndex = 0;
            // 
            // SongTrackAllChangeTrackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(738, 605);
            this.Controls.Add(this.customColorGroupBox4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.customColorGroupBox3);
            this.Controls.Add(this.customColorGroupBox2);
            this.Controls.Add(this.customColorGroupBox1);
            this.Name = "SongTrackAllChangeTrackForm";
            this.Text = "全トラックの変更";
            this.Load += new System.EventHandler(this.SongTrackAllChangeTrackForm_Load);
            this.panel1.ResumeLayout(false);
            this.customColorGroupBox3.ResumeLayout(false);
            this.customColorGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanNumericUpDown)).EndInit();
            this.customColorGroupBox2.ResumeLayout(false);
            this.customColorGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolNumericUpDown)).EndInit();
            this.customColorGroupBox1.ResumeLayout(false);
            this.customColorGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VoiceNumericUpDown)).EndInit();
            this.customColorGroupBox4.ResumeLayout(false);
            this.customColorGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TempoUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomColorGroupBox customColorGroupBox1;
        private System.Windows.Forms.Label label2;
        private ListBoxEx VoiceListbox;
        private CustomColorGroupBox customColorGroupBox2;
        private System.Windows.Forms.NumericUpDown VolNumericUpDown;
        private CustomColorGroupBox customColorGroupBox3;
        private System.Windows.Forms.NumericUpDown PanNumericUpDown;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ChangeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown VoiceNumericUpDown;
        private CustomColorGroupBox customColorGroupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown TempoUpDown1;
    }
}