namespace FEBuilderGBA
{
    partial class MainSimpleMenuEventErrorIgnoreErrorForm
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
            this.ErrorTextBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.CommentTextBox = new FEBuilderGBA.TextBoxEx();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "このエラーを非表示にしますか？";
            // 
            // ErrorTextBox
            // 
            this.ErrorTextBox.Location = new System.Drawing.Point(16, 45);
            this.ErrorTextBox.Multiline = true;
            this.ErrorTextBox.Name = "ErrorTextBox";
            this.ErrorTextBox.ReadOnly = true;
            this.ErrorTextBox.Size = new System.Drawing.Size(776, 121);
            this.ErrorTextBox.TabIndex = 1;
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKButton.Location = new System.Drawing.Point(579, 266);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(213, 45);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "エラーを非表示にする";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(16, 266);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(188, 45);
            this.MyCancelButton.TabIndex = 1;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "非表示にする理由";
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.ErrorMessage = "";
            this.CommentTextBox.Location = new System.Drawing.Point(16, 217);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Placeholder = "";
            this.CommentTextBox.Size = new System.Drawing.Size(776, 25);
            this.CommentTextBox.TabIndex = 0;
            // 
            // MainSimpleMenuEventErrorIgnoreErrorForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(804, 323);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.ErrorTextBox);
            this.Controls.Add(this.label1);
            this.Name = "MainSimpleMenuEventErrorIgnoreErrorForm";
            this.Text = "エラーを非表示にする";
            this.Load += new System.EventHandler(this.MainSimpleMenuEventErrorIgnoreErrorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ErrorTextBox;
        private TextBoxEx CommentTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Label label2;
    }
}