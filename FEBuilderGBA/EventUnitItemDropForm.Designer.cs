namespace FEBuilderGBA
{
    partial class EventUnitItemDropForm
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
            this.YesButton = new System.Windows.Forms.Button();
            this.NoButton = new System.Windows.Forms.Button();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FormIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // YesButton
            // 
            this.YesButton.Location = new System.Drawing.Point(12, 114);
            this.YesButton.Name = "YesButton";
            this.YesButton.Size = new System.Drawing.Size(238, 40);
            this.YesButton.TabIndex = 1;
            this.YesButton.Text = "アイテムドロップする";
            this.YesButton.UseVisualStyleBackColor = true;
            this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
            // 
            // NoButton
            // 
            this.NoButton.Location = new System.Drawing.Point(282, 114);
            this.NoButton.Name = "NoButton";
            this.NoButton.Size = new System.Drawing.Size(238, 40);
            this.NoButton.TabIndex = 2;
            this.NoButton.Text = "アイテムドロップしない";
            this.NoButton.UseVisualStyleBackColor = true;
            this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(554, 114);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(238, 40);
            this.MyCancelButton.TabIndex = 3;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(537, 36);
            this.label1.TabIndex = 4;
            this.label1.Text = "この敵を倒すと、アイテムドロップするようにしますか？\r\nアイテムドロップする場合、一番最後に持っているアイテムが対象になります。";
            // 
            // FormIcon
            // 
            this.FormIcon.Location = new System.Drawing.Point(12, 12);
            this.FormIcon.Name = "FormIcon";
            this.FormIcon.Size = new System.Drawing.Size(85, 71);
            this.FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FormIcon.TabIndex = 6;
            this.FormIcon.TabStop = false;
            // 
            // EventUnitItemDropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(807, 179);
            this.Controls.Add(this.FormIcon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.NoButton);
            this.Controls.Add(this.YesButton);
            this.Name = "EventUnitItemDropForm";
            this.Text = "アイテムドロップ";
            this.Load += new System.EventHandler(this.EventUnitItemDropForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button YesButton;
        private System.Windows.Forms.Button NoButton;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox FormIcon;
    }
}