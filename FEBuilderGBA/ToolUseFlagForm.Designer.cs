namespace FEBuilderGBA
{
    partial class ToolUseFlagForm
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
            this.panel14 = new System.Windows.Forms.Panel();
            this.MAP_LISTBOX = new FEBuilderGBA.ListBoxEx();
            this.LabelFilter = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel14.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel14
            // 
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.MAP_LISTBOX);
            this.panel14.Controls.Add(this.LabelFilter);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel14.Location = new System.Drawing.Point(0, 0);
            this.panel14.Margin = new System.Windows.Forms.Padding(2);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(258, 765);
            this.panel14.TabIndex = 189;
            // 
            // MAP_LISTBOX
            // 
            this.MAP_LISTBOX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MAP_LISTBOX.FormattingEnabled = true;
            this.MAP_LISTBOX.IntegralHeight = false;
            this.MAP_LISTBOX.ItemHeight = 18;
            this.MAP_LISTBOX.Location = new System.Drawing.Point(0, 26);
            this.MAP_LISTBOX.Margin = new System.Windows.Forms.Padding(4);
            this.MAP_LISTBOX.Name = "MAP_LISTBOX";
            this.MAP_LISTBOX.Size = new System.Drawing.Size(256, 737);
            this.MAP_LISTBOX.TabIndex = 0;
            this.MAP_LISTBOX.SelectedIndexChanged += new System.EventHandler(this.MAP_LISTBOX_SelectedIndexChanged);
            // 
            // LabelFilter
            // 
            this.LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabelFilter.Location = new System.Drawing.Point(0, 0);
            this.LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelFilter.Name = "LabelFilter";
            this.LabelFilter.Size = new System.Drawing.Size(256, 26);
            this.LabelFilter.TabIndex = 106;
            this.LabelFilter.Text = "マップ名";
            this.LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AddressList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(258, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(839, 738);
            this.panel1.TabIndex = 190;
            // 
            // AddressList
            // 
            this.AddressList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(0, 0);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(839, 738);
            this.AddressList.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(258, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(839, 27);
            this.panel2.TabIndex = 191;
            // 
            // ToolUseFlagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1097, 765);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel14);
            this.Name = "ToolUseFlagForm";
            this.Text = "章で利用しているフラグ";
            this.Load += new System.EventHandler(this.ToolUseFlagForm_Load);
            this.panel14.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel14;
        private ListBoxEx MAP_LISTBOX;
        private System.Windows.Forms.Label LabelFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private ListBoxEx AddressList;
    }
}