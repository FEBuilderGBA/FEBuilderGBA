namespace FEBuilderGBA
{
    partial class MapPictureBox
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
            this.CommandBar = new System.Windows.Forms.Panel();
            this.ChangeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ZoomComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.MapSelector = new System.Windows.Forms.ComboBox();
            this.Map = new FEBuilderGBA.InterpolatedPictureBox();
            this.CommandBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Map)).BeginInit();
            this.SuspendLayout();
            // 
            // CommandBar
            // 
            this.CommandBar.AutoSize = true;
            this.CommandBar.Controls.Add(this.ChangeComboBox);
            this.CommandBar.Controls.Add(this.label2);
            this.CommandBar.Controls.Add(this.ZoomComboBox);
            this.CommandBar.Controls.Add(this.label1);
            this.CommandBar.Controls.Add(this.label46);
            this.CommandBar.Controls.Add(this.MapSelector);
            this.CommandBar.Location = new System.Drawing.Point(2, 0);
            this.CommandBar.Margin = new System.Windows.Forms.Padding(2);
            this.CommandBar.Name = "CommandBar";
            this.CommandBar.Size = new System.Drawing.Size(837, 29);
            this.CommandBar.TabIndex = 1;
            // 
            // ChangeComboBox
            // 
            this.ChangeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChangeComboBox.FormattingEnabled = true;
            this.ChangeComboBox.Location = new System.Drawing.Point(550, -1);
            this.ChangeComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.ChangeComboBox.Name = "ChangeComboBox";
            this.ChangeComboBox.Size = new System.Drawing.Size(148, 26);
            this.ChangeComboBox.TabIndex = 6;
            this.ChangeComboBox.SelectedIndexChanged += new System.EventHandler(this.ChangeComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(474, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 26);
            this.label2.TabIndex = 5;
            this.label2.Text = "変化";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ZoomComboBox
            // 
            this.ZoomComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ZoomComboBox.FormattingEnabled = true;
            this.ZoomComboBox.Items.AddRange(new object[] {
            "200%",
            "175%",
            "150%",
            "125%",
            "100%",
            "75%",
            "50%"});
            this.ZoomComboBox.Location = new System.Drawing.Point(381, -1);
            this.ZoomComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.ZoomComboBox.Name = "ZoomComboBox";
            this.ZoomComboBox.Size = new System.Drawing.Size(93, 26);
            this.ZoomComboBox.TabIndex = 4;
            this.ZoomComboBox.SelectedIndexChanged += new System.EventHandler(this.ZoomComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(320, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "縮尺";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label46
            // 
            this.label46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label46.Location = new System.Drawing.Point(-1, -1);
            this.label46.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(53, 26);
            this.label46.TabIndex = 2;
            this.label46.Text = "マップ";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MapSelector
            // 
            this.MapSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MapSelector.FormattingEnabled = true;
            this.MapSelector.Location = new System.Drawing.Point(52, -2);
            this.MapSelector.Margin = new System.Windows.Forms.Padding(2);
            this.MapSelector.Name = "MapSelector";
            this.MapSelector.Size = new System.Drawing.Size(266, 26);
            this.MapSelector.TabIndex = 0;
            this.MapSelector.SelectedIndexChanged += new System.EventHandler(this.MapSelector_SelectedIndexChanged);
            // 
            // Map
            // 
            this.Map.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.Map.Location = new System.Drawing.Point(0, 26);
            this.Map.Margin = new System.Windows.Forms.Padding(2);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(100, 50);
            this.Map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Map.TabIndex = 0;
            this.Map.TabStop = false;
            this.Map.Paint += new System.Windows.Forms.PaintEventHandler(this.Map_Paint);
            this.Map.DoubleClick += new System.EventHandler(this.Map_DoubleClick);
            this.Map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
            this.Map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Map_MouseMove);
            // 
            // MapPictureBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.Controls.Add(this.Map);
            this.Controls.Add(this.CommandBar);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MapPictureBox";
            this.Size = new System.Drawing.Size(842, 380);
            this.Load += new System.EventHandler(this.MapPictureBox_Load);
            this.CommandBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Map)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MapSelector;
        private System.Windows.Forms.Label label46;
        public System.Windows.Forms.Panel CommandBar;
        private System.Windows.Forms.ComboBox ZoomComboBox;
        private System.Windows.Forms.Label label1;
        private InterpolatedPictureBox Map;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ChangeComboBox;
    }
}
