namespace FEBuilderGBA
{
    partial class WorldMapPathEditorForm
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
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.MapAddress = new System.Windows.Forms.NumericUpDown();
            this.WriteButton = new System.Windows.Forms.Button();
            this.PathType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.WorldMap = new FEBuilderGBA.InterpolatedPictureBox();
            this.PATHCHIPLIST = new FEBuilderGBA.InterpolatedPictureBox();
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapAddress)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PATHCHIPLIST)).BeginInit();
            this.SuspendLayout();
            // 
            // ControlPanel
            // 
            this.ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ControlPanel.Controls.Add(this.label5);
            this.ControlPanel.Controls.Add(this.MapAddress);
            this.ControlPanel.Controls.Add(this.WriteButton);
            this.ControlPanel.Controls.Add(this.PathType);
            this.ControlPanel.Controls.Add(this.label2);
            this.ControlPanel.Location = new System.Drawing.Point(12, 12);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(1063, 31);
            this.ControlPanel.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(578, -2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 32);
            this.label5.TabIndex = 30;
            this.label5.Text = "アドレス";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MapAddress
            // 
            this.MapAddress.Hexadecimal = true;
            this.MapAddress.Location = new System.Drawing.Point(704, 2);
            this.MapAddress.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.MapAddress.Name = "MapAddress";
            this.MapAddress.Size = new System.Drawing.Size(155, 25);
            this.MapAddress.TabIndex = 29;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(885, 0);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(176, 30);
            this.WriteButton.TabIndex = 17;
            this.WriteButton.Text = "ROM書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // PathType
            // 
            this.PathType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PathType.FormattingEnabled = true;
            this.PathType.Location = new System.Drawing.Point(124, 1);
            this.PathType.Name = "PathType";
            this.PathType.Size = new System.Drawing.Size(377, 26);
            this.PathType.TabIndex = 2;
            this.PathType.SelectedIndexChanged += new System.EventHandler(this.PathType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(-1, -1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "道";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.PATHCHIPLIST);
            this.panel1.Location = new System.Drawing.Point(12, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 614);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.WorldMap);
            this.panel2.Location = new System.Drawing.Point(138, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(937, 614);
            this.panel2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 576);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(412, 36);
            this.label1.TabIndex = 2;
            this.label1.Text = "右端のパネルは消しゴムです。\r\n道を消す場合は消しゴムを選択して、上書きしてください。";
            // 
            // WorldMap
            // 
            this.WorldMap.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.WorldMap.Location = new System.Drawing.Point(3, 3);
            this.WorldMap.Name = "WorldMap";
            this.WorldMap.Size = new System.Drawing.Size(32, 32);
            this.WorldMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.WorldMap.TabIndex = 1;
            this.WorldMap.TabStop = false;
            this.WorldMap.Paint += new System.Windows.Forms.PaintEventHandler(this.WorldMap_Paint);
            this.WorldMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WorldMap_MouseDown);
            this.WorldMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WorldMap_MouseMove);
            // 
            // PATHCHIPLIST
            // 
            this.PATHCHIPLIST.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.PATHCHIPLIST.Location = new System.Drawing.Point(3, 3);
            this.PATHCHIPLIST.Name = "PATHCHIPLIST";
            this.PATHCHIPLIST.Size = new System.Drawing.Size(32, 32);
            this.PATHCHIPLIST.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PATHCHIPLIST.TabIndex = 1;
            this.PATHCHIPLIST.TabStop = false;
            this.PATHCHIPLIST.Paint += new System.Windows.Forms.PaintEventHandler(this.PATHCHIPLIST_Paint);
            this.PATHCHIPLIST.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PATHCHIPLIST_MouseDown);
            this.PATHCHIPLIST.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PATHCHIPLIST_MouseMove);
            // 
            // WorldMapPathEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1087, 675);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ControlPanel);
            this.Name = "WorldMapPathEditorForm";
            this.Text = "道エディタ";
            this.Load += new System.EventHandler(this.WorldMapPathEditorForm_Load);
            this.ControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MapAddress)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PATHCHIPLIST)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.ComboBox PathType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown MapAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private InterpolatedPictureBox PATHCHIPLIST;
        private InterpolatedPictureBox WorldMap;
    }
}