namespace FEBuilderGBA
{
    partial class PointerToolBatchInputForm
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
            this.TextData = new FEBuilderGBA.TextBoxEx();
            this.RunButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "値:";
            // 
            // TextData
            // 
            this.TextData.ErrorMessage = "";
            this.TextData.Location = new System.Drawing.Point(13, 40);
            this.TextData.Multiline = true;
            this.TextData.Name = "TextData";
            this.TextData.Placeholder = "";
            this.TextData.Size = new System.Drawing.Size(877, 455);
            this.TextData.TabIndex = 3;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(655, 501);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(235, 34);
            this.RunButton.TabIndex = 4;
            this.RunButton.Text = "一括アドレス変換";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // PointerToolBatchInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(902, 547);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.TextData);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PointerToolBatchInputForm";
            this.Text = "コピー方法";
            this.Load += new System.EventHandler(this.PointerToolCopyToForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private TextBoxEx TextData;
        private System.Windows.Forms.Button RunButton;
    }
}