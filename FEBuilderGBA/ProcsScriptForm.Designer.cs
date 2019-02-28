namespace FEBuilderGBA
{
    partial class ProcsScriptForm
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
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddressList = new FEBuilderGBA.ListBoxEx();
            this.label30 = new System.Windows.Forms.Label();
            this.MainTab = new FEBuilderGBA.TabControlEx();
            this.ParamValue2 = new FEBuilderGBA.TextBoxEx();
            this.ParamValue1 = new FEBuilderGBA.TextBoxEx();
            this.CommentTextBox = new FEBuilderGBA.TextBoxEx();
            this.ASMTextBox = new FEBuilderGBA.TextBoxEx();
            this.ScriptCodeName = new FEBuilderGBA.TextBoxEx();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.AddressList);
            this.panel6.Controls.Add(this.label30);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(352, 836);
            this.panel6.TabIndex = 150;
            // 
            // AddressList
            // 
            this.AddressList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressList.FormattingEnabled = true;
            this.AddressList.IntegralHeight = false;
            this.AddressList.ItemHeight = 18;
            this.AddressList.Location = new System.Drawing.Point(0, 29);
            this.AddressList.Margin = new System.Windows.Forms.Padding(4);
            this.AddressList.Name = "AddressList";
            this.AddressList.Size = new System.Drawing.Size(350, 805);
            this.AddressList.TabIndex = 0;
            this.AddressList.SelectedIndexChanged += new System.EventHandler(this.AddressList_SelectedIndexChanged);
            // 
            // label30
            // 
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Dock = System.Windows.Forms.DockStyle.Top;
            this.label30.Location = new System.Drawing.Point(0, 0);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(350, 29);
            this.label30.TabIndex = 106;
            this.label30.Text = "名前";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainTab
            // 
            this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTab.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.MainTab.ItemSize = new System.Drawing.Size(200, 18);
            this.MainTab.Location = new System.Drawing.Point(352, 0);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(1278, 836);
            this.MainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTab.TabIndex = 151;
            // 
            // ParamValue2
            // 
            this.ParamValue2.ErrorMessage = "";
            this.ParamValue2.Location = new System.Drawing.Point(301, 31);
            this.ParamValue2.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue2.Name = "ParamValue2";
            this.ParamValue2.Placeholder = "";
            this.ParamValue2.ReadOnly = true;
            this.ParamValue2.Size = new System.Drawing.Size(218, 25);
            this.ParamValue2.TabIndex = 204;
            // 
            // ParamValue1
            // 
            this.ParamValue1.ErrorMessage = "";
            this.ParamValue1.Location = new System.Drawing.Point(301, 3);
            this.ParamValue1.Margin = new System.Windows.Forms.Padding(2);
            this.ParamValue1.Name = "ParamValue1";
            this.ParamValue1.Placeholder = "";
            this.ParamValue1.ReadOnly = true;
            this.ParamValue1.Size = new System.Drawing.Size(218, 25);
            this.ParamValue1.TabIndex = 203;
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.ErrorMessage = "";
            this.CommentTextBox.Location = new System.Drawing.Point(144, 3);
            this.CommentTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Placeholder = "";
            this.CommentTextBox.Size = new System.Drawing.Size(792, 25);
            this.CommentTextBox.TabIndex = 0;
            // 
            // ASMTextBox
            // 
            this.ASMTextBox.ErrorMessage = "";
            this.ASMTextBox.Location = new System.Drawing.Point(144, 57);
            this.ASMTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ASMTextBox.Name = "ASMTextBox";
            this.ASMTextBox.Placeholder = "";
            this.ASMTextBox.Size = new System.Drawing.Size(514, 25);
            this.ASMTextBox.TabIndex = 3;
            // 
            // ScriptCodeName
            // 
            this.ScriptCodeName.ErrorMessage = "";
            this.ScriptCodeName.Location = new System.Drawing.Point(144, 30);
            this.ScriptCodeName.Margin = new System.Windows.Forms.Padding(2);
            this.ScriptCodeName.Name = "ScriptCodeName";
            this.ScriptCodeName.Placeholder = "";
            this.ScriptCodeName.ReadOnly = true;
            this.ScriptCodeName.Size = new System.Drawing.Size(791, 25);
            this.ScriptCodeName.TabIndex = 1;
            // 
            // ProcsScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1630, 836);
            this.Controls.Add(this.MainTab);
            this.Controls.Add(this.panel6);
            this.Name = "ProcsScriptForm";
            this.Text = "Procs";
            this.Load += new System.EventHandler(this.ProcsScriptForm_Load);
            this.Shown += new System.EventHandler(this.ProcsScriptForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProcsScriptForm_KeyDown);
            this.Resize += new System.EventHandler(this.ProcsScriptForm_Resize);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label30;
        private ListBoxEx AddressList;
        private FEBuilderGBA.TextBoxEx ASMTextBox;
        private FEBuilderGBA.TextBoxEx ScriptCodeName;
        private TextBoxEx ParamValue2;
        private TextBoxEx ParamValue1;
        private TextBoxEx CommentTextBox;
        private TabControlEx MainTab;
    }
}