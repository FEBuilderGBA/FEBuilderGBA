namespace FEBuilderGBA
{
    partial class TextScriptFormCategorySelectForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.SelectButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Filter = new FEBuilderGBA.TextBoxEx();
            this.ListBox = new FEBuilderGBA.ListBoxEx();
            this.CategoryListBox = new FEBuilderGBA.ListBoxEx();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.SelectButton);
            this.panel1.Controls.Add(this.CategoryListBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 778);
            this.panel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(-1, -1);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 30);
            this.label6.TabIndex = 25;
            this.label6.Text = "カテゴリ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectButton
            // 
            this.SelectButton.Location = new System.Drawing.Point(2, 730);
            this.SelectButton.Margin = new System.Windows.Forms.Padding(2);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(171, 44);
            this.SelectButton.TabIndex = 1;
            this.SelectButton.Text = "命令を選択する";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.Filter);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.ListBox);
            this.panel2.Location = new System.Drawing.Point(188, 12);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 786);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(-1, -1);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 30);
            this.label1.TabIndex = 26;
            this.label1.Text = "検索";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Filter
            // 
            this.Filter.ErrorMessage = "";
            this.Filter.Location = new System.Drawing.Point(182, 1);
            this.Filter.Name = "Filter";
            this.Filter.Placeholder = "";
            this.Filter.Size = new System.Drawing.Size(783, 25);
            this.Filter.TabIndex = 0;
            this.Filter.TextChanged += new System.EventHandler(this.Filter_TextChanged);
            this.Filter.DoubleClick += new System.EventHandler(this.Filter_DoubleClick);
            this.Filter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Filter_KeyDown);
            // 
            // ListBox
            // 
            this.ListBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ListBox.FormattingEnabled = true;
            this.ListBox.ItemHeight = 24;
            this.ListBox.Location = new System.Drawing.Point(-1, 29);
            this.ListBox.Margin = new System.Windows.Forms.Padding(2);
            this.ListBox.Name = "ListBox";
            this.ListBox.Size = new System.Drawing.Size(970, 748);
            this.ListBox.Sorted = true;
            this.ListBox.TabIndex = 1;
            this.ListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListBox_KeyDown);
            this.ListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseDoubleClick);
            this.ListBox.MouseLeave += new System.EventHandler(this.ListBox_MouseLeave);
            this.ListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseMove);
            // 
            // CategoryListBox
            // 
            this.CategoryListBox.FormattingEnabled = true;
            this.CategoryListBox.ItemHeight = 18;
            this.CategoryListBox.Location = new System.Drawing.Point(-1, 29);
            this.CategoryListBox.Margin = new System.Windows.Forms.Padding(2);
            this.CategoryListBox.Name = "CategoryListBox";
            this.CategoryListBox.Size = new System.Drawing.Size(174, 634);
            this.CategoryListBox.TabIndex = 0;
            this.CategoryListBox.SelectedIndexChanged += new System.EventHandler(this.CategoryListBox_SelectedIndexChanged);
            this.CategoryListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CategoryListBox_KeyDown);
            // 
            // TextScriptFormCategorySelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1169, 809);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TextScriptFormCategorySelectForm";
            this.Text = "テキスト-エスケープコード選択";
            this.Load += new System.EventHandler(this.EventScriptFormCategorySelectForm_Load);
            this.Shown += new System.EventHandler(this.EventScriptFormCategorySelectForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ListBoxEx CategoryListBox;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Panel panel2;
        private ListBoxEx ListBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private FEBuilderGBA.TextBoxEx Filter;
    }
}