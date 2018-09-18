namespace FEBuilderGBA
{
    partial class EventTemplate2Form
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
            this.EnterByPlayer_button = new System.Windows.Forms.Button();
            this.EnterByUnit_button = new System.Windows.Forms.Button();
            this.EnterByUnitToGameOver_Button = new System.Windows.Forms.Button();
            this.DesertTreasure_Button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CALL_EndEvent_button = new System.Windows.Forms.Button();
            this.FormIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 12);
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
            // EnterByPlayer_button
            // 
            this.EnterByPlayer_button.Location = new System.Drawing.Point(12, 186);
            this.EnterByPlayer_button.Name = "EnterByPlayer_button";
            this.EnterByPlayer_button.Size = new System.Drawing.Size(759, 42);
            this.EnterByPlayer_button.TabIndex = 1;
            this.EnterByPlayer_button.Text = "自軍が侵入したら発動するイベントを作成";
            this.EnterByPlayer_button.UseVisualStyleBackColor = true;
            this.EnterByPlayer_button.Click += new System.EventHandler(this.EnterByPlayer_button_Click);
            // 
            // EnterByUnit_button
            // 
            this.EnterByUnit_button.Location = new System.Drawing.Point(12, 234);
            this.EnterByUnit_button.Name = "EnterByUnit_button";
            this.EnterByUnit_button.Size = new System.Drawing.Size(759, 42);
            this.EnterByUnit_button.TabIndex = 2;
            this.EnterByUnit_button.Text = "特定のユニットが侵入したら発動するイベントを作成";
            this.EnterByUnit_button.UseVisualStyleBackColor = true;
            this.EnterByUnit_button.Click += new System.EventHandler(this.EnterByUnit_button_Click);
            // 
            // EnterByUnitToGameOver_Button
            // 
            this.EnterByUnitToGameOver_Button.Location = new System.Drawing.Point(12, 282);
            this.EnterByUnitToGameOver_Button.Name = "EnterByUnitToGameOver_Button";
            this.EnterByUnitToGameOver_Button.Size = new System.Drawing.Size(759, 42);
            this.EnterByUnitToGameOver_Button.TabIndex = 3;
            this.EnterByUnitToGameOver_Button.Text = "特定のユニットが侵入したらゲームオーバーイベント";
            this.EnterByUnitToGameOver_Button.UseVisualStyleBackColor = true;
            this.EnterByUnitToGameOver_Button.Click += new System.EventHandler(this.GameOver_Button_Click);
            // 
            // DesertTreasure_Button
            // 
            this.DesertTreasure_Button.Location = new System.Drawing.Point(13, 329);
            this.DesertTreasure_Button.Name = "DesertTreasure_Button";
            this.DesertTreasure_Button.Size = new System.Drawing.Size(759, 42);
            this.DesertTreasure_Button.TabIndex = 4;
            this.DesertTreasure_Button.Text = "砂漠の財宝";
            this.DesertTreasure_Button.UseVisualStyleBackColor = true;
            this.DesertTreasure_Button.Click += new System.EventHandler(this.DesertTreasure_Button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 18);
            this.label2.TabIndex = 9;
            this.label2.Text = "テンプレート";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 397);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 18);
            this.label3.TabIndex = 12;
            this.label3.Text = "既存イベントを呼び出す";
            // 
            // CALL_EndEvent_button
            // 
            this.CALL_EndEvent_button.Location = new System.Drawing.Point(11, 428);
            this.CALL_EndEvent_button.Name = "CALL_EndEvent_button";
            this.CALL_EndEvent_button.Size = new System.Drawing.Size(759, 42);
            this.CALL_EndEvent_button.TabIndex = 6;
            this.CALL_EndEvent_button.Text = "章終了イベントを呼び出す(章クリア)";
            this.CALL_EndEvent_button.UseVisualStyleBackColor = true;
            this.CALL_EndEvent_button.Click += new System.EventHandler(this.CALL_EndEvent_button_Click);
            // 
            // FormIcon
            // 
            this.FormIcon.Location = new System.Drawing.Point(15, 12);
            this.FormIcon.Name = "FormIcon";
            this.FormIcon.Size = new System.Drawing.Size(64, 64);
            this.FormIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FormIcon.TabIndex = 13;
            this.FormIcon.TabStop = false;
            // 
            // EventTemplate2Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 484);
            this.Controls.Add(this.FormIcon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CALL_EndEvent_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DesertTreasure_Button);
            this.Controls.Add(this.EnterByUnitToGameOver_Button);
            this.Controls.Add(this.EnterByUnit_button);
            this.Controls.Add(this.EnterByPlayer_button);
            this.Controls.Add(this.BLANK_Button);
            this.Controls.Add(this.label1);
            this.Name = "EventTemplate2Form";
            this.Text = "新規にイベントを割り振りますか？";
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BLANK_Button;
        private System.Windows.Forms.Button EnterByPlayer_button;
        private System.Windows.Forms.Button EnterByUnit_button;
        private System.Windows.Forms.Button EnterByUnitToGameOver_Button;
        private System.Windows.Forms.Button DesertTreasure_Button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CALL_EndEvent_button;
        private System.Windows.Forms.PictureBox FormIcon;
    }
}