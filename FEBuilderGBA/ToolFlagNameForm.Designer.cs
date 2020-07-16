namespace FEBuilderGBA
{
    partial class ToolFlagNameForm
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
            this.panel8 = new System.Windows.Forms.Panel();
            this.AddressListExpandsButton_255 = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ToolUseFlagButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.WriteButton = new System.Windows.Forms.Button();
            this.FlagNameTextBox = new FEBuilderGBA.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.panel8.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.AddressListExpandsButton_255);
            this.panel8.Controls.Add(this.LabelFilter);
            this.panel8.Controls.Add(this.AddressList);
            this.panel8.Location = new System.Drawing.Point(11, 11);
            this.panel8.Margin = new System.Windows.Forms.Padding(2);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(358, 781);
            this.panel8.TabIndex = 64;
            // 
            // AddressListExpandsButton_255
            // 
            this.AddressListExpandsButton_255.Location = new System.Drawing.Point(-1, 791);
            this.AddressListExpandsButton_255.Margin = new System.Windows.Forms.Padding(2);
            this.AddressListExpandsButton_255.Name = "AddressListExpandsButton_255";
            this.AddressListExpandsButton_255.Size = new System.Drawing.Size(267, 30);
            this.AddressListExpandsButton_255.TabIndex = 116;
            this.AddressListExpandsButton_255.Text = "リストの拡張";
            this.AddressListExpandsButton_255.UseVisualStyleBackColor = true;
            // 
            // LabelFilter
            // 
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(0, -1);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(357, 26);
            this.LabelFilter.TabIndex = 55;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(-1, 24);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(359, 742);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            this.AddressList.DoubleClick += new System.EventHandler(this.AddressList_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ToolUseFlagButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DeleteButton);
            this.panel1.Controls.Add(this.WriteButton);
            this.panel1.Controls.Add(this.FlagNameTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(374, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(591, 779);
            this.panel1.TabIndex = 65;
            // 
            // ToolUseFlagButton
            // 
            this.ToolUseFlagButton.Location = new System.Drawing.Point(3, 370);
            this.ToolUseFlagButton.Margin = new System.Windows.Forms.Padding(2);
            this.ToolUseFlagButton.Name = "ToolUseFlagButton";
            this.ToolUseFlagButton.Size = new System.Drawing.Size(580, 44);
            this.ToolUseFlagButton.TabIndex = 5;
            this.ToolUseFlagButton.Text = "章内で利用しているフラグの確認";
            this.ToolUseFlagButton.UseVisualStyleBackColor = true;
            this.ToolUseFlagButton.Click += new System.EventHandler(this.ToolUseFlagButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 36);
            this.label2.TabIndex = 4;
            this.label2.Text = "フラグに名前を設定すると、より理解しやすくなります。\r\nフラグの名前は、ROMごとに別ファイルに保存します。";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(6, 134);
            this.DeleteButton.Margin = new System.Windows.Forms.Padding(2);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(311, 30);
            this.DeleteButton.TabIndex = 3;
            this.DeleteButton.Text = "ディフォルトに戻す";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(450, 2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(133, 30);
            this.WriteButton.TabIndex = 2;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // FlagNameTextBox
            // 
            this.FlagNameTextBox.ErrorMessage = "";
            this.FlagNameTextBox.Location = new System.Drawing.Point(3, 77);
            this.FlagNameTextBox.Name = "FlagNameTextBox";
            this.FlagNameTextBox.Size = new System.Drawing.Size(580, 25);
            this.FlagNameTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "フラグの名前";
            // 
            // ToolFlagNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(971, 804);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel8);
            this.Name = "ToolFlagNameForm";
            this.Text = "フラグに名前設定";
            this.Load += new System.EventHandler(this.ToolFlagNameForm_Load);
            this.panel8.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button AddressListExpandsButton_255;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private TextBoxEx FlagNameTextBox;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ToolUseFlagButton;
    }
}