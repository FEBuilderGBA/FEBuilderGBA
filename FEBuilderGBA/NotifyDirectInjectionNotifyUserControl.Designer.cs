namespace FEBuilderGBA
{
    partial class NotifyDirectInjectionNotifyUserControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.BIG_TEXT = new System.Windows.Forms.Label();
            this.AllowLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BIG_TEXT
            // 
            this.BIG_TEXT.AutoSize = true;
            this.BIG_TEXT.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BIG_TEXT.ForeColor = System.Drawing.Color.Black;
            this.BIG_TEXT.Location = new System.Drawing.Point(68, 9);
            this.BIG_TEXT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BIG_TEXT.Name = "BIG_TEXT";
            this.BIG_TEXT.Size = new System.Drawing.Size(535, 33);
            this.BIG_TEXT.TabIndex = 0;
            this.BIG_TEXT.Text = "このリストをダブルクリックで選択できます";
            // 
            // AllowLabel
            // 
            this.AllowLabel.AutoSize = true;
            this.AllowLabel.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.AllowLabel.ForeColor = System.Drawing.Color.Black;
            this.AllowLabel.Location = new System.Drawing.Point(19, 9);
            this.AllowLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AllowLabel.Name = "AllowLabel";
            this.AllowLabel.Size = new System.Drawing.Size(49, 33);
            this.AllowLabel.TabIndex = 1;
            this.AllowLabel.Text = "↓";
            // 
            // NotifyDirectInjectionNotifyUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.YellowGreen;
            this.Controls.Add(this.AllowLabel);
            this.Controls.Add(this.BIG_TEXT);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NotifyDirectInjectionNotifyUserControl";
            this.Size = new System.Drawing.Size(658, 51);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BIG_TEXT;
        private System.Windows.Forms.Label AllowLabel;
    }
}
