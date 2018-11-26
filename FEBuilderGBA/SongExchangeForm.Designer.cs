namespace FEBuilderGBA
{
    partial class SongExchangeForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.OpenOtherROMButton = new System.Windows.Forms.Button();
            this.OtherROMSongTable = new FEBuilderGBA.ListBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SongTable = new FEBuilderGBA.ListBoxEx();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.OpenOtherROMButton);
            this.panel2.Controls.Add(this.OtherROMSongTable);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.SongTable);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1306, 563);
            this.panel2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(607, 434);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 101);
            this.button1.TabIndex = 39;
            this.button1.Text = "<---------\r\n選択した曲を\r\n移植する\r\n<---------";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OpenOtherROMButton
            // 
            this.OpenOtherROMButton.Location = new System.Drawing.Point(733, 12);
            this.OpenOtherROMButton.Name = "OpenOtherROMButton";
            this.OpenOtherROMButton.Size = new System.Drawing.Size(565, 60);
            this.OpenOtherROMButton.TabIndex = 38;
            this.OpenOtherROMButton.Text = "別ROMを開く";
            this.OpenOtherROMButton.UseVisualStyleBackColor = true;
            this.OpenOtherROMButton.Click += new System.EventHandler(this.OpenOtherROMButton_Click);
            // 
            // OtherROMSongTable
            // 
            this.OtherROMSongTable.FormattingEnabled = true;
            this.OtherROMSongTable.HorizontalScrollbar = true;
            this.OtherROMSongTable.IntegralHeight = false;
            this.OtherROMSongTable.ItemHeight = 18;
            this.OtherROMSongTable.Location = new System.Drawing.Point(733, 78);
            this.OtherROMSongTable.Name = "OtherROMSongTable";
            this.OtherROMSongTable.Size = new System.Drawing.Size(565, 476);
            this.OtherROMSongTable.TabIndex = 36;
            this.OtherROMSongTable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OtherROMSongTable_MouseDoubleClick);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(11, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(590, 30);
            this.label3.TabIndex = 35;
            this.label3.Text = "サウンドテーブル";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SongTable
            // 
            this.SongTable.FormattingEnabled = true;
            this.SongTable.HorizontalScrollbar = true;
            this.SongTable.IntegralHeight = false;
            this.SongTable.ItemHeight = 18;
            this.SongTable.Location = new System.Drawing.Point(11, 44);
            this.SongTable.Name = "SongTable";
            this.SongTable.Size = new System.Drawing.Size(590, 512);
            this.SongTable.TabIndex = 0;
            this.SongTable.DoubleClick += new System.EventHandler(this.SongTable_DoubleClick);
            // 
            // SongExchangeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1306, 563);
            this.Controls.Add(this.panel2);
            this.Name = "SongExchangeForm";
            this.Text = "別ROMから曲のインポート";
            this.Load += new System.EventHandler(this.SongExchangeForm_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private ListBoxEx SongTable;
        private System.Windows.Forms.Label label3;
        private ListBoxEx OtherROMSongTable;
        private System.Windows.Forms.Button OpenOtherROMButton;
        private System.Windows.Forms.Button button1;
    }
}