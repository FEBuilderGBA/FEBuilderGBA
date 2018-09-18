namespace FEBuilderGBA
{
    partial class ToolThreeMargeForm
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
            this.MainPanel = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ExplainLabel = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SetMarkButton = new System.Windows.Forms.Button();
            this.MarkListButton = new System.Windows.Forms.Button();
            this.AllCancelButton = new System.Windows.Forms.Button();
            this.Address = new FEBuilderGBA.TextBoxEx();
            this.label23 = new System.Windows.Forms.Label();
            this.MainPanel.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.panel5);
            this.MainPanel.Controls.Add(this.panel3);
            this.MainPanel.Controls.Add(this.panel1);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(8);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Padding = new System.Windows.Forms.Padding(8);
            this.MainPanel.Size = new System.Drawing.Size(1564, 908);
            this.MainPanel.TabIndex = 154;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.AddressList);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(8, 32);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1546, 826);
            this.panel5.TabIndex = 112;
            // 
            // AddressList
            // 
            this.AddressList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 32;
            this.AddressList.Location = new System.Drawing.Point(0, 0);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(1546, 826);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            this.AddressList.DoubleClick += new System.EventHandler(this.AddressList_DoubleClick);
            this.AddressList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressList_KeyDown);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.label30);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(8, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1546, 24);
            this.panel3.TabIndex = 111;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ExplainLabel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(517, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1027, 22);
            this.panel4.TabIndex = 0;
            // 
            // ExplainLabel
            // 
            this.ExplainLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExplainLabel.Location = new System.Drawing.Point(30, 0);
            this.ExplainLabel.Name = "ExplainLabel";
            this.ExplainLabel.Size = new System.Drawing.Size(997, 22);
            this.ExplainLabel.TabIndex = 110;
            this.ExplainLabel.Text = "相違点を現在のROMへマージします。書き込んだらF5でエミュレータを動作して確認してください。";
            this.ExplainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            this.label30.Dock = System.Windows.Forms.DockStyle.Left;
            this.label30.Location = new System.Drawing.Point(0, 0);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(512, 22);
            this.label30.TabIndex = 106;
            this.label30.Text = "アドレス,長さ,対処法,ヒント";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.Address);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(8, 858);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1546, 40);
            this.panel1.TabIndex = 109;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.SetMarkButton);
            this.panel2.Controls.Add(this.MarkListButton);
            this.panel2.Controls.Add(this.AllCancelButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(644, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(902, 40);
            this.panel2.TabIndex = 114;
            // 
            // SetMarkButton
            // 
            this.SetMarkButton.Location = new System.Drawing.Point(3, 0);
            this.SetMarkButton.Name = "SetMarkButton";
            this.SetMarkButton.Size = new System.Drawing.Size(100, 38);
            this.SetMarkButton.TabIndex = 109;
            this.SetMarkButton.Text = "Set&Mark";
            this.SetMarkButton.UseVisualStyleBackColor = true;
            this.SetMarkButton.Click += new System.EventHandler(this.SetMarkButton_Click);
            // 
            // MarkListButton
            // 
            this.MarkListButton.Location = new System.Drawing.Point(109, 0);
            this.MarkListButton.Name = "MarkListButton";
            this.MarkListButton.Size = new System.Drawing.Size(100, 38);
            this.MarkListButton.TabIndex = 110;
            this.MarkListButton.Text = "Mark&List";
            this.MarkListButton.UseVisualStyleBackColor = true;
            this.MarkListButton.Click += new System.EventHandler(this.MarkListButton_Click);
            // 
            // AllCancelButton
            // 
            this.AllCancelButton.Location = new System.Drawing.Point(490, 0);
            this.AllCancelButton.Name = "AllCancelButton";
            this.AllCancelButton.Size = new System.Drawing.Size(409, 38);
            this.AllCancelButton.TabIndex = 108;
            this.AllCancelButton.Text = "変更をすべてキャンセルする";
            this.AllCancelButton.UseVisualStyleBackColor = true;
            this.AllCancelButton.Click += new System.EventHandler(this.AllCancelButton_Click);
            // 
            // Address
            // 
            this.Address.ErrorMessage = "";
            this.Address.Location = new System.Drawing.Point(90, 5);
            this.Address.Margin = new System.Windows.Forms.Padding(5);
            this.Address.Name = "Address";
            this.Address.Placeholder = "";
            this.Address.ReadOnly = true;
            this.Address.Size = new System.Drawing.Size(226, 25);
            this.Address.TabIndex = 113;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(-2, 1);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(83, 39);
            this.label23.TabIndex = 112;
            this.label23.Text = "アドレス";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToolThreeMargeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1564, 908);
            this.Controls.Add(this.MainPanel);
            this.Name = "ToolThreeMargeForm";
            this.Text = "マージ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolThreeMargeForm_FormClosing);
            this.Load += new System.EventHandler(this.ThreeMargeForm_Load);
            this.MainPanel.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Label label30;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Button AllCancelButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ExplainLabel;
        private System.Windows.Forms.Button MarkListButton;
        private System.Windows.Forms.Button SetMarkButton;
        private System.Windows.Forms.Label label23;
        private TextBoxEx Address;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
    }
}
