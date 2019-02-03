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
    public partial class TacticianAffinityFE7UForm : Form
    {
        public TacticianAffinityFE7UForm()
        {
            InitializeComponent();
        }

        private void RefreshAffinity()
        {
            
            Affinity.SelectedIndex = Program.ROM.Data[(int)ReadStartAddress.Value + 4 * ((int)birth_month.Value - 1)];
            Affinity.Update();
           
        }

        private void birth_month_ValueChanged(object sender, EventArgs e)
        {
            RefreshAffinity();
        }

        private void ReadStartAddress_ValueChanged(object sender, EventArgs e)
        {
            RefreshAffinity();
        }

        private void blood_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshAffinity();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Affinity.SelectedIndex == -1)
            {
                MessageBox.Show("属性を選んでください！");
                return;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            Program.ROM.write_u8((uint)(ReadStartAddress.Value + 4 * (birth_month.Value - 1)), (uint)Affinity.SelectedIndex, undodata);
            Program.Undo.Push(undodata);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
            InputFormRef.WriteButtonToYellow(this.save, false);
        }
    }
}
