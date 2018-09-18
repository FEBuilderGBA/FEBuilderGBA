namespace FEBuilderGBA
{
    partial class ToolThreeMargeCloseAlertForm
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
            this.labelEx1 = new FEBuilderGBA.LabelEx();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.NoButton = new System.Windows.Forms.Button();
            this.YesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelEx1
            // 
            this.labelEx1.ErrorMessage = "";
            this.labelEx1.Location = new System.Drawing.Point(13, 13);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(787, 140);
            this.labelEx1.TabIndex = 0;
            this.labelEx1.Text = "比較ツールを終了してもよろしいですか？\r\n現在、一部分だけ修正した状態になっています。\r\nこのままツールを閉じてもよろしいですか？\r\n\r\nマージをやめる場合、変更" +
    "をすべてキャンセルして、終了してください。";
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(13, 156);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(787, 42);
            this.MyCancelButton.TabIndex = 1;
            this.MyCancelButton.Text = "終了せずに、まだマージ作業を続けます。";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // NoButton
            // 
            this.NoButton.Location = new System.Drawing.Point(13, 215);
            this.NoButton.Name = "NoButton";
            this.NoButton.Size = new System.Drawing.Size(787, 42);
            this.NoButton.TabIndex = 2;
            this.NoButton.Text = "マージをやめます。マージでの変更点をすべてキャンセルします。";
            this.NoButton.UseVisualStyleBackColor = true;
            this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
            // 
            // YesButton
            // 
            this.YesButton.Location = new System.Drawing.Point(13, 296);
            this.YesButton.Name = "YesButton";
            this.YesButton.Size = new System.Drawing.Size(787, 42);
            this.YesButton.TabIndex = 3;
            this.YesButton.Text = "現在の結果で強制終了します。";
            this.YesButton.UseVisualStyleBackColor = true;
            this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
            // 
            // ToolThreeMargeCloseAlertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(812, 352);
            this.Controls.Add(this.YesButton);
            this.Controls.Add(this.NoButton);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.labelEx1);
            this.Name = "ToolThreeMargeCloseAlertForm";
            this.Text = "比較ツールを終了してもよろしいですか？";
            this.ResumeLayout(false);

        }

        #endregion

        private LabelEx labelEx1;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Button NoButton;
        private System.Windows.Forms.Button YesButton;
    }
}