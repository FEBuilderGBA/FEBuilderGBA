namespace FEBuilderGBA
{
    partial class MapStyleEditorFormWarningVanillaTileOverraideForm
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
            this.HelpTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OverraideButton = new System.Windows.Forms.Button();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.SilentCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(472, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "バニラのタイルセットを上書きしようとしています。";
            // 
            // HelpTextBox
            // 
            this.HelpTextBox.Location = new System.Drawing.Point(12, 180);
            this.HelpTextBox.Multiline = true;
            this.HelpTextBox.Name = "HelpTextBox";
            this.HelpTextBox.ReadOnly = true;
            this.HelpTextBox.Size = new System.Drawing.Size(823, 27);
            this.HelpTextBox.TabIndex = 1;
            this.HelpTextBox.DoubleClick += new System.EventHandler(this.HelpTextBox_DoubleClick);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(823, 100);
            this.label2.TabIndex = 2;
            this.label2.Text = "このタイルセットを上書きすると、このタイルセットを利用しているすべてのマップに影響を与えます。\r\n本当に上書きをしてもよろしいですか？\r\n\r\nバニラのタイルセット" +
    "を上書きせずに、新しいタイルセットを追加する場合は、こちらのヘルプをご覧ください。";
            // 
            // OverraideButton
            // 
            this.OverraideButton.Location = new System.Drawing.Point(12, 264);
            this.OverraideButton.Name = "OverraideButton";
            this.OverraideButton.Size = new System.Drawing.Size(452, 38);
            this.OverraideButton.TabIndex = 3;
            this.OverraideButton.Text = "上書きする";
            this.OverraideButton.UseVisualStyleBackColor = true;
            this.OverraideButton.Click += new System.EventHandler(this.OverraideButton_Click);
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(604, 264);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(231, 38);
            this.MyCancelButton.TabIndex = 4;
            this.MyCancelButton.Text = "キャンセル";
            this.MyCancelButton.UseVisualStyleBackColor = true;
            this.MyCancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SilentCheckBox
            // 
            this.SilentCheckBox.AutoSize = true;
            this.SilentCheckBox.Location = new System.Drawing.Point(12, 237);
            this.SilentCheckBox.Name = "SilentCheckBox";
            this.SilentCheckBox.Size = new System.Drawing.Size(290, 22);
            this.SilentCheckBox.TabIndex = 5;
            this.SilentCheckBox.Text = "次回起動時までこの警告を出さない";
            this.SilentCheckBox.UseVisualStyleBackColor = true;
            // 
            // MapStyleEditorFormWarningVanillaTileOverraideForm
            // 
            this.AcceptButton = this.OverraideButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(864, 318);
            this.Controls.Add(this.SilentCheckBox);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.OverraideButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HelpTextBox);
            this.Controls.Add(this.label1);
            this.Name = "MapStyleEditorFormWarningVanillaTileOverraideForm";
            this.Text = "警告";
            this.Load += new System.EventHandler(this.MapStyleEditorFormWarningVanillaTileOverraideForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox HelpTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OverraideButton;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.CheckBox SilentCheckBox;
    }
}