namespace FEBuilderGBA
{
    partial class DisASMDumpAllForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.AllMakeMAPFileButton = new System.Windows.Forms.Button();
            this.AllDisASMButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AllMakeMAPFileNodollSymButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ArgGrepButton1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(418, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "このゲームのプログラムデータをすべてファイルに出力します。\r\n処理には時間がかかるものがあります。";
            // 
            // AllMakeMAPFileButton
            // 
            this.AllMakeMAPFileButton.Location = new System.Drawing.Point(16, 213);
            this.AllMakeMAPFileButton.Name = "AllMakeMAPFileButton";
            this.AllMakeMAPFileButton.Size = new System.Drawing.Size(651, 55);
            this.AllMakeMAPFileButton.TabIndex = 63;
            this.AllMakeMAPFileButton.Text = "MAKE IDAMapFile";
            this.AllMakeMAPFileButton.UseVisualStyleBackColor = true;
            this.AllMakeMAPFileButton.Click += new System.EventHandler(this.AllMakeMAPFileButton_Click);
            // 
            // AllDisASMButton
            // 
            this.AllDisASMButton.Location = new System.Drawing.Point(12, 95);
            this.AllDisASMButton.Name = "AllDisASMButton";
            this.AllDisASMButton.Size = new System.Drawing.Size(655, 51);
            this.AllDisASMButton.TabIndex = 62;
            this.AllDisASMButton.Text = "全部逆アセンブルして保存する";
            this.AllDisASMButton.UseVisualStyleBackColor = true;
            this.AllDisASMButton.Click += new System.EventHandler(this.AllDisASMButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(392, 18);
            this.label2.TabIndex = 64;
            this.label2.Text = "すべてのコードを逆アセンブルしてファイルに保存します。";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 281);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(380, 18);
            this.label3.TabIndex = 65;
            this.label3.Text = "IDAにimportできる形式でMAPファイルを生成します。";
            // 
            // AllMakeMAPFileNodollSymButton
            // 
            this.AllMakeMAPFileNodollSymButton.Location = new System.Drawing.Point(16, 335);
            this.AllMakeMAPFileNodollSymButton.Name = "AllMakeMAPFileNodollSymButton";
            this.AllMakeMAPFileNodollSymButton.Size = new System.Drawing.Size(651, 55);
            this.AllMakeMAPFileNodollSymButton.TabIndex = 66;
            this.AllMakeMAPFileNodollSymButton.Text = "MAKE no$gba sym File";
            this.AllMakeMAPFileNodollSymButton.UseVisualStyleBackColor = true;
            this.AllMakeMAPFileNodollSymButton.Click += new System.EventHandler(this.AllMakeMAPFileNodollSymButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 405);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(498, 36);
            this.label4.TabIndex = 67;
            this.label4.Text = "no$gba debuggerで利用できるsym形式のMAPファイルを作成します。\r\nROMと同じディレクトリに設置してください。";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 537);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(279, 18);
            this.label5.TabIndex = 69;
            this.label5.Text = "特定の関数の引数だけを抽出します。";
            // 
            // ArgGrepButton1
            // 
            this.ArgGrepButton1.Location = new System.Drawing.Point(16, 467);
            this.ArgGrepButton1.Name = "ArgGrepButton1";
            this.ArgGrepButton1.Size = new System.Drawing.Size(651, 55);
            this.ArgGrepButton1.TabIndex = 68;
            this.ArgGrepButton1.Text = "Arg Grep";
            this.ArgGrepButton1.UseVisualStyleBackColor = true;
            this.ArgGrepButton1.Click += new System.EventHandler(this.ArgGrepButton1_Click);
            // 
            // DisASMDumpAllForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(679, 683);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ArgGrepButton1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AllMakeMAPFileNodollSymButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AllMakeMAPFileButton);
            this.Controls.Add(this.AllDisASMButton);
            this.Controls.Add(this.label1);
            this.Name = "DisASMDumpAllForm";
            this.Text = "全部ファイルに保存";
            this.Load += new System.EventHandler(this.DisASMDumpAllForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AllMakeMAPFileButton;
        private System.Windows.Forms.Button AllDisASMButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button AllMakeMAPFileNodollSymButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ArgGrepButton1;
    }
}