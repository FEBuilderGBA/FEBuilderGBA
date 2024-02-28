namespace FEBuilderGBA
{
    partial class PackedMemorySlotForm
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
            this.panel8 = new System.Windows.Forms.Panel();
            this.CComboBox = new System.Windows.Forms.ComboBox();
            this.OPLabelEx = new FEBuilderGBA.LabelEx();
            this.BComboBox = new System.Windows.Forms.ComboBox();
            this.AComboBox = new System.Windows.Forms.ComboBox();
            this.labelEx1 = new FEBuilderGBA.LabelEx();
            this.MESSAGE = new System.Windows.Forms.Label();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.CComboBox);
            this.panel8.Controls.Add(this.OPLabelEx);
            this.panel8.Controls.Add(this.BComboBox);
            this.panel8.Controls.Add(this.AComboBox);
            this.panel8.Controls.Add(this.labelEx1);
            this.panel8.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel8.Location = new System.Drawing.Point(10, 54);
            this.panel8.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(674, 82);
            this.panel8.TabIndex = 14;
            // 
            // CComboBox
            // 
            this.CComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CComboBox.FormattingEnabled = true;
            this.CComboBox.Location = new System.Drawing.Point(488, 27);
            this.CComboBox.Name = "CComboBox";
            this.CComboBox.Size = new System.Drawing.Size(181, 26);
            this.CComboBox.TabIndex = 2;
            // 
            // OPLabelEx
            // 
            this.OPLabelEx.AutoSize = true;
            this.OPLabelEx.ErrorMessage = "";
            this.OPLabelEx.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.OPLabelEx.Location = new System.Drawing.Point(420, 25);
            this.OPLabelEx.Name = "OPLabelEx";
            this.OPLabelEx.Size = new System.Drawing.Size(47, 28);
            this.OPLabelEx.TabIndex = 5;
            this.OPLabelEx.Text = " + ";
            // 
            // BComboBox
            // 
            this.BComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BComboBox.FormattingEnabled = true;
            this.BComboBox.Location = new System.Drawing.Point(230, 27);
            this.BComboBox.Name = "BComboBox";
            this.BComboBox.Size = new System.Drawing.Size(181, 26);
            this.BComboBox.TabIndex = 1;
            // 
            // AComboBox
            // 
            this.AComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AComboBox.FormattingEnabled = true;
            this.AComboBox.Location = new System.Drawing.Point(3, 27);
            this.AComboBox.Name = "AComboBox";
            this.AComboBox.Size = new System.Drawing.Size(181, 26);
            this.AComboBox.TabIndex = 0;
            // 
            // labelEx1
            // 
            this.labelEx1.AutoSize = true;
            this.labelEx1.ErrorMessage = "";
            this.labelEx1.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEx1.Location = new System.Drawing.Point(192, 27);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(27, 28);
            this.labelEx1.TabIndex = 2;
            this.labelEx1.Text = "=";
            // 
            // MESSAGE
            // 
            this.MESSAGE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MESSAGE.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.MESSAGE.Location = new System.Drawing.Point(10, 9);
            this.MESSAGE.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MESSAGE.Name = "MESSAGE";
            this.MESSAGE.Size = new System.Drawing.Size(676, 40);
            this.MESSAGE.TabIndex = 107;
            this.MESSAGE.Text = "MESSAGE";
            this.MESSAGE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ApplyButton.Location = new System.Drawing.Point(499, 9);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(181, 40);
            this.ApplyButton.TabIndex = 0;
            this.ApplyButton.Text = "適応";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // PackedMemorySlotForm
            // 
            this.AcceptButton = this.ApplyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(692, 148);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.MESSAGE);
            this.Controls.Add(this.panel8);
            this.Name = "PackedMemorySlotForm";
            this.Text = "PackedMemorySlot";
            this.Load += new System.EventHandler(this.PackedMemorySlotForm_Load);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label MESSAGE;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.ComboBox CComboBox;
        private LabelEx OPLabelEx;
        private System.Windows.Forms.ComboBox BComboBox;
        private System.Windows.Forms.ComboBox AComboBox;
        private LabelEx labelEx1;
    }
}