namespace FEBuilderGBA
{
    partial class MapSettingDifficultyForm
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
            this.DifficultyValue = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.J_HardBoost = new System.Windows.Forms.Label();
            this.J_NormalPenalty = new System.Windows.Forms.Label();
            this.J_EasyPenalty = new System.Windows.Forms.Label();
            this.EasyPenalty = new System.Windows.Forms.NumericUpDown();
            this.HardBoost = new System.Windows.Forms.NumericUpDown();
            this.NormalPenalty = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.DifficultyValue)).BeginInit();
            this.customColorGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EasyPenalty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HardBoost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NormalPenalty)).BeginInit();
            this.SuspendLayout();
            // 
            // DifficultyValue
            // 
            this.DifficultyValue.Hexadecimal = true;
            this.DifficultyValue.Location = new System.Drawing.Point(427, 191);
            this.DifficultyValue.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.DifficultyValue.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.DifficultyValue.Name = "DifficultyValue";
            this.DifficultyValue.Size = new System.Drawing.Size(95, 25);
            this.DifficultyValue.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = "";
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 186);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(378, 32);
            this.panel1.TabIndex = 58;
            this.panel1.Text = "難易度補正";
            this.panel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(268, 359);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(272, 43);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "変更する";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 232);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(527, 121);
            this.textBox1.TabIndex = 67;
            this.textBox1.Text = "難易度補正値を設定します。\r\nハードブーストを設定すると、ハードモードで敵が強くなります。\r\nペナルティを設定すると、ノーマルと簡易での敵が弱くなります。\r\n\r\n" +
    "http://feuniverse.us/t/fe7-fe8-difficulty-stat-changes/1295";
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.J_HardBoost);
            this.customColorGroupBox1.Controls.Add(this.J_NormalPenalty);
            this.customColorGroupBox1.Controls.Add(this.J_EasyPenalty);
            this.customColorGroupBox1.Controls.Add(this.EasyPenalty);
            this.customColorGroupBox1.Controls.Add(this.HardBoost);
            this.customColorGroupBox1.Controls.Add(this.NormalPenalty);
            this.customColorGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Size = new System.Drawing.Size(528, 161);
            this.customColorGroupBox1.TabIndex = 68;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "難易度補正";
            // 
            // J_HardBoost
            // 
            this.J_HardBoost.AccessibleDescription = "@MAPSETTING_HARD_BOOST";
            this.J_HardBoost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_HardBoost.Location = new System.Drawing.Point(7, 27);
            this.J_HardBoost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_HardBoost.Name = "J_HardBoost";
            this.J_HardBoost.Size = new System.Drawing.Size(311, 32);
            this.J_HardBoost.TabIndex = 59;
            this.J_HardBoost.Text = "ハードブースト";
            this.J_HardBoost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_NormalPenalty
            // 
            this.J_NormalPenalty.AccessibleDescription = "@MAPSETTING_NORMAL_MODE_LEVEL_PENALTY";
            this.J_NormalPenalty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_NormalPenalty.Location = new System.Drawing.Point(7, 84);
            this.J_NormalPenalty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_NormalPenalty.Name = "J_NormalPenalty";
            this.J_NormalPenalty.Size = new System.Drawing.Size(311, 32);
            this.J_NormalPenalty.TabIndex = 60;
            this.J_NormalPenalty.Text = "ノーマルモードペナルティ";
            this.J_NormalPenalty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_EasyPenalty
            // 
            this.J_EasyPenalty.AccessibleDescription = "@MAPSETTING_EAZY_MODE_LEVEL_PENALTY";
            this.J_EasyPenalty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_EasyPenalty.Location = new System.Drawing.Point(7, 115);
            this.J_EasyPenalty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.J_EasyPenalty.Name = "J_EasyPenalty";
            this.J_EasyPenalty.Size = new System.Drawing.Size(311, 32);
            this.J_EasyPenalty.TabIndex = 61;
            this.J_EasyPenalty.Text = "簡易モードペナルティ";
            this.J_EasyPenalty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EasyPenalty
            // 
            this.EasyPenalty.Location = new System.Drawing.Point(434, 122);
            this.EasyPenalty.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.EasyPenalty.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.EasyPenalty.Name = "EasyPenalty";
            this.EasyPenalty.Size = new System.Drawing.Size(76, 25);
            this.EasyPenalty.TabIndex = 2;
            this.EasyPenalty.ValueChanged += new System.EventHandler(this.HardBoost_ValueChanged);
            // 
            // HardBoost
            // 
            this.HardBoost.Location = new System.Drawing.Point(434, 34);
            this.HardBoost.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.HardBoost.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.HardBoost.Name = "HardBoost";
            this.HardBoost.Size = new System.Drawing.Size(76, 25);
            this.HardBoost.TabIndex = 0;
            this.HardBoost.ValueChanged += new System.EventHandler(this.HardBoost_ValueChanged);
            // 
            // NormalPenalty
            // 
            this.NormalPenalty.Location = new System.Drawing.Point(434, 86);
            this.NormalPenalty.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.NormalPenalty.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NormalPenalty.Name = "NormalPenalty";
            this.NormalPenalty.Size = new System.Drawing.Size(76, 25);
            this.NormalPenalty.TabIndex = 1;
            this.NormalPenalty.ValueChanged += new System.EventHandler(this.HardBoost_ValueChanged);
            // 
            // MapSettingDifficultyForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(559, 409);
            this.Controls.Add(this.customColorGroupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DifficultyValue);
            this.Controls.Add(this.panel1);
            this.Name = "MapSettingDifficultyForm";
            this.Text = "難易度補正";
            this.Load += new System.EventHandler(this.MapSettingDifficultyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DifficultyValue)).EndInit();
            this.customColorGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EasyPenalty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HardBoost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NormalPenalty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown DifficultyValue;
        private System.Windows.Forms.Label panel1;
        private System.Windows.Forms.Label J_HardBoost;
        private System.Windows.Forms.Label J_NormalPenalty;
        private System.Windows.Forms.Label J_EasyPenalty;
        private System.Windows.Forms.NumericUpDown HardBoost;
        private System.Windows.Forms.NumericUpDown NormalPenalty;
        private System.Windows.Forms.NumericUpDown EasyPenalty;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox textBox1;
        private CustomColorGroupBox customColorGroupBox1;
    }
}