namespace FEBuilderGBA
{
    partial class HexEditorMark
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
            this.JumpTo = new System.Windows.Forms.Button();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // JumpTo
            // 
            this.JumpTo.Location = new System.Drawing.Point(109, 598);
            this.JumpTo.Name = "JumpTo";
            this.JumpTo.Size = new System.Drawing.Size(272, 43);
            this.JumpTo.TabIndex = 23;
            this.JumpTo.Text = "移動する";
            this.JumpTo.UseVisualStyleBackColor = true;
            this.JumpTo.Click += new System.EventHandler(this.JumpTo_Click);
            // 
            // AddressList
            // 
            this.AddressList.FormattingEnabled = true;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(12, 44);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(365, 544);
            this.AddressList.TabIndex = 21;
            this.AddressList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AddressList_MouseDoubleClick);
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Location = new System.Drawing.Point(11, 9);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(370, 32);
            this.label12.TabIndex = 19;
            this.label12.Text = "マークしたアドレス";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HexEditorMark
            // 
            this.AcceptButton = this.JumpTo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(393, 648);
            this.Controls.Add(this.JumpTo);
            this.Controls.Add(this.AddressList);
            this.Controls.Add(this.label12);
            this.Name = "HexEditorMark";
            this.Text = "マークしたアドレス";
            this.Load += new System.EventHandler(this.HexEditorMark_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button JumpTo;
        private ListBoxEx AddressList;
        private System.Windows.Forms.Label label12;
    }
}