namespace FEBuilderGBA
{
    partial class MapTerrainBGLookupTableForm
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.ERROR_Not_Allocated = new System.Windows.Forms.Label();
            this.X_JUMP_FLOOR = new System.Windows.Forms.Label();
            this.L_0_BATTLEBGICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.L_0_BATTLEBG = new FEBuilderGBA.TextBoxEx();
            this.B0 = new System.Windows.Forms.NumericUpDown();
            this.J_0_BATTLEBG = new System.Windows.Forms.Label();
            this.WriteButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.panel5 = new System.Windows.Forms.Panel();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_BATTLEBGICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.ERROR_Not_Allocated);
            this.panel4.Controls.Add(this.X_JUMP_FLOOR);
            this.panel4.Controls.Add(this.L_0_BATTLEBGICON);
            this.panel4.Controls.Add(this.L_0_BATTLEBG);
            this.panel4.Controls.Add(this.B0);
            this.panel4.Controls.Add(this.J_0_BATTLEBG);
            this.panel4.Location = new System.Drawing.Point(481, 79);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(738, 635);
            this.panel4.TabIndex = 80;
            // 
            // ERROR_Not_Allocated
            // 
            this.ERROR_Not_Allocated.AutoSize = true;
            this.ERROR_Not_Allocated.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ERROR_Not_Allocated.ForeColor = System.Drawing.Color.Red;
            this.ERROR_Not_Allocated.Location = new System.Drawing.Point(69, 473);
            this.ERROR_Not_Allocated.Name = "ERROR_Not_Allocated";
            this.ERROR_Not_Allocated.Size = new System.Drawing.Size(599, 36);
            this.ERROR_Not_Allocated.TabIndex = 186;
            this.ERROR_Not_Allocated.Text = "拡張された領域にデータが割り当てられていません。\r\nパッチ「戦闘床地形と戦闘背景のリストを拡張する」から、データを割り振ってください。";
            this.ERROR_Not_Allocated.Visible = false;
            this.ERROR_Not_Allocated.Click += new System.EventHandler(this.ERROR_Not_Allocated_Click);
            // 
            // X_JUMP_FLOOR
            // 
            this.X_JUMP_FLOOR.AutoSize = true;
            this.X_JUMP_FLOOR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.X_JUMP_FLOOR.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.X_JUMP_FLOOR.Location = new System.Drawing.Point(69, 444);
            this.X_JUMP_FLOOR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X_JUMP_FLOOR.Name = "X_JUMP_FLOOR";
            this.X_JUMP_FLOOR.Size = new System.Drawing.Size(172, 18);
            this.X_JUMP_FLOOR.TabIndex = 73;
            this.X_JUMP_FLOOR.Text = "戦闘アニメの床へJump";
            this.X_JUMP_FLOOR.Click += new System.EventHandler(this.X_JUMP_FLOOR_Click);
            // 
            // L_0_BATTLEBGICON
            // 
            this.L_0_BATTLEBGICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.L_0_BATTLEBGICON.Location = new System.Drawing.Point(72, 62);
            this.L_0_BATTLEBGICON.Margin = new System.Windows.Forms.Padding(2);
            this.L_0_BATTLEBGICON.Name = "L_0_BATTLEBGICON";
            this.L_0_BATTLEBGICON.Size = new System.Drawing.Size(662, 343);
            this.L_0_BATTLEBGICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.L_0_BATTLEBGICON.TabIndex = 185;
            this.L_0_BATTLEBGICON.TabStop = false;
            // 
            // L_0_BATTLEBG
            // 
            this.L_0_BATTLEBG.ErrorMessage = "";
            this.L_0_BATTLEBG.Location = new System.Drawing.Point(235, 5);
            this.L_0_BATTLEBG.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.L_0_BATTLEBG.Name = "L_0_BATTLEBG";
            this.L_0_BATTLEBG.Placeholder = "";
            this.L_0_BATTLEBG.ReadOnly = true;
            this.L_0_BATTLEBG.Size = new System.Drawing.Size(169, 25);
            this.L_0_BATTLEBG.TabIndex = 80;
            // 
            // B0
            // 
            this.B0.Hexadecimal = true;
            this.B0.Location = new System.Drawing.Point(147, 4);
            this.B0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.B0.Name = "B0";
            this.B0.Size = new System.Drawing.Size(80, 25);
            this.B0.TabIndex = 79;
            // 
            // J_0_BATTLEBG
            // 
            this.J_0_BATTLEBG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_BATTLEBG.Location = new System.Drawing.Point(-1, 0);
            this.J_0_BATTLEBG.Name = "J_0_BATTLEBG";
            this.J_0_BATTLEBG.Size = new System.Drawing.Size(142, 30);
            this.J_0_BATTLEBG.TabIndex = 78;
            this.J_0_BATTLEBG.Text = "値";
            this.J_0_BATTLEBG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(571, -2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(167, 30);
            this.WriteButton.TabIndex = 9;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.FilterComboBox);
            this.panel3.Controls.Add(this.ReloadListButton);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.ReadCount);
            this.panel3.Controls.Add(this.ReadStartAddress);
            this.panel3.Location = new System.Drawing.Point(20, 18);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1199, 30);
            this.panel3.TabIndex = 79;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(-1, -3);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 34);
            this.label2.TabIndex = 160;
            this.label2.Text = "条件:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Location = new System.Drawing.Point(81, 1);
            this.FilterComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(373, 26);
            this.FilterComboBox.TabIndex = 159;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(888, 0);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(112, 30);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(459, -2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "先頭アドレス";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(715, -2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 30);
            this.label9.TabIndex = 24;
            this.label9.Text = "読込数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(806, 0);
            this.ReadCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(78, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(578, 1);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 27;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.SelectAddress);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.Address);
            this.panel5.Controls.Add(this.label55);
            this.panel5.Controls.Add(this.WriteButton);
            this.panel5.Location = new System.Drawing.Point(480, 49);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(738, 30);
            this.panel5.TabIndex = 78;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(382, -1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(158, 25);
            this.SelectAddress.TabIndex = 58;
            // 
            // label22
            // 
            this.label22.AccessibleDescription = "@SELECTION_ADDRESS";
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(255, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 57;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(119, 2);
            this.Address.Margin = new System.Windows.Forms.Padding(2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 55;
            // 
            // label55
            // 
            this.label55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label55.Location = new System.Drawing.Point(-1, 0);
            this.label55.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(115, 30);
            this.label55.TabIndex = 56;
            this.label55.Text = "アドレス";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(20, 53);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(453, 661);
            this.panel6.TabIndex = 146;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-3, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(455, 26);
            this.LabelFilter.TabIndex = 106;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 25);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(455, 630);
            this.AddressList.TabIndex = 0;
            // 
            // MapTerrainBGLookupTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1237, 726);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "MapTerrainBGLookupTableForm";
            this.Text = "地形背景対応表";
            this.Load += new System.EventHandler(this.MapTerrainFloorLookupTableForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L_0_BATTLEBGICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B0)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label J_0_BATTLEBG;
        private FEBuilderGBA.TextBoxEx L_0_BATTLEBG;
        private System.Windows.Forms.NumericUpDown B0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private InterpolatedPictureBox L_0_BATTLEBGICON;
        private TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label X_JUMP_FLOOR;
        private System.Windows.Forms.Label ERROR_Not_Allocated;
    }
}