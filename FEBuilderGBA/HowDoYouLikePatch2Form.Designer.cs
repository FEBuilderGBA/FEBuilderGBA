namespace FEBuilderGBA
{
    partial class HowDoYouLikePatch2Form
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
            this.NoRecommedPatchCheckBox = new System.Windows.Forms.CheckBox();
            this.FormIcon = new System.Windows.Forms.PictureBox();
            this.ReasonLabel = new System.Windows.Forms.Label();
            this.EnableButton = new System.Windows.Forms.Button();
            this.CancelCloseButton = new System.Windows.Forms.Button();
            this.EnableButton3 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.FormIcon);
            this.panel1.Controls.Add(this.ReasonLabel);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(808, 144);
            this.panel1.TabIndex = 0;
            // 
            // NoRecommedPatchCheckBox
            // 
            this.NoRecommedPatchCheckBox.AccessibleDescription = "@NORECOMMEDPATCH";
            this.NoRecommedPatchCheckBox.AutoSize = true;
            this.NoRecommedPatchCheckBox.Location = new System.Drawing.Point(595, 276);
            this.NoRecommedPatchCheckBox.Name = "NoRecommedPatchCheckBox";
            this.NoRecommedPatchCheckBox.Size = new System.Drawing.Size(193, 22);
            this.NoRecommedPatchCheckBox.TabIndex = 7;
            this.NoRecommedPatchCheckBox.Text = "このパッチを推奨しない";
            this.NoRecommedPatchCheckBox.UseVisualStyleBackColor = true;
            // 
            // FormIcon
            // 
            this.FormIcon.Location = new System.Drawing.Point(8, 9);
            this.FormIcon.Name = "FormIcon";
            this.FormIcon.Size = new System.Drawing.Size(128, 128);
            this.FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FormIcon.TabIndex = 5;
            this.FormIcon.TabStop = false;
            // 
            // ReasonLabel
            // 
            this.ReasonLabel.Location = new System.Drawing.Point(155, 13);
            this.ReasonLabel.Name = "ReasonLabel";
            this.ReasonLabel.Size = new System.Drawing.Size(650, 124);
            this.ReasonLabel.TabIndex = 1;
            this.ReasonLabel.Text = "Reason";
            // 
            // EnableButton
            // 
            this.EnableButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.EnableButton.Location = new System.Drawing.Point(15, 168);
            this.EnableButton.Name = "EnableButton";
            this.EnableButton.Size = new System.Drawing.Size(806, 35);
            this.EnableButton.TabIndex = 0;
            this.EnableButton.Text = "Enable";
            this.EnableButton.UseVisualStyleBackColor = true;
            // 
            // CancelCloseButton
            // 
            this.CancelCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelCloseButton.Location = new System.Drawing.Point(15, 304);
            this.CancelCloseButton.Name = "CancelCloseButton";
            this.CancelCloseButton.Size = new System.Drawing.Size(806, 35);
            this.CancelCloseButton.TabIndex = 2;
            this.CancelCloseButton.Text = "無視して続行する";
            this.CancelCloseButton.UseVisualStyleBackColor = true;
            this.CancelCloseButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EnableButton3
            // 
            this.EnableButton3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.EnableButton3.Location = new System.Drawing.Point(15, 209);
            this.EnableButton3.Name = "EnableButton3";
            this.EnableButton3.Size = new System.Drawing.Size(806, 35);
            this.EnableButton3.TabIndex = 1;
            this.EnableButton3.Text = "Enable";
            this.EnableButton3.UseVisualStyleBackColor = true;
            // 
            // HowDoYouLikePatch2Form
            // 
            this.AcceptButton = this.EnableButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.CancelCloseButton;
            this.ClientSize = new System.Drawing.Size(833, 351);
            this.Controls.Add(this.NoRecommedPatchCheckBox);
            this.Controls.Add(this.EnableButton3);
            this.Controls.Add(this.CancelCloseButton);
            this.Controls.Add(this.EnableButton);
            this.Controls.Add(this.panel1);
            this.Name = "HowDoYouLikePatch2Form";
            this.Load += new System.EventHandler(this.HowDoYouLikePatchForm2_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ReasonLabel;
        private System.Windows.Forms.Button EnableButton;
        private System.Windows.Forms.Button CancelCloseButton;
        private System.Windows.Forms.PictureBox FormIcon;
        private System.Windows.Forms.Button EnableButton3;
        private System.Windows.Forms.CheckBox NoRecommedPatchCheckBox;
    }
}