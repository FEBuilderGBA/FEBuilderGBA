namespace FEBuilderGBA
{
    partial class OtherTextForm
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
            this.J_0_TEXT = new System.Windows.Forms.Label();
            this.TextBox = new FEBuilderGBA.TextBoxEx();
            this.label14 = new System.Windows.Forms.Label();
            this.TextWriteButton = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.AddressPointer = new FEBuilderGBA.TextBoxEx();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.panel6 = new System.Windows.Forms.Panel();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.J_0_TEXT);
            this.panel4.Controls.Add(this.TextBox);
            this.panel4.Location = new System.Drawing.Point(286, 41);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(933, 360);
            this.panel4.TabIndex = 80;
            // 
            // J_0_TEXT
            // 
            this.J_0_TEXT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_0_TEXT.Location = new System.Drawing.Point(-1, 0);
            this.J_0_TEXT.Name = "J_0_TEXT";
            this.J_0_TEXT.Size = new System.Drawing.Size(142, 30);
            this.J_0_TEXT.TabIndex = 78;
            this.J_0_TEXT.Text = "名前";
            this.J_0_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBox
            // 
            this.TextBox.ErrorMessage = "";
            this.TextBox.Location = new System.Drawing.Point(148, 1);
            this.TextBox.Name = "TextBox";
            this.TextBox.Placeholder = "";
            this.TextBox.Size = new System.Drawing.Size(185, 25);
            this.TextBox.TabIndex = 77;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(361, -2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 30);
            this.label14.TabIndex = 52;
            this.label14.Text = "Size:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextWriteButton
            // 
            this.TextWriteButton.Location = new System.Drawing.Point(764, -1);
            this.TextWriteButton.Name = "TextWriteButton";
            this.TextWriteButton.Size = new System.Drawing.Size(167, 30);
            this.TextWriteButton.TabIndex = 9;
            this.TextWriteButton.Text = "書き込み";
            this.TextWriteButton.UseVisualStyleBackColor = true;
            this.TextWriteButton.Click += new System.EventHandler(this.TextWriteButton_Click);
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(-1, -1);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(142, 30);
            this.label16.TabIndex = 1;
            this.label16.Text = "アドレス";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.AddressPointer);
            this.panel5.Controls.Add(this.BlockSize);
            this.panel5.Controls.Add(this.TextWriteButton);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(286, 9);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(934, 30);
            this.panel5.TabIndex = 78;
            // 
            // AddressPointer
            // 
            this.AddressPointer.ErrorMessage = "";
            this.AddressPointer.Location = new System.Drawing.Point(150, 3);
            this.AddressPointer.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.AddressPointer.Name = "AddressPointer";
            this.AddressPointer.Placeholder = "";
            this.AddressPointer.ReadOnly = true;
            this.AddressPointer.Size = new System.Drawing.Size(169, 25);
            this.AddressPointer.TabIndex = 54;
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(444, -2);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(82, 25);
            this.BlockSize.TabIndex = 53;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(20, 11);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(260, 390);
            this.panel6.TabIndex = 146;
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(-1, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(259, 26);
            this.LabelFilter.TabIndex = 106;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 24);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(259, 364);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // OtherTextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1237, 413);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "OtherTextForm";
            this.Text = "その他ROM直値テキスト";
            this.Load += new System.EventHandler(this.OtherTextForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button TextWriteButton;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label J_0_TEXT;
        private FEBuilderGBA.TextBoxEx TextBox;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private FEBuilderGBA.TextBoxEx AddressPointer;
    }
}