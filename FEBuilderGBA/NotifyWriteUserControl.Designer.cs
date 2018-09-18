namespace FEBuilderGBA
{
    partial class NotifyWriteUserControl
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
            this.SuspendLayout();
            // 
            // BIG_TEXT
            // 
            this.BIG_TEXT.AutoSize = true;
            this.BIG_TEXT.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BIG_TEXT.ForeColor = System.Drawing.Color.Black;
            this.BIG_TEXT.Location = new System.Drawing.Point(89, 24);
            this.BIG_TEXT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BIG_TEXT.Name = "BIG_TEXT";
            this.BIG_TEXT.Size = new System.Drawing.Size(388, 33);
            this.BIG_TEXT.TabIndex = 0;
            this.BIG_TEXT.Text = "アドレス{}に書き込みました。";
            // 
            // NotifyWriteUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Yellow;
            this.Controls.Add(this.BIG_TEXT);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NotifyWriteUserControl";
            this.Size = new System.Drawing.Size(531, 91);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BIG_TEXT;
    }
}
