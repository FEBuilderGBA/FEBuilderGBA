namespace FEBuilderGBA
{
    partial class ToolUndoForm
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
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.rollbackButton = new System.Windows.Forms.Button();
            this.testplayButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddressList
            // 
            this.AddressList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(31, 31);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(1055, 796);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            this.AddressList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AddressList_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Controls.Add(this.AddressList);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1086, 827);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(1, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 164);
            this.label1.TabIndex = 57;
            this.label1.Text = "↑最新";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(2, 622);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 205);
            this.label2.TabIndex = 58;
            this.label2.Text = "より古い↓";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Location = new System.Drawing.Point(1, 1);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(1085, 26);
            this.label30.TabIndex = 56;
            this.label30.Text = "履歴(上が最新)";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rollbackButton
            // 
            this.rollbackButton.Location = new System.Drawing.Point(766, 846);
            this.rollbackButton.Name = "rollbackButton";
            this.rollbackButton.Size = new System.Drawing.Size(332, 39);
            this.rollbackButton.TabIndex = 2;
            this.rollbackButton.Text = "このバージョンに戻す";
            this.rollbackButton.UseVisualStyleBackColor = true;
            this.rollbackButton.Click += new System.EventHandler(this.rollbackButton_Click);
            // 
            // testplayButton
            // 
            this.testplayButton.Location = new System.Drawing.Point(12, 846);
            this.testplayButton.Name = "testplayButton";
            this.testplayButton.Size = new System.Drawing.Size(563, 39);
            this.testplayButton.TabIndex = 3;
            this.testplayButton.Text = "このバージョンをエミュレータでテストプレイ";
            this.testplayButton.UseVisualStyleBackColor = true;
            this.testplayButton.Click += new System.EventHandler(this.testplayButton_Click);
            // 
            // ToolUndoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1115, 889);
            this.Controls.Add(this.testplayButton);
            this.Controls.Add(this.rollbackButton);
            this.Controls.Add(this.panel1);
            this.Name = "ToolUndoForm";
            this.Text = "UNDO";
            this.Activated += new System.EventHandler(this.UndoForm_Activated);
            this.Load += new System.EventHandler(this.UndoForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListBoxEx AddressList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button rollbackButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button testplayButton;
    }
}