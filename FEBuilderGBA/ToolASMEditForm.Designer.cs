namespace FEBuilderGBA
{
    partial class ToolASMEditForm
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
            this.AllWriteButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.NotifyArea = new FEBuilderGBA.RichTextBoxEx();
            this.Code = new FEBuilderGBA.RichTextBoxEx();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AllWriteButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 44);
            this.panel1.TabIndex = 1;
            // 
            // AllWriteButton
            // 
            this.AllWriteButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.AllWriteButton.Location = new System.Drawing.Point(815, 0);
            this.AllWriteButton.Name = "AllWriteButton";
            this.AllWriteButton.Size = new System.Drawing.Size(185, 44);
            this.AllWriteButton.TabIndex = 0;
            this.AllWriteButton.Text = "書き込む";
            this.AllWriteButton.UseVisualStyleBackColor = true;
            this.AllWriteButton.Click += new System.EventHandler(this.AllWriteButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.NotifyArea);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 324);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1000, 148);
            this.panel2.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 148);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // NotifyArea
            // 
            this.NotifyArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotifyArea.ErrorMessage = "";
            this.NotifyArea.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.NotifyArea.Location = new System.Drawing.Point(0, 0);
            this.NotifyArea.Name = "NotifyArea";
            this.NotifyArea.Placeholder = "";
            this.NotifyArea.Size = new System.Drawing.Size(1000, 148);
            this.NotifyArea.TabIndex = 1;
            this.NotifyArea.Text = "";
            // 
            // Code
            // 
            this.Code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Code.ErrorMessage = "";
            this.Code.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Code.Location = new System.Drawing.Point(0, 44);
            this.Code.Name = "Code";
            this.Code.Placeholder = "";
            this.Code.Size = new System.Drawing.Size(1000, 280);
            this.Code.TabIndex = 0;
            this.Code.Text = "";
            this.Code.TextChanged += new System.EventHandler(this.Code_TextChanged);
            // 
            // ToolASMEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1000, 472);
            this.Controls.Add(this.Code);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "ToolASMEditForm";
            this.Text = "ASMコードの直編集";
            this.Load += new System.EventHandler(this.ToolASMEditForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBoxEx Code;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button AllWriteButton;
        private System.Windows.Forms.Panel panel2;
        private RichTextBoxEx NotifyArea;
        private System.Windows.Forms.Splitter splitter1;
    }
}