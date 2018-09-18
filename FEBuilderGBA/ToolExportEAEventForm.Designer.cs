namespace FEBuilderGBA
{
    partial class ToolExportEAEventForm
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
            this.panel14 = new System.Windows.Forms.Panel();
            this.label62 = new System.Windows.Forms.Label();
            this.MAP_LISTBOX = new FEBuilderGBA.ListBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.J_40 = new FEBuilderGBA.CustomColorGroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ExportMainTableButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.addEndGuardsCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportWMAPEvent2EAButton = new System.Windows.Forms.Button();
            this.ExportWMAPEventEAButton = new System.Windows.Forms.Button();
            this.ExportEAButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.customColorGroupBox1 = new FEBuilderGBA.CustomColorGroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ExportUndoDataButton = new System.Windows.Forms.Button();
            this.panel14.SuspendLayout();
            this.panel2.SuspendLayout();
            this.J_40.SuspendLayout();
            this.customColorGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel14
            // 
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.label62);
            this.panel14.Controls.Add(this.MAP_LISTBOX);
            this.panel14.Location = new System.Drawing.Point(11, 11);
            this.panel14.Margin = new System.Windows.Forms.Padding(2);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(284, 724);
            this.panel14.TabIndex = 189;
            // 
            // label62
            // 
            this.label62.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label62.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label62.Location = new System.Drawing.Point(0, -1);
            this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(283, 31);
            this.label62.TabIndex = 106;
            this.label62.Text = "マップ名";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MAP_LISTBOX
            // 
            this.MAP_LISTBOX.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MAP_LISTBOX.FormattingEnabled = true;
            this.MAP_LISTBOX.ItemHeight = 18;
            this.MAP_LISTBOX.Location = new System.Drawing.Point(0, 29);
            this.MAP_LISTBOX.Margin = new System.Windows.Forms.Padding(4);
            this.MAP_LISTBOX.Name = "MAP_LISTBOX";
            this.MAP_LISTBOX.Size = new System.Drawing.Size(283, 688);
            this.MAP_LISTBOX.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.customColorGroupBox1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.J_40);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.addEndGuardsCheckBox);
            this.panel2.Controls.Add(this.ExportWMAPEvent2EAButton);
            this.panel2.Controls.Add(this.ExportWMAPEventEAButton);
            this.panel2.Controls.Add(this.ExportEAButton);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(302, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(765, 724);
            this.panel2.TabIndex = 190;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 259);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(690, 64);
            this.label3.TabIndex = 7;
            this.label3.Text = "最新版のEAでは拡張領域のデータがダンプできないことがあるので、その場合は、古いEA ver9を利用してください。";
            // 
            // J_40
            // 
            this.J_40.BorderColor = System.Drawing.Color.Empty;
            this.J_40.Controls.Add(this.label4);
            this.J_40.Controls.Add(this.ExportMainTableButton);
            this.J_40.Location = new System.Drawing.Point(7, 346);
            this.J_40.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.J_40.Name = "J_40";
            this.J_40.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.J_40.Size = new System.Drawing.Size(741, 122);
            this.J_40.TabIndex = 6;
            this.J_40.TabStop = false;
            this.J_40.Text = "データ";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(689, 52);
            this.label4.TabIndex = 6;
            this.label4.Text = "ユニットやクラス、アイテムなどのテーブルをダンプします。";
            // 
            // ExportMainTableButton
            // 
            this.ExportMainTableButton.Location = new System.Drawing.Point(13, 20);
            this.ExportMainTableButton.Name = "ExportMainTableButton";
            this.ExportMainTableButton.Size = new System.Drawing.Size(686, 35);
            this.ExportMainTableButton.TabIndex = 2;
            this.ExportMainTableButton.Text = "主要テーブルのエクスポート";
            this.ExportMainTableButton.UseVisualStyleBackColor = true;
            this.ExportMainTableButton.Click += new System.EventHandler(this.ExportMainTableButton_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 662);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(689, 61);
            this.label2.TabIndex = 5;
            this.label2.Text = "インポートは、メニューの「実行」->「Event Assemblerで追加」で追加から実行してください。";
            // 
            // addEndGuardsCheckBox
            // 
            this.addEndGuardsCheckBox.AutoSize = true;
            this.addEndGuardsCheckBox.Checked = true;
            this.addEndGuardsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addEndGuardsCheckBox.Location = new System.Drawing.Point(7, 45);
            this.addEndGuardsCheckBox.Name = "addEndGuardsCheckBox";
            this.addEndGuardsCheckBox.Size = new System.Drawing.Size(336, 22);
            this.addEndGuardsCheckBox.TabIndex = 4;
            this.addEndGuardsCheckBox.Text = "エクスポート時にaddEndGuardsを付与する";
            this.addEndGuardsCheckBox.UseVisualStyleBackColor = true;
            // 
            // ExportWMAPEvent2EAButton
            // 
            this.ExportWMAPEvent2EAButton.Location = new System.Drawing.Point(7, 211);
            this.ExportWMAPEvent2EAButton.Name = "ExportWMAPEvent2EAButton";
            this.ExportWMAPEvent2EAButton.Size = new System.Drawing.Size(686, 35);
            this.ExportWMAPEvent2EAButton.TabIndex = 3;
            this.ExportWMAPEvent2EAButton.Text = "EA形式でワールドマップイベント(選択時)をエクスポート";
            this.ExportWMAPEvent2EAButton.UseVisualStyleBackColor = true;
            this.ExportWMAPEvent2EAButton.Click += new System.EventHandler(this.ExportWMAPEvent2EAButton_Click);
            // 
            // ExportWMAPEventEAButton
            // 
            this.ExportWMAPEventEAButton.Location = new System.Drawing.Point(7, 155);
            this.ExportWMAPEventEAButton.Name = "ExportWMAPEventEAButton";
            this.ExportWMAPEventEAButton.Size = new System.Drawing.Size(686, 35);
            this.ExportWMAPEventEAButton.TabIndex = 2;
            this.ExportWMAPEventEAButton.Text = "EA形式でワールドマップイベントをエクスポート";
            this.ExportWMAPEventEAButton.UseVisualStyleBackColor = true;
            this.ExportWMAPEventEAButton.Click += new System.EventHandler(this.ExportWMAPEventEAButton_Click);
            // 
            // ExportEAButton
            // 
            this.ExportEAButton.Location = new System.Drawing.Point(7, 95);
            this.ExportEAButton.Name = "ExportEAButton";
            this.ExportEAButton.Size = new System.Drawing.Size(686, 35);
            this.ExportEAButton.TabIndex = 1;
            this.ExportEAButton.Text = "EA形式でイベントをエクスポート";
            this.ExportEAButton.UseVisualStyleBackColor = true;
            this.ExportEAButton.Click += new System.EventHandler(this.ExportEAButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "EA形式でイベントをエクスポートします。";
            // 
            // customColorGroupBox1
            // 
            this.customColorGroupBox1.BorderColor = System.Drawing.Color.Empty;
            this.customColorGroupBox1.Controls.Add(this.label5);
            this.customColorGroupBox1.Controls.Add(this.ExportUndoDataButton);
            this.customColorGroupBox1.Location = new System.Drawing.Point(7, 500);
            this.customColorGroupBox1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.customColorGroupBox1.Name = "customColorGroupBox1";
            this.customColorGroupBox1.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.customColorGroupBox1.Size = new System.Drawing.Size(741, 122);
            this.customColorGroupBox1.TabIndex = 8;
            this.customColorGroupBox1.TabStop = false;
            this.customColorGroupBox1.Text = "UndoBuffer";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(689, 53);
            this.label5.TabIndex = 6;
            this.label5.Text = "今回このROMに行った変更点を出力します";
            // 
            // ExportUndoDataButton
            // 
            this.ExportUndoDataButton.Location = new System.Drawing.Point(13, 20);
            this.ExportUndoDataButton.Name = "ExportUndoDataButton";
            this.ExportUndoDataButton.Size = new System.Drawing.Size(686, 35);
            this.ExportUndoDataButton.TabIndex = 2;
            this.ExportUndoDataButton.Text = "UndoBufferのエクスポート";
            this.ExportUndoDataButton.UseVisualStyleBackColor = true;
            this.ExportUndoDataButton.Click += new System.EventHandler(this.ExportUndoDataButton_Click);
            // 
            // ToolExportEAEventForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1080, 746);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel14);
            this.Name = "ToolExportEAEventForm";
            this.Text = "EA形式でイベントをエクスポート";
            this.Load += new System.EventHandler(this.ToolExportEAEventForm_Load);
            this.panel14.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.J_40.ResumeLayout(false);
            this.customColorGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label label62;
        private ListBoxEx MAP_LISTBOX;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ExportEAButton;
        private System.Windows.Forms.Button ExportWMAPEventEAButton;
        private System.Windows.Forms.Button ExportWMAPEvent2EAButton;
        private System.Windows.Forms.CheckBox addEndGuardsCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private CustomColorGroupBox J_40;
        private System.Windows.Forms.Button ExportMainTableButton;
        private System.Windows.Forms.Label label4;
        private CustomColorGroupBox customColorGroupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ExportUndoDataButton;

    }
}