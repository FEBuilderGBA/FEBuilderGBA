namespace FEBuilderGBA
{
    partial class EventAssemblerForm
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
            this.FREEARE_PANEL = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.FREEAREA = new System.Windows.Forms.NumericUpDown();
            this.FREEAREA_DEF = new System.Windows.Forms.CheckBox();
            this.UndoButton = new System.Windows.Forms.Button();
            this.WriteButton = new System.Windows.Forms.Button();
            this.J_2_TEXT = new System.Windows.Forms.Label();
            this.SRCFilename = new FEBuilderGBA.TextBoxEx();
            this.SRCSelectButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.FREEARE_PANEL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FREEAREA)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SRCFilename);
            this.panel1.Controls.Add(this.SRCSelectButton);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.FREEARE_PANEL);
            this.panel1.Controls.Add(this.FREEAREA_DEF);
            this.panel1.Controls.Add(this.UndoButton);
            this.panel1.Controls.Add(this.WriteButton);
            this.panel1.Controls.Add(this.J_2_TEXT);
            this.panel1.Location = new System.Drawing.Point(13, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(668, 335);
            this.panel1.TabIndex = 0;
            // 
            // FREEARE_PANEL
            // 
            this.FREEARE_PANEL.Controls.Add(this.label1);
            this.FREEARE_PANEL.Controls.Add(this.FREEAREA);
            this.FREEARE_PANEL.Location = new System.Drawing.Point(10, 240);
            this.FREEARE_PANEL.Name = "FREEARE_PANEL";
            this.FREEARE_PANEL.Size = new System.Drawing.Size(430, 41);
            this.FREEARE_PANEL.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "フリーエリアの定義";
            // 
            // FREEAREA
            // 
            this.FREEAREA.Hexadecimal = true;
            this.FREEAREA.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.FREEAREA.Location = new System.Drawing.Point(238, 9);
            this.FREEAREA.Margin = new System.Windows.Forms.Padding(2);
            this.FREEAREA.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.FREEAREA.Name = "FREEAREA";
            this.FREEAREA.Size = new System.Drawing.Size(130, 25);
            this.FREEAREA.TabIndex = 17;
            // 
            // FREEAREA_DEF
            // 
            this.FREEAREA_DEF.AutoSize = true;
            this.FREEAREA_DEF.Checked = true;
            this.FREEAREA_DEF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FREEAREA_DEF.Location = new System.Drawing.Point(10, 211);
            this.FREEAREA_DEF.Name = "FREEAREA_DEF";
            this.FREEAREA_DEF.Size = new System.Drawing.Size(190, 22);
            this.FREEAREA_DEF.TabIndex = 18;
            this.FREEAREA_DEF.Text = "フリーエリアを定義する";
            this.FREEAREA_DEF.UseVisualStyleBackColor = true;
            this.FREEAREA_DEF.CheckedChanged += new System.EventHandler(this.FREEAREA_DEF_CheckedChanged);
            // 
            // UndoButton
            // 
            this.UndoButton.Location = new System.Drawing.Point(438, 139);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(215, 32);
            this.UndoButton.TabIndex = 16;
            this.UndoButton.Text = "UNDO";
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Visible = false;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(3, 139);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(215, 32);
            this.WriteButton.TabIndex = 15;
            this.WriteButton.Text = "スクリプト読込";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // J_2_TEXT
            // 
            this.J_2_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2_TEXT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.J_2_TEXT.Location = new System.Drawing.Point(2, 1);
            this.J_2_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2_TEXT.Name = "J_2_TEXT";
            this.J_2_TEXT.Size = new System.Drawing.Size(665, 67);
            this.J_2_TEXT.TabIndex = 13;
            this.J_2_TEXT.Text = "Event Assemblerでeventスクリプトを読み込んで現在のROMに適応します。";
            this.J_2_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SRCFilename
            // 
            this.SRCFilename.ErrorMessage = "";
            this.SRCFilename.Location = new System.Drawing.Point(307, 91);
            this.SRCFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SRCFilename.Name = "SRCFilename";
            this.SRCFilename.Placeholder = "";
            this.SRCFilename.Size = new System.Drawing.Size(346, 25);
            this.SRCFilename.TabIndex = 93;
            // 
            // SRCSelectButton
            // 
            this.SRCSelectButton.Location = new System.Drawing.Point(169, 87);
            this.SRCSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.SRCSelectButton.Name = "SRCSelectButton";
            this.SRCSelectButton.Size = new System.Drawing.Size(130, 31);
            this.SRCSelectButton.TabIndex = 92;
            this.SRCSelectButton.Text = "別ファイル選択";
            this.SRCSelectButton.UseVisualStyleBackColor = true;
            this.SRCSelectButton.Click += new System.EventHandler(this.SRCSelectButton_Click);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(4, 90);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 31);
            this.label9.TabIndex = 91;
            this.label9.Text = "スクリプト";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EventAssemblerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(693, 355);
            this.Controls.Add(this.panel1);
            this.Name = "EventAssemblerForm";
            this.Text = "Event Assemblerで追加";
            this.Load += new System.EventHandler(this.EventAssemblerForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.FREEARE_PANEL.ResumeLayout(false);
            this.FREEARE_PANEL.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FREEAREA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label J_2_TEXT;
        private System.Windows.Forms.Button UndoButton;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown FREEAREA;
        private System.Windows.Forms.CheckBox FREEAREA_DEF;
        private System.Windows.Forms.Panel FREEARE_PANEL;
        private TextBoxEx SRCFilename;
        private System.Windows.Forms.Button SRCSelectButton;
        private System.Windows.Forms.Label label9;
    }
}