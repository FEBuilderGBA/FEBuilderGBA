namespace FEBuilderGBA
{
    partial class ToolProblemReportForm
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
            this.MainTab = new System.Windows.Forms.TabControl();
            this.BeginPage = new System.Windows.Forms.TabPage();
            this.label8 = new FEBuilderGBA.LabelEx();
            this.label7 = new FEBuilderGBA.LabelEx();
            this.StartButton = new System.Windows.Forms.Button();
            this.Step1Page = new System.Windows.Forms.TabPage();
            this.labelEx1 = new FEBuilderGBA.LabelEx();
            this.label2 = new System.Windows.Forms.Label();
            this.AttachDataFilename = new FEBuilderGBA.TextBoxEx();
            this.AttachDataSelectButton = new System.Windows.Forms.Button();
            this.label6 = new FEBuilderGBA.LabelEx();
            this.Step1NextButton = new System.Windows.Forms.Button();
            this.Problem = new FEBuilderGBA.TextBoxEx();
            this.ProblemLabel = new FEBuilderGBA.LabelEx();
            this.Step2Page = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.OrignalFilename = new FEBuilderGBA.TextBoxEx();
            this.OrignalSelectButton = new System.Windows.Forms.Button();
            this.Step2PrevButton = new System.Windows.Forms.Button();
            this.Step2NextButton = new System.Windows.Forms.Button();
            this.label11 = new FEBuilderGBA.LabelEx();
            this.label9 = new FEBuilderGBA.LabelEx();
            this.label5 = new FEBuilderGBA.LabelEx();
            this.EndPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenCommunities = new System.Windows.Forms.CheckBox();
            this.URLTextBoxEx = new FEBuilderGBA.TextBoxEx();
            this.EndButton = new System.Windows.Forms.Button();
            this.label12 = new FEBuilderGBA.LabelEx();
            this.panel1.SuspendLayout();
            this.MainTab.SuspendLayout();
            this.BeginPage.SuspendLayout();
            this.Step1Page.SuspendLayout();
            this.Step2Page.SuspendLayout();
            this.EndPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MainTab);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(812, 545);
            this.panel1.TabIndex = 0;
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.BeginPage);
            this.MainTab.Controls.Add(this.Step1Page);
            this.MainTab.Controls.Add(this.Step2Page);
            this.MainTab.Controls.Add(this.EndPage);
            this.MainTab.Location = new System.Drawing.Point(4, -1);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(805, 543);
            this.MainTab.TabIndex = 0;
            // 
            // BeginPage
            // 
            this.BeginPage.BackColor = System.Drawing.SystemColors.Control;
            this.BeginPage.Controls.Add(this.label8);
            this.BeginPage.Controls.Add(this.label7);
            this.BeginPage.Controls.Add(this.StartButton);
            this.BeginPage.Location = new System.Drawing.Point(4, 28);
            this.BeginPage.Name = "BeginPage";
            this.BeginPage.Padding = new System.Windows.Forms.Padding(3);
            this.BeginPage.Size = new System.Drawing.Size(797, 511);
            this.BeginPage.TabIndex = 0;
            this.BeginPage.Text = "BeginPage";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ErrorMessage = "";
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(12, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(164, 24);
            this.label8.TabIndex = 10;
            this.label8.Text = "問題報告ツール";
            // 
            // label7
            // 
            this.label7.ErrorMessage = "";
            this.label7.Location = new System.Drawing.Point(9, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(771, 203);
            this.label7.TabIndex = 9;
            this.label7.Text = "このツールは、あなたのROMに存在する問題のレポートを作成するツールです。\r\nどうしても解決できない問題がある場合は、このツールでレポートを作成して、コミニティに" +
    "質問してみてください。\r\n\r\nこのツールを利用することで、問題を再現させ、問題を解決させるためのデータを簡単に作ることができます。";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(598, 454);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(172, 36);
            this.StartButton.TabIndex = 8;
            this.StartButton.Text = "始める";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // Step1Page
            // 
            this.Step1Page.BackColor = System.Drawing.SystemColors.Control;
            this.Step1Page.Controls.Add(this.labelEx1);
            this.Step1Page.Controls.Add(this.label2);
            this.Step1Page.Controls.Add(this.AttachDataFilename);
            this.Step1Page.Controls.Add(this.AttachDataSelectButton);
            this.Step1Page.Controls.Add(this.label6);
            this.Step1Page.Controls.Add(this.Step1NextButton);
            this.Step1Page.Controls.Add(this.Problem);
            this.Step1Page.Controls.Add(this.ProblemLabel);
            this.Step1Page.Location = new System.Drawing.Point(4, 28);
            this.Step1Page.Name = "Step1Page";
            this.Step1Page.Padding = new System.Windows.Forms.Padding(3);
            this.Step1Page.Size = new System.Drawing.Size(797, 511);
            this.Step1Page.TabIndex = 1;
            this.Step1Page.Text = "Step1Page";
            // 
            // labelEx1
            // 
            this.labelEx1.ErrorMessage = "";
            this.labelEx1.Location = new System.Drawing.Point(6, 390);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(775, 58);
            this.labelEx1.TabIndex = 98;
            this.labelEx1.Text = "戦闘アニメや画像データなど、問題を再現させるためのファイルがあれば添付してください。\r\n特にない場合は、空欄のままにしてください。\r\n最大2MBまで添付できます";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(6, 320);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 31);
            this.label2.TabIndex = 97;
            this.label2.Text = "添付データ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AttachDataFilename
            // 
            this.AttachDataFilename.ErrorMessage = "";
            this.AttachDataFilename.Location = new System.Drawing.Point(144, 361);
            this.AttachDataFilename.Margin = new System.Windows.Forms.Padding(4);
            this.AttachDataFilename.Name = "AttachDataFilename";
            this.AttachDataFilename.Placeholder = "";
            this.AttachDataFilename.Size = new System.Drawing.Size(638, 25);
            this.AttachDataFilename.TabIndex = 96;
            this.AttachDataFilename.DoubleClick += new System.EventHandler(this.AttachDataSelectButton_Click);
            // 
            // AttachDataSelectButton
            // 
            this.AttachDataSelectButton.Location = new System.Drawing.Point(6, 355);
            this.AttachDataSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.AttachDataSelectButton.Name = "AttachDataSelectButton";
            this.AttachDataSelectButton.Size = new System.Drawing.Size(130, 31);
            this.AttachDataSelectButton.TabIndex = 95;
            this.AttachDataSelectButton.Text = "ファイル選択";
            this.AttachDataSelectButton.UseVisualStyleBackColor = true;
            this.AttachDataSelectButton.Click += new System.EventHandler(this.AttachDataSelectButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ErrorMessage = "";
            this.label6.Location = new System.Drawing.Point(695, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 18);
            this.label6.TabIndex = 17;
            this.label6.Text = "Step 1/2";
            // 
            // Step1NextButton
            // 
            this.Step1NextButton.Location = new System.Drawing.Point(598, 458);
            this.Step1NextButton.Name = "Step1NextButton";
            this.Step1NextButton.Size = new System.Drawing.Size(172, 36);
            this.Step1NextButton.TabIndex = 16;
            this.Step1NextButton.Text = "次へ";
            this.Step1NextButton.UseVisualStyleBackColor = true;
            this.Step1NextButton.Click += new System.EventHandler(this.Step1NextButton_Click);
            // 
            // Problem
            // 
            this.Problem.ErrorMessage = "";
            this.Problem.Location = new System.Drawing.Point(6, 62);
            this.Problem.Multiline = true;
            this.Problem.Name = "Problem";
            this.Problem.Placeholder = "";
            this.Problem.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Problem.Size = new System.Drawing.Size(775, 247);
            this.Problem.TabIndex = 9;
            // 
            // ProblemLabel
            // 
            this.ProblemLabel.ErrorMessage = "";
            this.ProblemLabel.Location = new System.Drawing.Point(3, 22);
            this.ProblemLabel.Name = "ProblemLabel";
            this.ProblemLabel.Size = new System.Drawing.Size(669, 36);
            this.ProblemLabel.TabIndex = 8;
            this.ProblemLabel.Text = "どんな問題がありますか？\r\n問題を具体的に説明してください。";
            // 
            // Step2Page
            // 
            this.Step2Page.BackColor = System.Drawing.SystemColors.Control;
            this.Step2Page.Controls.Add(this.label4);
            this.Step2Page.Controls.Add(this.OrignalFilename);
            this.Step2Page.Controls.Add(this.OrignalSelectButton);
            this.Step2Page.Controls.Add(this.Step2PrevButton);
            this.Step2Page.Controls.Add(this.Step2NextButton);
            this.Step2Page.Controls.Add(this.label11);
            this.Step2Page.Controls.Add(this.label9);
            this.Step2Page.Controls.Add(this.label5);
            this.Step2Page.Location = new System.Drawing.Point(4, 28);
            this.Step2Page.Name = "Step2Page";
            this.Step2Page.Size = new System.Drawing.Size(797, 511);
            this.Step2Page.TabIndex = 3;
            this.Step2Page.Text = "Step2Page";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(4, 103);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 31);
            this.label4.TabIndex = 94;
            this.label4.Text = "無改造ROM";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OrignalFilename
            // 
            this.OrignalFilename.ErrorMessage = "";
            this.OrignalFilename.Location = new System.Drawing.Point(142, 144);
            this.OrignalFilename.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalFilename.Name = "OrignalFilename";
            this.OrignalFilename.Placeholder = "";
            this.OrignalFilename.Size = new System.Drawing.Size(638, 25);
            this.OrignalFilename.TabIndex = 93;
            this.OrignalFilename.DoubleClick += new System.EventHandler(this.OrignalFilename_DoubleClick);
            // 
            // OrignalSelectButton
            // 
            this.OrignalSelectButton.Location = new System.Drawing.Point(4, 138);
            this.OrignalSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.OrignalSelectButton.Name = "OrignalSelectButton";
            this.OrignalSelectButton.Size = new System.Drawing.Size(130, 31);
            this.OrignalSelectButton.TabIndex = 92;
            this.OrignalSelectButton.Text = "ファイル選択";
            this.OrignalSelectButton.UseVisualStyleBackColor = true;
            this.OrignalSelectButton.Click += new System.EventHandler(this.OrignalSelectButton_Click);
            // 
            // Step2PrevButton
            // 
            this.Step2PrevButton.Location = new System.Drawing.Point(398, 458);
            this.Step2PrevButton.Name = "Step2PrevButton";
            this.Step2PrevButton.Size = new System.Drawing.Size(172, 36);
            this.Step2PrevButton.TabIndex = 21;
            this.Step2PrevButton.Text = "戻る";
            this.Step2PrevButton.UseVisualStyleBackColor = true;
            this.Step2PrevButton.Click += new System.EventHandler(this.Step2PrevButton_Click);
            // 
            // Step2NextButton
            // 
            this.Step2NextButton.Location = new System.Drawing.Point(598, 458);
            this.Step2NextButton.Name = "Step2NextButton";
            this.Step2NextButton.Size = new System.Drawing.Size(172, 36);
            this.Step2NextButton.TabIndex = 20;
            this.Step2NextButton.Text = "作成";
            this.Step2NextButton.UseVisualStyleBackColor = true;
            this.Step2NextButton.Click += new System.EventHandler(this.Step2NextButton_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ErrorMessage = "";
            this.label11.Location = new System.Drawing.Point(695, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 18);
            this.label11.TabIndex = 19;
            this.label11.Text = "Step 2/2";
            // 
            // label9
            // 
            this.label9.ErrorMessage = "";
            this.label9.Location = new System.Drawing.Point(3, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(767, 126);
            this.label9.TabIndex = 14;
            this.label9.Text = "多くの国では、ROMを公開することは違法です。\r\nそのため、あなたの改造だけを取り出したupsパッチという差分データを作ります。\r\nupsパッチは、無改造ROMと" +
    "、あなたのROMを比較して、あなたの改造だけを取り出します。";
            // 
            // label5
            // 
            this.label5.ErrorMessage = "";
            this.label5.Location = new System.Drawing.Point(3, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(686, 61);
            this.label5.TabIndex = 12;
            this.label5.Text = "私たちが、問題を再現して調査するためには、upsパッチが必要です。\r\nupsパッチを作るために、元になる無改造ROMを指定してください。";
            // 
            // EndPage
            // 
            this.EndPage.BackColor = System.Drawing.SystemColors.Control;
            this.EndPage.Controls.Add(this.label1);
            this.EndPage.Controls.Add(this.OpenCommunities);
            this.EndPage.Controls.Add(this.URLTextBoxEx);
            this.EndPage.Controls.Add(this.EndButton);
            this.EndPage.Controls.Add(this.label12);
            this.EndPage.Location = new System.Drawing.Point(4, 28);
            this.EndPage.Name = "EndPage";
            this.EndPage.Size = new System.Drawing.Size(797, 511);
            this.EndPage.TabIndex = 4;
            this.EndPage.Text = "EndPage";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 18);
            this.label1.TabIndex = 24;
            this.label1.Text = "DiscordコミニティURL";
            // 
            // OpenCommunities
            // 
            this.OpenCommunities.AutoSize = true;
            this.OpenCommunities.Checked = true;
            this.OpenCommunities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OpenCommunities.Location = new System.Drawing.Point(7, 458);
            this.OpenCommunities.Name = "OpenCommunities";
            this.OpenCommunities.Size = new System.Drawing.Size(316, 22);
            this.OpenCommunities.TabIndex = 23;
            this.OpenCommunities.Text = "完了ボタンでDiscordコミニティURLを開く";
            this.OpenCommunities.UseVisualStyleBackColor = true;
            // 
            // URLTextBoxEx
            // 
            this.URLTextBoxEx.ErrorMessage = "";
            this.URLTextBoxEx.Location = new System.Drawing.Point(7, 216);
            this.URLTextBoxEx.Name = "URLTextBoxEx";
            this.URLTextBoxEx.Placeholder = "";
            this.URLTextBoxEx.Size = new System.Drawing.Size(763, 25);
            this.URLTextBoxEx.TabIndex = 22;
            this.URLTextBoxEx.DoubleClick += new System.EventHandler(this.URLTextBoxEx_DoubleClick);
            // 
            // EndButton
            // 
            this.EndButton.Location = new System.Drawing.Point(598, 458);
            this.EndButton.Name = "EndButton";
            this.EndButton.Size = new System.Drawing.Size(172, 36);
            this.EndButton.TabIndex = 21;
            this.EndButton.Text = "完了";
            this.EndButton.UseVisualStyleBackColor = true;
            this.EndButton.Click += new System.EventHandler(this.EndButton_Click);
            // 
            // label12
            // 
            this.label12.ErrorMessage = "";
            this.label12.Location = new System.Drawing.Point(4, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(766, 157);
            this.label12.TabIndex = 0;
            this.label12.Text = "レポートを作成しました。\r\nこの7zファイルに、問題を再現するのに必要なデータが格納されています。\r\nこのファイルをコミニティに投稿して質問してください。";
            // 
            // ToolProblemReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(837, 570);
            this.Controls.Add(this.panel1);
            this.Name = "ToolProblemReportForm";
            this.Text = "問題報告ツール";
            this.Load += new System.EventHandler(this.ToolProblemReportForm_Load);
            this.Shown += new System.EventHandler(this.ToolProblemReportForm_Shown);
            this.panel1.ResumeLayout(false);
            this.MainTab.ResumeLayout(false);
            this.BeginPage.ResumeLayout(false);
            this.BeginPage.PerformLayout();
            this.Step1Page.ResumeLayout(false);
            this.Step1Page.PerformLayout();
            this.Step2Page.ResumeLayout(false);
            this.Step2Page.PerformLayout();
            this.EndPage.ResumeLayout(false);
            this.EndPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage BeginPage;
        private System.Windows.Forms.TabPage Step1Page;
        private System.Windows.Forms.TabPage Step2Page;
        private System.Windows.Forms.TabPage EndPage;
        private System.Windows.Forms.Button StartButton;
        private TextBoxEx Problem;
        private LabelEx ProblemLabel;
        private LabelEx label5;
        private LabelEx label7;
        private LabelEx label8;
        private System.Windows.Forms.Button Step1NextButton;
        private LabelEx label9;
        private LabelEx label6;
        private LabelEx label11;
        private LabelEx label12;
        private System.Windows.Forms.Button Step2NextButton;
        private System.Windows.Forms.Button Step2PrevButton;
        private System.Windows.Forms.Button EndButton;
        private System.Windows.Forms.Label label4;
        private TextBoxEx OrignalFilename;
        private System.Windows.Forms.Button OrignalSelectButton;
        private TextBoxEx URLTextBoxEx;
        private System.Windows.Forms.CheckBox OpenCommunities;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TextBoxEx AttachDataFilename;
        private System.Windows.Forms.Button AttachDataSelectButton;
        private LabelEx labelEx1;
    }
}
