namespace FEBuilderGBA
{
    partial class ToolChangeProjectnameForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.ChangeButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.NewName = new FEBuilderGBA.TextBoxEx();
            this.CurrentName = new FEBuilderGBA.TextBoxEx();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "現在の名前";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "新しい名前";
            // 
            // ChangeButton
            // 
            this.ChangeButton.Location = new System.Drawing.Point(416, 155);
            this.ChangeButton.Name = "ChangeButton";
            this.ChangeButton.Size = new System.Drawing.Size(344, 42);
            this.ChangeButton.TabIndex = 1;
            this.ChangeButton.Text = "プロジェクト名の変更する";
            this.ChangeButton.UseVisualStyleBackColor = true;
            this.ChangeButton.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 217);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(745, 88);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "プロジェクトのファイル名の変更を安全に行います。\r\n過去のバックアップの名前も行います。";
            // 
            // NewName
            // 
            this.NewName.ErrorMessage = "";
            this.NewName.Location = new System.Drawing.Point(161, 102);
            this.NewName.Name = "NewName";
            this.NewName.Placeholder = "";
            this.NewName.Size = new System.Drawing.Size(599, 25);
            this.NewName.TabIndex = 0;
            // 
            // CurrentName
            // 
            this.CurrentName.ErrorMessage = "";
            this.CurrentName.Location = new System.Drawing.Point(161, 29);
            this.CurrentName.Name = "CurrentName";
            this.CurrentName.Placeholder = "";
            this.CurrentName.ReadOnly = true;
            this.CurrentName.Size = new System.Drawing.Size(599, 25);
            this.CurrentName.TabIndex = 2;
            // 
            // ToolChangeProjectnameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(775, 322);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ChangeButton);
            this.Controls.Add(this.NewName);
            this.Controls.Add(this.CurrentName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ToolChangeProjectnameForm";
            this.Text = "プロジェクト名の変更";
            this.Load += new System.EventHandler(this.ToolChangeProjectnameForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TextBoxEx CurrentName;
        private TextBoxEx NewName;
        private System.Windows.Forms.Button ChangeButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}