namespace FEBuilderGBA
{
    partial class ToolWorkSupport_UpdateQuestionDialogForm
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
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.labelEx1 = new FEBuilderGBA.LabelEx();
            this.FormIcon = new System.Windows.Forms.PictureBox();
            this.ForceUpdatebutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(12, 99);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(317, 42);
            this.MyCancelButton.TabIndex = 7;
            this.MyCancelButton.Text = "閉じる";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // labelEx1
            // 
            this.labelEx1.ErrorMessage = "";
            this.labelEx1.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEx1.Location = new System.Drawing.Point(82, 9);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(717, 87);
            this.labelEx1.TabIndex = 4;
            this.labelEx1.Text = "現在のバージョンが最新です。更新する必要はありません。version:{0}";
            // 
            // FormIcon
            // 
            this.FormIcon.Location = new System.Drawing.Point(12, 8);
            this.FormIcon.Name = "FormIcon";
            this.FormIcon.Size = new System.Drawing.Size(64, 64);
            this.FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FormIcon.TabIndex = 15;
            this.FormIcon.TabStop = false;
            // 
            // ForceUpdatebutton
            // 
            this.ForceUpdatebutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ForceUpdatebutton.Location = new System.Drawing.Point(349, 99);
            this.ForceUpdatebutton.Name = "ForceUpdatebutton";
            this.ForceUpdatebutton.Size = new System.Drawing.Size(450, 42);
            this.ForceUpdatebutton.TabIndex = 16;
            this.ForceUpdatebutton.Text = "強制アップデート";
            this.ForceUpdatebutton.UseVisualStyleBackColor = true;
            this.ForceUpdatebutton.Click += new System.EventHandler(this.ForceUpdatebutton_Click);
            // 
            // ToolWorkSupport_UpdateQuestionDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(815, 160);
            this.Controls.Add(this.ForceUpdatebutton);
            this.Controls.Add(this.FormIcon);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.labelEx1);
            this.Name = "ToolWorkSupport_UpdateQuestionDialogForm";
            this.Text = "現在のバージョンが最新です";
            this.Load += new System.EventHandler(this.ToolWorkSupport_UpdateQuestionDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MyCancelButton;
        private LabelEx labelEx1;
        private System.Windows.Forms.PictureBox FormIcon;
        private System.Windows.Forms.Button ForceUpdatebutton;
    }
}