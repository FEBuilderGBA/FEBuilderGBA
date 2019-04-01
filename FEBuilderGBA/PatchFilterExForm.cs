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
    public partial class PatchFilterExForm : Form
    {
        public PatchFilterExForm()
        {
            InitializeComponent();
            this.TagFilter = "";
            U.AddCancelButton(this);
        }

        public string TagFilter  { get; private set; }

        void Close(string filter)
        {
            this.TagFilter = filter;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Close("");
        }

        private void InstalledButton_Click(object sender, EventArgs e)
        {
            Close("!");
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            Close("#IMAGE ");
        }

        private void SoundButton_Click(object sender, EventArgs e)
        {
            Close("#SOUND ");
        }

        private void EngineButton_Click(object sender, EventArgs e)
        {
            Close("#ENGINE ");
        }

        private void EventButton_Click(object sender, EventArgs e)
        {
            Close("#EVENT ");
        }

        private void ESSENTIALFIXESButton_Click(object sender, EventArgs e)
        {
            Close("#ESSENTIALFIXES ");
        }

        private void SortFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                return;
            }

            string filter ;
            if (SortFilterComboBox.SelectedIndex == 1)
            {
                filter = "@SortDateA";
            }
            else if (SortFilterComboBox.SelectedIndex == 2)
            {
                filter = "@SortDateD";
            }
            else if (SortFilterComboBox.SelectedIndex == 3)
            {
                filter = "@SortName";
            }
            else
            {
                filter = "@SortNone";
            }

            Close(filter);
        }
        public void SetSort(string filter)
        {
            if (filter == "@SortDateA")
            {
                this.SortFilterComboBox.SelectedIndex = 1;
            }
            else if (filter == "@SortDateD")
            {
                this.SortFilterComboBox.SelectedIndex = 2;
            }
            else if (filter == "@SortName")
            {
                this.SortFilterComboBox.SelectedIndex = 3;
            }
            else if (filter == "@SortNone")
            {
                this.SortFilterComboBox.SelectedIndex = 0;
            }
        }
    }
}
