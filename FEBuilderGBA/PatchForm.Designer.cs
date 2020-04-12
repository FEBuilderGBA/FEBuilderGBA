namespace FEBuilderGBA
{
    partial class PatchForm
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
            this.PatchList = new FEBuilderGBA.ListBoxEx();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Filter = new FEBuilderGBA.TextBoxEx();
            this.FilterExLabel = new System.Windows.Forms.Label();
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.PatchFilename = new FEBuilderGBA.TextBoxEx();
            this.panel5 = new System.Windows.Forms.Panel();
            this.PatchOpenButton = new System.Windows.Forms.Button();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.TAB = new System.Windows.Forms.TabControl();
            this.ErrorPage = new System.Windows.Forms.TabPage();
            this.ERROR_label = new System.Windows.Forms.Label();
            this.ERROR_TextBox = new FEBuilderGBA.TextBoxEx();
            this.ElsePage = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.ELSE_TextBox = new FEBuilderGBA.TextBoxEx();
            this.PatchedPage = new System.Windows.Forms.TabPage();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.UnInstallButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PATCHED_TextBox = new FEBuilderGBA.TextBoxEx();
            this.PatchPage = new System.Windows.Forms.TabPage();
            this.ConflictPage = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.CONFLICT_TextBox = new FEBuilderGBA.TextBoxEx();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.ContentPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.TAB.SuspendLayout();
            this.ErrorPage.SuspendLayout();
            this.ElsePage.SuspendLayout();
            this.PatchedPage.SuspendLayout();
            this.ConflictPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PatchList);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 855);
            this.panel1.TabIndex = 2;
            // 
            // PatchList
            // 
            this.PatchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PatchList.FormattingEnabled = true;
            this.PatchList.IntegralHeight = false;
            this.PatchList.ItemHeight = 18;
            this.PatchList.Location = new System.Drawing.Point(0, 32);
            this.PatchList.Margin = new System.Windows.Forms.Padding(2);
            this.PatchList.Name = "PatchList";
            this.PatchList.Size = new System.Drawing.Size(584, 823);
            this.PatchList.TabIndex = 1;
            this.PatchList.SelectedIndexChanged += new System.EventHandler(this.PatchList_SelectedIndexChanged);
            this.PatchList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PatchList_KeyDown);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Filter);
            this.panel4.Controls.Add(this.FilterExLabel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(584, 32);
            this.panel4.TabIndex = 4;
            // 
            // Filter
            // 
            this.Filter.ErrorMessage = "";
            this.Filter.Location = new System.Drawing.Point(110, 3);
            this.Filter.Name = "Filter";
            this.Filter.Placeholder = "";
            this.Filter.Size = new System.Drawing.Size(470, 25);
            this.Filter.TabIndex = 0;
            this.Filter.TextChanged += new System.EventHandler(this.Filter_TextChanged);
            this.Filter.DoubleClick += new System.EventHandler(this.Filter_DoubleClick);
            this.Filter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Filter_KeyDown);
            // 
            // FilterExLabel
            // 
            this.FilterExLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilterExLabel.Location = new System.Drawing.Point(0, 0);
            this.FilterExLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FilterExLabel.Name = "FilterExLabel";
            this.FilterExLabel.Size = new System.Drawing.Size(107, 32);
            this.FilterExLabel.TabIndex = 4;
            this.FilterExLabel.Text = "Filter:";
            this.FilterExLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FilterExLabel.Click += new System.EventHandler(this.FilterExLabel_Click);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.panel3);
            this.ContentPanel.Controls.Add(this.TAB);
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentPanel.Location = new System.Drawing.Point(584, 0);
            this.ContentPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Size = new System.Drawing.Size(1035, 855);
            this.ContentPanel.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.PatchFilename);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 815);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1035, 40);
            this.panel3.TabIndex = 1;
            // 
            // PatchFilename
            // 
            this.PatchFilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PatchFilename.ErrorMessage = "";
            this.PatchFilename.Location = new System.Drawing.Point(0, 0);
            this.PatchFilename.Margin = new System.Windows.Forms.Padding(4);
            this.PatchFilename.Name = "PatchFilename";
            this.PatchFilename.Placeholder = "";
            this.PatchFilename.ReadOnly = true;
            this.PatchFilename.Size = new System.Drawing.Size(606, 25);
            this.PatchFilename.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.PatchOpenButton);
            this.panel5.Controls.Add(this.ReloadButton);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(606, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(425, 36);
            this.panel5.TabIndex = 5;
            // 
            // PatchOpenButton
            // 
            this.PatchOpenButton.Location = new System.Drawing.Point(2, -2);
            this.PatchOpenButton.Margin = new System.Windows.Forms.Padding(2);
            this.PatchOpenButton.Name = "PatchOpenButton";
            this.PatchOpenButton.Size = new System.Drawing.Size(233, 41);
            this.PatchOpenButton.TabIndex = 5;
            this.PatchOpenButton.Text = "パッチファイルを開く";
            this.PatchOpenButton.UseVisualStyleBackColor = true;
            this.PatchOpenButton.Click += new System.EventHandler(this.PatchOpenButton_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Location = new System.Drawing.Point(238, -2);
            this.ReloadButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(186, 41);
            this.ReloadButton.TabIndex = 4;
            this.ReloadButton.Text = "パッチリロード";
            this.ReloadButton.UseVisualStyleBackColor = true;
            this.ReloadButton.Click += new System.EventHandler(this.RealodButton_Click);
            // 
            // TAB
            // 
            this.TAB.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.TAB.Controls.Add(this.ErrorPage);
            this.TAB.Controls.Add(this.ElsePage);
            this.TAB.Controls.Add(this.PatchedPage);
            this.TAB.Controls.Add(this.PatchPage);
            this.TAB.Controls.Add(this.ConflictPage);
            this.TAB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TAB.Location = new System.Drawing.Point(0, 0);
            this.TAB.Margin = new System.Windows.Forms.Padding(0);
            this.TAB.Name = "TAB";
            this.TAB.SelectedIndex = 0;
            this.TAB.Size = new System.Drawing.Size(1035, 855);
            this.TAB.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TAB.TabIndex = 0;
            // 
            // ErrorPage
            // 
            this.ErrorPage.BackColor = System.Drawing.SystemColors.Control;
            this.ErrorPage.Controls.Add(this.ERROR_label);
            this.ErrorPage.Controls.Add(this.ERROR_TextBox);
            this.ErrorPage.Location = new System.Drawing.Point(4, 4);
            this.ErrorPage.Margin = new System.Windows.Forms.Padding(2);
            this.ErrorPage.Name = "ErrorPage";
            this.ErrorPage.Padding = new System.Windows.Forms.Padding(2);
            this.ErrorPage.Size = new System.Drawing.Size(1027, 823);
            this.ErrorPage.TabIndex = 0;
            this.ErrorPage.Text = "ERROR";
            // 
            // ERROR_label
            // 
            this.ERROR_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ERROR_label.Location = new System.Drawing.Point(7, 24);
            this.ERROR_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ERROR_label.Name = "ERROR_label";
            this.ERROR_label.Size = new System.Drawing.Size(999, 32);
            this.ERROR_label.TabIndex = 3;
            this.ERROR_label.Text = "エラーが発生しているため、このパッチは利用できません";
            this.ERROR_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ERROR_TextBox
            // 
            this.ERROR_TextBox.ErrorMessage = "";
            this.ERROR_TextBox.Location = new System.Drawing.Point(7, 84);
            this.ERROR_TextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ERROR_TextBox.Multiline = true;
            this.ERROR_TextBox.Name = "ERROR_TextBox";
            this.ERROR_TextBox.Placeholder = "";
            this.ERROR_TextBox.ReadOnly = true;
            this.ERROR_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ERROR_TextBox.Size = new System.Drawing.Size(999, 658);
            this.ERROR_TextBox.TabIndex = 0;
            // 
            // ElsePage
            // 
            this.ElsePage.BackColor = System.Drawing.SystemColors.Control;
            this.ElsePage.Controls.Add(this.label2);
            this.ElsePage.Controls.Add(this.ELSE_TextBox);
            this.ElsePage.Location = new System.Drawing.Point(4, 4);
            this.ElsePage.Margin = new System.Windows.Forms.Padding(2);
            this.ElsePage.Name = "ElsePage";
            this.ElsePage.Padding = new System.Windows.Forms.Padding(2);
            this.ElsePage.Size = new System.Drawing.Size(1027, 823);
            this.ElsePage.TabIndex = 1;
            this.ElsePage.Text = "ELSE";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(998, 32);
            this.label2.TabIndex = 6;
            this.label2.Text = "以下の条件を満たしていないため利用できません。";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ELSE_TextBox
            // 
            this.ELSE_TextBox.ErrorMessage = "";
            this.ELSE_TextBox.Location = new System.Drawing.Point(6, 84);
            this.ELSE_TextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ELSE_TextBox.Multiline = true;
            this.ELSE_TextBox.Name = "ELSE_TextBox";
            this.ELSE_TextBox.Placeholder = "";
            this.ELSE_TextBox.ReadOnly = true;
            this.ELSE_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ELSE_TextBox.Size = new System.Drawing.Size(998, 657);
            this.ELSE_TextBox.TabIndex = 5;
            // 
            // PatchedPage
            // 
            this.PatchedPage.BackColor = System.Drawing.SystemColors.Control;
            this.PatchedPage.Controls.Add(this.UpdateButton);
            this.PatchedPage.Controls.Add(this.UnInstallButton);
            this.PatchedPage.Controls.Add(this.label3);
            this.PatchedPage.Controls.Add(this.PATCHED_TextBox);
            this.PatchedPage.Location = new System.Drawing.Point(4, 4);
            this.PatchedPage.Margin = new System.Windows.Forms.Padding(4);
            this.PatchedPage.Name = "PatchedPage";
            this.PatchedPage.Padding = new System.Windows.Forms.Padding(4);
            this.PatchedPage.Size = new System.Drawing.Size(1027, 823);
            this.PatchedPage.TabIndex = 9;
            this.PatchedPage.Text = "PATCHED";
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(771, 56);
            this.UpdateButton.Margin = new System.Windows.Forms.Padding(2);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(233, 41);
            this.UpdateButton.TabIndex = 10;
            this.UpdateButton.Text = "アップデート";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // UnInstallButton
            // 
            this.UnInstallButton.Location = new System.Drawing.Point(771, 13);
            this.UnInstallButton.Margin = new System.Windows.Forms.Padding(2);
            this.UnInstallButton.Name = "UnInstallButton";
            this.UnInstallButton.Size = new System.Drawing.Size(233, 41);
            this.UnInstallButton.TabIndex = 9;
            this.UnInstallButton.Text = "アンインストール";
            this.UnInstallButton.UseVisualStyleBackColor = true;
            this.UnInstallButton.Click += new System.EventHandler(this.UnInstallButton_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(7, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(758, 74);
            this.label3.TabIndex = 8;
            this.label3.Text = "すでに適応済みです";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PATCHED_TextBox
            // 
            this.PATCHED_TextBox.ErrorMessage = "";
            this.PATCHED_TextBox.Location = new System.Drawing.Point(7, 103);
            this.PATCHED_TextBox.Margin = new System.Windows.Forms.Padding(2);
            this.PATCHED_TextBox.Multiline = true;
            this.PATCHED_TextBox.Name = "PATCHED_TextBox";
            this.PATCHED_TextBox.Placeholder = "";
            this.PATCHED_TextBox.ReadOnly = true;
            this.PATCHED_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PATCHED_TextBox.Size = new System.Drawing.Size(995, 647);
            this.PATCHED_TextBox.TabIndex = 7;
            // 
            // PatchPage
            // 
            this.PatchPage.AutoScroll = true;
            this.PatchPage.BackColor = System.Drawing.SystemColors.Control;
            this.PatchPage.Location = new System.Drawing.Point(4, 4);
            this.PatchPage.Margin = new System.Windows.Forms.Padding(2);
            this.PatchPage.Name = "PatchPage";
            this.PatchPage.Padding = new System.Windows.Forms.Padding(2);
            this.PatchPage.Size = new System.Drawing.Size(1027, 823);
            this.PatchPage.TabIndex = 8;
            this.PatchPage.Text = "Patch";
            // 
            // ConflictPage
            // 
            this.ConflictPage.BackColor = System.Drawing.SystemColors.Control;
            this.ConflictPage.Controls.Add(this.label4);
            this.ConflictPage.Controls.Add(this.CONFLICT_TextBox);
            this.ConflictPage.Location = new System.Drawing.Point(4, 4);
            this.ConflictPage.Name = "ConflictPage";
            this.ConflictPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConflictPage.Size = new System.Drawing.Size(1027, 823);
            this.ConflictPage.TabIndex = 10;
            this.ConflictPage.Text = "CONFLICT";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(758, 32);
            this.label4.TabIndex = 10;
            this.label4.Text = "競合するパッチがあるため、利用できません";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CONFLICT_TextBox
            // 
            this.CONFLICT_TextBox.ErrorMessage = "";
            this.CONFLICT_TextBox.Location = new System.Drawing.Point(7, 84);
            this.CONFLICT_TextBox.Margin = new System.Windows.Forms.Padding(2);
            this.CONFLICT_TextBox.Multiline = true;
            this.CONFLICT_TextBox.Name = "CONFLICT_TextBox";
            this.CONFLICT_TextBox.Placeholder = "";
            this.CONFLICT_TextBox.ReadOnly = true;
            this.CONFLICT_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CONFLICT_TextBox.Size = new System.Drawing.Size(995, 568);
            this.CONFLICT_TextBox.TabIndex = 9;
            // 
            // PatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1619, 855);
            this.Controls.Add(this.ContentPanel);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PatchForm";
            this.Text = "パッチ";
            this.Load += new System.EventHandler(this.PatchForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ContentPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.TAB.ResumeLayout(false);
            this.ErrorPage.ResumeLayout(false);
            this.ErrorPage.PerformLayout();
            this.ElsePage.ResumeLayout(false);
            this.ElsePage.PerformLayout();
            this.PatchedPage.ResumeLayout(false);
            this.PatchedPage.PerformLayout();
            this.ConflictPage.ResumeLayout(false);
            this.ConflictPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ListBoxEx PatchList;
        private System.Windows.Forms.Panel ContentPanel;
        private System.Windows.Forms.TabControl TAB;
        private System.Windows.Forms.TabPage ErrorPage;
        private FEBuilderGBA.TextBoxEx ERROR_TextBox;
        private System.Windows.Forms.TabPage ElsePage;
        private System.Windows.Forms.Button ReloadButton;
        private System.Windows.Forms.Label ERROR_label;
        private System.Windows.Forms.Label label2;
        private FEBuilderGBA.TextBoxEx ELSE_TextBox;
        private System.Windows.Forms.TabPage PatchPage;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button PatchOpenButton;
        private FEBuilderGBA.TextBoxEx PatchFilename;
        private System.Windows.Forms.TabPage PatchedPage;
        private System.Windows.Forms.Label label3;
        private FEBuilderGBA.TextBoxEx PATCHED_TextBox;
        private System.Windows.Forms.Label FilterExLabel;
        private FEBuilderGBA.TextBoxEx Filter;
        private System.Windows.Forms.Button UnInstallButton;
        private System.Windows.Forms.TabPage ConflictPage;
        private System.Windows.Forms.Label label4;
        private TextBoxEx CONFLICT_TextBox;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
    }
}