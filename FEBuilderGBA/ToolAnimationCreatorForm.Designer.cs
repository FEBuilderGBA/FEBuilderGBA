namespace FEBuilderGBA
{
    partial class ToolAnimationCreatorForm
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
            this.MainTab = new FEBuilderGBA.TabControlEx();
            this.ModeHint = new FEBuilderGBA.TextBoxEx();
            this.FileHint = new FEBuilderGBA.TextBoxEx();
            this.SoundInfo = new FEBuilderGBA.TextBoxEx();
            this.CodeInfo = new FEBuilderGBA.TextBoxEx();
            this.SkillSoundInfo = new FEBuilderGBA.TextBoxEx();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTab.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.MainTab.ItemSize = new System.Drawing.Size(200, 18);
            this.MainTab.Location = new System.Drawing.Point(0, 0);
            this.MainTab.Margin = new System.Windows.Forms.Padding(2);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(1651, 934);
            this.MainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTab.TabIndex = 2;
            // 
            // ModeHint
            // 
            this.ModeHint.ErrorMessage = "";
            this.ModeHint.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ModeHint.Location = new System.Drawing.Point(4, 82);
            this.ModeHint.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ModeHint.Multiline = true;
            this.ModeHint.Name = "ModeHint";
            this.ModeHint.Placeholder = "";
            this.ModeHint.ReadOnly = true;
            this.ModeHint.Size = new System.Drawing.Size(599, 31);
            this.ModeHint.TabIndex = 184;
            // 
            // FileHint
            // 
            this.FileHint.ErrorMessage = "";
            this.FileHint.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FileHint.Location = new System.Drawing.Point(4, 40);
            this.FileHint.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.FileHint.Multiline = true;
            this.FileHint.Name = "FileHint";
            this.FileHint.Placeholder = "";
            this.FileHint.ReadOnly = true;
            this.FileHint.Size = new System.Drawing.Size(599, 38);
            this.FileHint.TabIndex = 174;
            // 
            // SoundInfo
            // 
            this.SoundInfo.ErrorMessage = "";
            this.SoundInfo.Location = new System.Drawing.Point(109, 43);
            this.SoundInfo.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SoundInfo.Multiline = true;
            this.SoundInfo.Name = "SoundInfo";
            this.SoundInfo.Placeholder = "";
            this.SoundInfo.ReadOnly = true;
            this.SoundInfo.Size = new System.Drawing.Size(760, 98);
            this.SoundInfo.TabIndex = 45;
            // 
            // CodeInfo
            // 
            this.CodeInfo.ErrorMessage = "";
            this.CodeInfo.Location = new System.Drawing.Point(92, 43);
            this.CodeInfo.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CodeInfo.Multiline = true;
            this.CodeInfo.Name = "CodeInfo";
            this.CodeInfo.Placeholder = "";
            this.CodeInfo.ReadOnly = true;
            this.CodeInfo.Size = new System.Drawing.Size(764, 170);
            this.CodeInfo.TabIndex = 41;
            // 
            // SkillSoundInfo
            // 
            this.SkillSoundInfo.ErrorMessage = "";
            this.SkillSoundInfo.Location = new System.Drawing.Point(103, 43);
            this.SkillSoundInfo.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SkillSoundInfo.Multiline = true;
            this.SkillSoundInfo.Name = "SkillSoundInfo";
            this.SkillSoundInfo.Placeholder = "";
            this.SkillSoundInfo.ReadOnly = true;
            this.SkillSoundInfo.Size = new System.Drawing.Size(753, 61);
            this.SkillSoundInfo.TabIndex = 49;
            // 
            // ToolAnimationCreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1651, 934);
            this.Controls.Add(this.MainTab);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ToolAnimationCreatorForm";
            this.Text = "アニメーションエディタ";
            this.Load += new System.EventHandler(this.ToolAnimationCreatorForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ToolAnimationCreatorForm_KeyDown);
            this.Resize += new System.EventHandler(this.ToolAnimationCreatorForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private TextBoxEx FileHint;
        private TextBoxEx CodeInfo;
        private TextBoxEx SoundInfo;
        private TextBoxEx SkillSoundInfo;
        private TextBoxEx ModeHint;
        private TabControlEx MainTab;
    }
}