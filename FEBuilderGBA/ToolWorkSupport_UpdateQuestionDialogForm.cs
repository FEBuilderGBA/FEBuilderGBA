using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolWorkSupport_UpdateQuestionDialogForm : Form
    {
        public ToolWorkSupport_UpdateQuestionDialogForm()
        {
            InitializeComponent();
            FormIcon.Image = SystemIcons.Question.ToBitmap();
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void ForceUpdatebutton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();
        }

        public void SetVersion(string version)
        {
            this.Version = version;
        }

        string Version;
        private void ToolWorkSupport_UpdateQuestionDialogForm_Load(object sender, EventArgs e)
        {
            this.labelEx1.Text = R._("現在のバージョンが最新です。更新する必要はありません。version:{0}", this.Version);
        }

    }
}
