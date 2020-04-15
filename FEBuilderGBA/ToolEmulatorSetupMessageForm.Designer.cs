namespace FEBuilderGBA
{
    partial class ToolEmulatorSetupMessageForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.UseInitWizardButton = new System.Windows.Forms.Button();
            this.UseOptionManualButton2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(381, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "エミュレータが設定されていません。\r\n動作テストに利用するエミュレータを設定してください。";
            // 
            // UseInitWizardButton
            // 
            this.UseInitWizardButton.Location = new System.Drawing.Point(25, 94);
            this.UseInitWizardButton.Name = "UseInitWizardButton";
            this.UseInitWizardButton.Size = new System.Drawing.Size(653, 37);
            this.UseInitWizardButton.TabIndex = 1;
            this.UseInitWizardButton.Text = "InitWizardで自動設定する";
            this.UseInitWizardButton.UseVisualStyleBackColor = true;
            this.UseInitWizardButton.Click += new System.EventHandler(this.UseInitWizardButton_Click);
            // 
            // UseOptionManualButton2
            // 
            this.UseOptionManualButton2.Location = new System.Drawing.Point(25, 147);
            this.UseOptionManualButton2.Name = "UseOptionManualButton2";
            this.UseOptionManualButton2.Size = new System.Drawing.Size(653, 37);
            this.UseOptionManualButton2.TabIndex = 2;
            this.UseOptionManualButton2.Text = "Option画面から手動で設定する";
            this.UseOptionManualButton2.UseVisualStyleBackColor = true;
            this.UseOptionManualButton2.Click += new System.EventHandler(this.UseOptionManualButton2_Click);
            // 
            // ToolEmulatorSetupMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 215);
            this.Controls.Add(this.UseOptionManualButton2);
            this.Controls.Add(this.UseInitWizardButton);
            this.Controls.Add(this.label1);
            this.Name = "ToolEmulatorSetupMessageForm";
            this.Text = "エミュレータが設定されていません";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button UseInitWizardButton;
        private System.Windows.Forms.Button UseOptionManualButton2;
    }
}