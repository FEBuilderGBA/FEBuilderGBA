namespace FEBuilderGBA
{
    partial class MapPointerNewPLISTPopupForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.LINK_PLIST = new FEBuilderGBA.TextBoxEx();
            this.PLISTnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.AlreadyExtendsLabel = new System.Windows.Forms.Label();
            this.PLISTExtendsButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PLIST_EXPLAIN = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PLISTnumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "PLIST割り当てがありません。";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MyCancelButton);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.LINK_PLIST);
            this.panel1.Controls.Add(this.PLISTnumericUpDown);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.AlreadyExtendsLabel);
            this.panel1.Controls.Add(this.PLISTExtendsButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.PLIST_EXPLAIN);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(913, 363);
            this.panel1.TabIndex = 1;
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(389, 307);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(191, 39);
            this.MyCancelButton.TabIndex = 2;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(652, 307);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(191, 39);
            this.OKButton.TabIndex = 3;
            this.OKButton.Text = "割り当て";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // LINK_PLIST
            // 
            this.LINK_PLIST.Location = new System.Drawing.Point(304, 223);
            this.LINK_PLIST.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.LINK_PLIST.Multiline = true;
            this.LINK_PLIST.Name = "LINK_PLIST";
            this.LINK_PLIST.ReadOnly = true;
            this.LINK_PLIST.Size = new System.Drawing.Size(594, 62);
            this.LINK_PLIST.TabIndex = 1;
            // 
            // PLISTnumericUpDown
            // 
            this.PLISTnumericUpDown.Hexadecimal = true;
            this.PLISTnumericUpDown.Location = new System.Drawing.Point(181, 222);
            this.PLISTnumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.PLISTnumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PLISTnumericUpDown.Name = "PLISTnumericUpDown";
            this.PLISTnumericUpDown.Size = new System.Drawing.Size(107, 25);
            this.PLISTnumericUpDown.TabIndex = 0;
            this.PLISTnumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PLISTnumericUpDown.ValueChanged += new System.EventHandler(this.PLISTnumericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 18);
            this.label5.TabIndex = 5;
            this.label5.Text = "PLIST割り当て";
            // 
            // AlreadyExtendsLabel
            // 
            this.AlreadyExtendsLabel.AutoSize = true;
            this.AlreadyExtendsLabel.Location = new System.Drawing.Point(641, 175);
            this.AlreadyExtendsLabel.Name = "AlreadyExtendsLabel";
            this.AlreadyExtendsLabel.Size = new System.Drawing.Size(140, 18);
            this.AlreadyExtendsLabel.TabIndex = 4;
            this.AlreadyExtendsLabel.Text = "既に拡張済みです";
            this.AlreadyExtendsLabel.Visible = false;
            // 
            // PLISTExtendsButton
            // 
            this.PLISTExtendsButton.Location = new System.Drawing.Point(181, 169);
            this.PLISTExtendsButton.Name = "PLISTExtendsButton";
            this.PLISTExtendsButton.Size = new System.Drawing.Size(430, 32);
            this.PLISTExtendsButton.TabIndex = 5;
            this.PLISTExtendsButton.Text = "MapPointer(PLIST)を拡張";
            this.PLISTExtendsButton.UseVisualStyleBackColor = true;
            this.PLISTExtendsButton.Click += new System.EventHandler(this.PLISTExtendsButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "PLIST拡張";
            // 
            // PLIST_EXPLAIN
            // 
            this.PLIST_EXPLAIN.AutoSize = true;
            this.PLIST_EXPLAIN.Location = new System.Drawing.Point(4, 54);
            this.PLIST_EXPLAIN.Name = "PLIST_EXPLAIN";
            this.PLIST_EXPLAIN.Size = new System.Drawing.Size(861, 54);
            this.PLIST_EXPLAIN.TabIndex = 1;
            this.PLIST_EXPLAIN.Text = "空きPLISTを割り当てる必要があります。\r\nGBAFEのマップ、タイル、イベント、ワールドマップイベント、マップ変化は PLISTと呼ばれるポインタリストで管理" +
    "されています。\r\n(注意してください。間違ったPLISTを割り当てると危険です。バックアップを取った後でやることをお勧めします。)";
            // 
            // MapPointerNewPLISTPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(937, 385);
            this.Controls.Add(this.panel1);
            this.Name = "MapPointerNewPLISTPopupForm";
            this.Text = "PLIST割り当てがありません。";
            this.Load += new System.EventHandler(this.MapPointerNewPLISTPopupForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PLISTnumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown PLISTnumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label AlreadyExtendsLabel;
        private System.Windows.Forms.Button PLISTExtendsButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label PLIST_EXPLAIN;
        private FEBuilderGBA.TextBoxEx LINK_PLIST;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Button OKButton;
    }
}