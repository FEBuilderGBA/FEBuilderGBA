namespace FEBuilderGBA
{
    partial class WorldMapPathMoveEditorForm
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
            this.PathType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.WriteButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.AddressList = new ListBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.MapPictureBox = new FEBuilderGBA.MapPictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.N00_B3 = new System.Windows.Forms.NumericUpDown();
            this.N00_J_3 = new System.Windows.Forms.Label();
            this.N00_B2 = new System.Windows.Forms.NumericUpDown();
            this.N00_J_2 = new System.Windows.Forms.Label();
            this.N00_B1 = new System.Windows.Forms.NumericUpDown();
            this.N00_J_1 = new System.Windows.Forms.Label();
            this.N00_B0 = new System.Windows.Forms.NumericUpDown();
            this.N00_J_0 = new System.Windows.Forms.Label();
            this.W6 = new System.Windows.Forms.NumericUpDown();
            this.J_6 = new System.Windows.Forms.Label();
            this.W4 = new System.Windows.Forms.NumericUpDown();
            this.L_4_MAPXY_6 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N00_B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N00_B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N00_B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N00_B0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W4)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlPanel
            // 
            this.ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ControlPanel.Controls.Add(this.PathType);
            this.ControlPanel.Controls.Add(this.label2);
            this.ControlPanel.Location = new System.Drawing.Point(12, 12);
            this.ControlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(1063, 31);
            this.ControlPanel.TabIndex = 2;
            // 
            // PathType
            // 
            this.PathType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PathType.FormattingEnabled = true;
            this.PathType.Location = new System.Drawing.Point(316, 1);
            this.PathType.Margin = new System.Windows.Forms.Padding(2);
            this.PathType.Name = "PathType";
            this.PathType.Size = new System.Drawing.Size(377, 26);
            this.PathType.TabIndex = 2;
            this.PathType.SelectedIndexChanged += new System.EventHandler(this.PathType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(-1, -2);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(309, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "道";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(2, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 32);
            this.label5.TabIndex = 30;
            this.label5.Text = "アドレス";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(127, 2);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(143, 25);
            this.Address.TabIndex = 29;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(5, 40);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(176, 30);
            this.WriteButton.TabIndex = 17;
            this.WriteButton.Text = "ROM書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.AddressListExpandsButton);
            this.panel1.Controls.Add(this.AddressList);
            this.panel1.Location = new System.Drawing.Point(12, 49);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 530);
            this.panel1.TabIndex = 3;
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(2, 494);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(116, 30);
            this.AddressListExpandsButton.TabIndex = 112;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // AddressList
            // 
            this.AddressList.FormattingEnabled = true;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(2, 1);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(116, 490);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.MapPictureBox);
            this.panel2.Location = new System.Drawing.Point(427, 49);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(648, 530);
            this.panel2.TabIndex = 4;
            // 
            // MapPictureBox
            // 
            this.MapPictureBox.AutoScroll = true;
            this.MapPictureBox.Location = new System.Drawing.Point(2, -30);
            this.MapPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.MapPictureBox.Name = "MapPictureBox";
            this.MapPictureBox.Size = new System.Drawing.Size(643, 552);
            this.MapPictureBox.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Location = new System.Drawing.Point(137, 49);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(288, 530);
            this.panel3.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.WriteButton);
            this.panel4.Controls.Add(this.Address);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(4, 2);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(272, 77);
            this.panel4.TabIndex = 1;
            // 
            // N00_B3
            // 
            this.N00_B3.Hexadecimal = true;
            this.N00_B3.Location = new System.Drawing.Point(155, 103);
            this.N00_B3.Margin = new System.Windows.Forms.Padding(2);
            this.N00_B3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N00_B3.Name = "N00_B3";
            this.N00_B3.Size = new System.Drawing.Size(101, 25);
            this.N00_B3.TabIndex = 36;
            // 
            // N00_J_3
            // 
            this.N00_J_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N00_J_3.Location = new System.Drawing.Point(6, 99);
            this.N00_J_3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N00_J_3.Name = "N00_J_3";
            this.N00_J_3.Size = new System.Drawing.Size(140, 32);
            this.N00_J_3.TabIndex = 35;
            this.N00_J_3.Text = "00";
            this.N00_J_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N00_B2
            // 
            this.N00_B2.Hexadecimal = true;
            this.N00_B2.Location = new System.Drawing.Point(155, 72);
            this.N00_B2.Margin = new System.Windows.Forms.Padding(2);
            this.N00_B2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N00_B2.Name = "N00_B2";
            this.N00_B2.Size = new System.Drawing.Size(101, 25);
            this.N00_B2.TabIndex = 34;
            // 
            // N00_J_2
            // 
            this.N00_J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N00_J_2.Location = new System.Drawing.Point(6, 68);
            this.N00_J_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N00_J_2.Name = "N00_J_2";
            this.N00_J_2.Size = new System.Drawing.Size(140, 32);
            this.N00_J_2.TabIndex = 33;
            this.N00_J_2.Text = "00";
            this.N00_J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N00_B1
            // 
            this.N00_B1.Hexadecimal = true;
            this.N00_B1.Location = new System.Drawing.Point(155, 40);
            this.N00_B1.Margin = new System.Windows.Forms.Padding(2);
            this.N00_B1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N00_B1.Name = "N00_B1";
            this.N00_B1.Size = new System.Drawing.Size(101, 25);
            this.N00_B1.TabIndex = 32;
            // 
            // N00_J_1
            // 
            this.N00_J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N00_J_1.Location = new System.Drawing.Point(6, 37);
            this.N00_J_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N00_J_1.Name = "N00_J_1";
            this.N00_J_1.Size = new System.Drawing.Size(140, 32);
            this.N00_J_1.TabIndex = 31;
            this.N00_J_1.Text = "??";
            this.N00_J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N00_B0
            // 
            this.N00_B0.Hexadecimal = true;
            this.N00_B0.Location = new System.Drawing.Point(155, 9);
            this.N00_B0.Margin = new System.Windows.Forms.Padding(2);
            this.N00_B0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.N00_B0.Name = "N00_B0";
            this.N00_B0.Size = new System.Drawing.Size(101, 25);
            this.N00_B0.TabIndex = 30;
            // 
            // N00_J_0
            // 
            this.N00_J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N00_J_0.Location = new System.Drawing.Point(6, 6);
            this.N00_J_0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N00_J_0.Name = "N00_J_0";
            this.N00_J_0.Size = new System.Drawing.Size(140, 32);
            this.N00_J_0.TabIndex = 4;
            this.N00_J_0.Text = "??";
            this.N00_J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // W6
            // 
            this.W6.ForeColor = System.Drawing.Color.Red;
            this.W6.Location = new System.Drawing.Point(155, 166);
            this.W6.Margin = new System.Windows.Forms.Padding(2);
            this.W6.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W6.Name = "W6";
            this.W6.Size = new System.Drawing.Size(101, 25);
            this.W6.TabIndex = 41;
            // 
            // J_6
            // 
            this.J_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_6.Location = new System.Drawing.Point(6, 161);
            this.J_6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_6.Name = "J_6";
            this.J_6.Size = new System.Drawing.Size(140, 32);
            this.J_6.TabIndex = 40;
            this.J_6.Text = "Y";
            this.J_6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // W4
            // 
            this.W4.ForeColor = System.Drawing.Color.Red;
            this.W4.Location = new System.Drawing.Point(155, 135);
            this.W4.Margin = new System.Windows.Forms.Padding(2);
            this.W4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.W4.Name = "W4";
            this.W4.Size = new System.Drawing.Size(101, 25);
            this.W4.TabIndex = 39;
            // 
            // L_4_MAPXY_6
            // 
            this.L_4_MAPXY_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.L_4_MAPXY_6.Location = new System.Drawing.Point(6, 130);
            this.L_4_MAPXY_6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.L_4_MAPXY_6.Name = "L_4_MAPXY_6";
            this.L_4_MAPXY_6.Size = new System.Drawing.Size(140, 32);
            this.L_4_MAPXY_6.TabIndex = 38;
            this.L_4_MAPXY_6.Text = "X";
            this.L_4_MAPXY_6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.N00_J_0);
            this.panel5.Controls.Add(this.W6);
            this.panel5.Controls.Add(this.N00_B3);
            this.panel5.Controls.Add(this.J_6);
            this.panel5.Controls.Add(this.N00_B0);
            this.panel5.Controls.Add(this.W4);
            this.panel5.Controls.Add(this.N00_J_3);
            this.panel5.Controls.Add(this.L_4_MAPXY_6);
            this.panel5.Controls.Add(this.N00_J_1);
            this.panel5.Controls.Add(this.N00_B2);
            this.panel5.Controls.Add(this.N00_B1);
            this.panel5.Controls.Add(this.N00_J_2);
            this.panel5.Location = new System.Drawing.Point(137, 126);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(276, 450);
            this.panel5.TabIndex = 2;
            // 
            // WorldMapPathMoveEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1087, 586);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ControlPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WorldMapPathMoveEditorForm";
            this.Text = "道移動エディタ";
            this.Load += new System.EventHandler(this.WorldMapPathEditorForm_Load);
            this.ControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.N00_B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N00_B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N00_B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N00_B0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W4)).EndInit();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.ComboBox PathType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label5;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label N00_J_0;
        private System.Windows.Forms.NumericUpDown N00_B3;
        private System.Windows.Forms.Label N00_J_3;
        private System.Windows.Forms.NumericUpDown N00_B2;
        private System.Windows.Forms.Label N00_J_2;
        private System.Windows.Forms.NumericUpDown N00_B1;
        private System.Windows.Forms.Label N00_J_1;
        private System.Windows.Forms.NumericUpDown N00_B0;
        private System.Windows.Forms.NumericUpDown W6;
        private System.Windows.Forms.Label J_6;
        private System.Windows.Forms.NumericUpDown W4;
        private System.Windows.Forms.Label L_4_MAPXY_6;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private MapPictureBox MapPictureBox;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
    }
}