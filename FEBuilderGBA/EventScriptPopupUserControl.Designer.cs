namespace FEBuilderGBA
{
    partial class EventScriptPopupUserControl
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
            this.Tab = new System.Windows.Forms.TabControl();
            this.MAPTabPage = new System.Windows.Forms.TabPage();
            this.PICTabPage = new System.Windows.Forms.TabPage();
            this.MusicPagePage = new System.Windows.Forms.TabPage();
            this.PlaySappyButton = new System.Windows.Forms.Button();
            this.MAP = new FEBuilderGBA.MapPictureBox();
            this.PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.MusicName = new FEBuilderGBA.TextBoxEx();
            this.Tab.SuspendLayout();
            this.MAPTabPage.SuspendLayout();
            this.PICTabPage.SuspendLayout();
            this.MusicPagePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PIC)).BeginInit();
            this.SuspendLayout();
            // 
            // Tab
            // 
            this.Tab.Controls.Add(this.MAPTabPage);
            this.Tab.Controls.Add(this.PICTabPage);
            this.Tab.Controls.Add(this.MusicPagePage);
            this.Tab.Location = new System.Drawing.Point(0, 0);
            this.Tab.Name = "Tab";
            this.Tab.SelectedIndex = 0;
            this.Tab.Size = new System.Drawing.Size(380, 300);
            this.Tab.TabIndex = 0;
            // 
            // MAPTabPage
            // 
            this.MAPTabPage.Controls.Add(this.MAP);
            this.MAPTabPage.Location = new System.Drawing.Point(4, 28);
            this.MAPTabPage.Name = "MAPTabPage";
            this.MAPTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.MAPTabPage.Size = new System.Drawing.Size(372, 268);
            this.MAPTabPage.TabIndex = 0;
            this.MAPTabPage.Text = "MAPTabPage";
            this.MAPTabPage.UseVisualStyleBackColor = true;
            // 
            // PICTabPage
            // 
            this.PICTabPage.Controls.Add(this.PIC);
            this.PICTabPage.Location = new System.Drawing.Point(4, 28);
            this.PICTabPage.Name = "PICTabPage";
            this.PICTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PICTabPage.Size = new System.Drawing.Size(372, 268);
            this.PICTabPage.TabIndex = 1;
            this.PICTabPage.Text = "PICTabPage";
            this.PICTabPage.UseVisualStyleBackColor = true;
            // 
            // MusicPagePage
            // 
            this.MusicPagePage.BackColor = System.Drawing.SystemColors.Control;
            this.MusicPagePage.Controls.Add(this.MusicName);
            this.MusicPagePage.Controls.Add(this.PlaySappyButton);
            this.MusicPagePage.Location = new System.Drawing.Point(4, 28);
            this.MusicPagePage.Name = "MusicPagePage";
            this.MusicPagePage.Padding = new System.Windows.Forms.Padding(3);
            this.MusicPagePage.Size = new System.Drawing.Size(372, 268);
            this.MusicPagePage.TabIndex = 3;
            this.MusicPagePage.Text = "MusicPage";
            // 
            // PlaySappyButton
            // 
            this.PlaySappyButton.Location = new System.Drawing.Point(-2, -2);
            this.PlaySappyButton.Name = "PlaySappyButton";
            this.PlaySappyButton.Size = new System.Drawing.Size(298, 44);
            this.PlaySappyButton.TabIndex = 172;
            this.PlaySappyButton.Text = "曲をSappyで再生する";
            this.PlaySappyButton.UseVisualStyleBackColor = true;
            this.PlaySappyButton.Click += new System.EventHandler(this.PlaySappyButton_Click);
            // 
            // MAP
            // 
            this.MAP.AutoScroll = true;
            this.MAP.Location = new System.Drawing.Point(-3, 0);
            this.MAP.Margin = new System.Windows.Forms.Padding(2);
            this.MAP.Name = "MAP";
            this.MAP.Size = new System.Drawing.Size(369, 270);
            this.MAP.TabIndex = 1;
            // 
            // PIC
            // 
            this.PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.PIC.Location = new System.Drawing.Point(-2, 0);
            this.PIC.Name = "PIC";
            this.PIC.Size = new System.Drawing.Size(365, 270);
            this.PIC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PIC.TabIndex = 0;
            this.PIC.TabStop = false;
            // 
            // MusicName
            // 
            this.MusicName.ErrorMessage = "";
            this.MusicName.Location = new System.Drawing.Point(6, 48);
            this.MusicName.Name = "MusicName";
            this.MusicName.Placeholder = "";
            this.MusicName.ReadOnly = true;
            this.MusicName.Size = new System.Drawing.Size(360, 25);
            this.MusicName.TabIndex = 175;
            // 
            // EventScriptPopupUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.Controls.Add(this.Tab);
            this.Name = "EventScriptPopupUserControl";
            this.Size = new System.Drawing.Size(383, 303);
            this.Load += new System.EventHandler(this.EventScriptPopupUserControl_Load);
            this.Tab.ResumeLayout(false);
            this.MAPTabPage.ResumeLayout(false);
            this.PICTabPage.ResumeLayout(false);
            this.PICTabPage.PerformLayout();
            this.MusicPagePage.ResumeLayout(false);
            this.MusicPagePage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PIC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage MAPTabPage;
        private System.Windows.Forms.TabPage PICTabPage;
        public MapPictureBox MAP;
        public System.Windows.Forms.TabControl Tab;
        private System.Windows.Forms.TabPage MusicPagePage;
        private System.Windows.Forms.Button PlaySappyButton;
        private InterpolatedPictureBox PIC;
        private TextBoxEx MusicName;
    }
}
