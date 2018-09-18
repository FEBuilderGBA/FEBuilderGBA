namespace FEBuilderGBA
{
    partial class MapEditorResizeDialogForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.OK = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.B = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.R = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.L = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.T = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.H = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.W = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.Y = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.X = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.R)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OK);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.B);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.R);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.L);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.T);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.H);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.W);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.Y);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.X);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(404, 324);
            this.panel1.TabIndex = 0;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(268, 286);
            this.OK.Margin = new System.Windows.Forms.Padding(2);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(134, 35);
            this.OK.TabIndex = 26;
            this.OK.Text = "変更する";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(162, 206);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 32);
            this.label10.TabIndex = 25;
            this.label10.Text = "B";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B
            // 
            this.B.Location = new System.Drawing.Point(200, 212);
            this.B.Margin = new System.Windows.Forms.Padding(2);
            this.B.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.B.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.B.Name = "B";
            this.B.Size = new System.Drawing.Size(73, 25);
            this.B.TabIndex = 24;
            this.B.ValueChanged += new System.EventHandler(this.T_ValueChanged);
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(284, 157);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 32);
            this.label9.TabIndex = 23;
            this.label9.Text = "R";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // R
            // 
            this.R.Location = new System.Drawing.Point(323, 164);
            this.R.Margin = new System.Windows.Forms.Padding(2);
            this.R.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.R.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.R.Name = "R";
            this.R.Size = new System.Drawing.Size(73, 25);
            this.R.TabIndex = 22;
            this.R.ValueChanged += new System.EventHandler(this.T_ValueChanged);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(32, 157);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 32);
            this.label8.TabIndex = 21;
            this.label8.Text = "L";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L
            // 
            this.L.Location = new System.Drawing.Point(70, 164);
            this.L.Margin = new System.Windows.Forms.Padding(2);
            this.L.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.L.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.L.Name = "L";
            this.L.Size = new System.Drawing.Size(73, 25);
            this.L.TabIndex = 20;
            this.L.ValueChanged += new System.EventHandler(this.T_ValueChanged);
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(162, 104);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 32);
            this.label7.TabIndex = 19;
            this.label7.Text = "T";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // T
            // 
            this.T.Location = new System.Drawing.Point(200, 112);
            this.T.Margin = new System.Windows.Forms.Padding(2);
            this.T.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.T.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.T.Name = "T";
            this.T.Size = new System.Drawing.Size(73, 25);
            this.T.TabIndex = 18;
            this.T.ValueChanged += new System.EventHandler(this.T_ValueChanged);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(254, 61);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 32);
            this.label5.TabIndex = 17;
            this.label5.Text = "H";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // H
            // 
            this.H.Enabled = false;
            this.H.Location = new System.Drawing.Point(292, 70);
            this.H.Margin = new System.Windows.Forms.Padding(2);
            this.H.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.H.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.H.Name = "H";
            this.H.Size = new System.Drawing.Size(73, 25);
            this.H.TabIndex = 16;
            this.H.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(131, 61);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 32);
            this.label6.TabIndex = 15;
            this.label6.Text = "W";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // W
            // 
            this.W.Enabled = false;
            this.W.Location = new System.Drawing.Point(169, 70);
            this.W.Margin = new System.Windows.Forms.Padding(2);
            this.W.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.W.Name = "W";
            this.W.Size = new System.Drawing.Size(73, 25);
            this.W.TabIndex = 14;
            this.W.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(254, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 32);
            this.label4.TabIndex = 13;
            this.label4.Text = "Y";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Y
            // 
            this.Y.Location = new System.Drawing.Point(292, 19);
            this.Y.Margin = new System.Windows.Forms.Padding(2);
            this.Y.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(73, 25);
            this.Y.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(131, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 32);
            this.label3.TabIndex = 11;
            this.label3.Text = "X";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X
            // 
            this.X.Location = new System.Drawing.Point(169, 19);
            this.X.Margin = new System.Windows.Forms.Padding(2);
            this.X.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(73, 25);
            this.X.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(2, 61);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "サイズ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(2, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 32);
            this.label2.TabIndex = 4;
            this.label2.Text = "位置";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MapEditorResizeDialogForm
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(428, 348);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MapEditorResizeDialogForm";
            this.Text = "マップリサイズ";
            this.Load += new System.EventHandler(this.MapEditResizeDialogForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.B)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.R)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button OK;
        public System.Windows.Forms.NumericUpDown Y;
        public System.Windows.Forms.NumericUpDown X;
        public System.Windows.Forms.NumericUpDown H;
        public System.Windows.Forms.NumericUpDown W;
        public System.Windows.Forms.NumericUpDown R;
        public System.Windows.Forms.NumericUpDown L;
        public System.Windows.Forms.NumericUpDown T;
        public System.Windows.Forms.NumericUpDown B;
    }
}