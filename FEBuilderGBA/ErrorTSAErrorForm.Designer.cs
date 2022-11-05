namespace FEBuilderGBA
{
    partial class ErrorTSAErrorForm
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
            this.X_ErrorMessage = new FEBuilderGBA.TextBoxEx();
            this.X_ReOrderImage1Panel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.X_ReOrderImage1 = new FEBuilderGBA.InterpolatedPictureBox();
            this.ReOrder1Button = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.X_CancelImage = new FEBuilderGBA.InterpolatedPictureBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.X_ReOrderImage2Panel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.X_ReOrderImage2 = new FEBuilderGBA.InterpolatedPictureBox();
            this.ReOrder2Button = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.X_ReOrderImage1Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_ReOrderImage1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_CancelImage)).BeginInit();
            this.X_ReOrderImage2Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_ReOrderImage2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.X_ReOrderImage2Panel);
            this.panel1.Controls.Add(this.X_ErrorMessage);
            this.panel1.Controls.Add(this.X_ReOrderImage1Panel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1040, 823);
            this.panel1.TabIndex = 1;
            // 
            // X_ErrorMessage
            // 
            this.X_ErrorMessage.ErrorMessage = "";
            this.X_ErrorMessage.Location = new System.Drawing.Point(11, 40);
            this.X_ErrorMessage.Multiline = true;
            this.X_ErrorMessage.Name = "X_ErrorMessage";
            this.X_ErrorMessage.Placeholder = "";
            this.X_ErrorMessage.ReadOnly = true;
            this.X_ErrorMessage.Size = new System.Drawing.Size(475, 231);
            this.X_ErrorMessage.TabIndex = 8;
            // 
            // X_ReOrderImage1Panel
            // 
            this.X_ReOrderImage1Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_ReOrderImage1Panel.Controls.Add(this.label5);
            this.X_ReOrderImage1Panel.Controls.Add(this.X_ReOrderImage1);
            this.X_ReOrderImage1Panel.Controls.Add(this.ReOrder1Button);
            this.X_ReOrderImage1Panel.Location = new System.Drawing.Point(7, 272);
            this.X_ReOrderImage1Panel.Name = "X_ReOrderImage1Panel";
            this.X_ReOrderImage1Panel.Size = new System.Drawing.Size(1028, 277);
            this.X_ReOrderImage1Panel.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(484, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(535, 98);
            this.label5.TabIndex = 5;
            this.label5.Text = "減色処理や、パレットの入れ替えを行い、自動的に形式を満たせる形式に変換します";
            // 
            // X_ReOrderImage1
            // 
            this.X_ReOrderImage1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_ReOrderImage1.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_ReOrderImage1.Location = new System.Drawing.Point(3, -1);
            this.X_ReOrderImage1.Name = "X_ReOrderImage1";
            this.X_ReOrderImage1.Size = new System.Drawing.Size(475, 276);
            this.X_ReOrderImage1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.X_ReOrderImage1.TabIndex = 4;
            this.X_ReOrderImage1.TabStop = false;
            this.X_ReOrderImage1.Click += new System.EventHandler(this.X_ReOrderImage1_Click);
            // 
            // ReOrder1Button
            // 
            this.ReOrder1Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReOrder1Button.Location = new System.Drawing.Point(484, 3);
            this.ReOrder1Button.Name = "ReOrder1Button";
            this.ReOrder1Button.Size = new System.Drawing.Size(535, 36);
            this.ReOrder1Button.TabIndex = 3;
            this.ReOrder1Button.Text = "自動変換してインポート";
            this.ReOrder1Button.UseVisualStyleBackColor = true;
            this.ReOrder1Button.Click += new System.EventHandler(this.ReOrder1Button_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.X_CancelImage);
            this.panel2.Controls.Add(this.CloseButton);
            this.panel2.Location = new System.Drawing.Point(492, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(543, 102);
            this.panel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(116, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(426, 47);
            this.label1.TabIndex = 5;
            this.label1.Text = "インポート処理をキャンセルします.";
            // 
            // X_CancelImage
            // 
            this.X_CancelImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_CancelImage.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_CancelImage.Location = new System.Drawing.Point(3, 3);
            this.X_CancelImage.Name = "X_CancelImage";
            this.X_CancelImage.Size = new System.Drawing.Size(96, 96);
            this.X_CancelImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.X_CancelImage.TabIndex = 4;
            this.X_CancelImage.TabStop = false;
            this.X_CancelImage.Click += new System.EventHandler(this.X_CancelImage_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(119, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(419, 36);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "キャンセル";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(532, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "この画像は、必要な形式の基準を満たしていません。";
            // 
            // X_ReOrderImage2Panel
            // 
            this.X_ReOrderImage2Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_ReOrderImage2Panel.Controls.Add(this.label3);
            this.X_ReOrderImage2Panel.Controls.Add(this.X_ReOrderImage2);
            this.X_ReOrderImage2Panel.Controls.Add(this.ReOrder2Button);
            this.X_ReOrderImage2Panel.Location = new System.Drawing.Point(7, 551);
            this.X_ReOrderImage2Panel.Name = "X_ReOrderImage2Panel";
            this.X_ReOrderImage2Panel.Size = new System.Drawing.Size(1028, 277);
            this.X_ReOrderImage2Panel.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(484, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(535, 98);
            this.label3.TabIndex = 5;
            this.label3.Text = "規約を守っていないデータはパレットの最初の色としてインポートします。";
            // 
            // X_ReOrderImage2
            // 
            this.X_ReOrderImage2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_ReOrderImage2.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_ReOrderImage2.Location = new System.Drawing.Point(3, -1);
            this.X_ReOrderImage2.Name = "X_ReOrderImage2";
            this.X_ReOrderImage2.Size = new System.Drawing.Size(475, 276);
            this.X_ReOrderImage2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.X_ReOrderImage2.TabIndex = 4;
            this.X_ReOrderImage2.TabStop = false;
            // 
            // ReOrder2Button
            // 
            this.ReOrder2Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReOrder2Button.Location = new System.Drawing.Point(484, 3);
            this.ReOrder2Button.Name = "ReOrder2Button";
            this.ReOrder2Button.Size = new System.Drawing.Size(535, 36);
            this.ReOrder2Button.TabIndex = 3;
            this.ReOrder2Button.Text = "違反データは0に指定";
            this.ReOrder2Button.UseVisualStyleBackColor = true;
            this.ReOrder2Button.Click += new System.EventHandler(this.ReOrder2Button_Click);
            // 
            // ErrorTSAErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1055, 847);
            this.Controls.Add(this.panel1);
            this.Name = "ErrorTSAErrorForm";
            this.Text = "画像の形式が基準を満たしていません。";
            this.Load += new System.EventHandler(this.ErrorPaletteShowForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.X_ReOrderImage1Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_ReOrderImage1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_CancelImage)).EndInit();
            this.X_ReOrderImage2Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_ReOrderImage2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel X_ReOrderImage1Panel;
        private System.Windows.Forms.Label label5;
        private InterpolatedPictureBox X_ReOrderImage1;
        private System.Windows.Forms.Button ReOrder1Button;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private InterpolatedPictureBox X_CancelImage;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label label2;
        private TextBoxEx X_ErrorMessage;
        private System.Windows.Forms.Panel X_ReOrderImage2Panel;
        private System.Windows.Forms.Label label3;
        private InterpolatedPictureBox X_ReOrderImage2;
        private System.Windows.Forms.Button ReOrder2Button;

    }
}