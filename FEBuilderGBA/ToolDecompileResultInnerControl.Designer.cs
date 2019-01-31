namespace FEBuilderGBA
{
    partial class ToolDecompileResultInnerControl
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
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.Code = new FEBuilderGBA.RichTextBoxEx();
            this.SuspendLayout();
            // 
            // Code
            // 
            this.Code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Code.ErrorMessage = "";
            this.Code.Location = new System.Drawing.Point(0, 0);
            this.Code.Name = "Code";
            this.Code.Placeholder = "";
            this.Code.Size = new System.Drawing.Size(469, 360);
            this.Code.TabIndex = 0;
            this.Code.Text = "";
            // 
            // ToolDecompileResultInnerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.Code);
            this.Name = "ToolDecompileResultInnerControl";
            this.Size = new System.Drawing.Size(469, 360);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBoxEx Code;
    }
}
