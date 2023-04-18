namespace FEBuilderGBA
{
    partial class ImageBGSelectPopupForm
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
            this.VanillaTSAButton = new System.Windows.Forms.Button();
            this.BG244Button = new System.Windows.Forms.Button();
            this.BG255Button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // VanillaTSAButton
            // 
            this.VanillaTSAButton.Location = new System.Drawing.Point(23, 78);
            this.VanillaTSAButton.Name = "VanillaTSAButton";
            this.VanillaTSAButton.Size = new System.Drawing.Size(868, 62);
            this.VanillaTSAButton.TabIndex = 0;
            this.VanillaTSAButton.Text = "バニラ形式。TSA方式を利用する方法でインポートする";
            this.VanillaTSAButton.UseVisualStyleBackColor = true;
            this.VanillaTSAButton.Click += new System.EventHandler(this.VanillaTSAButton_Click);
            // 
            // BG244Button
            // 
            this.BG244Button.Location = new System.Drawing.Point(23, 224);
            this.BG244Button.Name = "BG244Button";
            this.BG244Button.Size = new System.Drawing.Size(868, 62);
            this.BG244Button.TabIndex = 1;
            this.BG244Button.Text = "TSAを利用しないBG224色(会話用)";
            this.BG244Button.UseVisualStyleBackColor = true;
            this.BG244Button.Click += new System.EventHandler(this.BG244Button_Click);
            // 
            // BG255Button
            // 
            this.BG255Button.Location = new System.Drawing.Point(23, 387);
            this.BG255Button.Name = "BG255Button";
            this.BG255Button.Size = new System.Drawing.Size(868, 62);
            this.BG255Button.TabIndex = 3;
            this.BG255Button.Text = "TSAを利用しないBG256色(カットシーン)";
            this.BG255Button.UseVisualStyleBackColor = true;
            this.BG255Button.Click += new System.EventHandler(this.BG255Button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(19, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(425, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "インポートするBGの形式を指定してください。";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(45, 289);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(766, 93);
            this.label3.TabIndex = 5;
            this.label3.Text = "このモードでインポートする画像は、224色のみ利用しますが、顔画像を表示する会話シーンに利用できます。\r\n減色ツールの「0A=TSAを利用しないBG224色(会話" +
    "シーン用)」で減色した画像を指定してください。";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(45, 452);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(766, 93);
            this.label2.TabIndex = 4;
            this.label2.Text = "このモードでインポートする画像は、256色すべてを利用できますが、顔画像を出さないカットシーンである必要があります。\r\n減色ツールの「09=TSAを利用しないBG" +
    "256色(カットシーン)」で減色した画像を指定してください。";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(45, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(766, 76);
            this.label1.TabIndex = 3;
            this.label1.Text = "バニラの仕様であるTSAを利用した最大8パレットを指定します。\r\n減色ツールの「01=背景(BG,CG)」で減色した画像を指定してください。";
            // 
            // ImageBGSelectPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 556);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BG255Button);
            this.Controls.Add(this.BG244Button);
            this.Controls.Add(this.VanillaTSAButton);
            this.Name = "ImageBGSelectPopupForm";
            this.Text = "どの形式でインポートしますか?";
            this.Load += new System.EventHandler(this.ImageBGSelectPopupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button VanillaTSAButton;
        private System.Windows.Forms.Button BG244Button;
        private System.Windows.Forms.Button BG255Button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}