namespace FEBuilderGBA
{
    partial class ToolSubtitleSetingDialogForm
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
            this.SimpleTranslateToTranslateDataFilename = new FEBuilderGBA.TextBoxEx();
            this.SimpleTranslateToTranslateDataFilenameButton = new System.Windows.Forms.Button();
            this.LabelSimpleTranslateToTranslateDataFilename = new System.Windows.Forms.Label();
            this.LabelSimpleTranslateToROMFilename = new System.Windows.Forms.Label();
            this.SimpleTranslateToROMFilename = new FEBuilderGBA.TextBoxEx();
            this.SimpleTranslateToROMFilenameSelectButton = new System.Windows.Forms.Button();
            this.LabelSimpleTranslateFromROMFilename = new System.Windows.Forms.Label();
            this.SimpleTranslateFromROMFilename = new FEBuilderGBA.TextBoxEx();
            this.SimpleTranslateFormROMFilenameSelectButton = new System.Windows.Forms.Button();
            this.ShowAlways = new System.Windows.Forms.CheckBox();
            this.ShowButton = new System.Windows.Forms.Button();
            this.HideButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SimpleTranslateToTranslateDataFilename
            // 
            this.SimpleTranslateToTranslateDataFilename.ErrorMessage = "";
            this.SimpleTranslateToTranslateDataFilename.Location = new System.Drawing.Point(211, 105);
            this.SimpleTranslateToTranslateDataFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateToTranslateDataFilename.Name = "SimpleTranslateToTranslateDataFilename";
            this.SimpleTranslateToTranslateDataFilename.Placeholder = "";
            this.SimpleTranslateToTranslateDataFilename.Size = new System.Drawing.Size(623, 25);
            this.SimpleTranslateToTranslateDataFilename.TabIndex = 109;
            this.SimpleTranslateToTranslateDataFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SimpleTranslateToTranslateDataFilename_MouseDoubleClick);
            // 
            // SimpleTranslateToTranslateDataFilenameButton
            // 
            this.SimpleTranslateToTranslateDataFilenameButton.Location = new System.Drawing.Point(840, 100);
            this.SimpleTranslateToTranslateDataFilenameButton.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateToTranslateDataFilenameButton.Name = "SimpleTranslateToTranslateDataFilenameButton";
            this.SimpleTranslateToTranslateDataFilenameButton.Size = new System.Drawing.Size(41, 31);
            this.SimpleTranslateToTranslateDataFilenameButton.TabIndex = 110;
            this.SimpleTranslateToTranslateDataFilenameButton.Text = "..";
            this.SimpleTranslateToTranslateDataFilenameButton.UseVisualStyleBackColor = true;
            this.SimpleTranslateToTranslateDataFilenameButton.Click += new System.EventHandler(this.SimpleTranslateToTranslateDataFilenameButton_Click);
            // 
            // LabelSimpleTranslateToTranslateDataFilename
            // 
            this.LabelSimpleTranslateToTranslateDataFilename.AccessibleDescription = "@TRANSLATE_HINT_FILE";
            this.LabelSimpleTranslateToTranslateDataFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelSimpleTranslateToTranslateDataFilename.Location = new System.Drawing.Point(12, 102);
            this.LabelSimpleTranslateToTranslateDataFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelSimpleTranslateToTranslateDataFilename.Name = "LabelSimpleTranslateToTranslateDataFilename";
            this.LabelSimpleTranslateToTranslateDataFilename.Size = new System.Drawing.Size(196, 31);
            this.LabelSimpleTranslateToTranslateDataFilename.TabIndex = 108;
            this.LabelSimpleTranslateToTranslateDataFilename.Text = "翻訳データ";
            this.LabelSimpleTranslateToTranslateDataFilename.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelSimpleTranslateToROMFilename
            // 
            this.LabelSimpleTranslateToROMFilename.AccessibleDescription = "@TRANSLATE_TO_ROM";
            this.LabelSimpleTranslateToROMFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelSimpleTranslateToROMFilename.Location = new System.Drawing.Point(12, 63);
            this.LabelSimpleTranslateToROMFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelSimpleTranslateToROMFilename.Name = "LabelSimpleTranslateToROMFilename";
            this.LabelSimpleTranslateToROMFilename.Size = new System.Drawing.Size(196, 31);
            this.LabelSimpleTranslateToROMFilename.TabIndex = 107;
            this.LabelSimpleTranslateToROMFilename.Text = "定型文ROM TO";
            this.LabelSimpleTranslateToROMFilename.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleTranslateToROMFilename
            // 
            this.SimpleTranslateToROMFilename.ErrorMessage = "";
            this.SimpleTranslateToROMFilename.Location = new System.Drawing.Point(211, 65);
            this.SimpleTranslateToROMFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateToROMFilename.Name = "SimpleTranslateToROMFilename";
            this.SimpleTranslateToROMFilename.Placeholder = "";
            this.SimpleTranslateToROMFilename.Size = new System.Drawing.Size(623, 25);
            this.SimpleTranslateToROMFilename.TabIndex = 104;
            this.SimpleTranslateToROMFilename.DoubleClick += new System.EventHandler(this.TranslateToROMFilename_DoubleClick);
            // 
            // SimpleTranslateToROMFilenameSelectButton
            // 
            this.SimpleTranslateToROMFilenameSelectButton.Location = new System.Drawing.Point(840, 60);
            this.SimpleTranslateToROMFilenameSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateToROMFilenameSelectButton.Name = "SimpleTranslateToROMFilenameSelectButton";
            this.SimpleTranslateToROMFilenameSelectButton.Size = new System.Drawing.Size(41, 31);
            this.SimpleTranslateToROMFilenameSelectButton.TabIndex = 105;
            this.SimpleTranslateToROMFilenameSelectButton.Text = "..";
            this.SimpleTranslateToROMFilenameSelectButton.UseVisualStyleBackColor = true;
            this.SimpleTranslateToROMFilenameSelectButton.Click += new System.EventHandler(this.TranslateToROMFilenameSelectButton_Click);
            // 
            // LabelSimpleTranslateFromROMFilename
            // 
            this.LabelSimpleTranslateFromROMFilename.AccessibleDescription = "@TRANSLATE_FROM_ROM";
            this.LabelSimpleTranslateFromROMFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelSimpleTranslateFromROMFilename.Location = new System.Drawing.Point(13, 26);
            this.LabelSimpleTranslateFromROMFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelSimpleTranslateFromROMFilename.Name = "LabelSimpleTranslateFromROMFilename";
            this.LabelSimpleTranslateFromROMFilename.Size = new System.Drawing.Size(196, 31);
            this.LabelSimpleTranslateFromROMFilename.TabIndex = 106;
            this.LabelSimpleTranslateFromROMFilename.Text = "定型文ROM FROM";
            this.LabelSimpleTranslateFromROMFilename.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleTranslateFromROMFilename
            // 
            this.SimpleTranslateFromROMFilename.ErrorMessage = "";
            this.SimpleTranslateFromROMFilename.Location = new System.Drawing.Point(212, 28);
            this.SimpleTranslateFromROMFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateFromROMFilename.Name = "SimpleTranslateFromROMFilename";
            this.SimpleTranslateFromROMFilename.Placeholder = "";
            this.SimpleTranslateFromROMFilename.Size = new System.Drawing.Size(623, 25);
            this.SimpleTranslateFromROMFilename.TabIndex = 102;
            this.SimpleTranslateFromROMFilename.DoubleClick += new System.EventHandler(this.TranslateFormROMFilename_DoubleClick);
            // 
            // SimpleTranslateFormROMFilenameSelectButton
            // 
            this.SimpleTranslateFormROMFilenameSelectButton.Location = new System.Drawing.Point(841, 23);
            this.SimpleTranslateFormROMFilenameSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.SimpleTranslateFormROMFilenameSelectButton.Name = "SimpleTranslateFormROMFilenameSelectButton";
            this.SimpleTranslateFormROMFilenameSelectButton.Size = new System.Drawing.Size(41, 31);
            this.SimpleTranslateFormROMFilenameSelectButton.TabIndex = 103;
            this.SimpleTranslateFormROMFilenameSelectButton.Text = "..";
            this.SimpleTranslateFormROMFilenameSelectButton.UseVisualStyleBackColor = true;
            this.SimpleTranslateFormROMFilenameSelectButton.Click += new System.EventHandler(this.TranslateFormROMFilenameSelectButton_Click);
            // 
            // ShowAlways
            // 
            this.ShowAlways.AutoSize = true;
            this.ShowAlways.Location = new System.Drawing.Point(12, 198);
            this.ShowAlways.Name = "ShowAlways";
            this.ShowAlways.Size = new System.Drawing.Size(303, 22);
            this.ShowAlways.TabIndex = 112;
            this.ShowAlways.Text = "字幕がない時でもウィンドウを表示する";
            this.ShowAlways.UseVisualStyleBackColor = true;
            // 
            // ShowButton
            // 
            this.ShowButton.Location = new System.Drawing.Point(13, 251);
            this.ShowButton.Name = "ShowButton";
            this.ShowButton.Size = new System.Drawing.Size(238, 44);
            this.ShowButton.TabIndex = 113;
            this.ShowButton.Text = "字幕表示";
            this.ShowButton.UseVisualStyleBackColor = true;
            this.ShowButton.Click += new System.EventHandler(this.ShowButton_Click);
            // 
            // HideButton
            // 
            this.HideButton.Location = new System.Drawing.Point(596, 251);
            this.HideButton.Name = "HideButton";
            this.HideButton.Size = new System.Drawing.Size(238, 44);
            this.HideButton.TabIndex = 114;
            this.HideButton.Text = "非表示";
            this.HideButton.UseVisualStyleBackColor = true;
            this.HideButton.Click += new System.EventHandler(this.HideButton_Click);
            // 
            // ToolSubtitleSetingDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 319);
            this.Controls.Add(this.HideButton);
            this.Controls.Add(this.ShowButton);
            this.Controls.Add(this.ShowAlways);
            this.Controls.Add(this.SimpleTranslateToTranslateDataFilename);
            this.Controls.Add(this.SimpleTranslateToTranslateDataFilenameButton);
            this.Controls.Add(this.LabelSimpleTranslateToTranslateDataFilename);
            this.Controls.Add(this.LabelSimpleTranslateToROMFilename);
            this.Controls.Add(this.SimpleTranslateToROMFilename);
            this.Controls.Add(this.SimpleTranslateToROMFilenameSelectButton);
            this.Controls.Add(this.LabelSimpleTranslateFromROMFilename);
            this.Controls.Add(this.SimpleTranslateFromROMFilename);
            this.Controls.Add(this.SimpleTranslateFormROMFilenameSelectButton);
            this.Name = "ToolSubtitleSetingDialogForm";
            this.Text = "字幕設定";
            this.Load += new System.EventHandler(this.ToolSubtitleSetingDialogForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBoxEx SimpleTranslateToTranslateDataFilename;
        private System.Windows.Forms.Button SimpleTranslateToTranslateDataFilenameButton;
        private System.Windows.Forms.Label LabelSimpleTranslateToTranslateDataFilename;
        private System.Windows.Forms.Label LabelSimpleTranslateToROMFilename;
        private TextBoxEx SimpleTranslateToROMFilename;
        private System.Windows.Forms.Button SimpleTranslateToROMFilenameSelectButton;
        private System.Windows.Forms.Label LabelSimpleTranslateFromROMFilename;
        private TextBoxEx SimpleTranslateFromROMFilename;
        private System.Windows.Forms.Button SimpleTranslateFormROMFilenameSelectButton;
        private System.Windows.Forms.CheckBox ShowAlways;
        private System.Windows.Forms.Button ShowButton;
        private System.Windows.Forms.Button HideButton;
    }
}