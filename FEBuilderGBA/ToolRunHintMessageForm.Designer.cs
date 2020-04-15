namespace FEBuilderGBA
{
    partial class ToolRunHintMessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolRunHintMessageForm));
            this.label1 = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.DoNotShowThisMessageAgain = new System.Windows.Forms.CheckBox();
            this.Detail = new FEBuilderGBA.TextBoxEx();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(26, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(314, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "これよりテスト実行を開始します。";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(580, 399);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(339, 33);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "開始";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // DoNotShowThisMessageAgain
            // 
            this.DoNotShowThisMessageAgain.AutoSize = true;
            this.DoNotShowThisMessageAgain.Location = new System.Drawing.Point(29, 405);
            this.DoNotShowThisMessageAgain.Name = "DoNotShowThisMessageAgain";
            this.DoNotShowThisMessageAgain.Size = new System.Drawing.Size(257, 22);
            this.DoNotShowThisMessageAgain.TabIndex = 3;
            this.DoNotShowThisMessageAgain.Text = "このメッセージを再度表示しない";
            this.DoNotShowThisMessageAgain.UseVisualStyleBackColor = true;
            // 
            // Detail
            // 
            this.Detail.ErrorMessage = "";
            this.Detail.Location = new System.Drawing.Point(29, 61);
            this.Detail.Multiline = true;
            this.Detail.Name = "Detail";
            this.Detail.Placeholder = "";
            this.Detail.Size = new System.Drawing.Size(890, 332);
            this.Detail.TabIndex = 0;
            this.Detail.Text = resources.GetString("Detail.Text");
            // 
            // ToolRunHintMessageForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 449);
            this.Controls.Add(this.DoNotShowThisMessageAgain);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Detail);
            this.Name = "ToolRunHintMessageForm";
            this.Text = "テスト実行";
            this.Load += new System.EventHandler(this.ToolRunHintMessageForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBoxEx Detail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.CheckBox DoNotShowThisMessageAgain;
    }
}