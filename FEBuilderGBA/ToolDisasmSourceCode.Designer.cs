namespace FEBuilderGBA
{
    partial class ToolDisasmSourceCode
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MakeButton = new System.Windows.Forms.Button();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.SrcCodeFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(8, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 31);
            this.label2.TabIndex = 91;
            this.label2.Text = "無改造ROM";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(208, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(939, 86);
            this.label1.TabIndex = 92;
            this.label1.Text = "ソースコードを生成するためのバニラROMを選択してください。\r\nもし、現在のROMの逆アセンブラを表示したい場合は、メニューのツール内にある逆アセンブル機能を利用" +
    "してください。\r\nソースコードの生成には、数分かかります。";
            // 
            // MakeButton
            // 
            this.MakeButton.Location = new System.Drawing.Point(208, 266);
            this.MakeButton.Name = "MakeButton";
            this.MakeButton.Size = new System.Drawing.Size(939, 32);
            this.MakeButton.TabIndex = 0;
            this.MakeButton.Text = "逆アセンブルソースコードの作成";
            this.MakeButton.UseVisualStyleBackColor = true;
            this.MakeButton.Click += new System.EventHandler(this.MakeButton_Click);
            // 
            // OrignalSelectButton
            // 
            this.OrignalSelectButton.Location = new System.Drawing.Point(208, 13);
            this.OrignalSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalSelectButton.Name = "OrignalSelectButton";
            this.OrignalSelectButton.Size = new System.Drawing.Size(130, 31);
            this.OrignalSelectButton.TabIndex = 1;
            this.OrignalSelectButton.Text = "ファイル選択";
            this.OrignalSelectButton.UseVisualStyleBackColor = true;
            this.OrignalSelectButton.Click += new System.EventHandler(this.OrignalSelectButton_Click);
            // 
            // SrcCodeFilename
            // 
            this.SrcCodeFilename.ErrorMessage = "";
            this.SrcCodeFilename.Location = new System.Drawing.Point(208, 176);
            this.SrcCodeFilename.Margin = new System.Windows.Forms.Padding(4);
            this.SrcCodeFilename.Name = "SrcCodeFilename";
            this.SrcCodeFilename.Placeholder = "";
            this.SrcCodeFilename.ReadOnly = true;
            this.SrcCodeFilename.Size = new System.Drawing.Size(926, 25);
            this.SrcCodeFilename.TabIndex = 94;
            // 
            // OrignalFilename
            // 
            this.OrignalFilename.ErrorMessage = "";
            this.OrignalFilename.Location = new System.Drawing.Point(346, 16);
            this.OrignalFilename.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalFilename.Name = "OrignalFilename";
            this.OrignalFilename.Placeholder = "";
            this.OrignalFilename.Size = new System.Drawing.Size(801, 25);
            this.OrignalFilename.TabIndex = 3;
            this.OrignalFilename.DoubleClick += new System.EventHandler(this.OrignalFilename_DoubleClick);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(8, 172);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 31);
            this.label3.TabIndex = 95;
            this.label3.Text = "保存先";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToolDisasmSourceCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 314);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SrcCodeFilename);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MakeButton);
            this.Controls.Add(this.OrignalSelectButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OrignalFilename);
            this.Name = "ToolDisasmSourceCode";
            this.Text = "ソースコードがないのでバニラROMから生成します";
            this.Load += new System.EventHandler(this.ToolDisasmSourceCode_Load);
            this.Shown += new System.EventHandler(this.ToolDisasmSourceCode_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MakeButton;
        private TextBoxEx OrignalFilename;
        private System.Windows.Forms.Button OrignalSelectButton;
        private TextBoxEx SrcCodeFilename;
        private System.Windows.Forms.Label label3;
    }
}