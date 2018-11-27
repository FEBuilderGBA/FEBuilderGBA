namespace FEBuilderGBA
{
    partial class EventScriptTemplateForm
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
            this.SelectButton = new System.Windows.Forms.Button();
            this.SelectFilename = new FEBuilderGBA.LabelEx();
            this.labelEx2 = new FEBuilderGBA.LabelEx();
            this.labelEx1 = new FEBuilderGBA.LabelEx();
            this.SampleEventListbox = new FEBuilderGBA.ListBoxEx();
            this.TemplateListbox = new FEBuilderGBA.ListBoxEx();
            this.SuspendLayout();
            // 
            // SelectButton
            // 
            this.SelectButton.Location = new System.Drawing.Point(718, 1);
            this.SelectButton.Margin = new System.Windows.Forms.Padding(2);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(330, 31);
            this.SelectButton.TabIndex = 3;
            this.SelectButton.Text = "このテンプレートを選択する";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // SelectFilename
            // 
            this.SelectFilename.AutoSize = true;
            this.SelectFilename.ErrorMessage = "";
            this.SelectFilename.Location = new System.Drawing.Point(249, 11);
            this.SelectFilename.Name = "SelectFilename";
            this.SelectFilename.Size = new System.Drawing.Size(123, 18);
            this.SelectFilename.TabIndex = 6;
            this.SelectFilename.Text = "SelectFilename";
            this.SelectFilename.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SelectFilename_MouseDoubleClick);
            // 
            // labelEx2
            // 
            this.labelEx2.AutoSize = true;
            this.labelEx2.ErrorMessage = "";
            this.labelEx2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEx2.Location = new System.Drawing.Point(13, 11);
            this.labelEx2.Name = "labelEx2";
            this.labelEx2.Size = new System.Drawing.Size(155, 18);
            this.labelEx2.TabIndex = 5;
            this.labelEx2.Text = "イベントテンプレート";
            // 
            // labelEx1
            // 
            this.labelEx1.AutoSize = true;
            this.labelEx1.ErrorMessage = "";
            this.labelEx1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEx1.Location = new System.Drawing.Point(13, 522);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(283, 18);
            this.labelEx1.TabIndex = 4;
            this.labelEx1.Text = "このテンプレートで追加されるイベント";
            // 
            // SampleEventListbox
            // 
            this.SampleEventListbox.FormattingEnabled = true;
            this.SampleEventListbox.IntegralHeight = false;
            this.SampleEventListbox.ItemHeight = 18;
            this.SampleEventListbox.Location = new System.Drawing.Point(12, 542);
            this.SampleEventListbox.Name = "SampleEventListbox";
            this.SampleEventListbox.Size = new System.Drawing.Size(1036, 239);
            this.SampleEventListbox.TabIndex = 1;
            // 
            // TemplateListbox
            // 
            this.TemplateListbox.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TemplateListbox.FormattingEnabled = true;
            this.TemplateListbox.IntegralHeight = false;
            this.TemplateListbox.ItemHeight = 20;
            this.TemplateListbox.Location = new System.Drawing.Point(12, 33);
            this.TemplateListbox.Name = "TemplateListbox";
            this.TemplateListbox.Size = new System.Drawing.Size(1036, 481);
            this.TemplateListbox.TabIndex = 0;
            this.TemplateListbox.SelectedIndexChanged += new System.EventHandler(this.TemplateList_SelectedIndexChanged);
            this.TemplateListbox.DoubleClick += new System.EventHandler(this.SelectButton_Click);
            this.TemplateListbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TemplateListbox_KeyDown);
            // 
            // EventScriptTemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1060, 790);
            this.Controls.Add(this.SelectFilename);
            this.Controls.Add(this.labelEx2);
            this.Controls.Add(this.labelEx1);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.SampleEventListbox);
            this.Controls.Add(this.TemplateListbox);
            this.Name = "EventScriptTemplateForm";
            this.Text = "テンプレートを選択してください";
            this.Load += new System.EventHandler(this.EventScriptTemplateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBoxEx TemplateListbox;
        private ListBoxEx SampleEventListbox;
        private System.Windows.Forms.Button SelectButton;
        private LabelEx labelEx1;
        private LabelEx labelEx2;
        private LabelEx SelectFilename;

    }
}