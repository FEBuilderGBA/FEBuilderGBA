namespace FEBuilderGBA
{
    partial class EventTemplate3Form
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
            this.BLANK_Button = new System.Windows.Forms.Button();
            this.TalkEvent_button = new System.Windows.Forms.Button();
            this.EnemyReinforcement_button = new System.Windows.Forms.Button();
            this.PlayerReinforcement = new System.Windows.Forms.Button();
            this.CALL_EndEvent_button = new System.Windows.Forms.Button();
            this.GAMEOVER_Button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EnemyReinforcementIfHard_button = new System.Windows.Forms.Button();
            this.FormIcon = new System.Windows.Forms.PictureBox();
            this.EnemyReinforcementByCounterButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(99, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "新規にイベントを割り振りますか？";
            // 
            // BLANK_Button
            // 
            this.BLANK_Button.Location = new System.Drawing.Point(12, 102);
            this.BLANK_Button.Name = "BLANK_Button";
            this.BLANK_Button.Size = new System.Drawing.Size(759, 42);
            this.BLANK_Button.TabIndex = 0;
            this.BLANK_Button.Text = "新規にイベント領域を割り振り、空のイベントを定義します。";
            this.BLANK_Button.UseVisualStyleBackColor = true;
            this.BLANK_Button.Click += new System.EventHandler(this.BLANK_Button_Click);
            // 
            // TalkEvent_button
            // 
            this.TalkEvent_button.Location = new System.Drawing.Point(12, 190);
            this.TalkEvent_button.Name = "TalkEvent_button";
            this.TalkEvent_button.Size = new System.Drawing.Size(759, 42);
            this.TalkEvent_button.TabIndex = 1;
            this.TalkEvent_button.Text = "会話イベント";
            this.TalkEvent_button.UseVisualStyleBackColor = true;
            this.TalkEvent_button.Click += new System.EventHandler(this.TalkEvent_button_Click);
            // 
            // EnemyReinforcement_button
            // 
            this.EnemyReinforcement_button.Location = new System.Drawing.Point(12, 238);
            this.EnemyReinforcement_button.Name = "EnemyReinforcement_button";
            this.EnemyReinforcement_button.Size = new System.Drawing.Size(759, 42);
            this.EnemyReinforcement_button.TabIndex = 2;
            this.EnemyReinforcement_button.Text = "敵増援";
            this.EnemyReinforcement_button.UseVisualStyleBackColor = true;
            this.EnemyReinforcement_button.Click += new System.EventHandler(this.EnemyReinforcement_button_Click);
            // 
            // PlayerReinforcement
            // 
            this.PlayerReinforcement.Location = new System.Drawing.Point(12, 341);
            this.PlayerReinforcement.Name = "PlayerReinforcement";
            this.PlayerReinforcement.Size = new System.Drawing.Size(759, 42);
            this.PlayerReinforcement.TabIndex = 4;
            this.PlayerReinforcement.Text = "援軍";
            this.PlayerReinforcement.UseVisualStyleBackColor = true;
            this.PlayerReinforcement.Click += new System.EventHandler(this.PlayerReinforcement_Click);
            // 
            // CALL_EndEvent_button
            // 
            this.CALL_EndEvent_button.Location = new System.Drawing.Point(12, 523);
            this.CALL_EndEvent_button.Name = "CALL_EndEvent_button";
            this.CALL_EndEvent_button.Size = new System.Drawing.Size(759, 42);
            this.CALL_EndEvent_button.TabIndex = 7;
            this.CALL_EndEvent_button.Text = "章終了イベントを呼び出す(章クリア)";
            this.CALL_EndEvent_button.UseVisualStyleBackColor = true;
            this.CALL_EndEvent_button.Click += new System.EventHandler(this.CALL_EndEvent_button_Click);
            // 
            // GAMEOVER_Button
            // 
            this.GAMEOVER_Button.Location = new System.Drawing.Point(12, 389);
            this.GAMEOVER_Button.Name = "GAMEOVER_Button";
            this.GAMEOVER_Button.Size = new System.Drawing.Size(759, 42);
            this.GAMEOVER_Button.TabIndex = 5;
            this.GAMEOVER_Button.Text = "ゲームオーバーイベント";
            this.GAMEOVER_Button.UseVisualStyleBackColor = true;
            this.GAMEOVER_Button.Click += new System.EventHandler(this.GAMEOVER_Button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "テンプレート";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 502);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 18);
            this.label3.TabIndex = 13;
            this.label3.Text = "既存イベントを呼び出す";
            // 
            // EnemyReinforcementIfHard_button
            // 
            this.EnemyReinforcementIfHard_button.Location = new System.Drawing.Point(12, 290);
            this.EnemyReinforcementIfHard_button.Name = "EnemyReinforcementIfHard_button";
            this.EnemyReinforcementIfHard_button.Size = new System.Drawing.Size(759, 42);
            this.EnemyReinforcementIfHard_button.TabIndex = 3;
            this.EnemyReinforcementIfHard_button.Text = "敵増援(難易度:ハードのみ)";
            this.EnemyReinforcementIfHard_button.UseVisualStyleBackColor = true;
            this.EnemyReinforcementIfHard_button.Click += new System.EventHandler(this.EnemyReinforcementIfHard_button_Click);
            // 
            // FormIcon
            // 
            this.FormIcon.Location = new System.Drawing.Point(15, 12);
            this.FormIcon.Name = "FormIcon";
            this.FormIcon.Size = new System.Drawing.Size(64, 64);
            this.FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FormIcon.TabIndex = 14;
            this.FormIcon.TabStop = false;
            // 
            // EnemyReinforcementByCounterButton
            // 
            this.EnemyReinforcementByCounterButton.Location = new System.Drawing.Point(12, 437);
            this.EnemyReinforcementByCounterButton.Name = "EnemyReinforcementByCounterButton";
            this.EnemyReinforcementByCounterButton.Size = new System.Drawing.Size(759, 42);
            this.EnemyReinforcementByCounterButton.TabIndex = 6;
            this.EnemyReinforcementByCounterButton.Text = "カウンターを利用して特定のイベントから数ターン増援";
            this.EnemyReinforcementByCounterButton.UseVisualStyleBackColor = true;
            this.EnemyReinforcementByCounterButton.Click += new System.EventHandler(this.EnemyReinforcementByCounterButton_Click);
            // 
            // EventTemplate3Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 586);
            this.Controls.Add(this.EnemyReinforcementByCounterButton);
            this.Controls.Add(this.EnemyReinforcementIfHard_button);
            this.Controls.Add(this.FormIcon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GAMEOVER_Button);
            this.Controls.Add(this.CALL_EndEvent_button);
            this.Controls.Add(this.PlayerReinforcement);
            this.Controls.Add(this.EnemyReinforcement_button);
            this.Controls.Add(this.TalkEvent_button);
            this.Controls.Add(this.BLANK_Button);
            this.Controls.Add(this.label1);
            this.Name = "EventTemplate3Form";
            this.Text = "新規にイベントを割り振りますか？";
            this.Load += new System.EventHandler(this.EventCondEventTemplate3Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BLANK_Button;
        private System.Windows.Forms.Button TalkEvent_button;
        private System.Windows.Forms.Button EnemyReinforcement_button;
        private System.Windows.Forms.Button PlayerReinforcement;
        private System.Windows.Forms.Button CALL_EndEvent_button;
        private System.Windows.Forms.Button GAMEOVER_Button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox FormIcon;
        private System.Windows.Forms.Button EnemyReinforcementIfHard_button;
        private System.Windows.Forms.Button EnemyReinforcementByCounterButton;
    }
}