namespace FEBuilderGBA
{
    partial class MapEditorAddMapChangeDialogForm
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
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.MyNewButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelEx1 = new FEBuilderGBA.LabelEx();
            this.FormIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(12, 407);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(787, 42);
            this.MyCancelButton.TabIndex = 7;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(12, 248);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(787, 42);
            this.EditButton.TabIndex = 6;
            this.EditButton.Text = "マップ変化の設定画面を出します。";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // MyNewButton
            // 
            this.MyNewButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyNewButton.Location = new System.Drawing.Point(12, 94);
            this.MyNewButton.Name = "MyNewButton";
            this.MyNewButton.Size = new System.Drawing.Size(787, 42);
            this.MyNewButton.TabIndex = 5;
            this.MyNewButton.Text = "新規にマップ変化を割り当てます。";
            this.MyNewButton.UseVisualStyleBackColor = true;
            this.MyNewButton.Click += new System.EventHandler(this.MyNewButton_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(786, 95);
            this.label2.TabIndex = 13;
            this.label2.Text = "訪問村や、宝箱、壊れる壁、古木などのために、\r\nマップ変化を新規に割り当てる場合は、ここから新規にマップ変化を割り当ててください。";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(786, 95);
            this.label1.TabIndex = 14;
            this.label1.Text = "マップ変化を減らす場合は、設定画面から消去してください。\r\n変更したマップ変化は、マップを切り替えたときに読み込まれます。";
            // 
            // labelEx1
            // 
            this.labelEx1.ErrorMessage = "";
            this.labelEx1.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEx1.Location = new System.Drawing.Point(82, 9);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(717, 58);
            this.labelEx1.TabIndex = 4;
            this.labelEx1.Text = "マップ変化をさらに追加で作成しますか？";
            // 
            // FormIcon
            // 
            this.FormIcon.Location = new System.Drawing.Point(12, 8);
            this.FormIcon.Name = "FormIcon";
            this.FormIcon.Size = new System.Drawing.Size(64, 64);
            this.FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FormIcon.TabIndex = 15;
            this.FormIcon.TabStop = false;
            // 
            // MapEditorAddMapChangeDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(815, 477);
            this.Controls.Add(this.FormIcon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.MyNewButton);
            this.Controls.Add(this.labelEx1);
            this.Name = "MapEditorAddMapChangeDialogForm";
            this.Text = "マップ変化をさらに追加で作成しますか？";
            this.Load += new System.EventHandler(this.MapEditorAddMapChangeDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button MyNewButton;
        private LabelEx labelEx1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox FormIcon;
    }
}