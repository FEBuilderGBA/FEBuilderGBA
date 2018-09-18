namespace FEBuilderGBA
{
    partial class SearchHelperControl
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
            this.searchlabel = new System.Windows.Forms.Label();
            this.EntryButton = new System.Windows.Forms.Button();
            this.SearchWord = new FEBuilderGBA.TextBoxEx();
            this.NextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // searchlabel
            // 
            this.searchlabel.AutoSize = true;
            this.searchlabel.Location = new System.Drawing.Point(3, 0);
            this.searchlabel.Name = "searchlabel";
            this.searchlabel.Size = new System.Drawing.Size(44, 18);
            this.searchlabel.TabIndex = 0;
            this.searchlabel.Text = "検索";
            // 
            // EntryButton
            // 
            this.EntryButton.Location = new System.Drawing.Point(145, 76);
            this.EntryButton.Name = "EntryButton";
            this.EntryButton.Size = new System.Drawing.Size(86, 30);
            this.EntryButton.TabIndex = 2;
            this.EntryButton.Text = "閉じる";
            this.EntryButton.UseVisualStyleBackColor = true;
            this.EntryButton.Click += new System.EventHandler(this.EntryButton_Click);
            // 
            // SearchWord
            // 
            this.SearchWord.ErrorMessage = "";
            this.SearchWord.Location = new System.Drawing.Point(4, 25);
            this.SearchWord.Name = "SearchWord";
            this.SearchWord.Placeholder = "";
            this.SearchWord.Size = new System.Drawing.Size(226, 25);
            this.SearchWord.TabIndex = 1;
            this.SearchWord.TextChanged += new System.EventHandler(this.SearchWord_TextChanged);
            this.SearchWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchWord_KeyDown);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(6, 76);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(118, 30);
            this.NextButton.TabIndex = 3;
            this.NextButton.Text = "次を検索";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // SearchHelperControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.EntryButton);
            this.Controls.Add(this.SearchWord);
            this.Controls.Add(this.searchlabel);
            this.Name = "SearchHelperControl";
            this.Size = new System.Drawing.Size(234, 110);
            this.Load += new System.EventHandler(this.SearchHelperControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label searchlabel;
        public System.Windows.Forms.Button EntryButton;
        public FEBuilderGBA.TextBoxEx SearchWord;
        public System.Windows.Forms.Button NextButton;
    }
}
