namespace FEBuilderGBA
{
    partial class WorldMapPathForm
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
            this.label7 = new System.Windows.Forms.Label();
            this.P8 = new System.Windows.Forms.NumericUpDown();
            this.J_ID_PATHMOVEEDITOR = new System.Windows.Forms.Label();
            this.B7 = new System.Windows.Forms.NumericUpDown();
            this.B6 = new System.Windows.Forms.NumericUpDown();
            this.J_6 = new System.Windows.Forms.Label();
            this.L_5_BASEPOINT = new FEBuilderGBA.TextBoxEx();
            this.B5 = new System.Windows.Forms.NumericUpDown();
            this.J_5 = new System.Windows.Forms.Label();
            this.L_4_BASEPOINT = new FEBuilderGBA.TextBoxEx();
            this.P0 = new System.Windows.Forms.NumericUpDown();
            this.B4 = new System.Windows.Forms.NumericUpDown();
            this.J_ID_PATHEDITOR = new System.Windows.Forms.Label();
            this.J_4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.WriteButton = new System.Windows.Forms.Button();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.MapPictureBox = new FEBuilderGBA.MapPictureBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.P8);
            this.panel2.Controls.Add(this.J_ID_PATHMOVEEDITOR);
            this.panel2.Controls.Add(this.B7);
            this.panel2.Controls.Add(this.B6);
            this.panel2.Controls.Add(this.J_6);
            this.panel2.Controls.Add(this.L_5_BASEPOINT);
            this.panel2.Controls.Add(this.B5);
            this.panel2.Controls.Add(this.J_5);
            this.panel2.Controls.Add(this.L_4_BASEPOINT);
            this.panel2.Controls.Add(this.P0);
            this.panel2.Controls.Add(this.B4);
            this.panel2.Controls.Add(this.J_ID_PATHEDITOR);
            this.panel2.Controls.Add(this.J_4);
            this.panel2.Location = new System.Drawing.Point(373, 80);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(882, 140);
            this.panel2.TabIndex = 188;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(474, 107);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(406, 26);
            this.label7.TabIndex = 241;
            this.label7.Text = "NULLの場合、拠点間を直線で通る";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P8
            // 
            this.P8.Hexadecimal = true;
            this.P8.Location = new System.Drawing.Point(325, 109);
            this.P8.Margin = new System.Windows.Forms.Padding(2);
            this.P8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P8.Name = "P8";
            this.P8.Size = new System.Drawing.Size(130, 25);
            this.P8.TabIndex = 241;
            // 
            // J_ID_PATHMOVEEDITOR
            // 
            this.J_ID_PATHMOVEEDITOR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_ID_PATHMOVEEDITOR.Location = new System.Drawing.Point(5, 109);
            this.J_ID_PATHMOVEEDITOR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_ID_PATHMOVEEDITOR.Name = "J_ID_PATHMOVEEDITOR";
            this.J_ID_PATHMOVEEDITOR.Size = new System.Drawing.Size(312, 26);
            this.J_ID_PATHMOVEEDITOR.TabIndex = 240;
            this.J_ID_PATHMOVEEDITOR.Text = "道を移動するパスのポインタ";
            this.J_ID_PATHMOVEEDITOR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B7
            // 
            this.B7.Hexadecimal = true;
            this.B7.Location = new System.Drawing.Point(398, 79);
            this.B7.Margin = new System.Windows.Forms.Padding(2);
            this.B7.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B7.Name = "B7";
            this.B7.Size = new System.Drawing.Size(65, 25);
            this.B7.TabIndex = 239;
            // 
            // B6
            // 
            this.B6.Hexadecimal = true;
            this.B6.Location = new System.Drawing.Point(325, 80);
            this.B6.Margin = new System.Windows.Forms.Padding(2);
            this.B6.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B6.Name = "B6";
            this.B6.Size = new System.Drawing.Size(65, 25);
            this.B6.TabIndex = 238;
            // 
            // J_6
            // 
            this.J_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_6.Location = new System.Drawing.Point(5, 83);
            this.J_6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_6.Name = "J_6";
            this.J_6.Size = new System.Drawing.Size(312, 26);
            this.J_6.TabIndex = 237;
            this.J_6.Text = "0000";
            this.J_6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_5_BASEPOINT
            // 
            this.L_5_BASEPOINT.ErrorMessage = "";
            this.L_5_BASEPOINT.Location = new System.Drawing.Point(398, 56);
            this.L_5_BASEPOINT.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_5_BASEPOINT.Name = "L_5_BASEPOINT";
            this.L_5_BASEPOINT.Placeholder = "";
            this.L_5_BASEPOINT.ReadOnly = true;
            this.L_5_BASEPOINT.Size = new System.Drawing.Size(159, 25);
            this.L_5_BASEPOINT.TabIndex = 236;
            // 
            // B5
            // 
            this.B5.Hexadecimal = true;
            this.B5.Location = new System.Drawing.Point(325, 56);
            this.B5.Margin = new System.Windows.Forms.Padding(2);
            this.B5.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B5.Name = "B5";
            this.B5.Size = new System.Drawing.Size(65, 25);
            this.B5.TabIndex = 235;
            // 
            // J_5
            // 
            this.J_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_5.Location = new System.Drawing.Point(5, 56);
            this.J_5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_5.Name = "J_5";
            this.J_5.Size = new System.Drawing.Size(312, 26);
            this.J_5.TabIndex = 234;
            this.J_5.Text = "道の起点になる拠点ID";
            this.J_5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_4_BASEPOINT
            // 
            this.L_4_BASEPOINT.ErrorMessage = "";
            this.L_4_BASEPOINT.Location = new System.Drawing.Point(398, 30);
            this.L_4_BASEPOINT.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_4_BASEPOINT.Name = "L_4_BASEPOINT";
            this.L_4_BASEPOINT.Placeholder = "";
            this.L_4_BASEPOINT.ReadOnly = true;
            this.L_4_BASEPOINT.Size = new System.Drawing.Size(159, 25);
            this.L_4_BASEPOINT.TabIndex = 233;
            // 
            // P0
            // 
            this.P0.Hexadecimal = true;
            this.P0.Location = new System.Drawing.Point(325, 4);
            this.P0.Margin = new System.Windows.Forms.Padding(2);
            this.P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(130, 25);
            this.P0.TabIndex = 211;
            // 
            // B4
            // 
            this.B4.Hexadecimal = true;
            this.B4.Location = new System.Drawing.Point(325, 30);
            this.B4.Margin = new System.Windows.Forms.Padding(2);
            this.B4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.B4.Name = "B4";
            this.B4.Size = new System.Drawing.Size(65, 25);
            this.B4.TabIndex = 232;
            // 
            // J_ID_PATHEDITOR
            // 
            this.J_ID_PATHEDITOR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_ID_PATHEDITOR.Location = new System.Drawing.Point(5, 4);
            this.J_ID_PATHEDITOR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_ID_PATHEDITOR.Name = "J_ID_PATHEDITOR";
            this.J_ID_PATHEDITOR.Size = new System.Drawing.Size(312, 26);
            this.J_ID_PATHEDITOR.TabIndex = 210;
            this.J_ID_PATHEDITOR.Text = "道データへのポインタ";
            this.J_ID_PATHEDITOR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // J_4
            // 
            this.J_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_4.Location = new System.Drawing.Point(5, 30);
            this.J_4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_4.Name = "J_4";
            this.J_4.Size = new System.Drawing.Size(312, 26);
            this.J_4.TabIndex = 231;
            this.J_4.Text = "道の起点になる拠点ID";
            this.J_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(12, 16);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1248, 30);
            this.panel1.TabIndex = 186;
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(460, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
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
            this.label1.Size = new System.Drawing.Size(115, 30);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(266, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 30);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(352, 2);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            this.ReadCount.Value = new decimal(new int[] {
            29,
            0,
            0,
            0});
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(120, 2);
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
            // AddressPanel
            // 
            this.AddressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AddressPanel.Controls.Add(this.WriteButton);
            this.AddressPanel.Controls.Add(this.BlockSize);
            this.AddressPanel.Controls.Add(this.label3);
            this.AddressPanel.Controls.Add(this.SelectAddress);
            this.AddressPanel.Controls.Add(this.label22);
            this.AddressPanel.Controls.Add(this.Address);
            this.AddressPanel.Controls.Add(this.label23);
            this.AddressPanel.Location = new System.Drawing.Point(374, 47);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(885, 30);
            this.AddressPanel.TabIndex = 185;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(716, -1);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(332, 2);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(248, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(545, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(137, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(422, 0);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(97, 4);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 8;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-1, 1);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 30);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.MapPictureBox);
            this.panel3.Location = new System.Drawing.Point(378, 223);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(882, 511);
            this.panel3.TabIndex = 189;
            // 
            // MapPictureBox
            // 
            this.MapPictureBox.AutoScroll = true;
            this.MapPictureBox.Location = new System.Drawing.Point(4, -30);
            this.MapPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.MapPictureBox.Name = "MapPictureBox";
            this.MapPictureBox.Size = new System.Drawing.Size(872, 537);
            this.MapPictureBox.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.AddressList);
            this.panel7.Controls.Add(this.LabelFilter);
            this.panel7.Location = new System.Drawing.Point(12, 50);
            this.panel7.Margin = new System.Windows.Forms.Padding(2);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(358, 684);
            this.panel7.TabIndex = 191;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(2, 23);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(350, 652);
            this.AddressList.TabIndex = 108;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(2, -6);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(355, 29);
            this.LabelFilter.TabIndex = 107;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WorldMapPathForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1268, 749);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressPanel);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "WorldMapPathForm";
            this.Text = "道";
            this.Load += new System.EventHandler(this.WorldMapPathForm_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B4)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown P0;
        private System.Windows.Forms.Label J_ID_PATHEDITOR;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel AddressPanel;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown P8;
        private System.Windows.Forms.Label J_ID_PATHMOVEEDITOR;
        private System.Windows.Forms.NumericUpDown B7;
        private System.Windows.Forms.NumericUpDown B6;
        private System.Windows.Forms.Label J_6;
        private FEBuilderGBA.TextBoxEx L_5_BASEPOINT;
        private System.Windows.Forms.NumericUpDown B5;
        private System.Windows.Forms.Label J_5;
        private FEBuilderGBA.TextBoxEx L_4_BASEPOINT;
        private System.Windows.Forms.NumericUpDown B4;
        private System.Windows.Forms.Label J_4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel7;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label LabelFilter;
        private MapPictureBox MapPictureBox;
    }
}