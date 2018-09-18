namespace FEBuilderGBA
{
    partial class DevTranslateForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.toLang = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TranslateButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DesignStringReverseButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.toLang2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DesignStringConvertButton = new System.Windows.Forms.Button();
            this.TranslateCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TranslateCheckBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.toLang);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TranslateButton);
            this.panel1.Location = new System.Drawing.Point(16, 16);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(722, 144);
            this.panel1.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 21;
            this.label2.Text = "言語";
            // 
            // toLang
            // 
            this.toLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toLang.FormattingEnabled = true;
            this.toLang.Location = new System.Drawing.Point(148, 12);
            this.toLang.Name = "toLang";
            this.toLang.Size = new System.Drawing.Size(304, 26);
            this.toLang.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 105);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 19;
            this.label1.Text = "翻訳";
            // 
            // TranslateButton
            // 
            this.TranslateButton.Location = new System.Drawing.Point(149, 100);
            this.TranslateButton.Margin = new System.Windows.Forms.Padding(4);
            this.TranslateButton.Name = "TranslateButton";
            this.TranslateButton.Size = new System.Drawing.Size(216, 28);
            this.TranslateButton.TabIndex = 18;
            this.TranslateButton.Text = "翻訳";
            this.TranslateButton.UseVisualStyleBackColor = true;
            this.TranslateButton.Click += new System.EventHandler(this.TranslateButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.DesignStringReverseButton);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.toLang2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.DesignStringConvertButton);
            this.panel2.Location = new System.Drawing.Point(17, 186);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(722, 141);
            this.panel2.TabIndex = 22;
            // 
            // DesignStringReverseButton
            // 
            this.DesignStringReverseButton.Location = new System.Drawing.Point(147, 99);
            this.DesignStringReverseButton.Margin = new System.Windows.Forms.Padding(4);
            this.DesignStringReverseButton.Name = "DesignStringReverseButton";
            this.DesignStringReverseButton.Size = new System.Drawing.Size(303, 28);
            this.DesignStringReverseButton.TabIndex = 22;
            this.DesignStringReverseButton.Text = "Reverse";
            this.DesignStringReverseButton.UseVisualStyleBackColor = true;
            this.DesignStringReverseButton.Click += new System.EventHandler(this.DesignStringReverseButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 15);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 18);
            this.label3.TabIndex = 21;
            this.label3.Text = "言語";
            // 
            // toLang2
            // 
            this.toLang2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toLang2.FormattingEnabled = true;
            this.toLang2.Location = new System.Drawing.Point(148, 12);
            this.toLang2.Name = "toLang2";
            this.toLang2.Size = new System.Drawing.Size(304, 26);
            this.toLang2.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 53);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 18);
            this.label4.TabIndex = 19;
            this.label4.Text = "form design";
            // 
            // DesignStringConvertButton
            // 
            this.DesignStringConvertButton.Location = new System.Drawing.Point(148, 48);
            this.DesignStringConvertButton.Margin = new System.Windows.Forms.Padding(4);
            this.DesignStringConvertButton.Name = "DesignStringConvertButton";
            this.DesignStringConvertButton.Size = new System.Drawing.Size(304, 28);
            this.DesignStringConvertButton.TabIndex = 18;
            this.DesignStringConvertButton.Text = "Convert";
            this.DesignStringConvertButton.UseVisualStyleBackColor = true;
            this.DesignStringConvertButton.Click += new System.EventHandler(this.DesignStringConvertButton_Click);
            // 
            // TranslateCheckBox
            // 
            this.TranslateCheckBox.AutoSize = true;
            this.TranslateCheckBox.Location = new System.Drawing.Point(149, 54);
            this.TranslateCheckBox.Name = "TranslateCheckBox";
            this.TranslateCheckBox.Size = new System.Drawing.Size(362, 32);
            this.TranslateCheckBox.TabIndex = 22;
            this.TranslateCheckBox.Text = "日本語ではなく、可能な限り英語から翻訳する";
            this.TranslateCheckBox.UseVisualStyleBackColor = true;
            // 
            // DevTranslateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(752, 342);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DevTranslateForm";
            this.Text = "DevTranslateForm";
            this.Load += new System.EventHandler(this.DevTranslateForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button TranslateButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox toLang;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox toLang2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button DesignStringConvertButton;
        private System.Windows.Forms.Button DesignStringReverseButton;
        private System.Windows.Forms.CheckBox TranslateCheckBox;
    }
}