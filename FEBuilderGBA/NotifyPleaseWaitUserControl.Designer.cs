namespace FEBuilderGBA
{
    partial class NotifyPleaseWaitUserControl
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
            this.MessageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BIG_TEXT
            // 
            this.BIG_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BIG_TEXT.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.BIG_TEXT.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BIG_TEXT.Location = new System.Drawing.Point(1, 0);
            this.BIG_TEXT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BIG_TEXT.Name = "BIG_TEXT";
            this.BIG_TEXT.Size = new System.Drawing.Size(850, 235);
            this.BIG_TEXT.TabIndex = 0;
            this.BIG_TEXT.Text = "しばらくお待ちください....";
            this.BIG_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MessageLabel
            // 
            this.MessageLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MessageLabel.Location = new System.Drawing.Point(4, 214);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(847, 23);
            this.MessageLabel.TabIndex = 1;
            this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NotifyPleaseWaitUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.BIG_TEXT);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NotifyPleaseWaitUserControl";
            this.Size = new System.Drawing.Size(855, 237);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label BIG_TEXT;
        private System.Windows.Forms.Label MessageLabel;
    }
}
