namespace FEBuilderGBA
{
    partial class ErrorPaletteShowForm
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
            this.X_ReOrderImage2Panel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.X_ReOrderImage2 = new FEBuilderGBA.InterpolatedPictureBox();
            this.ReOrder2Button = new System.Windows.Forms.Button();
            this.X_ReOrderImage1Panel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.X_ReOrderImage1 = new FEBuilderGBA.InterpolatedPictureBox();
            this.ReOrder1Button = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.X_OrignalImage = new FEBuilderGBA.InterpolatedPictureBox();
            this.ForceButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.X_CancelImage = new FEBuilderGBA.InterpolatedPictureBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ErrorMessage = new FEBuilderGBA.TextBoxEx();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.X_ReOrderImage2Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_ReOrderImage2)).BeginInit();
            this.X_ReOrderImage1Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_ReOrderImage1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_OrignalImage)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_CancelImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.X_ReOrderImage2Panel);
            this.panel1.Controls.Add(this.X_ReOrderImage1Panel);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.ErrorMessage);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1170, 661);
            this.panel1.TabIndex = 0;
            // 
            // X_ReOrderImage2Panel
            // 
            this.X_ReOrderImage2Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_ReOrderImage2Panel.Controls.Add(this.label4);
            this.X_ReOrderImage2Panel.Controls.Add(this.X_ReOrderImage2);
            this.X_ReOrderImage2Panel.Controls.Add(this.ReOrder2Button);
            this.X_ReOrderImage2Panel.Location = new System.Drawing.Point(446, 482);
            this.X_ReOrderImage2Panel.Name = "X_ReOrderImage2Panel";
            this.X_ReOrderImage2Panel.Size = new System.Drawing.Size(721, 170);
            this.X_ReOrderImage2Panel.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(179, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(510, 47);
            this.label4.TabIndex = 5;
            this.label4.Text = "できるだけ規定値2になるように再構築をしてインポートします。";
            // 
            // X_ReOrderImage2
            // 
            this.X_ReOrderImage2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_ReOrderImage2.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_ReOrderImage2.Location = new System.Drawing.Point(3, -1);
            this.X_ReOrderImage2.Name = "X_ReOrderImage2";
            this.X_ReOrderImage2.Size = new System.Drawing.Size(170, 170);
            this.X_ReOrderImage2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.X_ReOrderImage2.TabIndex = 4;
            this.X_ReOrderImage2.TabStop = false;
            this.X_ReOrderImage2.Click += new System.EventHandler(this.X_ReOrderImage2_Click);
            // 
            // ReOrder2Button
            // 
            this.ReOrder2Button.Location = new System.Drawing.Point(179, 3);
            this.ReOrder2Button.Name = "ReOrder2Button";
            this.ReOrder2Button.Size = new System.Drawing.Size(510, 36);
            this.ReOrder2Button.TabIndex = 4;
            this.ReOrder2Button.Text = "規定値2で再構築してインポート";
            this.ReOrder2Button.UseVisualStyleBackColor = true;
            this.ReOrder2Button.Click += new System.EventHandler(this.ReOrder2Button_Click);
            // 
            // X_ReOrderImage1Panel
            // 
            this.X_ReOrderImage1Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_ReOrderImage1Panel.Controls.Add(this.label5);
            this.X_ReOrderImage1Panel.Controls.Add(this.X_ReOrderImage1);
            this.X_ReOrderImage1Panel.Controls.Add(this.ReOrder1Button);
            this.X_ReOrderImage1Panel.Location = new System.Drawing.Point(446, 308);
            this.X_ReOrderImage1Panel.Name = "X_ReOrderImage1Panel";
            this.X_ReOrderImage1Panel.Size = new System.Drawing.Size(721, 170);
            this.X_ReOrderImage1Panel.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(179, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(510, 47);
            this.label5.TabIndex = 5;
            this.label5.Text = "できるだけ規定値になるように再構築をしてインポートします。";
            // 
            // X_ReOrderImage1
            // 
            this.X_ReOrderImage1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_ReOrderImage1.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_ReOrderImage1.Location = new System.Drawing.Point(3, -1);
            this.X_ReOrderImage1.Name = "X_ReOrderImage1";
            this.X_ReOrderImage1.Size = new System.Drawing.Size(170, 170);
            this.X_ReOrderImage1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.X_ReOrderImage1.TabIndex = 4;
            this.X_ReOrderImage1.TabStop = false;
            this.X_ReOrderImage1.Click += new System.EventHandler(this.X_ReOrderImage1_Click);
            // 
            // ReOrder1Button
            // 
            this.ReOrder1Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ReOrder1Button.Location = new System.Drawing.Point(179, 1);
            this.ReOrder1Button.Name = "ReOrder1Button";
            this.ReOrder1Button.Size = new System.Drawing.Size(510, 36);
            this.ReOrder1Button.TabIndex = 3;
            this.ReOrder1Button.Text = "規定値で再構築してインポート";
            this.ReOrder1Button.UseVisualStyleBackColor = true;
            this.ReOrder1Button.Click += new System.EventHandler(this.ReOrder1Button_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.X_OrignalImage);
            this.panel3.Controls.Add(this.ForceButton);
            this.panel3.Location = new System.Drawing.Point(447, 134);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(721, 170);
            this.panel3.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(179, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(509, 47);
            this.label3.TabIndex = 5;
            this.label3.Text = "規定値と違いますが、このまま強引にインポートします。";
            // 
            // X_OrignalImage
            // 
            this.X_OrignalImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_OrignalImage.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_OrignalImage.Location = new System.Drawing.Point(3, -1);
            this.X_OrignalImage.Name = "X_OrignalImage";
            this.X_OrignalImage.Size = new System.Drawing.Size(170, 170);
            this.X_OrignalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.X_OrignalImage.TabIndex = 4;
            this.X_OrignalImage.TabStop = false;
            this.X_OrignalImage.Click += new System.EventHandler(this.X_OrignalImage_Click);
            // 
            // ForceButton
            // 
            this.ForceButton.Location = new System.Drawing.Point(179, 1);
            this.ForceButton.Name = "ForceButton";
            this.ForceButton.Size = new System.Drawing.Size(510, 36);
            this.ForceButton.TabIndex = 4;
            this.ForceButton.Text = "無視して強行";
            this.ForceButton.UseVisualStyleBackColor = true;
            this.ForceButton.Visible = false;
            this.ForceButton.Click += new System.EventHandler(this.ForceButton_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.X_CancelImage);
            this.panel2.Controls.Add(this.CloseButton);
            this.panel2.Location = new System.Drawing.Point(447, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(721, 103);
            this.panel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(178, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(509, 47);
            this.label1.TabIndex = 5;
            this.label1.Text = "インポート処理をキャンセルします.";
            // 
            // X_CancelImage
            // 
            this.X_CancelImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_CancelImage.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_CancelImage.Location = new System.Drawing.Point(29, 3);
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
            this.CloseButton.Location = new System.Drawing.Point(178, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(510, 36);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "キャンセル";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.ErrorMessage = "";
            this.ErrorMessage.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.ErrorMessage.Location = new System.Drawing.Point(7, 27);
            this.ErrorMessage.Multiline = true;
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.Placeholder = "";
            this.ErrorMessage.Size = new System.Drawing.Size(433, 625);
            this.ErrorMessage.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(298, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "パレットが規定値と違います。";
            // 
            // ErrorPaletteShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(1194, 678);
            this.Controls.Add(this.panel1);
            this.Name = "ErrorPaletteShowForm";
            this.Text = "パレットが規定値と違います。";
            this.Load += new System.EventHandler(this.ErrorPaletteShowForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.X_ReOrderImage2Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_ReOrderImage2)).EndInit();
            this.X_ReOrderImage1Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_ReOrderImage1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_OrignalImage)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_CancelImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx ErrorMessage;
        private System.Windows.Forms.Button ForceButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Panel panel2;
        private InterpolatedPictureBox X_CancelImage;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private InterpolatedPictureBox X_OrignalImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel X_ReOrderImage2Panel;
        private System.Windows.Forms.Label label4;
        private InterpolatedPictureBox X_ReOrderImage2;
        private System.Windows.Forms.Button ReOrder2Button;
        private System.Windows.Forms.Panel X_ReOrderImage1Panel;
        private System.Windows.Forms.Label label5;
        private InterpolatedPictureBox X_ReOrderImage1;
        private System.Windows.Forms.Button ReOrder1Button;
    }
}