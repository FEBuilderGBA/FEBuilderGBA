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
    public partial class ImageBGSelectPopupForm : Form
    {
        public ImageBGSelectPopupForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
            this.Selected = SelectedType.None;
        }

        private void ImageBGSelectPopupForm_Load(object sender, EventArgs e)
        {

        }

        public SelectedType Selected { get; protected set; } // Huffman tree end (indirected twice)
        public enum SelectedType
        {
            None,
            VanillaTSA,
            BG224,
            BG255,
        }

        private void VanillaTSAButton_Click(object sender, EventArgs e)
        {
            this.Selected = SelectedType.VanillaTSA;
            this.Close();
        }

        private void BG244Button_Click(object sender, EventArgs e)
        {
            this.Selected = SelectedType.BG224;
            this.Close();
        }

        private void BG255Button_Click(object sender, EventArgs e)
        {
            this.Selected = SelectedType.BG255;
            this.Close();
        }
    }
}
