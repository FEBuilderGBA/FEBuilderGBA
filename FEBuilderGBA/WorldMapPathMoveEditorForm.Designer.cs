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
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.AddressListExpandsButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.MapPictureBox = new FEBuilderGBA.MapPictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.B3 = new System.Windows.Forms.NumericUpDown();
            this.J_3 = new System.Windows.Forms.Label();
            this.B2 = new System.Windows.Forms.NumericUpDown();
            this.J_2 = new System.Windows.Forms.Label();
            this.B1 = new System.Windows.Forms.NumericUpDown();
            this.J_1 = new System.Windows.Forms.Label();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.J_0 = new System.Windows.Forms.Label();
            this.W6 = new System.Windows.Forms.NumericUpDown();
            this.J_6 = new System.Windows.Forms.Label();
            this.W4 = new System.Windows.Forms.NumericUpDown();
            this.L_4_MAPXY_6 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.W4)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
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
            this.ControlPanel.Size = new System.Drawing.Size(1192, 31);
            this.ControlPanel.TabIndex = 2;
            // 
            // PathType
            // 
            this.PathType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PathType.FormattingEnabled = true;
            this.PathType.Location = new System.Drawing.Point(128, 2);
            this.PathType.Margin = new System.Windows.Forms.Padding(2);
            this.PathType.Name = "PathType";
            this.PathType.Size = new System.Drawing.Size(1063, 26);
            this.PathType.TabIndex = 2;
            this.PathType.SelectedIndexChanged += new System.EventHandler(this.PathType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(-1, -2);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "道";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(4, 2);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 32);
            this.label5.TabIndex = 30;
            this.label5.Text = "アドレス";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(143, 4);
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
            this.WriteButton.Location = new System.Drawing.Point(4, 113);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(270, 30);
            this.WriteButton.TabIndex = 17;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.AddressList);
            this.panel1.Location = new System.Drawing.Point(12, 120);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 540);
            this.panel1.TabIndex = 3;
            // 
            // AddressList
            // 
            this.AddressList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(0, 0);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(120, 540);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // AddressListExpandsButton
            // 
            this.AddressListExpandsButton.Location = new System.Drawing.Point(12, 675);
            this.AddressListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton.Name = "AddressListExpandsButton";
            this.AddressListExpandsButton.Size = new System.Drawing.Size(204, 30);
            this.AddressListExpandsButton.TabIndex = 112;
            this.AddressListExpandsButton.Text = "リストの拡張";
            this.AddressListExpandsButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.MapPictureBox);
            this.panel2.Location = new System.Drawing.Point(427, 49);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(777, 656);
            this.panel2.TabIndex = 4;
            // 
            // MapPictureBox
            // 
            this.MapPictureBox.Location = new System.Drawing.Point(2, -30);
            this.MapPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.MapPictureBox.Name = "MapPictureBox";
            this.MapPictureBox.Size = new System.Drawing.Size(770, 684);
            this.MapPictureBox.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.BlockSize);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.SelectAddress);
            this.panel3.Controls.Add(this.Address);
            this.panel3.Controls.Add(this.label22);
            this.panel3.Controls.Add(this.WriteButton);
            this.panel3.Location = new System.Drawing.Point(137, 120);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(288, 540);
            this.panel3.TabIndex = 5;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(143, 37);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 55;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(4, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 30);
            this.label3.TabIndex = 56;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(143, 72);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 54;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(4, 69);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(132, 30);
            this.label22.TabIndex = 53;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B3
            // 
            this.B3.Hexadecimal = true;
            this.B3.Location = new System.Drawing.Point(155, 103);
            this.B3.Margin = new System.Windows.Forms.Padding(2);
            this.B3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(101, 25);
            this.B3.TabIndex = 36;
            // 
            // J_3
            // 
            this.J_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_3.Location = new System.Drawing.Point(6, 99);
            this.J_3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_3.Name = "J_3";
            this.J_3.Size = new System.Drawing.Size(140, 32);
            this.J_3.TabIndex = 35;
            this.J_3.Text = "00";
            this.J_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B2
            // 
            this.B2.Hexadecimal = true;
            this.B2.Location = new System.Drawing.Point(155, 72);
            this.B2.Margin = new System.Windows.Forms.Padding(2);
            this.B2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(101, 25);
            this.B2.TabIndex = 34;
            // 
            // J_2
            // 
            this.J_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_2.Location = new System.Drawing.Point(6, 68);
            this.J_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_2.Name = "J_2";
            this.J_2.Size = new System.Drawing.Size(140, 32);
            this.J_2.TabIndex = 33;
            this.J_2.Text = "00";
            this.J_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B1
            // 
            this.B1.Hexadecimal = true;
            this.B1.Location = new System.Drawing.Point(155, 40);
            this.B1.Margin = new System.Windows.Forms.Padding(2);
            this.B1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(101, 25);
            this.B1.TabIndex = 32;
            // 
            // J_1
            // 
            this.J_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_1.Location = new System.Drawing.Point(6, 37);
            this.J_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_1.Name = "J_1";
            this.J_1.Size = new System.Drawing.Size(140, 32);
            this.J_1.TabIndex = 31;
            this.J_1.Text = "??";
            this.J_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(155, 9);
            this.B0.Margin = new System.Windows.Forms.Padding(2);
            this.B0.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(101, 25);
            this.B0.TabIndex = 30;
            // 
            // J_0
            // 
            this.J_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0.Location = new System.Drawing.Point(6, 6);
            this.J_0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_0.Name = "J_0";
            this.J_0.Size = new System.Drawing.Size(140, 32);
            this.J_0.TabIndex = 4;
            this.J_0.Text = "??";
            this.J_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panel5.Controls.Add(this.J_0);
            this.panel5.Controls.Add(this.W6);
            this.panel5.Controls.Add(this.B3);
            this.panel5.Controls.Add(this.J_6);
            this.panel5.Controls.Add(this.B0);
            this.panel5.Controls.Add(this.W4);
            this.panel5.Controls.Add(this.J_3);
            this.panel5.Controls.Add(this.L_4_MAPXY_6);
            this.panel5.Controls.Add(this.J_1);
            this.panel5.Controls.Add(this.B2);
            this.panel5.Controls.Add(this.B1);
            this.panel5.Controls.Add(this.J_2);
            this.panel5.Location = new System.Drawing.Point(137, 268);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(276, 392);
            this.panel5.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.ReloadListButton);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.ReadCount);
            this.panel6.Controls.Add(this.ReadStartAddress);
            this.panel6.Location = new System.Drawing.Point(12, 49);
            this.panel6.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(413, 65);
            this.panel6.TabIndex = 187;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(268, 31);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(130, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -2);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(-1, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 30);
            this.label4.TabIndex = 24;
            this.label4.Text = "読込数";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(128, 35);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(128, 2);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // WorldMapPathMoveEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1213, 710);
            this.Controls.Add(this.AddressListExpandsButton);
            this.Controls.Add(this.panel6);
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
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.W4)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
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
        private System.Windows.Forms.Label J_0;
        private System.Windows.Forms.NumericUpDown B3;
        private System.Windows.Forms.Label J_3;
        private System.Windows.Forms.NumericUpDown B2;
        private System.Windows.Forms.Label J_2;
        private System.Windows.Forms.NumericUpDown B1;
        private System.Windows.Forms.Label J_1;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.NumericUpDown W6;
        private System.Windows.Forms.Label J_6;
        private System.Windows.Forms.NumericUpDown W4;
        private System.Windows.Forms.Label L_4_MAPXY_6;
        private System.Windows.Forms.Button AddressListExpandsButton;
        private MapPictureBox MapPictureBox;
        private System.Windows.Forms.Panel panel5;
        private TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
    }
}