namespace FEBuilderGBA
{
    partial class ErrorLongMessageDialogForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.MyCloseButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ErrorMessage = new FEBuilderGBA.RichTextBoxEx();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(302, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "以下のエラーが発生しました。";
            // 
            // MyCloseButton
            // 
            this.MyCloseButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.MyCloseButton.Location = new System.Drawing.Point(981, 0);
            this.MyCloseButton.Name = "MyCloseButton";
            this.MyCloseButton.Size = new System.Drawing.Size(123, 43);
            this.MyCloseButton.TabIndex = 3;
            this.MyCloseButton.Text = "Close";
            this.MyCloseButton.UseVisualStyleBackColor = true;
            this.MyCloseButton.Click += new System.EventHandler(this.MyCloseButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MyCloseButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1104, 43);
            this.panel1.TabIndex = 4;
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorMessage.ErrorMessage = "";
            this.ErrorMessage.Location = new System.Drawing.Point(0, 43);
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.Placeholder = "";
            this.ErrorMessage.Size = new System.Drawing.Size(1104, 610);
            this.ErrorMessage.TabIndex = 0;
            this.ErrorMessage.Text = "";
            this.ErrorMessage.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.ErrorMessage_LinkClicked);
            // 
            // ErrorLongMessageDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 653);
            this.Controls.Add(this.ErrorMessage);
            this.Controls.Add(this.panel1);
            this.Name = "ErrorLongMessageDialogForm";
            this.Text = "エラー";
            this.Load += new System.EventHandler(this.ErrorLongMessageDialogForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ErrorLongMessageDialogForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBoxEx ErrorMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button MyCloseButton;
        private System.Windows.Forms.Panel panel1;
    }
}