namespace FEBuilderGBA
{
    partial class ToolDiffDebugSelectMethodPopup
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
            this.Method1Button = new System.Windows.Forms.Button();
            this.Method2Button = new System.Windows.Forms.Button();
            this.Explain = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Method1Button
            // 
            this.Method1Button.Location = new System.Drawing.Point(12, 181);
            this.Method1Button.Name = "Method1Button";
            this.Method1Button.Size = new System.Drawing.Size(735, 111);
            this.Method1Button.TabIndex = 0;
            this.Method1Button.Text = "OK ROMとNG ROMで値が違うもの\r\n且つ、OK ROMとCURRENTで値が違うもの\r\n且つ、NG ROMとCURRENTで値が同じもの\r\n(NG ROM" +
    "とCURRENTで同じバグが発生する場合はこちらを選択してください)";
            this.Method1Button.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Method1Button.UseVisualStyleBackColor = true;
            this.Method1Button.Click += new System.EventHandler(this.Method1Button_Click);
            // 
            // Method2Button
            // 
            this.Method2Button.Location = new System.Drawing.Point(12, 315);
            this.Method2Button.Name = "Method2Button";
            this.Method2Button.Size = new System.Drawing.Size(735, 118);
            this.Method2Button.TabIndex = 1;
            this.Method2Button.Text = "OK ROMとNG ROMで値が違うもの\r\n且つ、OK ROMとCURRENTで値が違うもの\r\n(NG ROMとCURRENTでバグの内容が違う場合はこちらを選択" +
    "してください)";
            this.Method2Button.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Method2Button.UseVisualStyleBackColor = true;
            this.Method2Button.Click += new System.EventHandler(this.Method2Button_Click);
            // 
            // Explain
            // 
            this.Explain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Explain.Location = new System.Drawing.Point(11, 9);
            this.Explain.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Explain.Name = "Explain";
            this.Explain.Size = new System.Drawing.Size(736, 158);
            this.Explain.TabIndex = 58;
            // 
            // ToolDiffDebugSelectMethodPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(759, 473);
            this.Controls.Add(this.Explain);
            this.Controls.Add(this.Method2Button);
            this.Controls.Add(this.Method1Button);
            this.Name = "ToolDiffDebugSelectMethodPopup";
            this.Text = "比較方法";
            this.Load += new System.EventHandler(this.ToolDiffDebugSelectMethodPopup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Method1Button;
        private System.Windows.Forms.Button Method2Button;
        private System.Windows.Forms.Label Explain;
    }
}