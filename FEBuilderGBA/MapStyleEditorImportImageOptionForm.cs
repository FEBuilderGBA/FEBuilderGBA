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
    public partial class MapStyleEditorImportImageOptionForm : Form
    {
        public MapStyleEditorImportImageOptionForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);

            button1.Focus();
        }

        private void MapStyleEditorImportImageOptionForm_Load(object sender, EventArgs e)
        {

        }

        public enum ImportOptionEnum
        {
              WithPalette
            , ImageOnly
            , OnePicture
        }
        public ImportOptionEnum ImportOption { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            ImportOption = ImportOptionEnum.WithPalette;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImportOption = ImportOptionEnum.ImageOnly;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ImportOption = ImportOptionEnum.OnePicture;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
