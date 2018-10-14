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
    public partial class MapEditorAddMapChangeDialogForm : Form
    {
        public MapEditorAddMapChangeDialogForm()
        {
            InitializeComponent();
            FormIcon.Image = SystemIcons.Question.ToBitmap();
        }

        private void MyNewButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.Close();
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void MapEditorAddMapChangeDialogForm_Load(object sender, EventArgs e)
        {

        }
    }
}
