namespace FEBuilderGBA
{
    partial class OverraideCheckWithErrorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverraideCheckWithErrorDialog));
            this.TitleLabel = new System.Windows.Forms.Label();
            this.OverraideMessage = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MyOKButton = new System.Windows.Forms.Button();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.DiaplayFilename = new System.Windows.Forms.Label();
            this.MyDisplayErrorButton = new System.Windows.Forms.Button();
            this.HintTextBox = new FEBuilderGBA.TextBoxEx();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TitleLabel.ForeColor = System.Drawing.Color.Red;
            this.TitleLabel.Location = new System.Drawing.Point(15, 9);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(694, 28);
            this.TitleLabel.TabIndex = 1;
            this.TitleLabel.Text = "このROMには、エラーがありますが、保存してもよろしいですか？";
            // 
            // OverraideMessage
            // 
            this.OverraideMessage.Location = new System.Drawing.Point(17, 55);
            this.OverraideMessage.Name = "OverraideMessage";
            this.OverraideMessage.Size = new System.Drawing.Size(972, 27);
            this.OverraideMessage.TabIndex = 26;
            this.OverraideMessage.Text = "現在の内容を以下のファイルに書き込んでもよろしいですか？";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 18);
            this.label2.TabIndex = 27;
            this.label2.Text = "ヒント";
            // 
            // MyOKButton
            // 
            this.MyOKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyOKButton.Location = new System.Drawing.Point(20, 128);
            this.MyOKButton.Name = "MyOKButton";
            this.MyOKButton.Size = new System.Drawing.Size(324, 51);
            this.MyOKButton.TabIndex = 0;
            this.MyOKButton.Text = "はい";
            this.MyOKButton.UseVisualStyleBackColor = true;
            this.MyOKButton.Click += new System.EventHandler(this.MyOKButton_Click);
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(745, 128);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(331, 51);
            this.MyCancelButton.TabIndex = 2;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // DiaplayFilename
            // 
            this.DiaplayFilename.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.DiaplayFilename.Location = new System.Drawing.Point(16, 82);
            this.DiaplayFilename.Name = "DiaplayFilename";
            this.DiaplayFilename.Size = new System.Drawing.Size(972, 41);
            this.DiaplayFilename.TabIndex = 29;
            // 
            // MyDisplayErrorButton
            // 
            this.MyDisplayErrorButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyDisplayErrorButton.Location = new System.Drawing.Point(385, 128);
            this.MyDisplayErrorButton.Name = "MyDisplayErrorButton";
            this.MyDisplayErrorButton.Size = new System.Drawing.Size(324, 51);
            this.MyDisplayErrorButton.TabIndex = 1;
            this.MyDisplayErrorButton.Text = "エラーを表示";
            this.MyDisplayErrorButton.UseVisualStyleBackColor = true;
            this.MyDisplayErrorButton.Click += new System.EventHandler(this.MyDisplayErrorButton_Click);
            // 
            // HintTextBox
            // 
            this.HintTextBox.ErrorMessage = "";
            this.HintTextBox.Location = new System.Drawing.Point(20, 273);
            this.HintTextBox.Multiline = true;
            this.HintTextBox.Name = "HintTextBox";
            this.HintTextBox.Placeholder = "";
            this.HintTextBox.Size = new System.Drawing.Size(1056, 192);
            this.HintTextBox.TabIndex = 30;
            this.HintTextBox.Text = resources.GetString("HintTextBox.Text");
            // 
            // OverraideCheckWithErrorDialog
            // 
            this.AcceptButton = this.MyOKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(1088, 477);
            this.Controls.Add(this.HintTextBox);
            this.Controls.Add(this.MyDisplayErrorButton);
            this.Controls.Add(this.DiaplayFilename);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.MyOKButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OverraideMessage);
            this.Controls.Add(this.TitleLabel);
            this.Name = "OverraideCheckWithErrorDialog";
            this.Text = "このROMには、エラーがありますが、保存してもよろしいですか？";
            this.Load += new System.EventHandler(this.OverraideCheckDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label OverraideMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button MyOKButton;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Label DiaplayFilename;
        private System.Windows.Forms.Button MyDisplayErrorButton;
        private TextBoxEx HintTextBox;
    }
}