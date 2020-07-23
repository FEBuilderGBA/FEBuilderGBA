namespace FEBuilderGBA
{
    partial class ImageItemIconForm
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
            this.DragTargetPanel2 = new System.Windows.Forms.Panel();
            this.LinkInternt = new System.Windows.Forms.Label();
            this.JumpToSystemPalette = new System.Windows.Forms.Label();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ReloadListButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReadCount = new System.Windows.Forms.NumericUpDown();
            this.ReadStartAddress = new System.Windows.Forms.NumericUpDown();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.WriteButton = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.DragTargetPanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ItemIconListExpandsButton = new System.Windows.Forms.Button();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.BlockSize = new FEBuilderGBA.TextBoxEx();
            this.SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.Comment = new FEBuilderGBA.TextBoxEx();
            this.X_ICON_REF_ITEM = new FEBuilderGBA.ListBoxEx();
            this.X_ICON_BIG_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.X_ICON_PIC = new FEBuilderGBA.InterpolatedPictureBox();
            this.DragTargetPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).BeginInit();
            this.AddressPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).BeginInit();
            this.DragTargetPanel.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X_ICON_BIG_PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_ICON_PIC)).BeginInit();
            this.SuspendLayout();
            // 
            // DragTargetPanel2
            // 
            this.DragTargetPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DragTargetPanel2.Controls.Add(this.LinkInternt);
            this.DragTargetPanel2.Controls.Add(this.JumpToSystemPalette);
            this.DragTargetPanel2.Controls.Add(this.ImportButton);
            this.DragTargetPanel2.Controls.Add(this.ExportButton);
            this.DragTargetPanel2.Location = new System.Drawing.Point(286, 418);
            this.DragTargetPanel2.Margin = new System.Windows.Forms.Padding(5);
            this.DragTargetPanel2.Name = "DragTargetPanel2";
            this.DragTargetPanel2.Size = new System.Drawing.Size(810, 60);
            this.DragTargetPanel2.TabIndex = 95;
            // 
            // LinkInternt
            // 
            this.LinkInternt.AutoSize = true;
            this.LinkInternt.Location = new System.Drawing.Point(491, 20);
            this.LinkInternt.Name = "LinkInternt";
            this.LinkInternt.Size = new System.Drawing.Size(273, 18);
            this.LinkInternt.TabIndex = 30;
            this.LinkInternt.Text = "インターネットから新しいリソースを探す";
            this.LinkInternt.Click += new System.EventHandler(this.LinkInternt_Click);
            // 
            // JumpToSystemPalette
            // 
            this.JumpToSystemPalette.AccessibleDescription = "@EXPLAIN_ITEMICON_SYSTEM_PALETTE";
            this.JumpToSystemPalette.AutoSize = true;
            this.JumpToSystemPalette.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JumpToSystemPalette.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.JumpToSystemPalette.Location = new System.Drawing.Point(363, 20);
            this.JumpToSystemPalette.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JumpToSystemPalette.Name = "JumpToSystemPalette";
            this.JumpToSystemPalette.Size = new System.Drawing.Size(111, 18);
            this.JumpToSystemPalette.TabIndex = 201;
            this.JumpToSystemPalette.Text = "パレットの変更";
            this.JumpToSystemPalette.Click += new System.EventHandler(this.JumpToSystemPalette_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(16, 14);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(160, 30);
            this.ImportButton.TabIndex = 65;
            this.ImportButton.Text = "画像読込";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(184, 14);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(160, 30);
            this.ExportButton.TabIndex = 64;
            this.ExportButton.Text = "画像取出";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ReloadListButton
            // 
            this.ReloadListButton.Location = new System.Drawing.Point(456, -1);
            this.ReloadListButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReloadListButton.Name = "ReloadListButton";
            this.ReloadListButton.Size = new System.Drawing.Size(111, 31);
            this.ReloadListButton.TabIndex = 25;
            this.ReloadListButton.Text = "再取得";
            this.ReloadListButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(227, -1);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 30);
            this.label3.TabIndex = 52;
            this.label3.Text = "Size:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -1);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 32);
            this.label1.TabIndex = 23;
            this.label1.Text = "先頭アドレス";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(269, -1);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 32);
            this.label2.TabIndex = 24;
            this.label2.Text = "読込数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ReloadListButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReadCount);
            this.panel1.Controls.Add(this.ReadStartAddress);
            this.panel1.Location = new System.Drawing.Point(16, 16);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1078, 30);
            this.panel1.TabIndex = 94;
            // 
            // ReadCount
            // 
            this.ReadCount.Location = new System.Drawing.Point(361, 1);
            this.ReadCount.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReadCount.Name = "ReadCount";
            this.ReadCount.Size = new System.Drawing.Size(79, 25);
            this.ReadCount.TabIndex = 28;
            // 
            // ReadStartAddress
            // 
            this.ReadStartAddress.Hexadecimal = true;
            this.ReadStartAddress.Location = new System.Drawing.Point(128, 1);
            this.ReadStartAddress.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ReadStartAddress.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.ReadStartAddress.Name = "ReadStartAddress";
            this.ReadStartAddress.Size = new System.Drawing.Size(130, 25);
            this.ReadStartAddress.TabIndex = 0;
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
            this.AddressPanel.Location = new System.Drawing.Point(286, 45);
            this.AddressPanel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(808, 30);
            this.AddressPanel.TabIndex = 93;
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(619, -2);
            this.WriteButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(188, 30);
            this.WriteButton.TabIndex = 53;
            this.WriteButton.Text = "書き込み";
            this.WriteButton.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(379, -1);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 39;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Address
            // 
            this.Address.Hexadecimal = true;
            this.Address.Location = new System.Drawing.Point(91, 1);
            this.Address.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(130, 25);
            this.Address.TabIndex = 0;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-3, -1);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(86, 32);
            this.label23.TabIndex = 1;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DragTargetPanel
            // 
            this.DragTargetPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DragTargetPanel.Controls.Add(this.Comment);
            this.DragTargetPanel.Controls.Add(this.label6);
            this.DragTargetPanel.Controls.Add(this.X_ICON_REF_ITEM);
            this.DragTargetPanel.Controls.Add(this.label5);
            this.DragTargetPanel.Controls.Add(this.label4);
            this.DragTargetPanel.Controls.Add(this.X_ICON_BIG_PIC);
            this.DragTargetPanel.Controls.Add(this.label7);
            this.DragTargetPanel.Controls.Add(this.X_ICON_PIC);
            this.DragTargetPanel.Location = new System.Drawing.Point(286, 75);
            this.DragTargetPanel.Margin = new System.Windows.Forms.Padding(5);
            this.DragTargetPanel.Name = "DragTargetPanel";
            this.DragTargetPanel.Size = new System.Drawing.Size(809, 333);
            this.DragTargetPanel.TabIndex = 92;
            // 
            // label6
            // 
            this.label6.AccessibleDescription = "@COMMENT";
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(5, 216);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 31);
            this.label6.TabIndex = 199;
            this.label6.Text = "コメント";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(314, 7);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(207, 31);
            this.label5.TabIndex = 71;
            this.label5.Text = "参照アイテム";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(5, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 31);
            this.label4.TabIndex = 70;
            this.label4.Text = "拡大";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(4, 8);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 31);
            this.label7.TabIndex = 68;
            this.label7.Text = "原寸大";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.ItemIconListExpandsButton);
            this.panel6.Controls.Add(this.LabelFilter);
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Location = new System.Drawing.Point(16, 48);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(263, 430);
            this.panel6.TabIndex = 72;
            // 
            // ItemIconListExpandsButton
            // 
            this.ItemIconListExpandsButton.Location = new System.Drawing.Point(2, 400);
            this.ItemIconListExpandsButton.Margin = new System.Windows.Forms.Padding(2);
            this.ItemIconListExpandsButton.Name = "ItemIconListExpandsButton";
            this.ItemIconListExpandsButton.Size = new System.Drawing.Size(259, 30);
            this.ItemIconListExpandsButton.TabIndex = 117;
            this.ItemIconListExpandsButton.Text = "リストの拡張";
            this.ItemIconListExpandsButton.UseVisualStyleBackColor = true;
            this.ItemIconListExpandsButton.Click += new System.EventHandler(this.ItemIconListExpandsButton_Click);
            // 
            // LabelFilter
            // 
            this.LabelFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Location = new System.Drawing.Point(1, 0);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(260, 26);
            this.LabelFilter.TabIndex = 55;
            this.LabelFilter.Text = "名前";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(1, 24);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(260, 364);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // BlockSize
            // 
            this.BlockSize.ErrorMessage = "";
            this.BlockSize.Location = new System.Drawing.Point(303, 1);
            this.BlockSize.Margin = new System.Windows.Forms.Padding(5);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Placeholder = "";
            this.BlockSize.ReadOnly = true;
            this.BlockSize.Size = new System.Drawing.Size(71, 25);
            this.BlockSize.TabIndex = 52;
            // 
            // SelectAddress
            // 
            this.SelectAddress.ErrorMessage = "";
            this.SelectAddress.Location = new System.Drawing.Point(503, 1);
            this.SelectAddress.Margin = new System.Windows.Forms.Padding(5);
            this.SelectAddress.Name = "SelectAddress";
            this.SelectAddress.Placeholder = "";
            this.SelectAddress.ReadOnly = true;
            this.SelectAddress.Size = new System.Drawing.Size(107, 25);
            this.SelectAddress.TabIndex = 40;
            // 
            // Comment
            // 
            this.Comment.ErrorMessage = "";
            this.Comment.Location = new System.Drawing.Point(174, 219);
            this.Comment.Name = "Comment";
            this.Comment.Placeholder = "";
            this.Comment.Size = new System.Drawing.Size(344, 25);
            this.Comment.TabIndex = 200;
            // 
            // X_ICON_REF_ITEM
            // 
            this.X_ICON_REF_ITEM.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.X_ICON_REF_ITEM.FormattingEnabled = true;
            this.X_ICON_REF_ITEM.IntegralHeight = false;
            this.X_ICON_REF_ITEM.ItemHeight = 18;
            this.X_ICON_REF_ITEM.Location = new System.Drawing.Point(552, 9);
            this.X_ICON_REF_ITEM.Margin = new System.Windows.Forms.Padding(4);
            this.X_ICON_REF_ITEM.Name = "X_ICON_REF_ITEM";
            this.X_ICON_REF_ITEM.Size = new System.Drawing.Size(256, 318);
            this.X_ICON_REF_ITEM.TabIndex = 92;
            // 
            // X_ICON_BIG_PIC
            // 
            this.X_ICON_BIG_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_ICON_BIG_PIC.Location = new System.Drawing.Point(176, 48);
            this.X_ICON_BIG_PIC.Margin = new System.Windows.Forms.Padding(5);
            this.X_ICON_BIG_PIC.Name = "X_ICON_BIG_PIC";
            this.X_ICON_BIG_PIC.Size = new System.Drawing.Size(80, 77);
            this.X_ICON_BIG_PIC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.X_ICON_BIG_PIC.TabIndex = 69;
            this.X_ICON_BIG_PIC.TabStop = false;
            // 
            // X_ICON_PIC
            // 
            this.X_ICON_PIC.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.X_ICON_PIC.Location = new System.Drawing.Point(175, 8);
            this.X_ICON_PIC.Margin = new System.Windows.Forms.Padding(5);
            this.X_ICON_PIC.Name = "X_ICON_PIC";
            this.X_ICON_PIC.Size = new System.Drawing.Size(40, 38);
            this.X_ICON_PIC.TabIndex = 67;
            this.X_ICON_PIC.TabStop = false;
            // 
            // ImageItemIconForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1100, 480);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.DragTargetPanel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddressPanel);
            this.Controls.Add(this.DragTargetPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ImageItemIconForm";
            this.Text = "アイテムアイコン";
            this.Load += new System.EventHandler(this.ImageIconForm_Load);
            this.DragTargetPanel2.ResumeLayout(false);
            this.DragTargetPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadStartAddress)).EndInit();
            this.AddressPanel.ResumeLayout(false);
            this.AddressPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Address)).EndInit();
            this.DragTargetPanel.ResumeLayout(false);
            this.DragTargetPanel.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X_ICON_BIG_PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X_ICON_PIC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel DragTargetPanel2;
        private System.Windows.Forms.Button ReloadListButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown ReadCount;
        private System.Windows.Forms.NumericUpDown ReadStartAddress;
        private FEBuilderGBA.TextBoxEx BlockSize;
        private System.Windows.Forms.Panel AddressPanel;
        private FEBuilderGBA.TextBoxEx SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown Address;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel DragTargetPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private ListBoxEx X_ICON_REF_ITEM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label LabelFilter;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportButton;
        private InterpolatedPictureBox X_ICON_PIC;
        private InterpolatedPictureBox X_ICON_BIG_PIC;
        private System.Windows.Forms.Button ItemIconListExpandsButton;
        private TextBoxEx Comment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Label JumpToSystemPalette;
        private System.Windows.Forms.Label LinkInternt;
    }
}