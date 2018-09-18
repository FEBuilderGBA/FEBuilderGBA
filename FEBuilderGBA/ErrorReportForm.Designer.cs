namespace FEBuilderGBA
{
    partial class ErrorReportForm
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
            this.ERROR_label1 = new System.Windows.Forms.Label();
            this.ErrorIcon = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SendErrorMessageButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.StackTrace = new FEBuilderGBA.TextBoxEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ErrorMessage = new FEBuilderGBA.TextBoxEx();
            this.MainTab = new System.Windows.Forms.TabControl();
            this.UserMessage = new FEBuilderGBA.TextBoxEx();
            this.ERROR_label3 = new System.Windows.Forms.Label();
            this.NeedUpdatePage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.X_NEED_UPDATE = new FEBuilderGBA.TextBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorIcon)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.MainTab.SuspendLayout();
            this.NeedUpdatePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ERROR_label1
            // 
            this.ERROR_label1.AutoSize = true;
            this.ERROR_label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ERROR_label1.ForeColor = System.Drawing.Color.Red;
            this.ERROR_label1.Location = new System.Drawing.Point(50, 12);
            this.ERROR_label1.Name = "ERROR_label1";
            this.ERROR_label1.Size = new System.Drawing.Size(575, 24);
            this.ERROR_label1.TabIndex = 0;
            this.ERROR_label1.Text = "未知のエラーが発生しました。開発者までご連絡ください。";
            // 
            // ErrorIcon
            // 
            this.ErrorIcon.Location = new System.Drawing.Point(12, 9);
            this.ErrorIcon.Name = "ErrorIcon";
            this.ErrorIcon.Size = new System.Drawing.Size(32, 32);
            this.ErrorIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ErrorIcon.TabIndex = 1;
            this.ErrorIcon.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "何をしたらエラーが起きたのか教えてください";
            // 
            // SendErrorMessageButton
            // 
            this.SendErrorMessageButton.Location = new System.Drawing.Point(706, 460);
            this.SendErrorMessageButton.Name = "SendErrorMessageButton";
            this.SendErrorMessageButton.Size = new System.Drawing.Size(312, 41);
            this.SendErrorMessageButton.TabIndex = 7;
            this.SendErrorMessageButton.Text = "エラーを開発者に報告する";
            this.SendErrorMessageButton.UseVisualStyleBackColor = true;
            this.SendErrorMessageButton.Click += new System.EventHandler(this.SendErrorMessageButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.StackTrace);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(998, 216);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "スタックトレース";
            // 
            // StackTrace
            // 
            this.StackTrace.ErrorMessage = "";
            this.StackTrace.Location = new System.Drawing.Point(7, 6);
            this.StackTrace.Multiline = true;
            this.StackTrace.Name = "StackTrace";
            this.StackTrace.Placeholder = "";
            this.StackTrace.Size = new System.Drawing.Size(984, 204);
            this.StackTrace.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.ErrorMessage);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(998, 216);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "エラーメッセージ";
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.ErrorMessage = "";
            this.ErrorMessage.Location = new System.Drawing.Point(6, 6);
            this.ErrorMessage.Multiline = true;
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.Placeholder = "";
            this.ErrorMessage.Size = new System.Drawing.Size(984, 204);
            this.ErrorMessage.TabIndex = 9;
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.tabPage1);
            this.MainTab.Controls.Add(this.tabPage2);
            this.MainTab.Controls.Add(this.NeedUpdatePage);
            this.MainTab.Location = new System.Drawing.Point(12, 54);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(1006, 248);
            this.MainTab.TabIndex = 10;
            // 
            // UserMessage
            // 
            this.UserMessage.ErrorMessage = "";
            this.UserMessage.Location = new System.Drawing.Point(11, 344);
            this.UserMessage.Multiline = true;
            this.UserMessage.Name = "UserMessage";
            this.UserMessage.Placeholder = "";
            this.UserMessage.Size = new System.Drawing.Size(1007, 96);
            this.UserMessage.TabIndex = 2;
            // 
            // ERROR_label3
            // 
            this.ERROR_label3.AutoSize = true;
            this.ERROR_label3.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ERROR_label3.ForeColor = System.Drawing.Color.Red;
            this.ERROR_label3.Location = new System.Drawing.Point(15, 469);
            this.ERROR_label3.Name = "ERROR_label3";
            this.ERROR_label3.Size = new System.Drawing.Size(513, 20);
            this.ERROR_label3.TabIndex = 11;
            this.ERROR_label3.Text = "このボタンを押して開発者にエラーを報告してください。 ---->";
            // 
            // NeedUpdatePage
            // 
            this.NeedUpdatePage.BackColor = System.Drawing.SystemColors.Control;
            this.NeedUpdatePage.Controls.Add(this.X_NEED_UPDATE);
            this.NeedUpdatePage.Controls.Add(this.label1);
            this.NeedUpdatePage.Location = new System.Drawing.Point(4, 28);
            this.NeedUpdatePage.Name = "NeedUpdatePage";
            this.NeedUpdatePage.Padding = new System.Windows.Forms.Padding(3);
            this.NeedUpdatePage.Size = new System.Drawing.Size(998, 216);
            this.NeedUpdatePage.TabIndex = 2;
            this.NeedUpdatePage.Text = "アップデートを確認してください";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(253, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "アップデートを確認してください";
            // 
            // X_NEED_UPDATE
            // 
            this.X_NEED_UPDATE.ErrorMessage = "";
            this.X_NEED_UPDATE.Location = new System.Drawing.Point(10, 49);
            this.X_NEED_UPDATE.Multiline = true;
            this.X_NEED_UPDATE.Name = "X_NEED_UPDATE";
            this.X_NEED_UPDATE.Placeholder = "";
            this.X_NEED_UPDATE.Size = new System.Drawing.Size(888, 151);
            this.X_NEED_UPDATE.TabIndex = 14;
            // 
            // ErrorReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1036, 522);
            this.Controls.Add(this.ERROR_label3);
            this.Controls.Add(this.MainTab);
            this.Controls.Add(this.UserMessage);
            this.Controls.Add(this.SendErrorMessageButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ErrorIcon);
            this.Controls.Add(this.ERROR_label1);
            this.Name = "ErrorReportForm";
            this.Text = "An unknown error occurred. Please contact the developer.";
            this.Load += new System.EventHandler(this.ErrorReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorIcon)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.MainTab.ResumeLayout(false);
            this.NeedUpdatePage.ResumeLayout(false);
            this.NeedUpdatePage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ERROR_label1;
        private System.Windows.Forms.PictureBox ErrorIcon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SendErrorMessageButton;
        private TextBoxEx UserMessage;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private TextBoxEx ErrorMessage;
        private System.Windows.Forms.TabControl MainTab;
        private TextBoxEx StackTrace;
        private System.Windows.Forms.Label ERROR_label3;
        private System.Windows.Forms.TabPage NeedUpdatePage;
        private System.Windows.Forms.Label label1;
        private TextBoxEx X_NEED_UPDATE;
    }
}