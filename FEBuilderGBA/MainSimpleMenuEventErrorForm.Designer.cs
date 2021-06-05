namespace FEBuilderGBA
{
    partial class MainSimpleMenuEventErrorForm
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
            this.EventPanel = new System.Windows.Forms.Panel();
            this.Explain = new System.Windows.Forms.Label();
            this.ShowAllError = new System.Windows.Forms.CheckBox();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.EventCond_Label = new System.Windows.Forms.Label();
            this.EventList = new FEBuilderGBA.ListBoxEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.EventPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EventPanel
            // 
            this.EventPanel.Controls.Add(this.EventList);
            this.EventPanel.Controls.Add(this.panel1);
            this.EventPanel.Controls.Add(this.Explain);
            this.EventPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventPanel.Location = new System.Drawing.Point(0, 0);
            this.EventPanel.Margin = new System.Windows.Forms.Padding(4);
            this.EventPanel.Name = "EventPanel";
            this.EventPanel.Size = new System.Drawing.Size(1183, 838);
            this.EventPanel.TabIndex = 134;
            // 
            // Explain
            // 
            this.Explain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Explain.Cursor = System.Windows.Forms.Cursors.Default;
            this.Explain.Location = new System.Drawing.Point(4, 784);
            this.Explain.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Explain.Name = "Explain";
            this.Explain.Size = new System.Drawing.Size(1161, 30);
            this.Explain.TabIndex = 118;
            this.Explain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ShowAllError
            // 
            this.ShowAllError.AutoSize = true;
            this.ShowAllError.Location = new System.Drawing.Point(507, 5);
            this.ShowAllError.Name = "ShowAllError";
            this.ShowAllError.Size = new System.Drawing.Size(194, 22);
            this.ShowAllError.TabIndex = 117;
            this.ShowAllError.Text = "非表示のエラーも表示";
            this.ShowAllError.UseVisualStyleBackColor = true;
            this.ShowAllError.CheckedChanged += new System.EventHandler(this.ShowAllError_CheckedChanged);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Location = new System.Drawing.Point(800, -1);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(379, 31);
            this.ReloadButton.TabIndex = 2;
            this.ReloadButton.Text = "再取得(Ctrl+R)";
            this.ReloadButton.UseVisualStyleBackColor = true;
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // EventCond_Label
            // 
            this.EventCond_Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EventCond_Label.Cursor = System.Windows.Forms.Cursors.Default;
            this.EventCond_Label.Location = new System.Drawing.Point(0, 0);
            this.EventCond_Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.EventCond_Label.Name = "EventCond_Label";
            this.EventCond_Label.Size = new System.Drawing.Size(502, 30);
            this.EventCond_Label.TabIndex = 0;
            this.EventCond_Label.Text = "エラー";
            this.EventCond_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EventList
            // 
            this.EventList.Cursor = System.Windows.Forms.Cursors.Default;
            this.EventList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.EventList.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.EventList.FormattingEnabled = true;
            this.EventList.IntegralHeight = false;
            this.EventList.ItemHeight = 50;
            this.EventList.Location = new System.Drawing.Point(0, 34);
            this.EventList.Margin = new System.Windows.Forms.Padding(4);
            this.EventList.Name = "EventList";
            this.EventList.Size = new System.Drawing.Size(1183, 804);
            this.EventList.TabIndex = 1;
            this.EventList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EventList_KeyDown);
            this.EventList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.EventList_MouseDoubleClick);
            this.EventList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EventList_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.EventCond_Label);
            this.panel1.Controls.Add(this.ReloadButton);
            this.panel1.Controls.Add(this.ShowAllError);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 34);
            this.panel1.TabIndex = 119;
            // 
            // MainSimpleMenuEventErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1183, 838);
            this.Controls.Add(this.EventPanel);
            this.Name = "MainSimpleMenuEventErrorForm";
            this.Text = "エラー";
            this.Load += new System.EventHandler(this.ErrorEventErrorForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainSimpleMenuEventErrorForm_KeyDown);
            this.EventPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel EventPanel;
        private System.Windows.Forms.Label EventCond_Label;
        private ListBoxEx EventList;
        private System.Windows.Forms.Button ReloadButton;
        private System.Windows.Forms.CheckBox ShowAllError;
        private System.Windows.Forms.Label Explain;
        private System.Windows.Forms.Panel panel1;
    }
}