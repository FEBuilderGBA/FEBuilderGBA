namespace FEBuilderGBA
{
    partial class CStringForm
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
            this.WriteButton = new System.Windows.Forms.Button();
            this.String = new FEBuilderGBA.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.WriteButton);
            this.panel1.Controls.Add(this.String);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 145);
            this.panel1.TabIndex = 0;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(148, 100);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(185, 34);
            this.WriteButton.TabIndex = 2;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // String
            // 
            this.String.Location = new System.Drawing.Point(42, 66);
            this.String.Name = "String";
            this.String.Size = new System.Drawing.Size(290, 25);
            this.String.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "直接ROMに書き込まれた文字列";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CStringForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(371, 168);
            this.Controls.Add(this.panel1);
            this.Name = "CStringForm";
            this.Text = "文字列直編集";
            this.Load += new System.EventHandler(this.CStringForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button WriteButton;
        private FEBuilderGBA.TextBoxEx String;
    }
}